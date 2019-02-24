using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Spotify.NetStandard.Client.Internal
{
    internal class SimpleServiceClient : IDisposable
    {
        private const string accept = "Accept";
        private const string type_json = "application/json";
        private const string type_jpeg = "image/jpeg";

        private static readonly AssemblyName AssemblyName = new AssemblyName(
            typeof(SimpleServiceClient).GetTypeInfo().Assembly.FullName);

        private static readonly ProductInfoHeaderValue UserAgent = new ProductInfoHeaderValue(
            AssemblyName.Name, AssemblyName.Version.ToString());

        private readonly JsonSerializer _jsonSerializer = new JsonSerializer();
        private static readonly TimeSpan _defaultTimeout = TimeSpan.FromSeconds(30);

        private readonly Lazy<HttpClient> _httpClient = new Lazy<HttpClient>(
            () => CreateClient(_defaultTimeout), LazyThreadSafetyMode.PublicationOnly);

        #region Public Properties
        public TimeSpan Timeout
        {
            get { return _httpClient.Value.Timeout; }
            set { _httpClient.Value.Timeout = value; }
        }
        #endregion Public Properties

        #region Private Methods
        /// <summary>
        /// Create Client
        /// </summary>
        /// <param name="timeout">TimeSpan</param>
        /// <param name="extraHeaders">Extra HTTP Headers</param>
        /// <returns></returns>
        private static HttpClient CreateClient(
            TimeSpan timeout,
            IEnumerable<KeyValuePair<string, string>> extraHeaders = null)
        {
            HttpClient client = new HttpClient(new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip
            });
            client.DefaultRequestHeaders.Add(accept, type_json);
            client.DefaultRequestHeaders.UserAgent.Add(UserAgent);
            client.Timeout = timeout;
            if (extraHeaders != null)
            {
                foreach (var header in extraHeaders)
                {
                    client.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
            }
            return client;
        }

        /// <summary>
        /// Get Json Content
        /// </summary>
        /// <typeparam name="TRequest">Request Type</typeparam>
        /// <param name="requestPayload">Request Payload</param>
        /// <returns>HttpContent</returns>
        private HttpContent GetJsonContent<TRequest>(
            TRequest requestPayload)
        {
            if (requestPayload == null) return null;
            return new StringContent(
                JsonConvert.SerializeObject(requestPayload,
                Formatting.None,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                }), 
                Encoding.UTF8, type_json);
        }

        /// <summary>
        /// Get Json Content
        /// </summary>
        /// <param name="requestBody">Dictionary of String, String</param>
        /// <returns>HttpContent</returns>
        private HttpContent GetJsonContent(
            Dictionary<string, string> requestBody)
        {
            if (requestBody == null) return null;
            return new StringContent(
                JsonConvert.SerializeObject(
                    requestBody.ToDictionary(
                    item => item.Key.ToString(), 
                    item => item.Value.ToString())), 
                Encoding.UTF8, type_json);
        }

        /// <summary>
        /// Get Jpeg Content
        /// </summary>
        /// <param name="fileBytes">File Bytes</param>
        /// <returns>Http Content</returns>
        private HttpContent GetJpegContent(
            byte[] fileBytes)
        {
            return new StringContent(Convert.ToBase64String(fileBytes), 
                Encoding.UTF8, type_jpeg);
        }

        /// <summary>
        /// Create Http Request
        /// </summary>
        /// <param name="method">Http Method e.g. GET, POST</param>
        /// <param name="uri">Uri</param>
        /// <param name="content">Http Content</param>
        /// <param name="extraHeaders">Extra Http Headers</param>
        /// <returns>HttpRequestMessage</returns>
        private HttpRequestMessage CreateHttpRequest(
            HttpMethod method,
            Uri uri,
            HttpContent content,
            Dictionary<string, string> extraHeaders)
        {
            HttpRequestMessage message = new HttpRequestMessage(method, uri);
            if (content != null)
            {
                message.Content = content;
            }
            if (extraHeaders != null)
            {
                foreach (var header in extraHeaders)
                {
                    message.Headers.Add(header.Key, header.Value);
                }
            }
            return message;
        }

        /// <summary>
        /// Parse Response
        /// </summary>
        /// <typeparam name="TResult">Result</typeparam>
        /// <typeparam name="TErrorResult">Error Result</typeparam>
        /// <param name="message">HttpResponseMessage</param>
        /// <returns>SimpleServiceResponse of Result, Error Result</returns>
        private async Task<SimpleServiceResult<TResult, TErrorResult>>
            ParseResponseAsync<TResult, TErrorResult>(
            HttpResponseMessage message)
            where TResult : class
            where TErrorResult : class
        {
            if (message.Content == null)
            {
                return new SimpleServiceResult<TResult, TErrorResult>
                {
                    HttpStatusCode = message.StatusCode
                };
            }
            var test = await message.Content.ReadAsStringAsync();
            using (Stream stream = await message.Content.ReadAsStreamAsync())
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    return new SimpleServiceResult<TResult, TErrorResult>
                    {
                        Result = message.IsSuccessStatusCode
                            ? _jsonSerializer.Deserialize(reader, typeof(TResult)) as TResult
                            : null,
                        ErrorResult = message.IsSuccessStatusCode
                            ? null
                            : _jsonSerializer.Deserialize(reader, typeof(TErrorResult)) as TErrorResult,
                        HttpStatusCode = message.StatusCode
                    };
                }
            }
        }

        /// <summary>
        /// Set Client Headers
        /// </summary>
        /// <param name="headers">Http Headers</param>
        private void SetClientHeaders(
            Dictionary<string, string> headers)
        {
            if (headers != null)
            {
                _httpClient.Value.DefaultRequestHeaders.Clear();
                foreach (KeyValuePair<string, string> header in headers)
                {
                    _httpClient.Value.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
            }
        }

        /// <summary>
        /// Clear Client Headers
        /// </summary>
        private void ClearClientHeaders()
        {
            _httpClient.Value.DefaultRequestHeaders.Clear();
        }

        /// <summary>
        /// Get Content
        /// </summary>
        /// <typeparam name="TRequest">Request Type</typeparam>
        /// <param name="request">Request of Type</param>
        /// <param name="requestBody">Request Body</param>
        /// <returns></returns>
        private HttpContent GetContent<TRequest>(TRequest request,
            Dictionary<string, string> requestBody = null)
        {
            return requestBody == null ?
                request == null ?
                null :
                GetJsonContent(request) :
                GetJsonContent(requestBody);
        }

        /// <summary>
        /// HTTP Request
        /// </summary>
        /// <typeparam name="TResult">The result data contract type</typeparam>
        /// <typeparam name="TErrorResult">The error result data contract type</typeparam>
        /// <param name="hostname">The HTTP host</param>
        /// <param name="relativeUri">A relative URL to append at the end of the HTTP host</param>
        /// <param name="method">HTTP Method</param>
        /// <param name="content">HTTP Content</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <param name="requestParameters">Optional query string parameters</param>
        /// <param name="extraHeaders">Optional HTTP headers</param>
        /// <returns>SimpleServiceResult</returns>
        private async Task<SimpleServiceResult<TResult, TErrorResult>>
            RequestAsync<TResult, TErrorResult>(
            Uri hostname,
            string relativeUri,
            HttpMethod method,
            HttpContent content,
            CancellationToken cancellationToken,
            Dictionary<string, string> requestParameters = null,
            Dictionary<string, string> extraHeaders = null)
            where TResult : class
            where TErrorResult : class
        {
            Uri uri = GetUri(hostname, relativeUri, requestParameters);
            SetClientHeaders(extraHeaders);
            using (HttpRequestMessage requestMessage = CreateHttpRequest(
                method, uri, content, extraHeaders))
            using (HttpResponseMessage httpResponseMessage =
                await _httpClient.Value.SendAsync(requestMessage, cancellationToken))
            {
                ClearClientHeaders();
                return await ParseResponseAsync<TResult, TErrorResult>(httpResponseMessage);
            }
        }
        #endregion Private Methods

        #region Public Methods
        /// <summary>
        /// GetUri
        /// </summary>
        /// <param name="hostname">The HTTP host</param>
        /// <param name="relativeUri">A relative URL to append at the end of the HTTP host</param>
        /// <param name="requestParameters">Optional query string parameters</param>
        /// <returns>Uri</returns>
        public Uri GetUri(
            Uri hostname,
            string relativeUri,
            Dictionary<string, string> requestParameters = null)
        {
            string relUri = 
                requestParameters == null ? 
                relativeUri : 
                requestParameters.Aggregate(relativeUri,
                (current, param) => current + 
                (current.Contains("?") ? "&" : "?") + param.Key + "=" + param.Value);
            return new Uri(hostname, relUri);
        }

        /// <summary>
        /// Dispose Object
        /// </summary>
        public virtual void Dispose()
        {
            if (_httpClient.IsValueCreated)
            {
                _httpClient.Value.Dispose();
            }
        }
        #endregion Public Methods

        #region HTTP GET
        /// <summary>
        /// GET Request
        /// </summary>
        /// <typeparam name="TResponse">Response Type</typeparam>
        /// <param name="hostname">The HTTP host</param>
        /// <param name="relativeUri">A relative URL to append at the end of the HTTP host</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <param name="parameters">Optional query string parameters</param>
        /// <param name="headers">Optional HTTP headers</param>
        /// <returns>Response & Status Code</returns>
        /// <returns></returns>
        public async Task<TResponse> GetRequestAsync<TResponse>(
            Uri hostname,
            string relativeUri,
            CancellationToken cancellationToken,
            Dictionary<string, string> parameters = null,
            Dictionary<string, string> headers = null)
            where TResponse : class
        {
            var result =
            await GetRequestAsync<TResponse, TResponse, TResponse>(
            hostname,
            relativeUri,
            null,
            cancellationToken,
            parameters,
            headers);
            return result.Result ?? result.ErrorResult;
        }

        /// <summary>
        /// GET Request
        /// </summary>
        /// <typeparam name="TResponse">Response Type</typeparam>
        /// <typeparam name="TRequest">Request Type</typeparam>
        /// <param name="hostname">The HTTP host</param>
        /// <param name="relativeUri">A relative URL to append at the end of the HTTP host</param>
        /// <param name="request">Optional Request</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <param name="parameters">Optional query string parameters</param>
        /// <param name="headers">Optional HTTP headers</param>
        /// <returns>Response & Status Code</returns>
        /// <returns></returns>
        public async Task<TResponse> GetRequestAsync<TRequest, TResponse>(
            Uri hostname,
            string relativeUri,
            TRequest request,
            CancellationToken cancellationToken,
            Dictionary<string, string> parameters = null,
            Dictionary<string, string> headers = null)
            where TRequest : class
            where TResponse : class
        {
            var result =
            await GetRequestAsync<TRequest, TResponse, TResponse>(
            hostname,
            relativeUri,
            request,
            cancellationToken,
            parameters,
            headers);
            return result.Result ?? result.ErrorResult;
        }

        /// <summary>
        /// HTTP GET Request
        /// </summary>
        /// <typeparam name="TRequest">The request data type</typeparam>
        /// <typeparam name="TResult">The result data contract type</typeparam>
        /// <typeparam name="TErrorResult">The error result data contract type</typeparam>
        /// <param name="hostname">The HTTP host</param>
        /// <param name="relativeUri">A relative URL to append at the end of the HTTP host</param>
        /// <param name="request">Optional Request Object</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <param name="requestParameters">Optional query string parameters</param>
        /// <param name="extraHeaders">Optional HTTP headers</param>
        /// <returns>SimpleServiceResult</returns>
        public Task<SimpleServiceResult<TResult, TErrorResult>>
            GetRequestAsync<TRequest, TResult, TErrorResult>(
            Uri hostname,
            string relativeUri,
            TRequest request,
            CancellationToken cancellationToken,
            Dictionary<string, string> requestParameters = null,
            Dictionary<string, string> extraHeaders = null)
            where TRequest : class
            where TResult : class
            where TErrorResult : class =>
            RequestAsync<TResult, TErrorResult>(
                hostname,
                relativeUri,
                HttpMethod.Get,
                GetJsonContent(request),
                cancellationToken,
                requestParameters,
                extraHeaders);
        #endregion HTTP GET

        #region HTTP POST
        /// <summary>
        /// POST Request
        /// </summary>
        /// <typeparam name="TResponse">Response Type</typeparam>
        /// <typeparam name="TRequest">Request Type</typeparam>
        /// <param name="hostname">The HTTP host</param>
        /// <param name="relativeUri">A relative URL to append at the end of the HTTP host</param>
        /// <param name="request">Optional Request</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <param name="body">Optional Body Parameters</param>
        /// <param name="parameters">Optional query string parameters</param>
        /// <param name="headers">Optional HTTP headers</param>
        /// <param name="useFormContent">Use Form Content</param>
        /// <returns>Response & Status Code</returns>
        /// <returns></returns>
        public async Task<TResponse> PostRequestAsync<TRequest, TResponse>(
            Uri hostname,
            string relativeUri,
            TRequest request,
            CancellationToken cancellationToken,
            Dictionary<string, string> body = null,
            Dictionary<string, string> parameters = null,
            Dictionary<string, string> headers = null,
            bool useFormContent = false)
            where TRequest : class
            where TResponse : class
        {
            var result =
            await PostRequestAsync<TRequest, TResponse, TResponse>(
            hostname,
            relativeUri,
            request,
            cancellationToken,
            body,
            parameters,
            headers,
            useFormContent);
            return result.Result ?? result.ErrorResult;
        }

        /// <summary>
        /// POST Request
        /// </summary>
        /// <typeparam name="TResponse">Response Type</typeparam>
        /// <typeparam name="TRequest">Request Type</typeparam>
        /// <param name="hostname">The HTTP host</param>
        /// <param name="relativeUri">A relative URL to append at the end of the HTTP host</param>
        /// <param name="request">Optional Request</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <param name="body">Optional Body Parameters</param>
        /// <param name="parameters">Optional query string parameters</param>
        /// <param name="headers">Optional HTTP headers</param>
        /// <returns>Response & Status Code</returns>
        /// <returns></returns>
        public async Task<(TResponse Response, HttpStatusCode StatusCode)>
            PostRequestAsync<TRequest, TResponse>(
            Uri hostname,
            string relativeUri,
            TRequest request,
            CancellationToken cancellationToken,
            Dictionary<string, string> body = null,
            Dictionary<string, string> parameters = null,
            Dictionary<string, string> headers = null)
            where TRequest : class
            where TResponse : class
        {
            var result =
            await PostRequestAsync<TRequest, TResponse, TResponse>(
            hostname,
            relativeUri,
            request,
            cancellationToken,
            body,
            parameters,
            headers);
            return (result.Result ?? result.ErrorResult, 
                result.HttpStatusCode);
        }

        /// <summary>
        /// HTTP POST Request
        /// </summary>
        /// <typeparam name="TRequest">The request data type</typeparam>
        /// <typeparam name="TResult">The result data contract type</typeparam>
        /// <typeparam name="TErrorResult">The error result data contract type</typeparam>
        /// <param name="hostname">The HTTP host</param>
        /// <param name="relativeUri">A relative URL to append at the end of the HTTP host</param>
        /// <param name="request">Request</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <param name="requestBody">Optional Request Body</param>
        /// <param name="requestParameters">Optional query string parameters</param>
        /// <param name="extraHeaders">Optional HTTP headers</param>
        /// <param name="useFormContent">Use Form Content</param>
        /// <returns>SimpleServiceResult</returns>
        public Task<SimpleServiceResult<TResult, TErrorResult>>
            PostRequestAsync<TRequest, TResult, TErrorResult>(
            Uri hostname,
            string relativeUri,
            TRequest request,
            CancellationToken cancellationToken,
            Dictionary<string, string> requestBody = null,
            Dictionary<string, string> requestParameters = null,
            Dictionary<string, string> extraHeaders = null,
            bool useFormContent = false)
            where TRequest : class
            where TResult : class
            where TErrorResult : class => 
            RequestAsync<TResult, TErrorResult>(
                hostname,
                relativeUri,
                HttpMethod.Post,
                useFormContent ?
                    new FormUrlEncodedContent(requestBody) :
                    GetContent(request, requestBody),
                cancellationToken,
                requestParameters,
                extraHeaders);
        #endregion HTTP POST

        #region HTTP DELETE
        /// <summary>
        /// DELETE Request
        /// </summary>
        /// <typeparam name="TResponse">Response Type</typeparam>
        /// <param name="hostname">The HTTP host</param>
        /// <param name="relativeUri">A relative URL to append at the end of the HTTP host</param>
        /// <param name="request">Request Object</param>
        /// <param name="cancellationToken"></param>
        /// <param name="parameters">Optional query string parameters</param>
        /// <param name="headers">Optional HTTP headers</param>
        /// <returns>Response & Status Code</returns>
        public async Task<(TResponse Response, HttpStatusCode StatusCode)>
            DeleteRequestAsync<TRequest, TResponse>(
            Uri hostname,
            string relativeUri,
            TRequest request,
            CancellationToken cancellationToken,
            Dictionary<string, string> parameters = null,
            Dictionary<string, string> headers = null)
            where TRequest : class
            where TResponse : class
        {
            var result =
            await DeleteRequestAsync<TRequest, TResponse, TResponse>(
            hostname,
            relativeUri,
            request,
            cancellationToken,
            parameters,
            headers);
            return (result.Result ?? result.ErrorResult, 
                result.HttpStatusCode);
        }

        /// <summary>
        /// HTTP DELETE Request
        /// </summary>
        /// <typeparam name="TResult">The result data contract type</typeparam>
        /// <typeparam name="TErrorResult">The error result data contract type</typeparam>
        /// <param name="hostname">The HTTP host</param>
        /// <param name="relativeUri">A relative URL to append at the end of the HTTP host</param>
        /// <param name="request">Request Object</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <param name="requestParameters">Optional query string parameters</param>
        /// <param name="extraHeaders">Optional HTTP headers</param>
        /// <returns>SimpleServiceResult</returns>
        public Task<SimpleServiceResult<TResult, TErrorResult>>
            DeleteRequestAsync<TRequest, TResult, TErrorResult>(
            Uri hostname,
            string relativeUri,
            TRequest request,
            CancellationToken cancellationToken,
            Dictionary<string, string> requestParameters = null,
            Dictionary<string, string> extraHeaders = null)
            where TRequest : class
            where TResult : class
            where TErrorResult : class => 
                RequestAsync<TResult, TErrorResult>(
                hostname,
                relativeUri,
                HttpMethod.Delete,
                GetJsonContent(request),
                cancellationToken,
                requestParameters,
                extraHeaders);
        #endregion HTTP DELETE

        #region HTTP PUT
        /// <summary>
        /// PUT Request
        /// </summary>
        /// <typeparam name="TResponse">Response Type</typeparam>
        /// <param name="hostname">The HTTP host</param>
        /// <param name="relativeUri">A relative URL to append at the end of the HTTP host</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <param name="fileBytes">Optional Request File Bytes</param>
        /// <param name="parameters">Optional query string parameters</param>
        /// <param name="headers">Optional HTTP headers</param>
        /// <returns>Response & Status Code</returns>
        /// <returns></returns>
        public async Task<(TResponse Response, HttpStatusCode StatusCode)>
            PutRequestAsync<TResponse>(
            Uri hostname,
            string relativeUri,
            CancellationToken cancellationToken,
            byte[] fileBytes = null,
            Dictionary<string, string> parameters = null,
            Dictionary<string, string> headers = null)
            where TResponse : class
        {
            var result = await PutRequestAsync<TResponse, TResponse, TResponse>(
            hostname,
            relativeUri,
            null,
            cancellationToken,
            fileBytes,
            parameters,
            headers);
            return (result.Result ?? result.ErrorResult,
                result.HttpStatusCode);
        }

        /// <summary>
        /// PUT Request
        /// </summary>
        /// <typeparam name="TRequest">Request Type</typeparam>
        /// <typeparam name="TResponse">Response Type</typeparam>
        /// <param name="hostname">The HTTP host</param>
        /// <param name="relativeUri">A relative URL to append at the end of the HTTP host</param>
        /// <param name="request">Optional Request</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <param name="fileBytes">Optional Request File Bytes</param>
        /// <param name="parameters">Optional query string parameters</param>
        /// <param name="headers">Optional HTTP headers</param>
        /// <param name="fileBytes">File Bytes</param>
        /// <returns>Response & Status Code</returns>
        /// <returns></returns>
        public async Task<(TResponse Response, HttpStatusCode StatusCode)> 
            PutRequestAsync<TRequest, TResponse>(
            Uri hostname,
            string relativeUri,
            TRequest request,
            CancellationToken cancellationToken,
            byte[] fileBytes = null,
            Dictionary<string, string> parameters = null,
            Dictionary<string, string> headers = null)
            where TRequest : class
            where TResponse : class
        {
            var result = 
            await PutRequestAsync<TRequest, TResponse, TResponse>(
            hostname,
            relativeUri,
            request,
            cancellationToken,
            fileBytes,
            parameters,
            headers);
            return (result.Result ?? result.ErrorResult, 
                result.HttpStatusCode);
        }

        /// <summary>
        /// HTTP PUT Request
        /// </summary>
        /// <typeparam name="TResult">The result data contract type</typeparam>
        /// <typeparam name="TErrorResult">The error result data contract type</typeparam>
        /// <param name="hostname">The HTTP host</param>
        /// <param name="relativeUri">A relative URL to append at the end of the HTTP host</param>
        /// <param name="cancellationToken"></param>
        /// <param name="fileBytes">Optional Request File Bytes</param>
        /// <param name="requestParameters">Optional query string parameters</param>
        /// <param name="extraHeaders">Optional HTTP headers</param>
        /// <param name="fileBytes">File Bytes</param>
        /// <returns>SimpleServiceResult</returns>
        public Task<SimpleServiceResult<TResult, TErrorResult>>
            PutRequestAsync<TRequest, TResult, TErrorResult>(
            Uri hostname,
            string relativeUri,
            TRequest request,
            CancellationToken cancellationToken,
            byte[] fileBytes = null,
            Dictionary<string, string> requestParameters = null,
            Dictionary<string, string> extraHeaders = null)
            where TRequest : class
            where TResult : class
            where TErrorResult : class => 
                RequestAsync<TResult, TErrorResult>(
                hostname,
                relativeUri,
                HttpMethod.Put,
                fileBytes != null ?
                    GetJpegContent(fileBytes) :
                    GetJsonContent(request),
                cancellationToken,
                requestParameters,
                extraHeaders);
        #endregion HTTP PUT
    }
}
