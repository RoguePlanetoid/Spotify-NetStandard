using Spotify.NetStandard.Client.Authentication;
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
        private static readonly Uri _hostName;
        private static readonly string _clientVersion;
        private readonly AuthenticationCache _authenticationCache;

        #region Private Methods
        /// <summary>
        /// Format Request Headers
        /// </summary>
        /// <param name="authType">Authentication Type</param>
        /// <returns>Dictionary of Headers</returns>
        private async Task<Dictionary<string, string>> FormatRequestHeadersAsync(
            AuthType authType = AuthType.Implicit)
        {
            Dictionary<string, string> headersToSend = new Dictionary<string, string>();
            AccessToken access = await _authenticationCache.CheckAndRenewTokenAsync(
            authType, new CancellationToken(false));
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
            Dictionary<string, string> parameters = new Dictionary<string, string>();

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
        /// Browse
        /// </summary>
        /// <param name="browseCategory">Category</param>
        /// <param name="country">Country</param>
        /// <param name="locale">Locale</param>
        /// <returns>Content Response</returns>
        private async Task<ContentResponse> BrowseAsync(
            string browseCategory, 
            string country, 
            string locale = null, 
            Dictionary<string, string> parameters = null, 
            Page page = null)
        {
            Dictionary<string, string> requestHeaders = await FormatRequestHeadersAsync();
            Dictionary<string, string> requestParameters = FormatRequestParameters(
                country: country, locale: locale,
                offset: page?.Offset, limit: page?.Limit);
            if (parameters != null)
            {
                foreach (KeyValuePair<string, string> item in parameters)
                    requestParameters.Add(item.Key, item.Value);
            }
            return await GetAsync<ContentResponse>(_hostName, $"/v1/browse/{browseCategory}",
                new CancellationToken(false), requestParameters, requestHeaders);
        }

        /// <summary>
        /// Get
        /// </summary>
        /// <param name="itemId">Spotify ID of the Item</param>
        /// <param name="lookupType">Lookup Type</param>
        /// <param name="parameters">Request Parameters</param>
        /// <param name="authType">Auth Type</param>
        /// <returns>Result of Type</returns>
        private async Task<T> GetAsync<T>(
            string itemId = null,
            string lookupType = null,
            Dictionary<string, string> parameters = null,
            AuthType authType = AuthType.Implicit) 
            where T : class
        {
            Dictionary<string, string> requestHeaders = await FormatRequestHeadersAsync(authType);
            string[] source = new string[] { lookupType };
            source = source[0].Contains("_") ?
                source[0].Split('_') :
                source;
            string relativeUri = (itemId == null) ?
                $"/v1/{lookupType}" :
                $"/v1/{lookupType}/{itemId}";
            return await GetAsync<T>(_hostName, relativeUri,
            new CancellationToken(false), parameters, requestHeaders);
        }

        /// <summary>
        /// List
        /// </summary>
        /// <typeparam name="T">Request Type</typeparam>
        /// <param name="itemIds">Spotify IDs of the Items</param>
        /// <param name="lookupType">Lookup Type</param>
        /// <param name="parameters">Request Parameters</param>
        /// <param name="authType">Auth Type</param>
        /// <returns>Response as Type</returns>
        private async Task<T> ListAsync<T>(
            List<string> itemIds, 
            string lookupType,
            Dictionary<string, string> parameters = null,
            AuthType authType = AuthType.Implicit) 
            where T : class
        {
            string ids = itemIds.Aggregate(string.Empty,
            (current, id) => current + (!string.IsNullOrEmpty(current) ? "," : string.Empty) + id);
            Dictionary<string, string> requestHeaders = await FormatRequestHeadersAsync(authType);
            string[] source = new string[] { lookupType };
            parameters.Add("ids", ids);
            source = source[0].Contains("_") ?
                source[0].Split('_') :
                source;
            string relativeUri = $"/v1/{lookupType}";
            return await GetAsync<T>(_hostName, relativeUri,
            new CancellationToken(false), parameters, requestHeaders);
        }

        /// <summary>
        /// Lookup API
        /// </summary>
        /// <param name="itemId">Spotify ID of the Item</param>
        /// <param name="lookupType">Lookup Type</param>
        /// <param name="country">Country</param>
        /// <param name="page">Page</param>
        /// <returns>Response of Type</returns>
        private async Task<T> LookupApiAsync<T>(
            string itemId, 
            string lookupType = null, 
            string country = null,
            Page page = null) where T : class
        {
            Dictionary<string, string> requestHeaders = await FormatRequestHeadersAsync();
            Dictionary<string, string> requestParameters = FormatRequestParameters(
            offset: page?.Offset, limit: page?.Limit, country: country);
            string[] source = new string[] { lookupType };
            source = source[0].Contains("_") ?
                source[0].Split('_') :
                source;
            string relativeUri = (source.Length == 1) ?
                $"/v1/{source[0]}/{itemId}" :
                $"/v1/{source[0]}/{itemId}/{source[1]}";
            return await GetAsync<T>(_hostName, relativeUri,
            new CancellationToken(false), requestParameters, requestHeaders);
        }

        /// <summary>
        /// Lookup API
        /// </summary>
        /// <param name="itemIds">Spotify IDs of the Items</param>
        /// <param name="lookupType">Lookup Type</param>
        /// <param name="country">Country</param>
        /// <param name="page">Page</param>
        /// <returns>Lookup Response</returns>
        private async Task<LookupResponse> LookupApiAsync(
            IEnumerable<string> itemIds, 
            string lookupType = null,
            string country = null, 
            Page page = null)
        {
            string ids = itemIds.Aggregate(string.Empty,
                (current, id) => current + (!string.IsNullOrEmpty(current) ? "," : string.Empty) + id);
            Dictionary<string, string> requestHeaders = await FormatRequestHeadersAsync();
            Dictionary<string, string> requestParameters = FormatRequestParameters(
            offset: page?.Offset, limit: page?.Limit, country: country);
            requestParameters.Add("ids", ids);
            string source = lookupType;
            return await GetAsync<LookupResponse>(_hostName, $"/v1/{source}/",
            new CancellationToken(false), requestParameters, requestHeaders);
        }

        /// <summary>
        /// Search API
        /// </summary>
        /// <param name="query">Search Query</param>
        /// <param name="searchType">Search Type</param>
        /// <param name="country">Country</param>
        /// <param name="continuationToken">Continuation Token</param>
        /// <returns>Content Rssponse</returns>
        private async Task<ContentResponse> SearchApiAsync(
            string query, 
            SearchType searchType, 
            string country = null, 
            Page page = null)
        {
            Dictionary<string, string> requestHeaders = await FormatRequestHeadersAsync();
            Dictionary<string, string> requestParameters = FormatRequestParameters(
            offset: page?.Offset, limit: page?.Limit, country: country);
            requestParameters.Add("type", searchType.GetDescription());
            if (!string.IsNullOrEmpty(query))
                requestParameters.Add("q", Uri.EscapeDataString(query));
            return await GetAsync<ContentResponse>(_hostName, $"/v1/search/",
            new CancellationToken(false), requestParameters, requestHeaders);
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
            _authenticationCache = authenticationCache;
        }
        #endregion Constructors

        #region Auth
        /// <summary>
        /// Auth
        /// </summary>
        /// <param name="redirectUri">Redirect Uri</param>
        /// <param name="state">State</param>
        /// <param name="scopes">Scopes</param>
        /// <returns>Uri</returns>
        public Uri Auth(
            Uri redirectUri, 
            string state, 
            params ScopeType[] scopes)
        {
            return _authenticationCache.GetAuth(redirectUri, state, 
                scopes.AsDelimitedString());
        }

        /// <summary>
        /// Auth
        /// </summary>
        /// <param name="responseUri">Response Uri</param>
        /// <param name="redirectUri">Redirect Uri</param>
        /// <param name="state">State</param>
        /// <returns>AccessToken on Success, Null if Not</returns>
        /// <exception cref="AuthCodeValueException">AuthCodeValueException</exception>
        /// <exception cref="AuthCodeStateException">AuthCodeStateException</exception>
        public Task<AccessToken> AuthAsync(
            Uri responseUri, 
            Uri redirectUri, 
            string state)
        {
            return _authenticationCache.GetAuth(responseUri, redirectUri, state);
        }

        /// <summary>
        /// Get Access Token
        /// </summary>
        /// <returns>Access Token</returns>
        public AccessToken GetToken() => _authenticationCache.AccessToken;

        /// <summary>
        /// Set Access Token
        /// </summary>
        /// <param name="value">Access Token</param>
        public void SetToken(AccessToken value) => _authenticationCache.AccessToken = value;
        #endregion Auth

        #region Navigate
        /// <summary>
        /// Navigate 
        /// </summary>
        /// <typeparam name="T">Response Type</typeparam>
        /// <param name="paging">Paging Object</param>
        /// <param name="navigateby">Navigate By</param>
        /// <returns>Content Response</returns>
        public async Task<ContentResponse> NavigateAsync<T>(
            Paging<T> paging, 
            NavigateBy navigateby)
        {
            Uri source = null;
            switch (navigateby.Direction)
            {
                case NavigateType.None:
                    source = new Uri(paging.Href);
                    break;
                case NavigateType.Previous:
                    if (paging.Previous != null) source = new Uri(paging.Previous);
                    break;
                case NavigateType.Next:
                    if (paging.Next != null) source = new Uri(paging.Next);
                    break;
            }
            if (source != null)
            {
                Dictionary<string, string> requestHeaders = await FormatRequestHeadersAsync();
                Dictionary<string, string> requestParameters = source.Query.QueryStringAsDictionary();
                return await GetAsync<ContentResponse>(new Uri($"{source.Scheme}://{source.Host}"),
                    source.AbsolutePath, new CancellationToken(false), requestParameters, requestHeaders);
            }
            return null;
        }
        #endregion Navigate

        #region Lookup
        /// <summary>
        /// Lookup
        /// </summary>
        /// <typeparam name="T">Response Type</typeparam>
        /// <param name="id">The Spotify ID for the album.</param>
        /// <param name="lookupType">Item Type</param>
        /// <param name="market">(Optional) ISO 3166-1 alpha-2 country code</param>
        /// <param name="page">Page</param>
        /// <returns>Lookup Response by Type</returns>
        public Task<T> LookupAsync<T>(
            string itemId,
            LookupType lookupType,
            string market = null,
            Page page = null)
            where T : class
        {
            return LookupApiAsync<T>(itemId, lookupType.GetDescription(), market);
        }

        /// <summary>
        /// Lookup
        /// </summary>
        /// <param name="itemIds">List of Spotify ID for the items</param>
        /// <param name="lookupType">Item Type</param>
        /// <param name="market">ISO 3166-1 alpha-2 country code</param>
        /// <param name="page">Page</param>
        /// <returns>Lookup Response</returns>
        public Task<LookupResponse> LookupAsync(
            List<string> itemIds,
            LookupType lookupType,
            string market = null,
            Page page = null)
        {
            return LookupApiAsync(itemIds, lookupType.GetDescription(), market, page);
        }
        #endregion Lookup

        #region Browse
        /// <summary>
        /// Get All Featured Playlists
        /// </summary>
        /// <param name="country">A country: an ISO 3166-1 alpha-2 country code. </param>
        /// <param name="locale">The desired language, consisting of a lowercase ISO 639-1 language code and an uppercase ISO 3166-1 alpha-2 country code, joined by an underscore</param>
        /// <param name="timestamp">A timestamp in ISO 8601 format: yyyy-MM-ddTHH:mm:ss</param>
        /// <param name="page">Page</param>
        /// <returns>Content Response</returns>
        public Task<ContentResponse> GetFeaturedPlaylistsAsync(
            string country = null,
            string locale = null,
            string timestamp = null,
            Page page = null)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            if (timestamp != null)
                parameters.Add("timestamp", timestamp);
            return BrowseAsync("featured-playlists", country, locale, parameters, page);
        }

        /// <summary>
        /// Get All New Releases
        /// </summary>
        /// <param name="country">A country: an ISO 3166-1 alpha-2 country code. </param>
        /// <param name="page">Page</param>
        /// <returns>Content Response</returns>
        public Task<ContentResponse> GetNewReleasesAsync(
            string country = null,
            Page page = null)
        {
            return BrowseAsync("new-releases", country: country, page: page);
        }

        /// <summary>
        /// Get an Artist's Top Tracks
        /// </summary>
        /// <param name="itemId">The Spotify ID for the artist.</param>
        /// <param name="country">A country: an ISO 3166-1 alpha-2 country code.</param>
        /// <returns>Lookup Response</returns>
        public Task<LookupResponse> GetArtistTopTracksAsync(
            string itemId, 
            string country)
        {
            return LookupApiAsync<LookupResponse>(itemId, "artists_top-tracks", country: country);
        }

        /// <summary>
        /// Get an Artist's Related Artists
        /// </summary>
        /// <param name="itemId">The Spotify ID for the artist.</param>
        /// <returns>Lookup Response</returns>
        public Task<LookupResponse> GetArtistRelatedArtistsAsync(
            string itemId)
        {
            return LookupApiAsync<LookupResponse>(itemId, "artists_related-artists");
        }

        /// <summary>
        /// Get All Categories
        /// </summary>
        /// <param name="country">A country: an ISO 3166-1 alpha-2 country code. </param>
        /// <param name="locale">The desired language, consisting of a lowercase ISO 639-1 language code and an uppercase ISO 3166-1 alpha-2 country code, joined by an underscore</param>
        /// <param name="page">Page</param>
        /// <returns>Content Response</returns>
        public Task<ContentResponse> GetCategoriesAsync(
            string country = null,
            string locale = null,
            Page page = null)
        {
            return BrowseAsync("categories", country: country, locale: locale, page: page);
        }
        #endregion Browse

        #region Search
        /// <summary>
        /// Search for an Item
        /// </summary>
        /// <param name="query">Search query keywords and optional field filters and operators.</param>
        /// <param name="searchType">A comma-separated list of item types to search across. Valid types are: album , artist, playlist, and track. </param>
        /// <param name="market">An ISO 3166-1 alpha-2 country code</param>
        /// <param name="page">Page</param>
        /// <returns>Content Response</returns>
        public Task<ContentResponse> SearchAsync(
            string query,
            SearchType searchType,
            string market = null,
            Page page = null)
        {
            return SearchApiAsync(query, searchType, market, page);
        }
        #endregion Search

        #region Recommendations
        /// <summary>
        /// Get Recommendations
        /// </summary>
        /// <param name="seedArtists">List of Spotify IDs for seed artists</param>
        /// <param name="seedGenres">List of any genres in the set of available genre seeds</param>
        /// <param name="seedTracks">List of Spotify IDs for a seed track</param>
        /// <param name="limit">The target size of the list of recommended tracks. Default: 20. Minimum: 1. Maximum: 100.</param>
        /// <param name="market">An ISO 3166-1 alpha-2 country code</param>
        /// <param name="minTuneableTrack">Multiple values. For each tunable track attribute, a hard floor on the selected track attribute’s value can be provided</param>
        /// <param name="maxTuneableTrack">Multiple values. For each tunable track attribute, a hard ceiling on the selected track attribute’s value can be provided.</param>
        /// <param name="targetTuneableTrack">Multiple values. For each of the tunable track attributes (below) a target value may be provided.</param>
        /// <returns>Recommendation Response Object</returns>
        public Task<RecommendationsResponse> GetRecommendationsAsync(
            string[] seedArtists = null,
            string[] seedGenres = null,
            string[] seedTracks = null,
            int? limit = null,
            string market = null,
            TuneableTrack minTuneableTrack = null,
            TuneableTrack maxTuneableTrack = null,
            TuneableTrack targetTuneableTrack = null)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();

            if (seedArtists != null)
                parameters.Add("seed_artists", seedArtists.AsDelimitedString());

            if (seedGenres != null)
                parameters.Add("seed_genres", seedGenres.AsDelimitedString());

            if (seedTracks != null)
                parameters.Add("seed_tracks", seedTracks.AsDelimitedString());

            if (limit != null)
                parameters.Add("limit", limit.Value.ToString());

            if (market != null)
                parameters.Add("market", market);

            if (minTuneableTrack != null)
                minTuneableTrack.Set(parameters, "min");

            if (maxTuneableTrack != null)
                maxTuneableTrack.Set(parameters, "max");

            if (targetTuneableTrack != null)
                targetTuneableTrack.Set(parameters, "target");

            return GetAsync<RecommendationsResponse>(
                lookupType: "recommendations", parameters: parameters);
        }

        /// <summary>
        /// Get Recommendation Genres
        /// </summary>
        /// <returns>Available Genre Seeds Object</returns>
        public Task<AvailableGenreSeeds> GetRecommendationGenres()
        {
            return GetAsync<AvailableGenreSeeds>(
                lookupType: "recommendations/available-genre-seeds");
        }
        #endregion Recommendations
    }
}