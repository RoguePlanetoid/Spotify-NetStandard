﻿using Spotify.NetStandard.Client.Authentication;
using Spotify.NetStandard.Client.Authentication.Enums;
using Spotify.NetStandard.Client.Authentication.Internal;
using Spotify.NetStandard.Client.Interfaces;
using Spotify.NetStandard.Enums;
using Spotify.NetStandard.Requests;
using Spotify.NetStandard.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Spotify.NetStandard.Client.Internal
{
    /// <summary>
    /// Spotify Client
    /// </summary>
    internal class SpotifyClient : SimpleServiceClient, ISpotifyClient
    {
        #region Private Members
        private static readonly Uri _hostName;
        private static readonly string _clientVersion;

        private static ISpotifyApi _api;
        private readonly AuthenticationCache _authenticationCache;
        #endregion Private Members

        #region Private Methods
        /// <summary>
        /// Format Request Headers
        /// </summary>
        /// <param name="tokenType">Authentication Token Type</param>
        /// <returns>Dictionary of Headers</returns>
        private async Task<Dictionary<string, string>> FormatRequestHeadersAsync(
            TokenType tokenType = TokenType.Access)
        {
            Dictionary<string, string> headersToSend = 
                new Dictionary<string, string>();
            AccessToken access = await 
                _authenticationCache.CheckAndRenewTokenAsync(
                tokenType, new CancellationToken(false));
            headersToSend.Add("Authorization", "Bearer " + access.Token);
            return headersToSend;
        }

        /// <summary>
        /// Format Request Parameters
        /// </summary>
        /// <param name="limit">Limit</param>
        /// <param name="offset">Offset</param>
        /// <param name="country">Country</param>
        /// <param name="locale">Locale</param>
        /// <returns>Dictionary of Request Parameters</returns>
        private Dictionary<string, string> FormatRequestParameters(
            int? limit = null,
            int? offset = null,
            string country = null,
            string locale = null)
        {
            Dictionary<string, string> parameters = 
                new Dictionary<string, string>();

            if (limit != null)
                parameters.Add("limit", limit.Value.ToString());

            if (offset != null)
                parameters.Add("offset", offset.Value.ToString());

            if (!string.IsNullOrEmpty(country))
                parameters.Add("country", country);

            if (!string.IsNullOrEmpty(locale))
                parameters.Add("locale", locale);

            return parameters;
        }

        /// <summary>
        /// Format Ids Parameter
        /// </summary>
        /// <param name="itemIds">IDs of the Items</param>
        /// <returns>Ids as String</returns>
        private string FormatIdsParameter(List<string> itemIds)
        {
            return itemIds.Aggregate(string.Empty,
                (current, id) => current + 
                (!string.IsNullOrEmpty(current) ? "," : string.Empty) + id);
        }
        
        /// <summary>
        /// Format Cursor Parameters
        /// </summary>
        /// <param name="limit">Limit</param>
        /// <param name="after">After</param>
        /// <returns>Dictionary of Request Parameters</returns>
        private Dictionary<string, string> FormatCursorParameters(
            int? limit = null,
            string after = null)
        {
            Dictionary<string, string> parameters =
                            new Dictionary<string, string>();

            if (limit != null)
                parameters.Add("limit", limit.Value.ToString());

            if (after != null)
                parameters.Add("after", after);

            return parameters;
        }

        /// <summary>
        /// Get Status
        /// </summary>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="response"></param>
        /// <param name="code"></param>
        /// <param name="successCode"></param>
        private TResponse GetStatus<TResponse>(
            TResponse response, 
            int code, 
            int successCode)
            where TResponse : Status
        {
            bool success = code == successCode;
            if (response == null)
                response = (TResponse)new Status();
            response.Code = code;
            response.Success = success;
            return response;
        }

        /// <summary>
        /// Browse
        /// </summary>
        /// <param name="browseCategory">Category</param>
        /// <param name="country">Country</param>
        /// <param name="locale">Locale</param>
        /// <returns>Content Response</returns>
        private async Task<ContentResponse> GetBrowseAsync(
            string browseCategory,
            string country,
            string locale = null,
            Dictionary<string, string> parameters = null,
            Page page = null)
        {
            Dictionary<string, string> requestHeaders = 
                await FormatRequestHeadersAsync();
            Dictionary<string, string> requestParameters = 
                FormatRequestParameters(
                country: country, locale: locale,
                offset: page?.Offset, limit: page?.Limit);
            if (parameters != null)
            {
                foreach (KeyValuePair<string, string> item in parameters)
                    requestParameters.Add(item.Key, item.Value);
            }
            return await GetRequestAsync<ContentResponse>(_hostName, 
                $"/v1/browse/{browseCategory}",
                new CancellationToken(false), 
                requestParameters, requestHeaders);
        }

        /// <summary>
        /// Lookup API
        /// </summary>
        /// <typeparam name="TResult">Result Type</typeparam>
        /// <param name="itemId">Spotify ID of the Item</param>
        /// <param name="lookupType">Lookup Type</param>
        /// <param name="country">Country</param>
        /// <param name="key">Parameter Key</param>
        /// <param name="value">Parameter Value</param>
        /// <param name="page">Page Offset & Limit</param>
        /// <returns>Response of Type</returns>
        private async Task<TResult> LookupApiAsync<TResult>(
            string itemId,
            string lookupType = null,
            string country = null,
            string key = null,
            string value = null,
            Page page = null) 
            where TResult : class
        {
            Dictionary<string, string> headers = 
                await FormatRequestHeadersAsync();
            Dictionary<string, string> parameters = 
                FormatRequestParameters(
                offset: page?.Offset, limit: page?.Limit, country: country);
            if (key != null && value != null)
            {
                parameters.Add(key, value);
            }
            string[] source = new string[] { lookupType };
            source = source[0].Contains("_") ?
                source[0].Split('_') :
                source;
            string relativeUri = (source.Length == 1) ?
                $"/v1/{source[0]}/{itemId}" :
                $"/v1/{source[0]}/{itemId}/{source[1]}";
            return await GetRequestAsync<TResult>(_hostName, relativeUri,
                new CancellationToken(false), 
                parameters, headers);
        }

        /// <summary>
        /// Lookup API
        /// </summary>
        /// <param name="itemIds">Spotify IDs of the Items</param>
        /// <param name="lookupType">Lookup Type</param>
        /// <param name="country">Country</param>
        /// <param name="page">Page Offset & Limit</param>
        /// <returns>Lookup Response</returns>
        private async Task<LookupResponse> LookupApiAsync(
            List<string> itemIds,
            string lookupType = null,
            string country = null,
            Page page = null)
        {
            string ids = FormatIdsParameter(itemIds);
            Dictionary<string, string> headers = 
                await FormatRequestHeadersAsync();
            Dictionary<string, string> parameters = 
                FormatRequestParameters(
                    offset: page?.Offset, limit: page?.Limit, country: country);
            parameters.Add("ids", ids);
            return await GetRequestAsync<LookupResponse>(
                _hostName, $"/v1/{lookupType}/",
                new CancellationToken(false), parameters, headers);
        }

        /// <summary>
        /// Lookup Cursor API
        /// </summary>
        /// <typeparam name="TResult">Result Type</typeparam>
        /// <param name="lookupType">Lookup Type</param>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        /// <param name="cusror">Cursor Limit & After</param>
        /// <param name="tokenType">Token Type</param>
        /// <returns>Response Object</returns>
        private async Task<TResult> LookupCursorApiAsync<TResult>(
            string lookupType = null,
            string key = null,
            string value = null,
            Cursor cursor = null,
            TokenType tokenType = TokenType.Access)
            where TResult : class
        {
            Dictionary<string, string> headers =
                await FormatRequestHeadersAsync(tokenType);
            Dictionary<string, string> parameters =
                FormatCursorParameters(
                limit: cursor?.Limit, after: cursor?.After);
            if (key != null && value != null)
            {
                parameters.Add(key, value);
            }
            string[] source = new string[] { lookupType };
            source = source[0].Contains("_") ?
                source[0].Split('_') :
                source;
            string relativeUri = (source.Length == 1) ?
                $"/v1/{source[0]}/" :
                $"/v1/{source[0]}/{source[1]}";
            return await GetRequestAsync<TResult>(_hostName, relativeUri,
                new CancellationToken(false),
                parameters, headers);
        }

        /// <summary>
        /// Get API
        /// </summary>
        /// <typeparam name="TResponse">Response Type</typeparam>
        /// <param name="itemId">Spotify ID of the Item</param>
        /// <param name="lookupType">Lookup Type</param>
        /// <param name="parameters">Request Parameters</param>
        /// <param name="tokenType">Token Type</param>
        /// <returns>Response Object</returns>
        private async Task<TResponse> GetApiAsync<TResponse>(
            string itemId = null,
            string lookupType = null,
            Dictionary<string, string> parameters = null,
            TokenType tokenType = TokenType.Access)
            where TResponse : class
        {
            Dictionary<string, string> headers =
                await FormatRequestHeadersAsync(tokenType);
            string[] source = new string[] { lookupType };
            source = source[0].Contains("_") ?
                source[0].Split('_') :
                source;
            string relativeUri = (itemId == null) ?
                $"/v1/{lookupType}" :
                $"/v1/{lookupType}/{itemId}";
            return await GetRequestAsync<TResponse>(_hostName, relativeUri,
            new CancellationToken(false), parameters, headers);
        }

        /// <summary>
        /// Get API
        /// </summary>
        /// <typeparam name="TResponse">Reponse Type</typeparam>
        /// <param name="itemIds">Spotify IDs of the Items</param>
        /// <param name="lookupType">Lookup Type</param>
        /// <param name="parameters">Request Parameters</param>
        /// <param name="tokenType">Token Type</param>
        /// <returns>Response Object</returns>
        private async Task<TResponse> GetApiAsync<TResponse>(
            List<string> itemIds,
            string lookupType,
            Dictionary<string, string> parameters = null,
            TokenType tokenType = TokenType.Access)
            where TResponse : class
        {
            string ids = FormatIdsParameter(itemIds);
            Dictionary<string, string> headers =
                await FormatRequestHeadersAsync(tokenType);
            string[] source = new string[] { lookupType };
            if (parameters == null)
                parameters = new Dictionary<string, string>();
            parameters.Add("ids", ids);
            source = source[0].Contains("_") ?
                source[0].Split('_') :
                source;
            return await GetRequestAsync<TResponse>(
                _hostName, $"/v1/{lookupType}",
                new CancellationToken(false),
                parameters, headers);
        }

        /// <summary>
        /// Post API
        /// </summary>
        /// <typeparam name="TRequest">Request Type</typeparam>
        /// <typeparam name="TResponse">Response Type</typeparam>
        /// <param name="itemIds">Spotify IDs of the Items</param>
        /// <param name="lookupType">Lookup Type</param>
        /// <param name="request">Request</param>
        /// <param name="body">Request Body</param>
        /// <param name="parameters">Request Parameters</param>
        /// <param name="tokenType">Token Type</param>
        /// <param name="successCode">Success Code</param>
        /// <returns>Response Object</returns>
        private async Task<TResponse> PostApiAsync<TRequest, TResponse>(
            List<string> itemIds,
            string lookupType,
            TRequest request,
            Dictionary<string, string> body,
            Dictionary<string, string> parameters,
            TokenType tokenType,
            int successCode) 
            where TRequest : class
            where TResponse : Status
        {
            Dictionary<string, string> headers =
                await FormatRequestHeadersAsync(tokenType);
            string[] source = new string[] { lookupType };
            if(itemIds != null)
                parameters.Add("ids", FormatIdsParameter(itemIds));
            source = source[0].Contains("_") ?
                source[0].Split('_') :
                source;
            string relativeUri = $"/v1/{lookupType}";
            var (response, statusCode) =
                await PostRequestAsync<TRequest, TResponse>(
                _hostName,
                relativeUri,
                request,
                new CancellationToken(false),
                body,
                parameters,
                headers);
            return GetStatus(response, (int)statusCode, successCode);
        }

        /// <summary>
        /// Post API
        /// </summary>
        /// <typeparam name="TRequest">Request Type</typeparam>
        /// <typeparam name="TResponse">Response Type</typeparam>
        /// <param name="itemIds">Spotify IDs of the Items</param>
        /// <param name="lookupType">Lookup Type</param>
        /// <param name="request">Request Object</param>
        /// <param name="body">Request Body</param>
        /// <param name="parameters">Request Parameters</param>
        /// <param name="tokenType">Token Type</param>
        /// <returns>Response Object</returns>
        private async Task<TResponse> PostApiAsync<TRequest, TResponse>(
            List<string> itemIds,
            string lookupType,
            TRequest request,
            Dictionary<string, string> body,
            Dictionary<string, string> parameters,
            TokenType tokenType)
            where TRequest : class
            where TResponse : class
        {
            Dictionary<string, string> headers =
                await FormatRequestHeadersAsync(tokenType);
            string[] source = new string[] { lookupType };
            if (itemIds != null)
                parameters.Add("ids", FormatIdsParameter(itemIds));
            source = source[0].Contains("_") ?
                source[0].Split('_') :
                source;
            string relativeUri = $"/v1/{lookupType}";
            var (response, statusCode) =
                await PostRequestAsync<TRequest, TResponse>(
                _hostName,
                relativeUri,
                request,
                new CancellationToken(false),
                body,
                parameters,
                headers);
            return response;
        }

        /// <summary>
        /// Put API
        /// </summary>
        /// <typeparam name="TRequest">Request Type</typeparam>
        /// <typeparam name="TResponse">Response Type</typeparam>
        /// <param name="tokenType">Auth Type</param>
        /// <param name="itemIds">Spotify IDs of the Items</param>
        /// <param name="lookupType">Lookup Type</param>
        /// <param name="request">Request Object</param>
        /// <param name="fileBytes">File Bytes</param>
        /// <param name="parameters">Request Parameters</param>
        /// <param name="successCode">Success Code</param>
        /// <returns>Response Object</returns>
        private async Task<TResponse> PutApiAsync<TRequest, TResponse>(
            List<string> itemIds, 
            string lookupType,
            TRequest request,
            byte[] fileBytes,
            Dictionary<string, string> parameters, 
            TokenType tokenType,
            int successCode)
            where TRequest : class
            where TResponse : Status
        {
            Dictionary<string, string> headers = 
                await FormatRequestHeadersAsync(tokenType);
            string[] source = new string[] { lookupType };
            if(itemIds != null)
                parameters.Add("ids", FormatIdsParameter(itemIds));
            source = source[0].Contains("_") ?
                source[0].Split('_') :
                source;
            string relativeUri = $"/v1/{lookupType}";
            var (response, statusCode) =
                await PutRequestAsync<TRequest, TResponse>(
                _hostName,
                relativeUri,
                request,
                new CancellationToken(false),
                fileBytes,
                parameters,
                headers);
            return GetStatus(response, (int)statusCode, successCode);
        }

        /// <summary>
        /// Delete API
        /// </summary>
        /// <typeparam name="TRequest">Request Type</typeparam>
        /// <typeparam name="TResponse">Response Type</typeparam>
        /// <param name="tokenType">Auth Type</param>
        /// <param name="itemIds">Spotify IDs of the Items</param>
        /// <param name="lookupType">Lookup Type</param>
        /// <param name="request">Request Object</param>
        /// <param name="parameters">Request Parameters</param>
        /// <param name="successCode">Success Code</param>
        /// <returns>Response Object</returns>
        private async Task<TResponse> DeleteApiAsync<TRequest, TResponse>(
            List<string> itemIds,
            string lookupType,
            TRequest request,
            Dictionary<string, string> parameters,
            TokenType tokenType,
            int successCode)
            where TRequest : class
            where TResponse : Status
        {
            Dictionary<string, string> headers =
                await FormatRequestHeadersAsync(tokenType);
            string[] source = new string[] { lookupType };
            if(itemIds != null)
                parameters.Add("ids", FormatIdsParameter(itemIds));
            source = source[0].Contains("_") ?
                source[0].Split('_') :
                source;
            string relativeUri = $"/v1/{lookupType}";
            var (response, statusCode) =
                await DeleteRequestAsync<TRequest, TResponse>(
                _hostName,
                relativeUri,
                request,
                new CancellationToken(false),
                parameters,
                headers);
            return GetStatus(response, (int)statusCode, successCode);
        }
        #endregion Private Methods

        #region Constructors
        /// <summary>
        /// Spotify Client
        /// </summary>
        static SpotifyClient()
        {
            _hostName = new Uri("https://api.spotify.com");
            Assembly assembly = typeof(SpotifyClient).GetTypeInfo().Assembly;
            AssemblyName assemblyName = new AssemblyName(assembly.FullName);
            _clientVersion = $"{assemblyName.Version.Major}.{assemblyName.Version.Minor}";
        }

        /// <summary>
        /// Spotify Client
        /// </summary>
        /// <param name="authenticationCache">Authentication Cache</param>
        internal SpotifyClient(
            AuthenticationCache authenticationCache)
        {
            _api = new SpotifyApi(this);
            _authenticationCache = authenticationCache;
        }
        #endregion Constructors

        #region Public Properties
        /// <summary>
        /// Spotify API
        /// </summary>
        public ISpotifyApi Api => _api;
        #endregion Public Properties

        #region Public Methods
        /// <summary>
        /// Get Access Token
        /// </summary>
        /// <returns>Access Token</returns>
        public AccessToken GetToken() => 
            _authenticationCache.AccessToken;

        /// <summary>
        /// Set Access Token
        /// </summary>
        /// <param name="value">Access Token</param>
        public void SetToken(AccessToken value) => 
            _authenticationCache.AccessToken = value;

        /// <summary>
        /// Navigate 
        /// </summary>
        /// <typeparam name="T">Response Type</typeparam>
        /// <param name="paging">Paging Object</param>
        /// <param name="navigateType">Navigate Type</param>
        /// <returns>Content Response</returns>
        public async Task<ContentResponse> NavigateAsync<T>(
            Paging<T> paging, 
            NavigateType navigateType)
        {
            Uri source = null;
            switch (navigateType)
            {
                case NavigateType.None:
                    source = new Uri(paging.Href);
                    break;
                case NavigateType.Previous:
                    if (paging.Previous != null)
                        source = new Uri(paging.Previous);
                    break;
                case NavigateType.Next:
                    if (paging.Next != null)
                        source = new Uri(paging.Next);
                    break;
            }
            if (source != null)
            {
                Dictionary<string, string> headers = 
                    await FormatRequestHeadersAsync();
                Dictionary<string, string> parameters = 
                    source.Query.QueryStringAsDictionary();
                return await GetRequestAsync<ContentResponse>(
                    new Uri($"{source.Scheme}://{source.Host}"),
                    source.AbsolutePath, 
                    new CancellationToken(false), 
                    parameters, headers);
            }
            return null;
        }

        /// <summary>
        /// Search
        /// </summary>
        /// <param name="query">(Required) Search Query</param>
        /// <param name="searchType">(Required) Search results include hits from all the specified item types.</param>
        /// <param name="country">(Optional) An ISO 3166-1 alpha-2 country code or the string from_token</param>
        /// <param name="external">(Optional) Include any relevant audio content that is hosted externally. </param>
        /// <param name="page">(Optional) Limit: The maximum number of items to return - Offset: The index of the first item to return</param>
        /// <returns>Content Response</returns>
        public async Task<ContentResponse> SearchAsync(
            string query,
            SearchType searchType,
            string country = null,
            bool? external = null,
            Page page = null)
        {
            Dictionary<string, string> headers = 
                await FormatRequestHeadersAsync();
            Dictionary<string, string> parameters = 
                FormatRequestParameters(
                    offset: page?.Offset, limit: page?.Limit, country: country);
            if (searchType != null)
            {
                parameters.Add("type", searchType.Get()?.AsDelimitedString());
            }
            if (!string.IsNullOrEmpty(query))
                parameters.Add("q", Uri.EscapeDataString(query));
            if (external == true)
                parameters.Add("include_external", "audio");
            return await GetRequestAsync<ContentResponse>(
                    _hostName, $"/v1/search/",
                    new CancellationToken(false), 
                    parameters, headers);
        }

        /// <summary>
        /// Lookup
        /// </summary>
        /// <typeparam name="T">Response Type</typeparam>
        /// <param name="itemId">(Required) The Spotify ID for the album.</param>
        /// <param name="lookupType">(Required) Item Type</param>
        /// <param name="market">(Optional) ISO 3166-1 alpha-2 country code or the string from_token</param>
        /// <param name="key">(Optional) Query Parameter Key</param>
        /// <param name="value">(Optional) Query Parameter Value</param>
        /// <param name="page">(Optional) Limit: The maximum number of items to return - Offset: The index of the first item to return</param>
        /// <returns>Lookup Response by Type</returns>
        public Task<T> LookupAsync<T>(
            string itemId,
            LookupType lookupType,
            string market = null,
            string key = null,
            string value = null,
            Page page = null)
            where T : class
        {
            return LookupApiAsync<T>(
                itemId, lookupType.GetDescription(), 
                market, key, value, page);
        }

        /// <summary>
        /// Lookup
        /// </summary>
        /// <param name="itemIds">(Required) List of Spotify ID for the items</param>
        /// <param name="lookupType">(Required) Lookup Item Type</param>
        /// <param name="market">(Optional) ISO 3166-1 alpha-2 country code or the string from_token</param>
        /// <param name="page">(Optional) Limit: The maximum number of items to return - Offset: The index of the first item to return</param>
        /// <returns>Lookup Response</returns>
        public Task<LookupResponse> LookupAsync(
            List<string> itemIds,
            LookupType lookupType,
            string market = null,
            Page page = null)
        {
            return LookupApiAsync(
                itemIds, lookupType.GetDescription(), 
                market, page);
        }

        /// <summary>
        /// Lookup Featured Playlists
        /// </summary>
        /// <param name="country">(Optional) A country: an ISO 3166-1 alpha-2 country code. </param>
        /// <param name="locale">(Optional) The desired language, consisting of a lowercase ISO 639-1 language code and an uppercase ISO 3166-1 alpha-2 country code, joined by an underscore</param>
        /// <param name="timestamp">(Optional) Use this parameter to specify the user’s local time to get results tailored for that specific date and time in the day.</param>
        /// <param name="page">(Optional) Limit: The maximum number of items to return. Default: 20. Minimum: 1. Maximum: 50. - Offset: The index of the first item to return. Default: 0</param>
        /// <returns>Content Response</returns>
        public Task<ContentResponse> LookupFeaturedPlaylistsAsync(
            string country = null,
            string locale = null,
            DateTime? timestamp = null,
            Page page = null)
        {
            Dictionary<string, string> parameters = 
                new Dictionary<string, string>();
            if (timestamp != null)
            {
                parameters.Add("timestamp", 
                    timestamp.Value.ToString("yyyy-MM-ddTHH:mm:ss"));
            }
            return GetBrowseAsync(
                "featured-playlists", country, locale, 
                parameters, page);
        }

        /// <summary>
        /// Lookup New Releases
        /// </summary>
        /// <param name="country">(Optional) A country: an ISO 3166-1 alpha-2 country code. </param>
        /// <param name="page">(Optional) Limit: The maximum number of items to return. Default: 20. Minimum: 1. Maximum: 50. - Offset: The index of the first item to return. Default: 0</param>
        /// <returns>Content Response</returns>
        public Task<ContentResponse> LookupNewReleasesAsync(
            string country = null,
            Page page = null)
        {
            return GetBrowseAsync(
                "new-releases", country: country, 
                page: page);
        }

        /// <summary>
        /// Lookup Artist's Albums
        /// </summary>
        /// <param name="id">(Required) The Spotify ID for the artist.</param>
        /// <param name="includeGroup">(Optional) Filters the response. If not supplied, all album types will be returned</param>
        /// <param name="market">(Optional) An ISO 3166-1 alpha-2 country code</param>
        /// <param name="page">(Optional) Limit: The number of album objects to return. Default: 20. Minimum: 1. Maximum: 50 - Offset: The index of the first album to return. Default: 0 (i.e., the first album).</param>
        /// <returns>Paging List of Album</returns>
        public Task<Paging<Album>> LookupArtistAlbumsAsync(
            string itemId,
            IncludeGroup includeGroup = null,
            string market = null,
            Page page = null)
        {
            string key = null;
            string value = null;
            if (includeGroup != null)
            {
                key = "include_groups";
                value = includeGroup.Get()?.AsDelimitedString();
            }
            return LookupAsync<Paging<Album>>(
                itemId, LookupType.ArtistAlbums, market, 
                key, value, page);
        }

        /// <summary>
        /// Lookup Artist's Top Tracks
        /// </summary>
        /// <param name="itemId">(Required) The Spotify ID for the artist.</param>
        /// <param name="market">(Required) A country: an ISO 3166-1 alpha-2 country code or the string from_token</param>
        /// <returns>Lookup Response</returns>
        public Task<LookupResponse> LookupArtistTopTracksAsync(
            string itemId, 
            string market)
        {
            return LookupApiAsync<LookupResponse>(
                itemId, "artists_top-tracks", 
                country: market);
        }

        /// <summary>
        /// Lookup Artist's Related Artists
        /// </summary>
        /// <param name="itemId">(Required) The Spotify ID for the artist.</param>
        /// <returns>Lookup Response</returns>
        public Task<LookupResponse> LookupArtistRelatedArtistsAsync(
            string itemId)
        {
            return LookupApiAsync<LookupResponse>(
                itemId, "artists_related-artists");
        }

        /// <summary>
        /// Lookup All Categories
        /// </summary>
        /// <param name="country">(Optional) A country: an ISO 3166-1 alpha-2 country code. </param>
        /// <param name="locale">(Optional) The desired language, consisting of a lowercase ISO 639-1 language code and an uppercase ISO 3166-1 alpha-2 country code, joined by an underscore</param>
        /// <param name="page">(Optional) Limit: The maximum number of categories to return. Default: 20. Minimum: 1. Maximum: 50. - Offset: The index of the first item to return. Default: 0</param>
        /// <returns>Content Response</returns>
        public Task<ContentResponse> LookupAllCategoriesAsync(
            string country = null,
            string locale = null,
            Page page = null)
        {
            return GetBrowseAsync(
                "categories", country: country, locale: locale, 
                page: page);
        }

        /// <summary>
        /// Lookup Category 
        /// </summary>
        /// <param name="categoryId">The Spotify category ID for the category.</param>
        /// <param name="country">(Optional) A country: an ISO 3166-1 alpha-2 country code. </param>
        /// <param name="locale">(Optional) The desired language, consisting of an ISO 639-1 language code and an ISO 3166-1 alpha-2 country code, joined by an underscore.</param>
        /// <returns>Category Object</returns>
        public Task<Category> LookupCategoryAsync(
            string categoryId,
            string country = null,
            string locale = null)
        {
            string key = null;
            string value = null;
            if (locale != null)
            {
                key = "locale";
                value = locale;
            }
            return LookupAsync<Category>(
                categoryId, LookupType.Categories, country,
                key, value);
        }

        /// <summary>
        /// Lookup Recommendations
        /// </summary>
        /// <param name="seedArtists">List of Spotify IDs for seed artists</param>
        /// <param name="seedGenres">List of any genres in the set of available genre seeds</param>
        /// <param name="seedTracks">List of Spotify IDs for a seed track</param>
        /// <param name="limit">The target size of the list of recommended tracks. Default: 20. Minimum: 1. Maximum: 100.</param>
        /// <param name="market">An ISO 3166-1 alpha-2 country code</param>
        /// <param name="minTuneableTrack">Multiple values. For each tunable track attribute, a hard floor on the selected track attribute’s value can be provided</param>
        /// <param name="maxTuneableTrack">Multiple values. For each tunable track attribute, a hard ceiling on the selected track attribute’s value can be provided.</param>
        /// <param name="targetTuneableTrack">Multiple values. For each of the tunable track attributes a target value may be provided.</param>
        /// <returns>Recommendation Response Object</returns>
        public Task<RecommendationsResponse> LookupRecommendationsAsync(
            List<string> seedArtists = null,
            List<string> seedGenres = null,
            List<string> seedTracks = null,
            int? limit = null,
            string market = null,
            TuneableTrack minTuneableTrack = null,
            TuneableTrack maxTuneableTrack = null,
            TuneableTrack targetTuneableTrack = null)
        {
            Dictionary<string, string> parameters = 
                new Dictionary<string, string>();

            if (seedArtists != null)
                parameters.Add("seed_artists", 
                    seedArtists.ToArray().AsDelimitedString());

            if (seedGenres != null)
                parameters.Add("seed_genres", 
                    seedGenres.ToArray().AsDelimitedString());

            if (seedTracks != null)
                parameters.Add("seed_tracks", 
                    seedTracks.ToArray().AsDelimitedString());

            if (limit != null)
                parameters.Add("limit", 
                    limit.Value.ToString());

            if (market != null)
                parameters.Add("market", market);

            if (minTuneableTrack != null)
                minTuneableTrack.SetParameter(parameters, "min");

            if (maxTuneableTrack != null)
                maxTuneableTrack.SetParameter(parameters, "max");

            if (targetTuneableTrack != null)
                targetTuneableTrack.SetParameter(parameters, "target");

            return GetApiAsync<RecommendationsResponse>(
                lookupType: "recommendations", parameters: parameters);
        }

        /// <summary>
        /// Lookup Recommendation Genres
        /// </summary>
        /// <returns>Available Genre Seeds Object</returns>
        public Task<AvailableGenreSeeds> LookupRecommendationGenres()
        {
            return GetApiAsync<AvailableGenreSeeds>(
                lookupType: "recommendations/available-genre-seeds");
        }
        #endregion Public Methods

        #region Authenticate
        /// <summary>
        /// Auth User
        /// </summary>
        /// <param name="redirectUri">Redirect Uri</param>
        /// <param name="state">State</param>
        /// <param name="scope">Scope</param>
        /// <returns>Uri</returns>
        public Uri AuthUser(
            Uri redirectUri,
            string state,
            Scope scope)
        {
            return _authenticationCache.GetAuth(
                redirectUri, state,
                scope.Get());
        }

        /// <summary>
        /// Auth User
        /// </summary>
        /// <param name="responseUri">Response Uri</param>
        /// <param name="redirectUri">Redirect Uri</param>
        /// <param name="state">State</param>
        /// <returns>AccessToken on Success, Null if Not</returns>
        /// <exception cref="AuthCodeValueException">AuthCodeValueException</exception>
        /// <exception cref="AuthCodeStateException">AuthCodeStateException</exception>
        public Task<AccessToken> AuthUserAsync(
            Uri responseUri,
            Uri redirectUri,
            string state)
        {
            return _authenticationCache.GetAuth(
                responseUri, redirectUri, state);
        }
        #endregion Authenticate

        #region Authenticated Follow API
        /// <summary>
        /// Get Following State for Artists/Users
        /// </summary>
        /// <param name="itemIds">(Required) List of the artist or the user Spotify IDs to check.</param>
        /// <param name="followType">Type: either artist or user.</param>
        /// <returns>List of true or false values</returns>
        /// <exception cref="AuthTokenRequiredException"></exception>
        public Task<List<bool>> AuthLookupFollowingStateAsync(
            List<string> itemIds,
            FollowType followType)
        {
            var parameters =
            new Dictionary<string, string>()
            {
                { "type", followType.GetDescription() }
            };
            return GetApiAsync<List<bool>>(itemIds, 
                "me/following/contains",
                parameters, TokenType.User);
        }

        /// <summary>
        /// Check if Users Follow a Playlist
        /// </summary>
        /// <param name="itemIds">(Required) List of Spotify User IDs ; the ids of the users that you want to check to see if they follow the playlist. Maximum: 5 ids.</param>
        /// <param name="playlistId">The Spotify ID of the playlist.</param>
        /// <returns>List of true or false values</returns>
        /// <exception cref="AuthTokenRequiredException"></exception>
        public Task<List<bool>> AuthLookupUserFollowingPlaylistAsync(
            List<string> itemIds,
            string playlistId)
        {
            return GetApiAsync<List<bool>>(itemIds, 
                $"playlists/{playlistId}/followers/contains",
                null, TokenType.User);
        }

        /// <summary>
        /// Follow Artists or Users
        /// </summary>
        /// <param name="itemIds">(Required) List of the artist or the user Spotify IDs.</param>
        /// <param name="followType">Either artist or user</param>
        /// <returns>Status Object</returns>
        /// <exception cref="AuthTokenRequiredException"></exception>
        public Task<Status> AuthFollowAsync(
            List<string> itemIds, 
            FollowType followType)
        {
            var parameters =
            new Dictionary<string, string>()
            {
                { "type", followType.GetDescription() }
            };
            return PutApiAsync<Status, Status>(itemIds, 
            "me/following", 
            null, null, parameters, TokenType.User, 204);
        }

        /// <summary>
        /// Follow a Playlist
        /// </summary>
        /// <param name="playlistId">(Required) The Spotify ID of the playlist. Any playlist can be followed, regardless of its public/private status, as long as you know its playlist ID.</param>
        /// <param name="isPublic">(Optional) Defaults to true. If true the playlist will be included in user’s public playlists, if false it will remain private. To be able to follow playlists privately, the user must have granted the playlist-modify-private scope.</param>
        /// <returns>Status Object</returns>
        /// <exception cref="AuthTokenRequiredException"></exception>
        public Task<Status> AuthFollowPlaylistAsync(
            string playlistId,
            bool isPublic = true)
        {
            var request = new PublicRequest() { IsPublic = isPublic };
            return PutApiAsync<PublicRequest, Status>(null,
                $"playlists/{playlistId}/followers", 
                request, null, null, TokenType.User, 200);
        }

        /// <summary>
        /// Get User's Followed Artists
        /// </summary>
        /// <param name="cursor">(Optional) Cursor</param>
        /// <returns>CursorPaging of Artist Object</returns>
        /// <exception cref="AuthTokenRequiredException"></exception>
        public async Task<CursorPaging<Artist>> AuthLookupFollowedArtistsAsync(
            Cursor cursor = null)
        {
            return (await LookupCursorApiAsync<ContentCursorResponse>(
                $"me/following", 
                "type", "artist", cursor, TokenType.User)).Artists;
        }

        /// <summary>
        /// Unfollow Artists or Users
        /// </summary>
        /// <param name="itemIds">(Required) List of the artist or the user Spotify IDs.</param>
        /// <param name="followType">Either artist or user</param>
        /// <returns>Status Object</returns>
        /// <exception cref="AuthTokenRequiredException"></exception>
        public Task<Status> AuthUnfollowAsync(
            List<string> itemIds,
            FollowType followType)
        {
            var parameters =
            new Dictionary<string, string>()
            {
                { "type", followType.GetDescription() }
            };
            return DeleteApiAsync<Status, Status>(itemIds, 
                "me/following", 
                null, parameters, TokenType.User, 204);
        }

        /// <summary>
        /// Unfollow Playlist
        /// </summary>
        /// <param name="playlistId">(Required) The Spotify ID of the playlist that is to be no longer followed.</param>
        /// <returns>Status Object</returns>
        /// <exception cref="AuthTokenRequiredException"></exception>
        public Task<Status> AuthUnfollowPlaylistAsync(
            string playlistId)
        {
            return DeleteApiAsync<Status, Status>(null, 
                $"playlists/{playlistId}/followers", 
                null, null, TokenType.User, 200);
        }
        #endregion Authenticated Follow API

        #region Authenticated Playlists API
        /// <summary>
        /// Add Tracks to a Playlist
        /// </summary>
        /// <param name="playlistId">The Spotify ID for the playlist.</param>
        /// <param name="uris">(Optional) List of Spotify track URIs to add.</param>
        /// <param name="position">(Optional) The position to insert the tracks, a zero-based index.</param>
        /// <returns>Snapshot Object</returns>
        /// <exception cref="AuthTokenRequiredException"></exception>
        public Task<Snapshot> AuthAddTracksToPlaylistAsync(
            string playlistId,
            UriListRequest uris = null,
            int? position = null)
        {
            var parameters = new Dictionary<string, string>();
            if (position != null)
                parameters.Add("position", position.ToString());
            return PostApiAsync<UriListRequest, Snapshot>(null,
                $"playlists/{playlistId}/tracks", 
                uris, null, parameters, TokenType.User, 201);
        }

        /// <summary>
        /// Remove Tracks from a Playlist
        /// </summary>
        /// <param name="playlistId">(Required) The Spotify ID for the playlist.</param>
        /// <param name="request">(Optional) Tracks: An array of objects containing Spotify URIs of the tracks to remove. Snapshot ID : The playlist’s snapshot ID against which you want to make the changes. The API will validate that the specified tracks exist and in the specified positions and make the changes, even if more recent changes have been made to the playlist.</param>
        /// <returns>Snapshot Object</returns>
        /// <exception cref="AuthTokenRequiredException"></exception>
        public Task<Snapshot> AuthRemoveTracksFromPlaylistAsync(
            string playlistId,
            PlaylistTracksRequest request = null)
        {
            return DeleteApiAsync<PlaylistTracksRequest, Snapshot>(null,
                $"playlists/{playlistId}/tracks", 
                request, null, TokenType.User, 200);
        }

        /// <summary>
        /// Get a Playlist Cover Image
        /// </summary>
        /// <param name="playlistId">(Require) The Spotify ID for the playlist.</param>
        /// <returns>Image Object</returns>
        /// <exception cref="AuthTokenRequiredException"></exception>
        public async Task<Image> AuthGetPlaylistCoverImageAsync(
            string playlistId)
        {
            return (await GetApiAsync<List<Image>>((string)null,
                $"playlists/{playlistId}/images", 
                null, TokenType.User)).FirstOrDefault();
        }

        /// <summary>
        /// Upload a Custom Playlist Cover Image
        /// </summary>
        /// <param name="playlistId">The Spotify ID for the playlist.</param>
        /// <param name="file">JPEG File Bytes</param>
        /// <returns>Status Object</returns>
        public Task<Status> AuthUploadCustomPlaylistImageAsync(
            string playlistId, 
            byte[] file)
        {
            return PutApiAsync<Status, Status>(null,
                $"playlists/{playlistId}/images", 
                null, file, null, TokenType.User, 202);
        }

        /// <summary>
        /// Get a List of Current User's Playlists
        /// </summary>
        /// <param name="cursor">(Optional) Cursor</param>
        /// <returns>CursorPaging of Playlist Object</returns>
        /// <exception cref="AuthTokenRequiredException"></exception>
        public Task<CursorPaging<Playlist>> AuthLookupUserPlaylistsAsync(
            Cursor cursor = null)
        {
            return LookupCursorApiAsync<CursorPaging<Playlist>>(
                "me/playlists", 
                null, null, cursor, TokenType.User);
        }

        /// <summary>
        /// Change a Playlist's Details
        /// </summary>
        /// <param name="playlistId">(Required) The Spotify ID for the playlist.</param>
        /// <param name="request">(Optional) Playlist Request Object</param>
        /// <returns>Status Object</returns>
        /// <exception cref="AuthTokenRequiredException"></exception>
        public Task<Status> AuthChangePlaylistDetailsAsync(
            string playlistId,
            PlaylistRequest request)
        {
            return PutApiAsync<PlaylistRequest, Status>(null,
                $"playlists/{playlistId}", request,
                null, null, TokenType.User, 200);
        }

        /// <summary>
        /// Get a List of a User's Playlists
        /// </summary>
        /// <param name="userId">(Required) The user’s Spotify user ID.</param>
        /// <param name="cursor">(Optional) Limit: The maximum number of playlists to return. Default: 20. Minimum: 1. Maximum: 50. - Offset: The index of the first playlist to return. Default: 0 (the first object). Maximum offset: 100</param>
        /// <returns>CursorPaging of Playlist Object</returns>
        /// <exception cref="AuthTokenRequiredException"></exception>
        public Task<CursorPaging<Playlist>> AuthLookupUserPlaylistsAsync(
            string userId,
            Cursor cursor = null)
        {
            return LookupCursorApiAsync<CursorPaging<Playlist>>(
                $"users/{userId}/playlists", 
                null, null, cursor, TokenType.User);
        }

        /// <summary>
        /// Replace a Playlist's Tracks
        /// </summary>
        /// <param name="playlistId">(Required) The Spotify ID for the playlist.</param>
        /// <param name="uris">(Optional) Uri List Request.</param>
        /// <exception cref="AuthTokenRequiredException"></exception>
        public Task<Status> AuthReplacePlaylistTracksAsync(
            string playlistId,
            UriListRequest uris)
        {
            return PutApiAsync<UriListRequest, Status>(null,
                $"playlists/{playlistId}/tracks", 
                uris, null, null, TokenType.User, 201);
        }

        /// <summary>
        /// Create a Playlist
        /// </summary>
        /// <param name="userId">(Required) The user’s Spotify user ID.</param>
        /// <param name="request">(Required) Playlist Request</param>
        /// <returns>Playlist Object</returns>
        /// <exception cref="AuthTokenRequiredException"></exception>
        public Task<Playlist> AuthCreatePlaylistAsync(
            string userId,
            PlaylistRequest request)
        {
            return PostApiAsync<PlaylistRequest, Playlist>(null,
            $"users/{userId}/playlists", 
            request, null, null, TokenType.User);
        }

        /// <summary>
        /// Reorder a Playlist's Tracks
        /// </summary>
        /// <param name="playlistId">The Spotify ID for the playlist.</param>
        /// <param name="request">(Required) Playlist Reorder Request</param>
        /// <returns>Snapshot Object</returns>
        /// <exception cref="AuthTokenRequiredException"></exception>
        public Task<Snapshot> AuthReorderPlaylistTracksAsync(
            string playlistId,
            PlaylistReorderRequest request)
        {
            return PutApiAsync<PlaylistReorderRequest, Snapshot>(null,
                $"playlists/{playlistId}/tracks", 
                request, null, null, TokenType.User, 200);
        }
        #endregion Authenticated Playlists API 

        #region Authenticated User Profile API
        /// <summary>
        /// Get a User's Profile
        /// </summary>
        /// <param name="userId">The user’s Spotify user ID.</param>
        /// <returns>Public User Object</returns>
        /// <exception cref="AuthTokenRequiredException"></exception>
        public Task<PublicUser> AuthLookupUserProfileAsync(
            string userId)
        {
            return GetApiAsync<PublicUser>(userId, 
                "users", 
                null, TokenType.User);
        }

        /// <summary>
        /// Get Current User's Profile
        /// </summary>
        /// <returns>Private User Object</returns>
        /// <exception cref="AuthTokenRequiredException"></exception>
        public Task<PrivateUser> AuthLookupUserProfileAsync()
        {
            return GetApiAsync<PrivateUser>((string)null, 
                "me", 
                null, TokenType.User);
        }
        #endregion Authenticated User Profile API
    }
}