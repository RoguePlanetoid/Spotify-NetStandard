using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Spotify.NetStandard.Client.Internal
{
    internal class SimpleServiceClient : IDisposable
    {
        private const string accept = "Accept";
        private const string accept_header = "application/json";

        private static readonly AssemblyName AssemblyName = new AssemblyName(
            typeof(SimpleServiceClient).GetTypeInfo().Assembly.FullName);

        private static readonly ProductInfoHeaderValue UserAgent = new ProductInfoHeaderValue(
            AssemblyName.Name, AssemblyName.Version.ToString());

        private readonly JsonSerializer _jsonSerializer = new JsonSerializer();
        private static readonly TimeSpan _defaultTimeout = TimeSpan.FromSeconds(30);

        private readonly Lazy<HttpClient> _httpClient = new Lazy<HttpClient>(
            () => CreateClient(_defaultTimeout), LazyThreadSafetyMode.PublicationOnly);

        public TimeSpan Timeout
        {
            get { return _httpClient.Value.Timeout; }
            set { _httpClient.Value.Timeout = value; }
        }

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
            client.DefaultRequestHeaders.Add(accept, accept_header);
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
        /// Create Http Content
        /// </summary>
        /// <typeparam name="TRequest">Request Type</typeparam>
        /// <param name="requestPayload">Request Payload</param>
        /// <param name="writer">Stream Writer</param>
        /// <param name="stream">Memory Stream</param>
        /// <returns>HttpContent</returns>
        private HttpContent CreateHttpContent<TRequest>(
            TRequest requestPayload, 
            StreamWriter writer, 
            MemoryStream stream)
        {
            _jsonSerializer.Serialize(writer, requestPayload, typeof(TRequest));
            writer.Flush();
            stream.Seek(0, SeekOrigin.Begin);
            HttpContent content = new StreamContent(stream);
            content.Headers.Add("Content-Type", "application/json");
            return content;
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
            _httpClient.Value.DefaultRequestHeaders.Clear();
            foreach(KeyValuePair<string, string> header in headers)
            {
                _httpClient.Value.DefaultRequestHeaders.Add(header.Key, header.Value);
            }
        }

        /// <summary>
        /// Clear Client Headers
        /// </summary>
        private void ClearClientHeaders()
        {
            _httpClient.Value.DefaultRequestHeaders.Clear();
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
            string relUri = requestParameters == null ? relativeUri : requestParameters.Aggregate(relativeUri,
                (current, param) => current + (current.Contains("?") ? "&" : "?") + param.Key + "=" + param.Value);
            return new Uri(hostname, relUri);
        }

        /// <summary>
        /// Issue an HTTP GET request
        /// </summary>
        /// <typeparam name="TResult">The result data contract type</typeparam>
        /// <typeparam name="TErrorResult">The error result data contract type</typeparam>
        /// <param name="hostname">The HTTP host</param>
        /// <param name="relativeUri">A relative URL to append at the end of the HTTP host</param>
        /// <param name="cancellationToken"></param>
        /// <param name="requestParameters">Optional query string parameters</param>
        /// <param name="extraHeaders">Optional HTTP headers</param>
        public async Task<SimpleServiceResult<TResult, TErrorResult>> GetAsync<TResult, TErrorResult>(
            Uri hostname, 
            string relativeUri,
            CancellationToken cancellationToken,
            Dictionary<string, string> requestParameters = null,
            Dictionary<string, string> extraHeaders = null)
            where TResult : class
            where TErrorResult : class
        {
            Uri uri = GetUri(hostname, relativeUri, requestParameters);
            using (HttpRequestMessage httpRequestMessage = CreateHttpRequest(HttpMethod.Get, uri, null, extraHeaders))
            using (HttpResponseMessage httpResponseMessage = await _httpClient.Value.SendAsync(httpRequestMessage, cancellationToken))
            {
                return await ParseResponseAsync<TResult, TErrorResult>(httpResponseMessage);
            }
        }

        /// <summary>
        /// Issue an HTTP GET request
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="hostname"></param>
        /// <param name="relativeUri"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="requestParameters"></param>
        /// <param name="extraHeaders"></param>
        /// <returns></returns>
        public async Task<TResult> GetAsync<TResult>(
            Uri hostname, 
            string relativeUri,
            CancellationToken cancellationToken,
            Dictionary<string, string> requestParameters = null,
            Dictionary<string, string> extraHeaders = null)
            where TResult : class
        {
            SimpleServiceResult<TResult, TResult> result = await GetAsync<TResult, TResult>(
                hostname, relativeUri, cancellationToken, requestParameters, extraHeaders);
            return result.Result ?? result.ErrorResult;
        }

        /// <summary>
        /// Issue HTTP POST request
        /// </summary>
        /// <typeparam name="TResult">The result data contract type</typeparam>
        /// <typeparam name="TErrorResult">The error result data contract type</typeparam>
        /// <typeparam name="TRequest">The request data contract type</typeparam>
        /// <param name="hostname">The HTTP host</param>
        /// <param name="relativeUri">A relative URL to append at the end of the HTTP host</param>
        /// <param name="requestPayload"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="requestParameters">Optional query string parameters</param>
        /// <param name="extraHeaders">Optional HTTP headers</param>
        public async Task<SimpleServiceResult<TResult, TErrorResult>> PostAsync<TResult, TErrorResult, TRequest>(
            Uri hostname, 
            string relativeUri,
            TRequest requestPayload,
            CancellationToken cancellationToken,
            Dictionary<string, string> bodyParameters = null,
            Dictionary<string, string> requestParameters = null,
            Dictionary<string, string> extraHeaders = null)
            where TResult : class
            where TErrorResult : class
        {
            Uri uri = GetUri(hostname, relativeUri, requestParameters);
            using (MemoryStream stream = new MemoryStream())
            using (StreamWriter writer = new StreamWriter(stream))
            using (HttpContent content = bodyParameters == null ? CreateHttpContent(requestPayload, writer, stream) : new FormUrlEncodedContent(bodyParameters))
            using (HttpRequestMessage requestMessage = CreateHttpRequest(HttpMethod.Post, uri, content, extraHeaders))
            using (HttpResponseMessage result = await _httpClient.Value.SendAsync(requestMessage, cancellationToken))
            {
                return await ParseResponseAsync<TResult, TErrorResult>(result);
            }
        }

        /// <summary>
        /// PostAsync
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <typeparam name="TRequest"></typeparam>
        /// <param name="hostname"></param>
        /// <param name="relativeUri"></param>
        /// <param name="requestPayload"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="requestParameters"></param>
        /// <param name="extraHeaders"></param>
        /// <returns></returns>
        public async Task<TResult> PostAsync<TResult, TRequest>(
            Uri hostname, string relativeUri,
            TRequest requestPayload,
            CancellationToken cancellationToken,
            Dictionary<string, string> bodyParameters = null,
            Dictionary<string, string> requestParameters = null,
            Dictionary<string, string> extraHeaders = null)
            where TResult : class
        {
            SimpleServiceResult<TResult, TResult> result = await PostAsync<TResult, TResult, TRequest>(
                hostname, relativeUri, requestPayload, cancellationToken, bodyParameters, requestParameters, extraHeaders);
            return result.Result ?? result.ErrorResult;
        }

        /// <summary>
        /// Put Async
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <typeparam name="TErrorResult"></typeparam>
        /// <typeparam name="TRequest"></typeparam>
        /// <param name="hostname"></param>
        /// <param name="relativeUri"></param>
        /// <param name="requestPayload"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="bodyParameters"></param>
        /// <param name="requestParameters"></param>
        /// <param name="extraHeaders"></param>
        /// <returns></returns>
        public async Task<SimpleServiceResult<TResult, TErrorResult>> PutAsync<TResult, TErrorResult, TRequest>(
            Uri hostname, string relativeUri,
            TRequest requestPayload,
            CancellationToken cancellationToken,
            Dictionary<string, string> bodyParameters = null,
            Dictionary<string, string> requestParameters = null,
            Dictionary<string, string> extraHeaders = null)
            where TResult : class
            where TErrorResult : class
        {
            Uri uri = GetUri(hostname, relativeUri, requestParameters);
            SetClientHeaders(extraHeaders);
            using (MemoryStream stream = new MemoryStream())
            using (StreamWriter writer = new StreamWriter(stream))
            using (HttpContent content = bodyParameters == null ? CreateHttpContent(requestPayload, writer, stream) : new FormUrlEncodedContent(bodyParameters))
            using (HttpResponseMessage result = await _httpClient.Value.PutAsync(uri, content, cancellationToken))
            {
                ClearClientHeaders();
                return await ParseResponseAsync<TResult, TErrorResult>(result);
            }
        }

        /// <summary>
        /// Issue an HTTP PUT request
        /// </summary>
        /// <typeparam name="TResult">The result data contract type</typeparam>
        /// <typeparam name="TErrorResult">The error result data contract type</typeparam>
        /// <param name="hostname">The HTTP host</param>
        /// <param name="relativeUri">A relative URL to append at the end of the HTTP host</param>
        /// <param name="cancellationToken"></param>
        /// <param name="requestParameters">Optional query string parameters</param>
        /// <param name="extraHeaders">Optional HTTP headers</param>
        public async Task<SimpleServiceResult<TResult, TErrorResult>> PutAsync<TResult, TErrorResult>(
            Uri hostname, string relativeUri,
            CancellationToken cancellationToken,
            Dictionary<string, string> requestParameters = null,
            Dictionary<string, string> extraHeaders = null)
            where TResult : class
            where TErrorResult : class
        {
            Uri uri = GetUri(hostname, relativeUri, requestParameters);
            SetClientHeaders(extraHeaders);
            using (HttpResponseMessage httpResponseMessage = await _httpClient.Value.PutAsync(uri, null, cancellationToken))
            {
                ClearClientHeaders();
                return await ParseResponseAsync<TResult, TErrorResult>(httpResponseMessage);
            }
        }

        /// <summary>
        /// Issue HTTP PUT Request
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <typeparam name="TRequest"></typeparam>
        /// <param name="hostname"></param>
        /// <param name="relativeUri"></param>
        /// <param name="requestPayload"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="requestParameters"></param>
        /// <param name="extraHeaders"></param>
        /// <returns></returns>
        public async Task<TResult> PutAsync<TResult, TRequest>(
            Uri hostname, string relativeUri,
            TRequest requestPayload,
            CancellationToken cancellationToken,
            Dictionary<string, string> bodyParameters = null,
            Dictionary<string, string> requestParameters = null,
            Dictionary<string, string> extraHeaders = null)
            where TResult : class
        {
            SimpleServiceResult<TResult, TResult> result = await PutAsync<TResult, TResult, TRequest>(
                hostname, relativeUri, requestPayload, cancellationToken, bodyParameters, requestParameters, extraHeaders);
            return result.Result ?? result.ErrorResult;
        }

        /// <summary>
        /// Issue a HTTP PUT request
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="hostname"></param>
        /// <param name="relativeUri"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="requestParameters"></param>
        /// <param name="extraHeaders"></param>
        /// <returns></returns>
        public async Task<TResult> PutAsync<TResult>(
            Uri hostname, string relativeUri,
            CancellationToken cancellationToken,
            Dictionary<string, string> requestParameters = null,
            Dictionary<string, string> extraHeaders = null)
            where TResult : class
        {
            SimpleServiceResult<TResult, TResult> result = await PutAsync<TResult, TResult>(
                hostname, relativeUri, cancellationToken, requestParameters, extraHeaders);
            TResult value = result.Result ?? result.ErrorResult;
            return value;
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
    }
}
