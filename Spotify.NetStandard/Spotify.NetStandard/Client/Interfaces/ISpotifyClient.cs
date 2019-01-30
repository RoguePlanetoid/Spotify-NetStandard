using Spotify.NetStandard.Client.Authentication;
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
        #region Auth
        /// <summary>
        /// Auth
        /// </summary>
        /// <param name="redirectUri">Redirect Uri</param>
        /// <param name="state">State</param>
        /// <param name="scopes">Scopes</param>
        /// <returns>Uri</returns>
        Uri Auth(
            Uri redirectUri,
            string state,
            params ScopeType[] scopes);

        /// <summary>
        /// Auth
        /// </summary>
        /// <param name="responseUri">Response Uri</param>
        /// <param name="redirectUri">Redirect Uri</param>
        /// <param name="state">State</param>
        /// <returns>AccessToken on Success, Null if Not</returns>
        /// <exception cref="AuthCodeValueException">AuthCodeValueException</exception>
        /// <exception cref="AuthCodeStateException">AuthCodeStateException</exception>
        Task<AccessToken> AuthAsync(
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
        #endregion Auth

        #region Navigate
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
        Task<T> LookupAsync<T>(
            string itemId,
            LookupType lookupType,          
            string market = null,
            Page page = null)
            where T : class;

        /// <summary>
        /// Lookup
        /// </summary>
        /// <param name="itemIds">List of Spotify ID for the items</param>
        /// <param name="lookupType">Item Type</param>
        /// <param name="market">ISO 3166-1 alpha-2 country code</param>
        /// <param name="page">Page</param>
        /// <returns>Lookup Response</returns>
        Task<LookupResponse> LookupAsync(
            List<string> itemIds,
            LookupType lookupType,
            string market = null,
            Page page = null);
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
        Task<ContentResponse> GetFeaturedPlaylistsAsync(
            string country = null, 
            string locale = null, 
            string timestamp = null,
            Page page = null);

        /// <summary>
        /// Get All New Releases
        /// </summary>
        /// <param name="country">A country: an ISO 3166-1 alpha-2 country code. </param>
        /// <param name="page">Page</param>
        /// <returns>Content Response</returns>
        Task<ContentResponse> GetNewReleasesAsync(
            string country = null,
            Page page = null);

        /// <summary>
        /// Get an Artist's Top Tracks
        /// </summary>
        /// <param name="itemId">The Spotify ID for the artist.</param>
        /// <param name="country">A country: an ISO 3166-1 alpha-2 country code.</param>
        /// <returns>Lookup Response</returns>
        Task<LookupResponse> GetArtistTopTracksAsync(
            string itemId,
            string country);

        /// <summary>
        /// Get an Artist's Related Artists
        /// </summary>
        /// <param name="itemId">The Spotify ID for the artist.</param>
        /// <returns>Lookup Response</returns>
        Task<LookupResponse> GetArtistRelatedArtistsAsync(
            string itemId);

        /// <summary>
        /// Get All Categories
        /// </summary>
        /// <param name="country">A country: an ISO 3166-1 alpha-2 country code. </param>
        /// <param name="locale">The desired language, consisting of a lowercase ISO 639-1 language code and an uppercase ISO 3166-1 alpha-2 country code, joined by an underscore</param>
        /// <param name="page">Page</param>
        /// <returns>Content Response</returns>
        Task<ContentResponse> GetCategoriesAsync(
            string country = null,
            string locale = null,
            Page page = null);
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
        Task<ContentResponse> SearchAsync(
            string query,
            SearchType searchType,
            string market = null,
            Page page = null);
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
        Task<RecommendationsResponse> GetRecommendationsAsync(
            string[] seedArtists = null,
            string[] seedGenres = null,
            string[] seedTracks = null,
            int? limit = null, 
            string market = null,
            TuneableTrack minTuneableTrack = null,
            TuneableTrack maxTuneableTrack = null,
            TuneableTrack targetTuneableTrack = null);

        /// <summary>
        /// Get Recommendation Genres
        /// </summary>
        /// <returns>Available Genre Seeds Object</returns>
        Task<AvailableGenreSeeds> GetRecommendationGenres();
        #endregion Recommendations
    }
}