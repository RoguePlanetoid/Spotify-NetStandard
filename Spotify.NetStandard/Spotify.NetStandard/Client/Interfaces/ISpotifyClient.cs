using Spotify.NetStandard.Client.Authentication;
using Spotify.NetStandard.Client.Internal;
using Spotify.NetStandard.Enums;
using Spotify.NetStandard.Requests;
using Spotify.NetStandard.Responses;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Spotify.NetStandard.Client.Interfaces
{
    /// <summary>
    /// Spotify Client
    /// </summary>
    public interface ISpotifyClient : IDisposable
    {
        #region Properties
        /// <summary>
        /// Spotify API
        /// </summary>
        ISpotifyApi Api { get; }
        #endregion Properties

        #region Public Methods
        /// <summary>
        /// Auth User
        /// </summary>
        /// <param name="redirectUri">Redirect Uri</param>
        /// <param name="state">State</param>
        /// <param name="scope">Scope</param>
        /// <returns>Uri</returns>
        Uri AuthUser(
            Uri redirectUri,
            string state,
            Scope scope);

        /// <summary>
        /// Auth User
        /// </summary>
        /// <param name="responseUri">Response Uri</param>
        /// <param name="redirectUri">Redirect Uri</param>
        /// <param name="state">State</param>
        /// <returns>AccessToken on Success, Null if Not</returns>
        /// <exception cref="AuthCodeValueException">AuthCodeValueException</exception>
        /// <exception cref="AuthCodeStateException">AuthCodeStateException</exception>
        Task<AccessToken> AuthUserAsync(
            Uri responseUri,
            Uri redirectUri,
            string state);

        /// <summary>
        /// Get Access Token
        /// </summary>
        /// <returns>Access Token</returns>
        AccessToken GetToken();

        /// <summary>
        /// Set Access Token
        /// </summary>
        /// <param name="value">Access Token</param>
        void SetToken(AccessToken value);

        /// <summary>
        /// Navigate 
        /// </summary>
        /// <typeparam name="T">Response Type</typeparam>
        /// <param name="paging">Paging Object</param>
        /// <param name="navigateby">Navigate By</param>
        /// <returns>Content Response</returns>
        Task<ContentResponse> NavigateAsync<T>(
            Paging<T> paging,
            NavigateBy navigateby);

        /// <summary>
        /// Search
        /// </summary>
        /// <param name="query">(Required) Search Query</param>
        /// <param name="searchType">(Required) Search results include hits from all the specified item types.</param>
        /// <param name="country">(Optional) An ISO 3166-1 alpha-2 country code or the string from_token</param>
        /// <param name="external">(Optional) Include any relevant audio content that is hosted externally. </param>
        /// <param name="page">(Optional) Limit: The maximum number of items to return - Offset: The index of the first item to return</param>
        /// <returns>Content Response</returns>
        Task<ContentResponse> SearchAsync(
            string query,
            SearchType searchType,
            string market = null,
            bool? external = null,
            Page page = null);

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
        Task<T> LookupAsync<T>(
            string itemId,
            LookupType lookupType,
            string market = null,
            string key = null,
            string value = null,
            Page page = null)
            where T : class;

        /// <summary>
        /// Lookup
        /// </summary>
        /// <param name="itemIds">(Required) List of Spotify ID for the items</param>
        /// <param name="lookupType">(Required) Lookup Item Type</param>
        /// <param name="market">(Optional) ISO 3166-1 alpha-2 country code or the string from_token</param>
        /// <param name="page">(Optional) Limit: The maximum number of items to return - Offset: The index of the first item to return</param>
        /// <returns>Lookup Response</returns>
        Task<LookupResponse> LookupAsync(
            List<string> itemIds,
            LookupType lookupType,
            string market = null,
            Page page = null);

        /// <summary>
        /// Lookup Featured Playlists
        /// </summary>
        /// <param name="country">(Optional) A country: an ISO 3166-1 alpha-2 country code. </param>
        /// <param name="locale">(Optional) The desired language, consisting of a lowercase ISO 639-1 language code and an uppercase ISO 3166-1 alpha-2 country code, joined by an underscore</param>
        /// <param name="timestamp">(Optional) Use this parameter to specify the user’s local time to get results tailored for that specific date and time in the day.</param>
        /// <param name="page">(Optional) Limit: The maximum number of items to return. Default: 20. Minimum: 1. Maximum: 50. - Offset: The index of the first item to return. Default: 0</param>
        /// <returns>Content Response</returns>
        Task<ContentResponse> LookupFeaturedPlaylistsAsync(
            string country = null,
            string locale = null,
            DateTime? timestamp = null,
            Page page = null);

        /// <summary>
        /// Lookup New Releases
        /// </summary>
        /// <param name="country">(Optional) A country: an ISO 3166-1 alpha-2 country code. </param>
        /// <param name="page">(Optional) Limit: The maximum number of items to return. Default: 20. Minimum: 1. Maximum: 50. - Offset: The index of the first item to return. Default: 0</param>
        /// <returns>Content Response</returns>
        Task<ContentResponse> LookupNewReleasesAsync(
            string country = null,
            Page page = null);

        /// <summary>
        /// Lookup Artist's Albums
        /// </summary>
        /// <param name="id">(Required) The Spotify ID for the artist.</param>
        /// <param name="includeGroup">(Optional) Filters the response. If not supplied, all album types will be returned</param>
        /// <param name="market">(Optional) An ISO 3166-1 alpha-2 country code</param>
        /// <param name="page">(Optional) Limit: The number of album objects to return. Default: 20. Minimum: 1. Maximum: 50 - Offset: The index of the first album to return. Default: 0 (i.e., the first album).</param>
        /// <returns>Paging List of Album</returns>
        Task<Paging<Album>> LookupArtistAlbumsAsync(
            string itemId,
            IncludeGroup includeGroup = null,
            string market = null,
            Page page = null);

        /// <summary>
        /// Lookup Artist's Top Tracks
        /// </summary>
        /// <param name="itemId">(Required) The Spotify ID for the artist.</param>
        /// <param name="market">(Required) A country: an ISO 3166-1 alpha-2 country code or if null uses from_token.</param>
        /// <returns>Lookup Response</returns>
        Task<LookupResponse> LookupArtistTopTracksAsync(
            string itemId,
            string market);

        /// <summary>
        /// Lookup Artist's Related Artists
        /// </summary>
        /// <param name="itemId">(Required) The Spotify ID for the artist.</param>
        /// <returns>Lookup Response</returns>
        Task<LookupResponse> LookupArtistRelatedArtistsAsync(
            string itemId);

        /// <summary>
        /// Lookup All Categories
        /// </summary>
        /// <param name="country">(Optional) A country: an ISO 3166-1 alpha-2 country code. </param>
        /// <param name="locale">(Optional) The desired language, consisting of a lowercase ISO 639-1 language code and an uppercase ISO 3166-1 alpha-2 country code, joined by an underscore</param>
        /// <param name="page">(Optional) Limit: The maximum number of categories to return. Default: 20. Minimum: 1. Maximum: 50. - Offset: The index of the first item to return. Default: 0</param>
        /// <returns>Content Response</returns>
        Task<ContentResponse> LookupAllCategoriesAsync(
            string country = null,
            string locale = null,
            Page page = null);

        /// <summary>
        /// Lookup Category 
        /// </summary>
        /// <param name="categoryId">The Spotify category ID for the category.</param>
        /// <param name="country">(Optional) A country: an ISO 3166-1 alpha-2 country code. </param>
        /// <param name="locale">(Optional) The desired language, consisting of an ISO 639-1 language code and an ISO 3166-1 alpha-2 country code, joined by an underscore.</param>
        /// <returns>Category Object</returns>
        Task<Category> LookupCategoryAsync(
           string categoryId,
           string country = null,
           string locale = null);

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
        Task<RecommendationsResponse> LookupRecommendationsAsync(
            List<string> seedArtists = null,
            List<string> seedGenres = null,
            List<string> seedTracks = null,
            int? limit = null, 
            string market = null,
            TuneableTrack minTuneableTrack = null,
            TuneableTrack maxTuneableTrack = null,
            TuneableTrack targetTuneableTrack = null);

        /// <summary>
        /// Lookup Recommendation Genres
        /// </summary>
        /// <returns>Available Genre Seeds Object</returns>
        Task<AvailableGenreSeeds> LookupRecommendationGenres();
        #endregion Recommendations
    }
}