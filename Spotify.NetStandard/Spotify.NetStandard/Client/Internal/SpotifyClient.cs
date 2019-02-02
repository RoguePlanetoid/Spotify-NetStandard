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
            return await GetAsync<ContentResponse>(_hostName, 
                $"/v1/browse/{browseCategory}",
                new CancellationToken(false), 
                requestParameters, requestHeaders);
        }

        /// <summary>
        /// Get Lookup
        /// </summary>
        /// <param name="itemId">Spotify ID of the Item</param>
        /// <param name="lookupType">Lookup Type</param>
        /// <param name="parameters">Request Parameters</param>
        /// <param name="tokenType">Token Type</param>
        /// <returns>Result of Type</returns>
        private async Task<T> GetLookupAsync<T>(
            string itemId = null,
            string lookupType = null,
            Dictionary<string, string> parameters = null,
            TokenType tokenType = TokenType.Access)
            where T : class
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
            return await GetAsync<T>(_hostName, relativeUri,
                new CancellationToken(false), parameters, headers);
        }

        /// <summary>
        /// List Lookup
        /// </summary>
        /// <typeparam name="T">Request Type</typeparam>
        /// <param name="itemIds">Spotify IDs of the Items</param>
        /// <param name="lookupType">Lookup Type</param>
        /// <param name="parameters">Request Parameters</param>
        /// <param name="tokenType">Token Type</param>
        /// <returns>Response as Type</returns>
        private async Task<T> ListLookupAsync<T>(
            List<string> itemIds,
            string lookupType,
            Dictionary<string, string> parameters = null,
            TokenType tokenType = TokenType.Access)
            where T : class
        {
            string ids = FormatIdsParameter(itemIds);
            Dictionary<string, string> headers = 
                await FormatRequestHeadersAsync(tokenType);
            string[] source = new string[] { lookupType };
            parameters.Add("ids", ids);
            source = source[0].Contains("_") ?
                source[0].Split('_') :
                source;
            return await GetAsync<T>(
                _hostName, $"/v1/{lookupType}",
                new CancellationToken(false), 
                parameters, headers);
        }

        /// <summary>
        /// Lookup API
        /// </summary>
        /// <param name="itemId">Spotify ID of the Item</param>
        /// <param name="lookupType">Lookup Type</param>
        /// <param name="country">Country</param>
        /// <param name="page">Page Offset & Limit</param>
        /// <returns>Response of Type</returns>
        private async Task<T> LookupApiAsync<T>(
            string itemId,
            string lookupType = null,
            string country = null,
            string key = null,
            string value = null,
            Page page = null) where T : class
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
            return await GetAsync<T>(_hostName, relativeUri,
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
            return await GetAsync<LookupResponse>(
                _hostName, $"/v1/{lookupType}/",
                new CancellationToken(false), parameters, headers);
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
                return await GetAsync<ContentResponse>(
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
            return await GetAsync<ContentResponse>(
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

            return GetLookupAsync<RecommendationsResponse>(
                lookupType: "recommendations", parameters: parameters);
        }

        /// <summary>
        /// Lookup Recommendation Genres
        /// </summary>
        /// <returns>Available Genre Seeds Object</returns>
        public Task<AvailableGenreSeeds> LookupRecommendationGenres()
        {
            return GetLookupAsync<AvailableGenreSeeds>(
                lookupType: "recommendations/available-genre-seeds");
        }
        #endregion Public Methods
    }
}