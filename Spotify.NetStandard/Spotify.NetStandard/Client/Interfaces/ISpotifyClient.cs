﻿namespace Spotify.NetStandard.Client.Interfaces;

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
    /// Refresh Token
    /// </summary>
    /// <returns>Access Token</returns>
    Task<AccessToken> RefreshToken();

    /// <summary>
    /// Refresh Token
    /// </summary>
    /// <param name="value">Access Token</param>
    /// <returns>Access Token</returns>
    Task<AccessToken> RefreshToken(AccessToken value);

    /// <summary>
    /// Authenticated Get
    /// </summary>
    /// <typeparam name="TResponse">Response Type</typeparam>
    /// <param name="hostname">Hostname</param>
    /// <param name="endpoint">Endpoint</param>
    /// <param name="parameters">Parameters</param>
    /// <returns>Response</returns>
    Task<TResponse> AuthGetAsync<TResponse>(
        string hostname, string endpoint,
        Dictionary<string, string> parameters)
        where TResponse : class;

    /// <summary>
    /// Authenticated Get
    /// </summary>
    /// <typeparam name="TResponse">Response Type</typeparam>
    /// <param name="source">Source Uri</param>
    /// <returns>Response</returns>
    Task<TResponse> AuthGetAsync<TResponse>(Uri source)
        where TResponse : class;

    /// <summary>
    /// Authenticated Navigate 
    /// </summary>
    /// <typeparam name="TResponse">Response Type</typeparam>
    /// <param name="cursor">Cursor Object</param>
    /// <param name="navigateType">Navigate Type</param>
    /// <returns>Content Response</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    Task<CursorPaging<TResponse>> AuthNavigateAsync<TResponse>(
        CursorPaging<TResponse> cursor,
        NavigateType navigateType);

    /// <summary>
    /// Paging
    /// </summary>
    /// <typeparam name="TResponse">Response Type</typeparam>
    /// <param name="paging">Paging</param>
    /// <param name="navigateType">Navigate Type</param>
    /// <returns>Content Response</returns>
    /// <exception cref="AuthAccessTokenRequiredException"></exception>
    Task<Paging<TResponse>> PagingAsync<TResponse>(
        Paging<TResponse> paging,
        NavigateType navigateType);

    /// <summary>
    /// Get
    /// </summary>
    /// <typeparam name="TResponse">Response Type</typeparam>
    /// <param name="hostname">Hostname</param>
    /// <param name="endpoint">Endpoint</param>
    /// <param name="parameters">Parameters</param>
    /// <returns>Response</returns>
    Task<TResponse> GetAsync<TResponse>(
        string hostname, string endpoint,
        Dictionary<string, string> parameters)
        where TResponse : class;

    /// <summary>
    /// Get
    /// </summary>
    /// <typeparam name="TResponse">Response Type</typeparam>
    /// <param name="source">Source Uri</param>
    /// <returns>Response</returns>
    Task<TResponse> GetAsync<TResponse>(Uri source)
        where TResponse : class;

    /// <summary>
    /// Navigate 
    /// </summary>
    /// <typeparam name="TResponse">Response Type</typeparam>
    /// <param name="paging">Paging Object</param>
    /// <param name="navigateType">Navigate Type</param>
    /// <returns>Content Response</returns>
    /// <exception cref="AuthAccessTokenRequiredException"></exception>
    Task<ContentResponse> NavigateAsync<TResponse>(
        Paging<TResponse> paging,
        NavigateType navigateType);

    /// <summary>
    /// Search
    /// </summary>
    /// <param name="query">(Required) Search Query</param>
    /// <param name="searchType">(Required) Search results include hits from all the specified item types.</param>
    /// <param name="market">(Optional) An ISO 3166-1 alpha-2 country code or the string from_token</param>
    /// <param name="external">(Optional) Include any relevant audio content that is hosted externally. </param>
    /// <param name="page">(Optional) Limit: The maximum number of items to return - Offset: The index of the first item to return</param>
    /// <returns>Content Response</returns>
    /// <exception cref="AuthAccessTokenRequiredException"></exception>
    Task<ContentResponse> SearchAsync(
        string query,
        SearchType searchType,
        string market = null,
        bool? external = null,
        Page page = null);

    /// <summary>
    /// Lookup
    /// </summary>
    /// <typeparam name="TResponse">Response Type</typeparam>
    /// <param name="itemId">(Required) The Spotify ID for the album.</param>
    /// <param name="lookupType">(Required) Item Type</param>
    /// <param name="market">(Optional) ISO 3166-1 alpha-2 country code or the string from_token</param>
    /// <param name="country">(Optional) ISO 3166-1 alpha-2 country code or the string from_token</param>
    /// <param name="fields">(Optional) Filters for the query: a comma-separated list of the fields to return for Playlist and PlaylistTracks LookupType if omitted, all fields are returned</param>
    /// <param name="key">(Optional) Query Parameter Key</param>
    /// <param name="value">(Optional) Query Parameter Value</param>
    /// <param name="page">(Optional) Limit: The maximum number of items to return - Offset: The index of the first item to return</param>
    /// <returns>Lookup Response by Type</returns>
    /// <exception cref="AuthAccessTokenRequiredException"></exception>
    Task<TResponse> LookupAsync<TResponse>(
        string itemId,
        LookupType lookupType,
        string market = null,
        string country = null,
        string fields = null,
        string key = null,
        string value = null,
        Page page = null)
        where TResponse : class;

    /// <summary>
    /// Lookup
    /// </summary>
    /// <param name="itemIds">(Required) List of Spotify ID for the items</param>
    /// <param name="lookupType">(Required) Lookup Item Type</param>
    /// <param name="market">(Optional) ISO 3166-1 alpha-2 country code or the string from_token</param>
    /// <param name="page">(Optional) Limit: The maximum number of items to return - Offset: The index of the first item to return</param>
    /// <returns>Lookup Response</returns>
    /// <exception cref="AuthAccessTokenRequiredException"></exception>
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
    /// <exception cref="AuthAccessTokenRequiredException"></exception>
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
    /// <exception cref="AuthAccessTokenRequiredException"></exception>
    Task<ContentResponse> LookupNewReleasesAsync(
        string country = null,
        Page page = null);

    /// <summary>
    /// Lookup Artist's Albums
    /// </summary>
    /// <param name="itemId">(Required) The Spotify ID for the artist.</param>
    /// <param name="includeGroup">(Optional) Filters the response. If not supplied, all album types will be returned</param>
    /// <param name="market">(Optional) An ISO 3166-1 alpha-2 country code</param>
    /// <param name="page">(Optional) Limit: The number of album objects to return. Default: 20. Minimum: 1. Maximum: 50 - Offset: The index of the first album to return. Default: 0 (i.e., the first album).</param>
    /// <returns>Paging List of Album</returns>
    /// <exception cref="AuthAccessTokenRequiredException"></exception>
    Task<Paging<Album>> LookupArtistAlbumsAsync(
        string itemId,
        IncludeGroup includeGroup = null,
        string market = null,
        Page page = null);

    /// <summary>
    /// Lookup Artist's Top Tracks
    /// </summary>
    /// <param name="itemId">(Required) The Spotify ID for the artist.</param>
    /// <param name="market">(Required) A country: an ISO 3166-1 alpha-2 country code or the string from_token</param>
    /// <returns>Lookup Response</returns>
    /// <exception cref="AuthAccessTokenRequiredException"></exception>
    Task<LookupResponse> LookupArtistTopTracksAsync(
        string itemId,
        string market);

    /// <summary>
    /// Lookup Artist's Related Artists
    /// </summary>
    /// <param name="itemId">(Required) The Spotify ID for the artist.</param>
    /// <returns>Lookup Response</returns>
    /// <exception cref="AuthAccessTokenRequiredException"></exception>
    Task<LookupResponse> LookupArtistRelatedArtistsAsync(
        string itemId);

    /// <summary>
    /// Lookup All Categories
    /// </summary>
    /// <param name="country">(Optional) A country: an ISO 3166-1 alpha-2 country code. </param>
    /// <param name="locale">(Optional) The desired language, consisting of a lowercase ISO 639-1 language code and an uppercase ISO 3166-1 alpha-2 country code, joined by an underscore</param>
    /// <param name="page">(Optional) Limit: The maximum number of categories to return. Default: 20. Minimum: 1. Maximum: 50. - Offset: The index of the first item to return. Default: 0</param>
    /// <returns>Content Response</returns>
    /// <exception cref="AuthAccessTokenRequiredException"></exception>
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
    /// <exception cref="AuthAccessTokenRequiredException"></exception>
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
    /// <exception cref="AuthAccessTokenRequiredException"></exception>
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
    /// <exception cref="AuthAccessTokenRequiredException"></exception>
    Task<AvailableGenreSeeds> LookupRecommendationGenres();

    /// <summary>
    /// Get a Playlist
    /// </summary>
    /// <param name="playlistId">(Required) The Spotify ID for the playlist.</param>
    /// <param name="market">(Optional) An ISO 3166-1 alpha-2 country code or the string from_token</param>
    /// <param name="fields">(Optional) Filters for the query: a comma-separated list of the fields to return. If omitted, all fields are returned.</param>
    /// <param name="additionalTypes">(Optional) List of item types that your client supports besides the default track type. Valid types are track and episode. An unsupported type in the response is expected to be represented as null value in the item field. This parameter was introduced to allow existing clients to maintain their current behaviour and might be deprecated in the future.</param>
    /// <returns>Playlist Object</returns>
    /// <exception cref="AuthAccessTokenRequiredException"></exception>
    Task<Playlist> LookupPlaylistAsync(
                string playlistId,
                string market = null,
                string fields = null,
                List<string> additionalTypes = null);

    /// <summary>
    /// Get a Playlist's Items
    /// </summary>
    /// <param name="id">(Required) The Spotify ID for the playlist.</param>
    /// <param name="market">(Optional) An ISO 3166-1 alpha-2 country code or the string from_token</param>
    /// <param name="page">(Optional) Limit: The maximum number of items to return. Default: 100. Minimum: 1. Maximum: 100. - Offset: The index of the first item to return. Default: 0</param>
    /// <param name="fields">(Optional) Filters for the query: a comma-separated list of the fields to return. If omitted, all fields are returned.</param>
    /// <param name="additionalTypes">(Optional) List of item types that your client supports besides the default track type. Valid types are track and episode. An unsupported type in the response is expected to be represented as null value in the item field. This parameter was introduced to allow existing clients to maintain their current behaviour and might be deprecated in the future.</param>
    /// <returns>Paging List of Playlist Track Object</returns>
    /// <exception cref="AuthAccessTokenRequiredException"></exception>
    Task<Paging<PlaylistTrack>> LookupPlaylistItemsAsync(
        string id,
        string market = null,
        Page page = null,
        string fields = null,
        List<string> additionalTypes = null);

    /// <summary>
    /// Get Available Markets
    /// </summary>
    /// <returns>List of Markets where Spotify is available</returns>
    /// <exception cref="AuthAccessTokenRequiredException"></exception>
    Task<AvailableMarkets> LookupAvailableMarkets();
    #endregion Public Methods

    #region Authenticate
    /// <summary>
    /// Auth User - Authorisation Code Flow
    /// </summary>
    /// <param name="redirectUri">(Required) Redirect Uri</param>
    /// <param name="state">(Recommended) State used to mitigate cross-site request forgery attacks</param>
    /// <param name="scope">(Optional) Authorisation Scopes</param>
    /// <param name="showDialog">(Optional) Whether or not to force the user to approve the app again if they’ve already done so.</param>
    /// <returns>Uri</returns>
    Uri AuthUser(
        Uri redirectUri,
        string state,
        Scope scope,
        bool showDialog = false);

    /// <summary>
    /// Auth User - Authorisation Code Flow
    /// </summary>
    /// <param name="responseUri">(Required) Response Uri</param>
    /// <param name="redirectUri">(Required) Redirect Uri</param>
    /// <param name="state">(Recommended) State used to mitigate cross-site request forgery attacks</param>
    /// <returns>AccessToken on Success, Null if Not</returns>
    /// <exception cref="AuthCodeValueException">AuthCodeValueException</exception>
    /// <exception cref="AuthCodeStateException">AuthCodeStateException</exception>
    Task<AccessToken> AuthUserAsync(
        Uri responseUri,
        Uri redirectUri,
        string state);

    /// <summary>
    /// Auth User - Authorisation Code Flow with PKCE
    /// </summary>
    /// <param name="redirectUri">(Required) Redirect Uri</param>
    /// <param name="state">(Recommended) State used to mitigate cross-site request forgery attacks</param>
    /// <param name="scope">(Optional) Authorisation Scopes</param>
    /// <param name="codeVerifier">Generated Code Verifier for Proof Key For Code Exchange</param>
    /// <param name="showDialog">(Optional) Whether or not to force the user to approve the app again if they’ve already done so.</param>
    /// <returns>Uri</returns>
    Uri AuthUser(
        Uri redirectUri,
        string state,
        Scope scope,
        out string codeVerifier,
        bool showDialog = false);

    /// <summary>
    /// Auth User - Authorisation Code Flow with PKCE
    /// </summary>
    /// <param name="responseUri">(Required) Response Uri</param>
    /// <param name="redirectUri">(Required) Redirect Uri</param>
    /// <param name="state">(Recommended) State used to mitigate cross-site request forgery attacks</param>
    /// <param name="codeVerifier">Generated Code Verifier for Proof Key For Code Exchange</param>
    /// <returns>AccessToken on Success, Null if Not</returns>
    /// <exception cref="AuthCodeVerifierRequiredException">AuthCodeVerifierRequiredException</exception>
    /// <exception cref="AuthCodeValueException">AuthCodeValueException</exception>
    /// <exception cref="AuthCodeStateException">AuthCodeStateException</exception>
    Task<AccessToken> AuthUserAsync(
        Uri responseUri,
        Uri redirectUri,
        string state,
        string codeVerifier);

    /// <summary>
    /// Auth - Client Credentials Flow
    /// </summary>
    /// <returns>AccessToken on Success, Null if Not</returns>
    Task<AccessToken> AuthAsync();

    /// <summary>
    /// Auth User Implicit - Implicit Grant Flow
    /// </summary>
    /// <param name="redirectUri">(Required) Redirect Uri</param>
    /// <param name="state">(Recommended) State used to mitigate cross-site request forgery attacks</param>
    /// <param name="scope">(Optional) Authorisation Scopes</param>
    /// <param name="showDialog">(Optional) Whether or not to force the user to approve the app again if they’ve already done so.</param>
    /// <returns>Uri</returns>
    Uri AuthUserImplicit(
        Uri redirectUri,
        string state,
        Scope scope,
        bool showDialog = false);

    /// <summary>
    /// Auth User Implicit - Implicit Grant Flow
    /// </summary>
    /// <param name="responseUri">(Required) Response Uri</param>
    /// <param name="redirectUri">(Required) Redirect Uri</param>
    /// <param name="state">(Recommended) State used to mitigate cross-site request forgery attacks</param>
    /// <returns>AccessToken on Success, Null if Not</returns>
    /// <exception cref="AuthTokenValueException">AuthCodeValueException</exception>
    /// <exception cref="AuthTokenStateException">AuthCodeStateException</exception>
    AccessToken AuthUserImplicit(
        Uri responseUri,
        Uri redirectUri,
        string state);
    #endregion Authenticate

    #region Authenticated Follow API
    /// <summary>
    /// Get Following State for Artists/Users
    /// <para>Scopes: FollowRead</para>
    /// </summary>
    /// <param name="itemIds">(Required) List of the artist or the user Spotify IDs to check.</param>
    /// <param name="followType">(Required) Either artist or user.</param>
    /// <returns>List of true or false values</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    Task<Bools> AuthLookupFollowingStateAsync(
        List<string> itemIds,
        FollowType followType);

    /// <summary>
    /// Check if Users Follow a Playlist
    /// <para>Scopes: PlaylistReadPrivate</para>
    /// </summary>
    /// <param name="itemIds">(Required) List of Spotify User IDs, the ids of the users that you want to check to see if they follow the playlist. Maximum: 5 ids.</param>
    /// <param name="playlistId">(Required) The Spotify ID of the playlist.</param>
    /// <returns>List of true or false values</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    Task<Bools> AuthLookupUserFollowingPlaylistAsync(
        List<string> itemIds,
        string playlistId);

    /// <summary>
    /// Follow Artists or Users
    /// <para>Scopes: FollowModify</para>
    /// </summary>
    /// <param name="itemIds">(Required) List of the artist or the user Spotify IDs.</param>
    /// <param name="followType">(Required) Either artist or user</param>
    /// <returns>Status Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    Task<Status> AuthFollowAsync(
        List<string> itemIds,
        FollowType followType);

    /// <summary>
    /// Follow a Playlist
    /// <para>Scopes: FollowModify</para>
    /// </summary>
    /// <param name="playlistId">(Required) The Spotify ID of the playlist. Any playlist can be followed, regardless of its public/private status, as long as you know its playlist ID.</param>
    /// <param name="isPublic">(Optional) Defaults to true. If true the playlist will be included in user’s public playlists, if false it will remain private. To be able to follow playlists privately, the user must have granted the playlist-modify-private scope.</param>
    /// <returns>Status Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    Task<Status> AuthFollowPlaylistAsync(
        string playlistId,
        bool isPublic = true);

    /// <summary>
    /// Get User's Followed Artists
    /// <para>Scopes: FollowRead</para>
    /// </summary>
    /// <param name="cursor">(Optional) Limit: The maximum number of items to return. Default: 20. Minimum: 1. Maximum: 50. - After: The last artist ID retrieved from the previous request.</param>
    /// <returns>CursorPaging of Artist Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    Task<CursorPaging<Artist>> AuthLookupFollowedArtistsAsync(
        Cursor cursor = null);

    /// <summary>
    /// Unfollow Artists or Users
    /// <para>Scopes: FollowModify</para>
    /// </summary>
    /// <param name="itemIds">(Required) List of the artist or the user Spotify IDs.</param>
    /// <param name="followType">(Required) Either artist or user</param>
    /// <returns>Status Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    Task<Status> AuthUnfollowAsync(
        List<string> itemIds,
        FollowType followType);

    /// <summary>
    /// Unfollow Playlist
    /// <para>Scopes: PlaylistModifyPublic, PlaylistModifyPrivate</para>
    /// </summary>
    /// <param name="playlistId">(Required) The Spotify ID of the playlist that is to be no longer followed.</param>
    /// <returns>Status Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    Task<Status> AuthUnfollowPlaylistAsync(
        string playlistId);
    #endregion Authenticated Follow API

    #region Authenticated Playlists API
    /// <summary>
    /// Add Items to a Playlist
    /// <para>Scopes: PlaylistModifyPublic, PlaylistModifyPrivate</para>
    /// </summary>
    /// <param name="playlistId">(Required) The Spotify ID for the playlist.</param>
    /// <param name="uris">(Optional) List of Spotify URIs to add, can be track or episode URIs</param>
    /// <param name="position">(Optional) The position to insert the tracks, a zero-based index. If omitted, the items will be appended to the playlist. Items are added in the order they are listed.</param>
    /// <returns>Snapshot Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    Task<Snapshot> AuthAddTracksToPlaylistAsync(
        string playlistId,
        UriListRequest uris = null,
        int? position = null);

    /// <summary>
    /// Remove Items from a Playlist
    /// <para>Scopes: PlaylistModifyPublic, PlaylistModifyPrivate</para>
    /// </summary>
    /// <param name="playlistId">(Required) The Spotify ID for the playlist.</param>
    /// <param name="request">(Optional) Tracks: An array of objects containing Spotify URIs of the tracks or episodes to remove. Snapshot ID : The playlist’s snapshot ID against which you want to make the changes. The API will validate that the specified tracks exist and in the specified positions and make the changes, even if more recent changes have been made to the playlist.</param>
    /// <returns>Snapshot Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    Task<Snapshot> AuthRemoveTracksFromPlaylistAsync(
        string playlistId,
        PlaylistTracksRequest request = null);

    /// <summary>
    /// Get a Playlist Cover Image
    /// </summary>
    /// <param name="playlistId">(Required) The Spotify ID for the playlist.</param>
    /// <returns>Image Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    Task<List<Image>> AuthGetPlaylistCoverImageAsync(
        string playlistId);

    /// <summary>
    /// Upload a Custom Playlist Cover Image
    /// <para>Scopes: UserGeneratedContentImageUpload, PlaylistModifyPublic, PlaylistModifyPrivate</para>
    /// </summary>
    /// <param name="playlistId">(Required) The Spotify ID for the playlist.</param>
    /// <param name="file">(Required) JPEG Image File Bytes</param>
    /// <returns>Status Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    Task<Status> AuthUploadCustomPlaylistImageAsync(
        string playlistId,
        byte[] file);

    /// <summary>
    /// Get a List of Current User's Playlists
    /// <para>Scopes: PlaylistReadPrivate, PlaylistReadCollaborative</para>
    /// </summary>
    /// <param name="cursor">(Optional) Limit: The maximum number of playlists to return. Default: 20. Minimum: 1. Maximum: 50. - The index of the first playlist to return. Default: 0 (the first object). Maximum offset: 100. Use with limit to get the next set of playlists.</param>
    /// <returns>CursorPaging of Simplified Playlist Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    Task<CursorPaging<SimplifiedPlaylist>> AuthLookupUserPlaylistsAsync(
        Cursor cursor = null);

    /// <summary>
    /// Change a Playlist's Details
    /// <para>Scopes: PlaylistModifyPublic, PlaylistModifyPrivate</para>
    /// </summary>
    /// <param name="playlistId">(Required) The Spotify ID for the playlist.</param>
    /// <param name="request">(Optional) Playlist Request Object</param>
    /// <returns>Status Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    Task<Status> AuthChangePlaylistDetailsAsync(
        string playlistId,
        PlaylistRequest request);

    /// <summary>
    /// Get a List of a User's Playlists
    /// <para>Scopes: PlaylistReadPrivate, PlaylistReadCollaborative</para>
    /// </summary>
    /// <param name="userId">(Required) The user’s Spotify user ID.</param>
    /// <param name="cursor">(Optional) Limit: The maximum number of playlists to return. Default: 20. Minimum: 1. Maximum: 50. - Offset: The index of the first playlist to return. Default: 0 (the first object). Maximum offset: 100</param>
    /// <returns>CursorPaging of Simplified Playlist Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    Task<CursorPaging<SimplifiedPlaylist>> AuthLookupUserPlaylistsAsync(
        string userId,
        Cursor cursor = null);

    /// <summary>
    /// Replace a Playlist's Tracks
    /// <para>Scopes: PlaylistModifyPublic, PlaylistModifyPrivate</para>
    /// </summary>
    /// <param name="playlistId">(Required) The Spotify ID for the playlist.</param>
    /// <param name="uris">(Optional) Uri List Request.</param>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    Task<Status> AuthReplacePlaylistTracksAsync(
        string playlistId,
        UriListRequest uris);

    /// <summary>
    /// Create a Playlist
    /// <para>Scopes: PlaylistModifyPublic, PlaylistModifyPrivate</para>
    /// </summary>
    /// <param name="userId">(Required) The user’s Spotify user ID.</param>
    /// <param name="request">(Required) Playlist Request</param>
    /// <returns>Playlist Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    Task<Playlist> AuthCreatePlaylistAsync(
        string userId,
        PlaylistRequest request);

    /// <summary>
    /// Reorder a Playlist's Tracks
    /// <para>Scopes: PlaylistModifyPublic, PlaylistModifyPrivate</para>
    /// </summary>
    /// <param name="playlistId">(Required) The Spotify ID for the playlist.</param>
    /// <param name="request">(Required) Playlist Reorder Request</param>
    /// <returns>Snapshot Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    Task<Snapshot> AuthReorderPlaylistTracksAsync(
        string playlistId,
        PlaylistReorderRequest request);
    #endregion Authenticated Playlists API 

    #region Authenticated Library API
    /// <summary>
    /// Check User's Saved Albums
    /// <para>Scopes: LibraryRead</para>
    /// </summary>
    /// <param name="itemIds">(Required) List of the Spotify IDs for the albums</param>
    /// <returns>List of true or false values</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    Task<Bools> AuthLookupCheckUserSavedAlbumsAsync(
        List<string> itemIds);

    /// <summary>
    /// Save Tracks for User
    /// <para>Scopes: LibraryModify</para>
    /// </summary>
    /// <param name="itemIds">(Required) List of the Spotify IDs for the tracks</param>
    /// <returns>Status Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    Task<Status> AuthSaveUserTracksAsync(
         List<string> itemIds);

    /// <summary>
    /// Remove Albums for Current User
    /// <para>Scopes: LibraryModify</para>
    /// </summary>
    /// <param name="itemIds">(Required) List of the Spotify IDs for the albums</param>
    /// <returns>Status Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    Task<Status> AuthRemoveUserAlbumsAsync(
         List<string> itemIds);

    /// <summary>
    /// Save Albums for Current User
    /// <para>Scopes: LibraryModify</para>
    /// </summary>
    /// <param name="itemIds">(Required) List of the Spotify IDs for the albums</param>
    /// <returns>Status Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    Task<Status> AuthSaveUserAlbumsAsync(
         List<string> itemIds);

    /// <summary>
    /// Remove User's Saved Tracks
    /// <para>Scopes: LibraryModify</para>
    /// </summary>
    /// <param name="itemIds">(Required) List of the Spotify IDs for the tracks</param>
    /// <returns>Status Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    Task<Status> AuthRemoveUserTracksAsync(
         List<string> itemIds);

    /// <summary>
    /// Get User's Saved Albums
    /// <para>Scopes: LibraryRead</para>
    /// </summary>
    /// <param name="market">(Optional) An ISO 3166-1 alpha-2 country code or the string from_token. Provide this parameter if you want to apply Track Relinking.</param>
    /// <param name="cursor">(Optional) Limit: The maximum number of objects to return. Default: 20. Minimum: 1. Maximum: 50. - Offset: The index of the first object to return. Default: 0 (i.e., the first object). Use with limit to get the next set of objects.</param>
    /// <returns>Cursor Paging of Saved Album Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    Task<CursorPaging<SavedAlbum>> AuthLookupUserSavedAlbumsAsync(
        string market = null,
        Cursor cursor = null);

    /// <summary>
    /// Get User's Saved Tracks
    /// <para>Scopes: LibraryRead</para>
    /// </summary>
    /// <param name="market">(Optional) An ISO 3166-1 alpha-2 country code or the string from_token. Provide this parameter if you want to apply Track Relinking.</param>
    /// <param name="cursor">(Optional) Limit: The maximum number of objects to return. Default: 20. Minimum: 1. Maximum: 50. - Offset: The index of the first object to return. Default: 0 (i.e., the first object). Use with limit to get the next set of objects.</param>
    /// <returns>Cursor Paging of Saved Track Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    Task<CursorPaging<SavedTrack>> AuthLookupUserSavedTracksAsync(
        string market = null,
        Cursor cursor = null);

    /// <summary>
    /// Check User's Saved Tracks
    /// <para>Scopes: LibraryRead</para>
    /// </summary>
    /// <param name="itemIds">(Required) List of the Spotify IDs for the tracks</param>
    /// <returns>List of true or false values</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    Task<Bools> AuthLookupCheckUserSavedTracksAsync(
        List<string> itemIds);

    /// <summary>
    /// Get User's Saved Shows
    /// <para>Scopes: LibraryRead</para>
    /// </summary>
    /// <param name="cursor">(Optional) Limit: The maximum number of objects to return. Default: 20. Minimum: 1. Maximum: 50. - Offset: The index of the first object to return. Default: 0 (i.e., the first object). Use with limit to get the next set of objects.</param>
    /// <returns>Cursor Paging of Saved Show Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    Task<CursorPaging<SavedShow>> AuthLookupUserSavedShowsAsync(
        Cursor cursor = null);

    /// <summary>
    /// Check User's Saved Shows
    /// <para>Scopes: LibraryRead</para>
    /// </summary>
    /// <param name="itemIds">(Required) List of the Spotify IDs for the shows</param>
    /// <returns>List of true or false values</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    Task<Bools> AuthLookupCheckUserSavedShowsAsync(
        List<string> itemIds);

    /// <summary>
    /// Save Shows for Current User
    /// <para>Scopes: LibraryModify</para>
    /// </summary>
    /// <param name="itemIds">(Required) List of the Spotify IDs for the shows</param>
    /// <returns>Status Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    Task<Status> AuthSaveUserShowsAsync(
        List<string> itemIds);

    /// <summary>
    /// Remove User's Saved Shows
    /// <para>Scopes: LibraryModify</para>
    /// </summary>
    /// <param name="itemIds">(Required) List of the Spotify IDs for the shows</param>
    /// <param name="market">(Optional) An ISO 3166-1 alpha-2 country code. If a country code is specified, only shows that are available in that market will be removed. If a valid user access token is specified in the request header, the country associated with the user account will take priority over this parameter</param>
    /// <returns>Status Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    Task<Status> AuthRemoveUserShowsAsync(
        List<string> itemIds, string market = null);

    /// <summary>
    /// Get User's Saved Episodes
    /// <para>Scopes: LibraryRead</para>
    /// </summary>
    /// <param name="cursor">(Optional) Limit: The maximum number of objects to return. Default: 20. Minimum: 1. Maximum: 50. - Offset: The index of the first object to return. Default: 0 (i.e., the first object). Use with limit to get the next set of objects.</param>
    /// <returns>Cursor Paging of Saved Episode Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    Task<CursorPaging<SavedEpisode>> AuthLookupUserSavedEpisodesAsync(
        Cursor cursor = null);

    /// <summary>
    /// Save Episodes for User
    /// <para>Scopes: LibraryModify</para>
    /// </summary>
    /// <param name="itemIds">(Required) List of the Spotify IDs for the episodes</param>
    /// <returns>Status Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    Task<Status> AuthSaveUserEpisodesAsync(
        List<string> itemIds);

    /// <summary>
    /// Remove User's Saved Episodes
    /// <para>Scopes: LibraryModify</para>
    /// </summary>
    /// <param name="itemIds">(Required) List of the Spotify IDs for the episodes</param>
    /// <param name="market">(Optional) An ISO 3166-1 alpha-2 country code. If a country code is specified, only episodes that are available in that market will be removed. If a valid user access token is specified in the request header, the country associated with the user account will take priority over this parameter</param>
    /// <returns>Status Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    Task<Status> AuthRemoveUserEpisodesAsync(
        List<string> itemIds, string market = null);

    /// <summary>
    /// Check User's Saved Episodes
    /// <para>Scopes: LibraryRead</para>
    /// </summary>
    /// <param name="itemIds">(Required) List of the Spotify IDs for the episodes</param>
    /// <returns>List of true or false values</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    Task<Bools> AuthLookupCheckUserSavedEpisodesAsync(
        List<string> itemIds);
    #endregion Authenticated Library API

    #region Authenticated Player API
    /// <summary>
    /// Add an Item to the User's Playback Queue
    /// <para>Scopes: ConnectModifyPlaybackState</para>
    /// </summary>
    /// <param name="uri">(Required) The uri of the item to add to the queue. Must be a track or an episode uri.</param>
    /// <param name="deviceId">(Optional) The id of the device this command is targeting. If not supplied, the user’s currently active device is the target.</param>
    /// <returns>Status Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    Task<Status> AuthUserPlaybackAddToQueueAsync(
        string uri, string deviceId = null);

    /// <summary>
    /// Skip User’s Playback To Next Track
    /// <para>Scopes: ConnectModifyPlaybackState</para>
    /// </summary>
    /// <param name="deviceId">(Optional) The id of the device this command is targeting. If not supplied, the user’s currently active device is the target.</param>
    /// <returns>Status Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    Task<Status> AuthUserPlaybackNextTrackAsync(
        string deviceId = null);

    /// <summary>
    /// Seek To Position In Currently Playing Track
    /// <para>Scopes: ConnectModifyPlaybackState</para>
    /// </summary>
    /// <param name="position">(Required) The position in milliseconds to seek to. Must be a positive number. Passing in a position that is greater than the length of the track will cause the player to start playing the next song.</param>
    /// <param name="deviceId">(Optional) The id of the device this command is targeting. If not supplied, the user’s currently active device is the target.</param>
    /// <returns>Status Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    Task<Status> AuthUserPlaybackSeekTrackAsync(
         int position,
         string deviceId = null);

    /// <summary>
    /// Get a User's Available Devices
    /// <para>Scopes: ConnectReadPlaybackState</para>
    /// </summary>
    /// <returns>Devices Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    Task<Devices> AuthLookupUserPlaybackDevicesAsync();

    /// <summary>
    /// Toggle Shuffle For User’s Playback
    /// <para>Scopes: ConnectModifyPlaybackState</para>
    /// </summary>
    /// <param name="state">(Required) true : Shuffle user’s playback, false : Do not shuffle user’s playback</param>
    /// <param name="deviceId">(Optional) The id of the device this command is targeting. If not supplied, the user’s currently active device is the target.</param>
    /// <returns>Status Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    Task<Status> AuthUserPlaybackToggleShuffleAsync(
         bool state,
         string deviceId = null);

    /// <summary>
    /// Transfer a User's Playback
    /// <para>Scopes: ConnectModifyPlaybackState</para>
    /// </summary>
    /// <param name="request">(Required) Devices Request Object</param>
    /// <returns>Status Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    Task<Status> AuthUserPlaybackTransferAsync(
         DevicesRequest request);

    /// <summary>
    /// Get Current User's Recently Played Tracks
    /// <para>Scopes: ListeningRecentlyPlayed</para>
    /// </summary>
    /// <param name="cursor">(Optional) Limit: The maximum number of items to return. Default: 20. Minimum: 1. Maximum: 50. - After: A Unix timestamp in milliseconds. Returns all items after (but not including) this cursor position. If after is specified, before must not be specified. Before - (Optional) A Unix timestamp in milliseconds. Returns all items before (but not including) this cursor position. If before is specified, after must not be specified.</param>
    /// <returns>Cursor Paging of Play History Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    Task<CursorPaging<PlayHistory>> AuthLookupUserRecentlyPlayedTracksAsync(
        Cursor cursor = null);

    /// <summary>
    /// Start/Resume a User's Playback
    /// <para>Scopes: ConnectModifyPlaybackState</para>
    /// </summary>
    /// <param name="request">(Optional) Playback Request Object</param>
    /// <param name="deviceId">(Optional) The id of the device this command is targeting. If not supplied, the user’s currently active device is the target.</param>
    /// <returns>Status Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    Task<Status> AuthUserPlaybackStartResumeAsync(
        PlaybackRequest request = null,
        string deviceId = null);

    /// <summary>
    /// Set Repeat Mode On User’s Playback
    /// <para>Scopes: ConnectModifyPlaybackState</para>
    /// </summary>
    /// <param name="state">(Required) track, context or off. track will repeat the current track. context will repeat the current context. off will turn repeat off.</param>
    /// <param name="deviceId">(Optional) The id of the device this command is targeting. If not supplied, the user’s currently active device is the target.</param>
    /// <returns>Status Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    Task<Status> AuthUserPlaybackSetRepeatModeAsync(
        RepeatState state,
        string deviceId = null);

    /// <summary>
    /// Pause a User's Playback
    /// <para>Scopes: ConnectModifyPlaybackState</para>
    /// </summary>
    /// <param name="deviceId">(Optional) The id of the device this command is targeting. If not supplied, the user’s currently active device is the target.</param>
    /// <returns>Status Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    Task<Status> AuthUserPlaybackPauseAsync(
        string deviceId = null);

    /// <summary>
    /// Skip User’s Playback To Previous Track
    /// <para>Scopes: ConnectModifyPlaybackState</para>
    /// </summary>
    /// <param name="deviceId">(Optional) The id of the device this command is targeting. If not supplied, the user’s currently active device is the target.</param>
    /// <returns>Status Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    Task<Status> AuthUserPlaybackPreviousTrackAsync(
        string deviceId = null);

    /// <summary>
    /// Get Information About The User's Current Playback
    /// <para>Scopes: ConnectReadPlaybackState</para>
    /// </summary>
    /// <param name="market">(Optional) An ISO 3166-1 alpha-2 country code or the string from_token. Provide this parameter if you want to apply Track Relinking.</param>
    /// <param name="additionalTypes">(Optional) List of item types that your client supports besides the default track type. Valid types are track and episode. An unsupported type in the response is expected to be represented as null value in the item field. This parameter was introduced to allow existing clients to maintain their current behaviour and might be deprecated in the future.</param>
    /// <returns>Currently Playing Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    Task<CurrentlyPlaying> AuthLookupUserPlaybackCurrentAsync(
        string market = null, List<string> additionalTypes = null);

    /// <summary>
    /// Get the User's Currently Playing Track
    /// <para>Scopes: ConnectReadCurrentlyPlaying, ConnectReadPlaybackState</para>
    /// </summary>
    /// <param name="market">(Optional) An ISO 3166-1 alpha-2 country code or the string from_token. Provide this parameter if you want to apply Track Relinking.</param>
    /// <param name="additionalTypes">(Optional) List of item types that your client supports besides the default track type. Valid types are track and episode. An unsupported type in the response is expected to be represented as null value in the item field. This parameter was introduced to allow existing clients to maintain their current behaviour and might be deprecated in the future.</param>
    /// <returns>Simplified Currently Playing Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    Task<SimplifiedCurrentlyPlaying> AuthLookupUserPlaybackCurrentTrackAsync(
        string market = null, List<string> additionalTypes = null);

    /// <summary>
    /// Set Volume For User's Playback
    /// <para>Scopes: ConnectModifyPlaybackState</para>
    /// </summary>
    /// <param name="percent">(Required) The volume to set. Must be a value from 0 to 100 inclusive.</param>
    /// <param name="deviceId">(Optional) The id of the device this command is targeting. If not supplied, the user’s currently active device is the target.</param>
    /// <returns>Status Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    Task<Status> AuthUserPlaybackSetVolumeAsync(
         int percent,
         string deviceId = null);

    /// <summary>
    /// Get the User's Queue
    /// <para>Scopes: ConnectReadCurrentlyPlaying</para>
    /// </summary>
    /// <returns>Queue Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    Task<Queue> AuthLookupUserQueueAsync();
    #endregion Authenticated Player API

    #region Authenticated Personalisation API
    /// <summary>
    /// Get a User's Top Artists
    /// <para>Scopes: ListeningTopRead</para>
    /// </summary>
    /// <param name="timeRange">(Optional) Over what time frame the affinities are computed. Long Term: alculated from several years of data and including all new data as it becomes available, Medium Term: (Default) approximately last 6 months, Short Term: approximately last 4 weeks</param>
    /// <param name="cursor">(Optional) Limit: The number of entities to return. Default: 20. Minimum: 1. Maximum: 50. For example - Offset: he index of the first entity to return. Default: 0. Use with limit to get the next set of entities.</param>
    /// <returns>Cursor Paging of Artist Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    Task<CursorPaging<Artist>> AuthLookupUserTopArtistsAsync(
        TimeRange? timeRange = null,
        Cursor cursor = null);

    /// <summary>
    /// Get a User's Top Tracks
    /// <para>Scopes: ListeningTopRead</para>
    /// </summary>
    /// <param name="timeRange">(Optional) Over what time frame the affinities are computed. Long Term: alculated from several years of data and including all new data as it becomes available, Medium Term: (Default) approximately last 6 months, Short Term: approximately last 4 weeks</param>
    /// <param name="cursor">(Optional) Limit: The number of entities to return. Default: 20. Minimum: 1. Maximum: 50. For example - Offset: he index of the first entity to return. Default: 0. Use with limit to get the next set of entities.</param>
    /// <returns>Cursor Paging of Track Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    Task<CursorPaging<Track>> AuthLookupUserTopTracksAsync(
        TimeRange? timeRange = null,
        Cursor cursor = null);
    #endregion Authenticated Personalisation API

    #region Authenticated User Profile API
    /// <summary>
    /// Get a User's Profile
    /// </summary>
    /// <param name="userId">The user’s Spotify user ID.</param>
    /// <returns>Public User Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    Task<PublicUser> AuthLookupUserProfileAsync(
        string userId);

    /// <summary>
    /// Get Current User's Profile
    /// <para>Scopes: UserReadEmail, UserReadPrivate</para>
    /// </summary>
    /// <returns>Private User Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    Task<PrivateUser> AuthLookupUserProfileAsync();
    #endregion Authenticated User Profile API
}