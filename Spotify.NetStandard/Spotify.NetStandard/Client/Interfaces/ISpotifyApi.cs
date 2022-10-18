namespace Spotify.NetStandard.Client.Interfaces;

/// <summary>
/// Spotify API
/// </summary>
public interface ISpotifyApi
{
    #region Public Properties
    /// <summary>
    /// Spotify Client
    /// </summary>
    ISpotifyClient Client { get; }
    #endregion Public Properties

    #region Authentication
    /// <summary>
    /// Get Authorisation Code Auth Uri - Authorisation Code Flow  with Proof Key For Code Exchange (PKCE)
    /// </summary>
    /// <param name="redirectUri">Redirect Uri</param>
    /// <param name="state">State used to mitigate cross-site request forgery attacks</param>
    /// <param name="scope">Authorisation Scopes</param>
    /// <param name="codeVerifier">Generated Code Verifier for Proof Key For Code Exchange</param>
    /// <param name="showDialog">(Optional) Whether or not to force the user to approve the app again if they’ve already done so.</param>
    /// <returns>Uri</returns>
    Uri GetAuthorisationCodeAuthUri(
        Uri redirectUri,
        string state,
        Scope scope,
        out string codeVerifier,
        bool showDialog = false);

    /// <summary>
    /// Get Authorisation Code Auth Token - Authorization Code Flow with Proof Key For Code Exchange (PKCE)
    /// </summary>
    /// <param name="responseUri">Response Uri</param>
    /// <param name="redirectUri">Redirect Uri</param>
    /// <param name="state">State used to mitigate cross-site request forgery attacks</param>
    /// <param name="codeVerifier">Provide Code Verifier for Proof Key For Code Exchange</param>
    /// <returns>AccessToken on Success, Null if Not</returns>
    /// <exception cref="AuthCodeVerifierRequiredException">AuthCodeVerifierRequiredException</exception>
    /// <exception cref="AuthCodeValueException">AuthCodeValueException</exception>
    /// <exception cref="AuthCodeStateException">AuthCodeStateException</exception>
    Task<AccessToken> GetAuthorisationCodeAuthTokenAsync(
        Uri responseUri,
        Uri redirectUri,
        string state,
        string codeVerifier);

    /// <summary>
    /// Get Client Credentials Auth Token - Client Credentials Flow
    /// </summary>
    /// <returns>AccessToken on Success, Null if Not</returns>
    Task<AccessToken> GetClientCredentialsAuthTokenAsync();

    /// <summary>
    /// Get Implicit Grant Auth Uri - Implicit Grant Flow
    /// </summary>
    /// <param name="redirectUri">Redirect Uri</param>
    /// <param name="state">State used to mitigate cross-site request forgery attacks</param>
    /// <param name="scope">Authorisation Scopes</param>
    /// <param name="showDialog">(Optional) Whether or not to force the user to approve the app again if they’ve already done so.</param>
    /// <returns>Uri</returns>
    Uri GetImplicitGrantAuthUri(
        Uri redirectUri,
        string state,
        Scope scope,
        bool showDialog = false);

    /// <summary>
    /// Get Implicit Grant Auth Token - Implicit Grant Flow
    /// </summary>
    /// <param name="responseUri">Response Uri</param>
    /// <param name="redirectUri">Redirect Uri</param>
    /// <param name="state">State used to mitigate cross-site request forgery attacks</param>
    /// <returns>AccessToken on Success, Null if Not</returns>
    /// <exception cref="AuthTokenValueException">AuthTokenValueException</exception>
    /// <exception cref="AuthTokenStateException">AuthTokenStateException</exception>
    AccessToken GetImplicitGrantAuthToken(
        Uri responseUri,
        Uri redirectUri,
        string state);
    #endregion Authentication

    #region Albums
    /// <summary>
    /// Get an Album
    /// </summary>
    /// <param name="id">(Required) The Spotify ID of the album.</param>
    /// <param name="market">(Optional) The market you’d like to request. An ISO 3166-1 alpha-2 country code.</param>
    /// <returns>Album Object</returns>
    /// <exception cref="AuthAccessTokenRequiredException"></exception>
    Task<Album> GetAlbumAsync(
        string id,
        string market = null);

    /// <summary>
    /// Get Several Albums
    /// </summary>
    /// <param name="ids">(Required) List of the Spotify IDs for the albums. Maximum: 20 IDs.</param>
    /// <param name="market">(Optional) An ISO 3166-1 alpha-2 country code or the string from_token. Provide this parameter if you want to apply Track Relinking.</param>
    /// <returns>List of Album Object</returns>
    /// <exception cref="AuthAccessTokenRequiredException"></exception>
    Task<List<Album>> GetMultipleAlbumsAsync(
        List<string> ids,
        string market = null);

    /// <summary>
    /// Get an Album's Tracks
    /// </summary>
    /// <param name="id">(Required) The Spotify ID of the album.</param>      
    /// <param name="market">(Optional) An ISO 3166-1 alpha-2 country code or the string from_token. Provide this parameter if you want to apply Track Relinking.</param>
    /// <param name="page">(Optional) Limit: The maximum number of tracks to return. Default: 20. Minimum: 1. Maximum: 50. - Offset: The index of the first track to return</param>
    /// <returns>Paging of Track Object</returns>
    /// <exception cref="AuthAccessTokenRequiredException"></exception>
    Task<Paging<Track>> GetAlbumTracksAsync(
        string id,
        string market = null,
        Page page = null);

    /// <summary>
    /// Get Saved Albums
    /// <para>Scopes: LibraryRead</para>
    /// </summary>
    /// <param name="market">(Optional) An ISO 3166-1 alpha-2 country code or the string from_token. Provide this parameter if you want to apply Track Relinking.</param>
    /// <param name="cursor">(Optional) Limit: The maximum number of objects to return. Default: 20. Minimum: 1. Maximum: 50. - Offset: The index of the first object to return. Default: 0 (i.e., the first object). Use with limit to get the next set of objects.</param>
    /// <returns>Cursor Paging of Saved Album Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    Task<CursorPaging<SavedAlbum>> GetUserSavedAlbumsAsync(
        string market = null,
        Cursor cursor = null);

    /// <summary>
    /// Save Albums for Current User
    /// <para>Scopes: LibraryModify</para>
    /// </summary>
    /// <param name="itemIds">(Required) List of the Spotify IDs for the albums</param>
    /// <returns>Status Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    Task<Status> SaveUserAlbumsAsync(
        List<string> itemIds);

    /// <summary>
    /// Remove Albums for Current User
    /// <para>Scopes: LibraryModify</para>
    /// </summary>
    /// <param name="itemIds">(Required) List of the Spotify IDs for the albums</param>
    /// <returns>Status Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    Task<Status> RemoveUserAlbumsAsync(
        List<string> itemIds);

    /// <summary>
    /// Check User's Saved Albums
    /// <para>Scopes: LibraryRead</para>
    /// </summary>
    /// <param name="itemIds">(Required) List of the Spotify IDs for the albums</param>
    /// <returns>List of true or false values</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    Task<Bools> CheckUserSavedAlbumsAsync(
        List<string> itemIds);

    /// <summary>
    /// Get All New Releases
    /// </summary>
    /// <param name="country">(Optional) A country: an ISO 3166-1 alpha-2 country code. </param>
    /// <param name="page">(Optional) Limit: The maximum number of items to return. Default: 20. Minimum: 1. Maximum: 50. - Offset: The index of the first item to return. Default: 0</param>
    /// <returns>Paging List of Album Object</returns>
    /// <exception cref="AuthAccessTokenRequiredException"></exception>
    Task<Paging<Album>> GetAllNewReleasesAsync(
        string country = null,
        Page page = null);
    #endregion Albums

    #region Artists
    /// <summary>
    /// Get an Artist
    /// </summary>
    /// <param name="id">(Required) The Spotify ID of the artist.</param>
    /// <returns>Artist Object</returns>
    /// <exception cref="AuthAccessTokenRequiredException"></exception>
    Task<Artist> GetArtistAsync(
        string id);

    /// <summary>
    /// Get Several Artists
    /// </summary>
    /// <param name="ids">(Required) List of the Spotify IDs for the artists. Maximum: 50 IDs.</param>
    /// <returns>List of Artist Object</returns>
    /// <exception cref="AuthAccessTokenRequiredException"></exception>
    Task<List<Artist>> GetMultipleArtistsAsync(
    List<string> ids);

    /// <summary>
    /// Get an Artist's Albums
    /// </summary>
    /// <param name="id">(Required) The Spotify ID for the artist.</param>
    /// <param name="includeGroup">(Optional) Filters the response. If not supplied, all album types will be returned</param>
    /// <param name="market">(Optional) An ISO 3166-1 alpha-2 country code or the string from_token</param>
    /// <param name="page">(Optional) Limit: The number of album objects to return. Default: 20. Minimum: 1. Maximum: 50. - Offset: The index of the first album to return. Default: 0</param>
    /// <returns>Paging List of Album Object</returns>
    /// <exception cref="AuthAccessTokenRequiredException"></exception>
    Task<Paging<Album>> GetArtistAlbumsAsync(
        string id,
        IncludeGroup includeGroup = null,
        string market = null,
        Page page = null);

    /// <summary>
    /// Get an Artist's Top Tracks
    /// </summary>
    /// <param name="id">(Required) The Spotify ID for the artist.</param>
    /// <param name="market">(Required) An ISO 3166-1 alpha-2 country code or the string from_token</param>
    /// <returns>List of Track Object</returns>
    /// <exception cref="AuthAccessTokenRequiredException"></exception>
    Task<List<Track>> GetArtistTopTracksAsync(
        string id,
        string market);

    /// <summary>
    /// Get an Artist's Related Artists
    /// </summary>
    /// <param name="id">(Requird) The Spotify ID for the artist.</param>
    /// <returns>List of Artist Object</returns>
    /// <exception cref="AuthAccessTokenRequiredException"></exception>
    Task<List<Artist>> GetArtistRelatedArtistsAsync(
        string id);
    #endregion Artists

    #region Shows
    /// <summary>
    /// Get a Show
    /// <para>(Optional) Scopes: PlaybackPositionRead for ResumePoint</para>
    /// </summary>
    /// <param name="id">(Required) The Spotify ID for the show.</param>
    /// <param name="market">(Optional) An ISO 3166-1 alpha-2 country code. If a country code is specified, only shows and episodes that are available in that market will be returned. If a valid user access token is specified in the request header, the country associated with the user account will take priority over this parameter.</param>
    /// <returns>Show Object</returns>
    /// <exception cref="AuthAccessTokenRequiredException"></exception>
    Task<Show> GetShowAsync(
        string id,
        string market = null);

    /// <summary>
    /// Get Several Shows
    /// <para>(Optional) Scopes: PlaybackPositionRead for ResumePoint</para>
    /// </summary>
    /// <param name="ids">(Required) List of the Spotify IDs for the shows. Maximum: 50 IDs.</param>
    /// <param name="market">(Optional) An ISO 3166-1 alpha-2 country code. If a country code is specified, only shows and episodes that are available in that market will be returned. If a valid user access token is specified in the request header, the country associated with the user account will take priority over this parameter.</param>
    /// <returns>List of Show Object</returns>
    /// <exception cref="AuthAccessTokenRequiredException"></exception>
    Task<List<Show>> GetMultipleShowsAsync(
        List<string> ids,
        string market = null);

    /// <summary>
    /// Get Show Episodes
    /// <para>(Optional) Scopes: PlaybackPositionRead for ResumePoint</para>
    /// </summary>
    /// <param name="id">(Required) The Spotify ID for the show</param>      
    /// <param name="market">(Optional) An ISO 3166-1 alpha-2 country code. If a country code is specified, only shows and episodes that are available in that market will be returned. If a valid user access token is specified in the request header, the country associated with the user account will take priority over this parameter.</param>
    /// <param name="page">(Optional) Limit: The maximum number of tracks to return. Default: 20. Minimum: 1. Maximum: 50. - Offset: The index of the first track to return</param>
    /// <returns>Paging of Episode Object</returns>
    /// <exception cref="AuthAccessTokenRequiredException"></exception>
    Task<Paging<SimplifiedEpisode>> GetShowEpisodesAsync(
        string id,
        string market = null,
        Page page = null);

    /// <summary>
    /// Get User's Saved Shows
    /// <para>Scopes: LibraryRead</para>
    /// </summary>
    /// <param name="cursor">(Optional) Limit: The maximum number of objects to return. Default: 20. Minimum: 1. Maximum: 50. - Offset: The index of the first object to return. Default: 0 (i.e., the first object). Use with limit to get the next set of objects.</param>
    /// <returns>Cursor Paging of Saved Show Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    Task<CursorPaging<SavedShow>> GetUserSavedShowsAsync(
        Cursor cursor = null);

    /// <summary>
    /// Save Shows for Current User
    /// <para>Scopes: LibraryModify</para>
    /// </summary>
    /// <param name="itemIds">(Required) List of the Spotify IDs for the shows</param>
    /// <returns>Status Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    Task<Status> SaveUserShowsAsync(
        List<string> itemIds);

    /// <summary>
    /// Remove User's Saved Shows
    /// <para>Scopes: LibraryModify</para>
    /// </summary>
    /// <param name="itemIds">(Required) List of the Spotify IDs for the shows</param>
    /// <param name="market">(Optional) An ISO 3166-1 alpha-2 country code. If a country code is specified, only shows that are available in that market will be removed. If a valid user access token is specified in the request header, the country associated with the user account will take priority over this parameter</param>
    /// <returns>Status Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    Task<Status> RemoveUserShowsAsync(
        List<string> itemIds, string market = null);

    /// <summary>
    /// Check User's Saved Shows
    /// <para>Scopes: LibraryRead</para>
    /// </summary>
    /// <param name="itemIds">(Required) List of the Spotify IDs for the shows</param>
    /// <returns>List of true or false values</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    Task<Bools> CheckUserSavedShowsAsync(
        List<string> itemIds);
    #endregion Shows

    #region Episodes
    /// <summary>
    /// Get an Episode
    /// <para>(Optional) Scopes: PlaybackPositionRead for ResumePoint</para>
    /// </summary>
    /// <param name="id">(Required) The Spotify ID for the episode.</param>
    /// <param name="market">(Optional) An ISO 3166-1 alpha-2 country code. If a country code is specified, only shows and episodes that are available in that market will be returned. If a valid user access token is specified in the request header, the country associated with the user account will take priority over this parameter.</param>
    /// <returns>Episode Object</returns>
    /// <exception cref="AuthAccessTokenRequiredException"></exception>
    Task<Episode> GetEpisodeAsync(
        string id,
        string market = null);

    /// <summary>
    /// Get Several Episodes
    /// <para>(Optional) Scopes: PlaybackPositionRead for ResumePoint</para>
    /// </summary>
    /// <param name="ids">(Required) List of the Spotify IDs for the episodes. Maximum: 50 ID</param>
    /// <param name="market">(Optional) An ISO 3166-1 alpha-2 country code. If a country code is specified, only shows and episodes that are available in that market will be returned. If a valid user access token is specified in the request header, the country associated with the user account will take priority over this parameter.</param>
    /// <returns>List of Episode Object</returns>
    /// <exception cref="AuthAccessTokenRequiredException"></exception>
    Task<List<Episode>> GetMultipleEpisodesAsync(
        List<string> ids,
        string market = null);

    /// <summary>
    /// Get User's Saved Episodes
    /// <para>Scopes: LibraryRead</para>
    /// </summary>
    /// <param name="cursor">(Optional) Limit: The maximum number of objects to return. Default: 20. Minimum: 1. Maximum: 50. - Offset: The index of the first object to return. Default: 0 (i.e., the first object). Use with limit to get the next set of objects.</param>
    /// <returns>Cursor Paging of Saved Episode Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    Task<CursorPaging<SavedEpisode>> GetUserSavedEpisodesAsync(
        Cursor cursor = null);

    /// <summary>
    /// Save Episodes for User
    /// <para>Scopes: LibraryModify</para>
    /// </summary>
    /// <param name="itemIds">(Required) List of the Spotify IDs for the episodes</param>
    /// <returns>Status Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
   Task<Status> SaveUserEpisodesAsync(
        List<string> itemIds);

    /// <summary>
    /// Remove User's Saved Episodes
    /// <para>Scopes: LibraryModify</para>
    /// </summary>
    /// <param name="itemIds">(Required) List of the Spotify IDs for the episodes</param>
    /// <param name="market">(Optional) An ISO 3166-1 alpha-2 country code. If a country code is specified, only episodes that are available in that market will be removed. If a valid user access token is specified in the request header, the country associated with the user account will take priority over this parameter</param>
    /// <returns>Status Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    Task<Status> RemoveUserEpisodesAsync(
        List<string> itemIds, string market = null);

    /// <summary>
    /// Check User's Saved Episodes
    /// <para>Scopes: LibraryRead</para>
    /// </summary>
    /// <param name="itemIds">(Required) List of the Spotify IDs for the episodes</param>
    /// <returns>List of true or false values</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    Task<Bools> CheckUserSavedEpisodesAsync(
        List<string> itemIds);
    #endregion Episodes

    #region Audiobooks
    /// <summary>
    /// Get an Audiobook
    /// </summary>
    /// <param name="id">(Required) The Spotify ID of the audiobook.</param>
    /// <param name="market">(Optional) The market you’d like to request. An ISO 3166-1 alpha-2 country code.</param>
    /// <returns>Audiobook Object</returns>
    /// <exception cref="AuthAccessTokenRequiredException"></exception>
    Task<Audiobook> GetAudiobookAsync(
        string id,
        string market = null);

    /// <summary>
    /// Get Several Audiobooks
    /// </summary>
    /// <param name="ids">(Required) List of the Spotify IDs for the audiobooks. Maximum: 50 IDs.</param>
    /// <param name="market">(Optional) An ISO 3166-1 alpha-2 country code or the string from_token.</param>
    /// <returns>List of Album Object</returns>
    /// <exception cref="AuthAccessTokenRequiredException"></exception>
    Task<List<Audiobook>> GetMultipleAudiobooksAsync(
        List<string> ids,
        string market = null);

    /// <summary>
    /// Get Audiobook Chapters
    /// </summary>
    /// <param name="id">(Required) The Spotify ID of the audiobook.</param>      
    /// <param name="market">(Optional) An ISO 3166-1 alpha-2 country code or the string from_token.</param>
    /// <param name="page">(Optional) Limit: The maximum number of tracks to return. Default: 20. Minimum: 1. Maximum: 50. - Offset: The index of the first track to return</param>
    /// <returns>Paging of Chapter Object</returns>
    /// <exception cref="AuthAccessTokenRequiredException"></exception>
    Task<Paging<Chapter>> GetAudiobookChaptersAsync(
        string id,
        string market = null,
        Page page = null);
    #endregion Audiobooks

    #region Chapters
    /// <summary>
    /// Get a Chapter
    /// </summary>
    /// <param name="id">(Required) The Spotify ID of the chapter.</param>
    /// <param name="market">(Optional) The market you’d like to request. An ISO 3166-1 alpha-2 country code.</param>
    /// <returns>Chapter Object</returns>
    /// <exception cref="AuthAccessTokenRequiredException"></exception>
    Task<Chapter> GetChapterAsync(
        string id,
        string market = null);

    /// <summary>
    /// Get Several Chapters
    /// </summary>
    /// <param name="ids">(Required) List of the Spotify IDs for the chapters. Maximum: 50 IDs.</param>
    /// <param name="market">(Optional) An ISO 3166-1 alpha-2 country code or the string from_token.</param>
    /// <returns>List of Chapter Object</returns>
    /// <exception cref="AuthAccessTokenRequiredException"></exception>
    Task<List<Chapter>> GetMultipleChaptersAsync(
        List<string> ids,
        string market = null);
    #endregion Chapters

    #region Tracks
    /// <summary>
    /// Get a Track
    /// </summary>
    /// <param name="id">(Required) The Spotify ID for the track.</param>
    /// <param name="market">(Optional) An ISO 3166-1 alpha-2 country code or the string from_token. Provide this parameter if you want to apply Track Relinking.</param>
    /// <returns>Track Object</returns>
    /// <exception cref="AuthAccessTokenRequiredException"></exception>
    Task<Track> GetTrackAsync(
        string id,
        string market = null);

    /// <summary>
    /// Get Several Tracks
    /// </summary>
    /// <param name="ids">(Required) List of the Spotify IDs for the tracks. Maximum: 50 IDs.</param>
    /// <param name="market">(Optional) An ISO 3166-1 alpha-2 country code or the string from_token. Provide this parameter if you want to apply Track Relinking.</param>
    /// <returns>List of Track Object</returns>
    /// <exception cref="AuthAccessTokenRequiredException"></exception>
    Task<List<Track>> GetSeveralTracksAsync(
        List<string> ids,
        string market = null);

    /// <summary>
    /// Get User's Saved Tracks
    /// <para>Scopes: LibraryRead</para>
    /// </summary>
    /// <param name="market">(Optional) An ISO 3166-1 alpha-2 country code or the string from_token. Provide this parameter if you want to apply Track Relinking.</param>
    /// <param name="cursor">(Optional) Limit: The maximum number of objects to return. Default: 20. Minimum: 1. Maximum: 50. - Offset: The index of the first object to return. Default: 0 (i.e., the first object). Use with limit to get the next set of objects.</param>
    /// <returns>Cursor Paging of Saved Track Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    Task<CursorPaging<SavedTrack>> GetUserSavedTracksAsync(
        string market = null,
        Cursor cursor = null);

    /// <summary>
    /// Save Tracks for User
    /// <para>Scopes: LibraryModify</para>
    /// </summary>
    /// <param name="itemIds">(Required) List of the Spotify IDs for the tracks</param>
    /// <returns>Status Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    Task<Status> SaveUserTracksAsync(
        List<string> itemIds);

    /// <summary>
    /// Remove User's Saved Tracks
    /// <para>Scopes: LibraryModify</para>
    /// </summary>
    /// <param name="itemIds">(Required) List of the Spotify IDs for the tracks</param>
    /// <returns>Status Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    Task<Status> RemoveUserTracksAsync(
        List<string> itemIds);

    /// <summary>
    /// Check User's Saved Tracks
    /// <para>Scopes: LibraryRead</para>
    /// </summary>
    /// <param name="itemIds">(Required) List of the Spotify IDs for the tracks</param>
    /// <returns>List of true or false values</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    Task<Bools> CheckUserSavedTracksAsync(
        List<string> itemIds);

    /// <summary>
    /// Get Tracks' Audio Features
    /// </summary>
    /// <param name="ids">(Required) List of the Spotify IDs for the tracks. Maximum: 100 IDs.</param>
    /// <returns>List of Audio Features Object</returns>
    /// <exception cref="AuthAccessTokenRequiredException"></exception>
    Task<List<AudioFeatures>> GetSeveralTracksAudioFeaturesAsync(
        List<string> ids);

    /// <summary>
    /// Get Track's Audio Features
    /// </summary>
    /// <param name="id">(Required) The Spotify ID for the track</param>
    /// <returns>Audio Features Object</returns>
    /// <exception cref="AuthAccessTokenRequiredException"></exception>
    Task<AudioFeatures> GetTrackAudioFeaturesAsync(
        string id);

    /// <summary>
    /// Get Track's Audio Analysis
    /// </summary>
    /// <param name="id">(Required) The Spotify ID for the track</param>
    /// <returns>Audio Analysis Object</returns>
    /// <exception cref="AuthAccessTokenRequiredException"></exception>
    Task<AudioAnalysis> GetTrackAudioAnalysisAsync(
        string id);

    /// <summary>
    /// Get Recommendations
    /// </summary>
    /// <param name="seedArtists">(Required) List of Spotify IDs for seed artists. Up to 5 seed values may be provided in any combination of seedArtists, seedTracks and seedGenres.</param>
    /// <param name="seedGenres">(Required) List of any genres in the set of available genre seeds. Up to 5 seed values may be provided in any combination of seedArtists, seedTracks and seedGenres.</param>
    /// <param name="seedTracks">(Required) List of Spotify IDs for a seed track. Up to 5 seed values may be provided in any combination of seedArtists, seedTracks and seedGenres.</param>
    /// <param name="limit">(Optional) The target size of the list of recommended tracks. Default: 20. Minimum: 1. Maximum: 100.</param>
    /// <param name="market">(Optional) An ISO 3166-1 alpha-2 country code or the string from_token</param>
    /// <param name="minTuneableTrack">(Optional) Multiple values. For each tunable track attribute, a hard floor on the selected track attribute’s value can be provided</param>
    /// <param name="maxTuneableTrack">(Optional) Multiple values. For each tunable track attribute, a hard ceiling on the selected track attribute’s value can be provided.</param>
    /// <param name="targetTuneableTrack">(Optional) Multiple values. For each of the tunable track attributes (below) a target value may be provided.</param>
    /// <returns>Recommendation Response Object</returns>
    /// <exception cref="AuthAccessTokenRequiredException"></exception>
    Task<RecommendationsResponse> GetRecommendationsAsync(
        List<string> seedArtists = null,
        List<string> seedGenres = null,
        List<string> seedTracks = null,
        int? limit = null,
        string market = null,
        TuneableTrack minTuneableTrack = null,
        TuneableTrack maxTuneableTrack = null,
        TuneableTrack targetTuneableTrack = null);
    #endregion Tracks

    #region Search
    /// <summary>
    /// Search for Item
    /// </summary>
    /// <param name="query">(Required) Search query keywords and optional field filters and operators.</param>
    /// <param name="searchType">(Required) List of item types to search across.</param>
    /// <param name="market">(Optional) An ISO 3166-1 alpha-2 country code or the string from_token.</param>
    /// <param name="external">(Optional) Include any relevant audio content that is hosted externally. </param>
    /// <param name="page">(Optional) Limit: Maximum number of results to return. Default: 20 Minimum: 1 Maximum: 50 - Offset: The index of the first track to return</param>
    /// <returns>Content Response</returns>
    /// <exception cref="AuthAccessTokenRequiredException"></exception>
    Task<ContentResponse> SearchForItemAsync(
        string query,
        SearchType searchType,
        string market = null,
        bool? external = null,
        Page page = null);
    #endregion Search

    #region Users
    /// <summary>
    /// Get Current User's Profile
    /// <para>Scopes: UserReadEmail, UserReadPrivate</para>
    /// </summary>
    /// <returns>Private User Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    Task<PrivateUser> GetUserProfileAsync();

    /// <summary>
    /// Get a User's Top Items (Artists)
    /// <para>Scopes: ListeningTopRead</para>
    /// </summary>
    /// <param name="timeRange">(Optional) Over what time frame the affinities are computed. Long Term: alculated from several years of data and including all new data as it becomes available, Medium Term: (Default) approximately last 6 months, Short Term: approximately last 4 weeks</param>
    /// <param name="cursor">(Optional) Limit: The number of entities to return. Default: 20. Minimum: 1. Maximum: 50. For example - Offset: he index of the first entity to return. Default: 0. Use with limit to get the next set of entities.</param>
    /// <returns>Cursor Paging of Artist Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    Task<CursorPaging<Artist>> GetUserTopArtistsAsync(
        TimeRange? timeRange = null,
        Cursor cursor = null);

    /// <summary>
    /// Get a User's Top Items (Tracks)
    /// <para>Scopes: ListeningTopRead</para>
    /// </summary>
    /// <param name="timeRange">(Optional) Over what time frame the affinities are computed. Long Term: alculated from several years of data and including all new data as it becomes available, Medium Term: (Default) approximately last 6 months, Short Term: approximately last 4 weeks</param>
    /// <param name="cursor">(Optional) Limit: The number of entities to return. Default: 20. Minimum: 1. Maximum: 50. For example - Offset: he index of the first entity to return. Default: 0. Use with limit to get the next set of entities.</param>
    /// <returns>Cursor Paging of Track Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    Task<CursorPaging<Track>> GetUserTopTracksAsync(
        TimeRange? timeRange = null,
        Cursor cursor = null);

    /// <summary>
    /// Get User's Profile
    /// </summary>
    /// <param name="userId">The user’s Spotify user ID.</param>
    /// <returns>Public User Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    Task<PublicUser> GetUserProfileAsync(
        string userId);

    /// <summary>
    /// Follow Playlist
    /// <para>Scopes: FollowModify</para>
    /// </summary>
    /// <param name="playlistId">(Required) The Spotify ID of the playlist. Any playlist can be followed, regardless of its public/private status, as long as you know its playlist ID.</param>
    /// <param name="isPublic">(Optional) Defaults to true. If true the playlist will be included in user’s public playlists, if false it will remain private. To be able to follow playlists privately, the user must have granted the playlist-modify-private scope.</param>
    /// <returns>Status Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    Task<Status> FollowPlaylistAsync(
        string playlistId,
        bool isPublic = true);

    /// <summary>
    /// Unfollow Playlist
    /// <para>Scopes: PlaylistModifyPublic, PlaylistModifyPrivate</para>
    /// </summary>
    /// <param name="playlistId">(Required) The Spotify ID of the playlist that is to be no longer followed.</param>
    /// <returns>Status Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    Task<Status> UnfollowPlaylistAsync(
        string playlistId);

    /// <summary>
    /// Get Followed Artists
    /// <para>Scopes: FollowRead</para>
    /// </summary>
    /// <param name="cursor">(Optional) Limit: The maximum number of items to return. Default: 20. Minimum: 1. Maximum: 50. - After: The last artist ID retrieved from the previous request.</param>
    /// <returns>CursorPaging of Artist Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    Task<CursorPaging<Artist>> GetUsersFollowedArtistsAsync(
        Cursor cursor = null);

    /// <summary>
    /// Follow Artists or Users
    /// <para>Scopes: FollowModify</para>
    /// </summary>
    /// <param name="ids">(Required) List of the artist or the user Spotify IDs.</param>
    /// <param name="followType">(Required) Either artist or user</param>
    /// <returns>Status Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    Task<Status> FollowArtistsOrUsersAsync(
        List<string> ids,
        FollowType followType);

    /// <summary>
    /// Unfollow Artists or Users
    /// <para>Scopes: FollowModify</para>
    /// </summary>
    /// <param name="ids">(Required) List of the artist or the user Spotify IDs.</param>
    /// <param name="followType">(Required) Either artist or user</param>
    /// <returns>Status Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    Task<Status> UnfollowArtistsOrUsersAsync(
        List<string> ids,
        FollowType followType);

    /// <summary>
    /// Check if User Follows Artists or Users
    /// <para>Scopes: FollowRead</para>
    /// </summary>
    /// <param name="ids">(Required) List of the artist or the user Spotify IDs to check.</param>
    /// <param name="followType">(Required) Either artist or user.</param>
    /// <returns>List of true or false values</returns>
    Task<Bools> GetFollowingStateForArtistsOrUsersAsync(
        List<string> ids,
        FollowType followType);

    /// <summary>
    /// Check if Users Follow a Playlist
    /// <para>Scopes: PlaylistReadPrivate</para>
    /// </summary>
    /// <param name="ids">(Required) List of Spotify User IDs, the ids of the users that you want to check to see if they follow the playlist. Maximum: 5 ids.</param>
    /// <param name="playlistId">(Required) The Spotify ID of the playlist.</param>
    /// <returns>List of true or false values</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    Task<Bools> CheckUsersFollowingPlaylistAsync(
        List<string> ids,
        string playlistId);
    #endregion Users

    #region Playlists
    /// <summary>
    /// Get a Playlist
    /// </summary>
    /// <param name="playlistId">(Required) The Spotify ID for the playlist.</param>
    /// <param name="fields">(Optional) Filters for the query: a comma-separated list of the fields to return. If omitted, all fields are returned.</param>
    /// <param name="market">(Optional) An ISO 3166-1 alpha-2 country code or the string from_token</param>
    /// <param name="additionalTypes">(Optional) List of item types that your client supports besides the default track type. Valid types are track and episode. An unsupported type in the response is expected to be represented as null value in the item field. This parameter was introduced to allow existing clients to maintain their current behaviour and might be deprecated in the future.</param>
    /// <returns>Playlist Object</returns>
    /// <exception cref="AuthAccessTokenRequiredException"></exception>
    Task<Playlist> GetPlaylistAsync(
        string playlistId,
        string fields = null,
        string market = null,
        List<string> additionalTypes = null);

    /// <summary>
    /// Change Playlist Details
    /// <para>Scopes: PlaylistModifyPublic, PlaylistModifyPrivate</para>
    /// </summary>
    /// <param name="playlistId">(Required) The Spotify ID for the playlist.</param>
    /// <param name="name">(Optional) The new name for the playlist, for example "My New Playlist Title"</param>
    /// <param name="isPublic">(Optional) If true the playlist will be public, if false it will be private.</param>
    /// <param name="isCollaborative">(Optional) If true , the playlist will become collaborative and other users will be able to modify the playlist in their Spotify client. Note: You can only set collaborative to true on non-public playlists.</param>
    /// <param name="description">(Optional) Value for playlist description as displayed in Spotify Clients and in the Web API.</param>
    /// <returns>Status Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    Task<Status> ChangePlaylistDetailsAsync(
        string playlistId,
        string name = null,
        bool? isPublic = null,
        bool? isCollaborative = null,
        string description = null);

    /// <summary>
    /// Get Playlist Items
    /// </summary>
    /// <param name="id">(Required) The Spotify ID for the playlist.</param>
    /// <param name="market">(Optional) An ISO 3166-1 alpha-2 country code or the string from_token</param>
    /// <param name="page">(Optional) Limit: The maximum number of items to return. Default: 100. Minimum: 1. Maximum: 100. - Offset: The index of the first item to return. Default: 0</param>
    /// <param name="fields">(Optional) Filters for the query: a comma-separated list of the fields to return. If omitted, all fields are returned.</param>
    /// <param name="additionalTypes">(Optional) List of item types that your client supports besides the default track type. Valid types are track and episode. An unsupported type in the response is expected to be represented as null value in the item field. This parameter was introduced to allow existing clients to maintain their current behaviour and might be deprecated in the future.</param>
    /// <returns>Paging List of Playlist Track Object</returns>
    /// <exception cref="AuthAccessTokenRequiredException"></exception>
    Task<Paging<PlaylistTrack>> GetPlaylistTracksAsync(
        string id,
        string market = null,
        Page page = null,
        string fields = null,
        List<string> additionalTypes = null);

    /// <summary>
    /// Add Items to Playlist
    /// <para>Scopes: PlaylistModifyPublic, PlaylistModifyPrivate</para>
    /// </summary>
    /// <param name="playlistId">The Spotify ID for the playlist.</param>
    /// <param name="uris">(Optional) List of Spotify track URIs to add.</param>
    /// <param name="position">(Optional) The position to insert the tracks, a zero-based index.</param>
    /// <returns>Snapshot Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    Task<Snapshot> AddTracksToPlaylistAsync(
        string playlistId,
        List<string> uris = null,
        int? position = null);

    /// <summary>
    /// Update Playlist Items
    /// <para>Scopes: PlaylistModifyPublic, PlaylistModifyPrivate</para>
    /// </summary>
    /// <param name="playlistId">The Spotify ID for the playlist.</param>
    /// <param name="rangeStart">(Required) The position of the first track to be reordered.</param>
    /// <param name="insertBefore">(Required) The position where the tracks should be inserted. To reorder the tracks to the end of the playlist, simply set insert_before to the position after the last track.</param>
    /// <param name="rangeLength">(Optional) The amount of tracks to be reordered. Defaults to 1 if not set. The range of tracks to be reordered begins from the range_start position, and includes the range_length subsequent tracks.</param>
    /// <param name="snapshotId">(Optional) The playlist’s snapshot ID against which you want to make the changes.</param>
    /// <returns>Snapshot Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    Task<Snapshot> ReorderPlaylistTracksAsync(
        string playlistId,
        int rangeStart,
        int insertBefore,
        int? rangeLength = 1,
        string snapshotId = null);

    /// <summary>
    /// Replace Playlist Items
    /// <para>Scopes: PlaylistModifyPublic, PlaylistModifyPrivate</para>
    /// </summary>
    /// <param name="playlistId">(Required) The Spotify ID for the playlist.</param>
    /// <param name="uris">(Optional) List of Spotify track URIs.</param>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    Task<Status> ReplacePlaylistTracksAsync(
        string playlistId,
        List<string> uris);

    /// <summary>
    /// Remove Playlist Items
    /// <para>Scopes: PlaylistModifyPublic, PlaylistModifyPrivate</para>
    /// </summary>
    /// <param name="playlistId">(Required) The Spotify ID for the playlist.</param>
    /// <param name="uris">(Required) List of Spotify URIs of the tracks to remove</param>
    /// <param name="snapshotId">(Optional) The playlist’s snapshot ID against which you want to make the changes. The API will validate that the specified tracks exist and in the specified positions and make the changes, even if more recent changes have been made to the playlist.</param>
    /// <param name="uriPositions">(Optional) List of Positions for each of the Uris in the playlist, positions are zero-indexed, that is the first item in the playlist has the value 0, the second item 1, and so on</param>
    /// <returns>Snapshot Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    Task<Snapshot> RemoveTracksFromPlaylistAsync(
        string playlistId,
        List<string> uris,
        string snapshotId = null,
        List<List<int>> uriPositions = null);

    /// <summary>
    /// Get User's Playlists
    /// <para>Scopes: PlaylistReadPrivate, PlaylistReadCollaborative</para>
    /// </summary>
    /// <param name="userId">(Required) The user’s Spotify user ID.</param>
    /// <param name="cursor">(Optional) Limit: The maximum number of playlists to return. Default: 20. Minimum: 1. Maximum: 50. - Offset: The index of the first playlist to return. Default: 0 (the first object). Maximum offset: 100</param>
    /// <returns>CursorPaging of Simplified Playlist Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    Task<CursorPaging<SimplifiedPlaylist>> GetUserPlaylistsAsync(
        string userId,
        Cursor cursor = null);

    /// <summary>
    /// Get User's Playlists
    /// <para>Scopes: PlaylistReadPrivate, PlaylistReadCollaborative</para>
    /// </summary>
    /// <param name="cursor">(Optional) Limit: The maximum number of playlists to return. Default: 20. Minimum: 1. Maximum: 50. - The index of the first playlist to return. Default: 0 (the first object). Maximum offset: 100. Use with limit to get the next set of playlists.</param>
    /// <returns>CursorPaging of Simplified Playlist Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    Task<CursorPaging<SimplifiedPlaylist>> GetUserPlaylistsAsync(
        Cursor cursor = null);

    /// <summary>
    /// Create Playlist
    /// <para>Scopes: PlaylistModifyPublic, PlaylistModifyPrivate</para>
    /// </summary>
    /// <param name="userId">(Required) The user’s Spotify user ID.</param>
    /// <param name="name">(Required) The name for the new playlist, for example "Your Coolest Playlist" . This name does not need to be unique; a user may have several playlists with the same name.</param>
    /// <param name="isPublic">(Optional) Defaults to true . If true the playlist will be public, if false it will be private. To be able to create private playlists, the user must have granted the playlist-modify-private scope</param>
    /// <param name="isCollaborative">(Optional) Defaults to false . If true the playlist will be collaborative. Note that to create a collaborative playlist you must also set public to false . To create collaborative playlists you must have granted playlist-modify-private and playlist-modify-public scopes.</param>
    /// <param name="description">(Optional) Value for playlist description as displayed in Spotify Clients and in the Web API.</param>
    /// <returns>Playlist Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    Task<Playlist> CreatePlaylistAsync(
        string userId,
        string name,
        bool? isPublic = null,
        bool? isCollaborative = null,
        string description = null);

    /// <summary>
    /// Get Featured Playlists
    /// </summary>
    /// <param name="country">(Optional) A country: an ISO 3166-1 alpha-2 country code. Provide this parameter if you want the list of returned items to be relevant to a particular country</param>
    /// <param name="locale">(Optional) The desired language, consisting of a lowercase ISO 639-1 language code and an uppercase ISO 3166-1 alpha-2 country code, joined by an underscore</param>
    /// <param name="timeStamp">(Optional) Use this parameter to specify the user’s local time to get results tailored for that specific date and time in the day.</param>
    /// <param name="page">(Optional) Limit: The maximum number of items to return. Default: 20. Minimum: 1. Maximum: 50. - Offset: The index of the first item to return. Default: 0</param>
    /// <returns>Paging List of Simplified Playlist Object</returns>
    /// <exception cref="AuthAccessTokenRequiredException"></exception>
    Task<Paging<SimplifiedPlaylist>> GetAllFeaturedPlaylistsAsync(
        string country = null,
        string locale = null,
        DateTime? timeStamp = null,
        Page page = null);

    /// <summary>
    /// Get Category's Playlists
    /// </summary>
    /// <param name="categoryId">(Required) The Spotify category ID for the category.</param>
    /// <param name="country">(Optional) A country: an ISO 3166-1 alpha-2 country code. </param>
    /// <param name="page">(Optional) Limit: The maximum number of items to return. Default: 20. Minimum: 1. Maximum: 50. - Offset: The index of the first item to return. Default: 0</param>
    /// <returns>Paging List of Simplified Playlist Object</returns>
    /// <exception cref="AuthAccessTokenRequiredException"></exception>
    Task<Paging<SimplifiedPlaylist>> GetCategoryPlaylistsAsync(
        string categoryId,
        string country = null,
        Page page = null);

    /// <summary>
    /// Get Playlist Cover Image
    /// </summary>
    /// <param name="playlistId">(Required) The Spotify ID for the playlist.</param>
    /// <returns>List of Image Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    Task<List<Image>> GetPlaylistCoverImageAsync(
        string playlistId);

    /// <summary>
    /// Add Custom Playlist Cover Image
    /// <para>Scopes: UserGeneratedContentImageUpload, PlaylistModifyPublic, PlaylistModifyPrivate</para>
    /// </summary>
    /// <param name="playlistId">(Required) The Spotify ID for the playlist.</param>
    /// <param name="file">(Required) JPEG Image File Bytes</param>
    /// <returns>Status Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    Task<Status> UploadCustomPlaylistCoverImageAsync(
        string playlistId,
        byte[] file);
    #endregion Playlists

    #region Categories
    /// <summary>
    /// Get Several Browse Categories
    /// </summary>
    /// <param name="country">(Optional) A country: an ISO 3166-1 alpha-2 country code. </param>
    /// <param name="locale">(Optional) The desired language, consisting of an ISO 639-1 language code and an ISO 3166-1 alpha-2 country code, joined by an underscore.</param>
    /// <param name="page">(Optional) Limit: The maximum number of categories to return. Default: 20. Minimum: 1. Maximum: 50. Offset: The index of the first item to return. Default: 0</param>
    /// <returns>Paging List of Category Object</returns>
    /// <exception cref="AuthAccessTokenRequiredException"></exception>
    Task<Paging<Category>> GetAllCategoriesAsync(
        string country = null,
        string locale = null,
        Page page = null);

    /// <summary>
    /// Get Single Browse Category
    /// </summary>
    /// <param name="categoryId">(Required) The Spotify category ID for the category.</param>
    /// <param name="country">(Optional) A country: an ISO 3166-1 alpha-2 country code. </param>
    /// <param name="locale">(Optional) The desired language, consisting of an ISO 639-1 language code and an ISO 3166-1 alpha-2 country code, joined by an underscore.</param>
    /// <returns>Category Object</returns>
    /// <exception cref="AuthAccessTokenRequiredException"></exception>
   Task<Category> GetCategoryAsync(
        string categoryId,
        string country = null,
        string locale = null);
    #endregion Categories

    #region Genres
    /// <summary>
    /// Get Available Genre Seeds
    /// </summary>
    /// <returns>Available Genre Seeds Object</returns>
    /// <exception cref="AuthAccessTokenRequiredException"></exception>
    Task<AvailableGenreSeeds> GetRecommendationGenresAsync();
    #endregion Genres

    #region Player
    /// <summary>
    /// Get Playback State
    /// <para>Scopes: ConnectReadPlaybackState</para>
    /// </summary>
    /// <param name="market">(Optional) An ISO 3166-1 alpha-2 country code or the string from_token. Provide this parameter if you want to apply Track Relinking.</param>
    /// <param name="additionalTypes">(Optional) List of item types that your client supports besides the default track type. Valid types are track and episode. An unsupported type in the response is expected to be represented as null value in the item field. This parameter was introduced to allow existing clients to maintain their current behaviour and might be deprecated in the future.</param>
    /// <returns>Currently Playing Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    Task<CurrentlyPlaying> GetUserPlaybackCurrentAsync(
        string market = null, List<string> additionalTypes = null);

    /// <summary>
    /// Transfer Playback
    /// <para>Scopes: ConnectModifyPlaybackState</para>
    /// </summary>
    /// <param name="deviceIds">(Required) List containing the ID of the device on which playback should be started/transferred. Although an array is accepted, only a single device_id is currently supported.</param>
    /// <param name="play">(Optional) true: ensure playback happens on new device. false or not provided: keep the current playback state.</param>
    /// <returns>Status Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    Task<Status> UserPlaybackTransferAsync(
         List<string> deviceIds,
         bool? play = null);

    /// <summary>
    /// Get Available Devices
    /// <para>Scopes: ConnectReadPlaybackState</para>
    /// </summary>
    /// <returns>Devices Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    Task<Devices> GetUserPlaybackDevicesAsync();

    /// <summary>
    /// Get Currently Playing Track
    /// <para>Scopes: ConnectReadCurrentlyPlaying, ConnectReadPlaybackState</para>
    /// </summary>
    /// <param name="market">(Optional) An ISO 3166-1 alpha-2 country code or the string from_token. Provide this parameter if you want to apply Track Relinking.</param>
    /// <param name="additionalTypes">(Optional) List of item types that your client supports besides the default track type. Valid types are track and episode. An unsupported type in the response is expected to be represented as null value in the item field. This parameter was introduced to allow existing clients to maintain their current behaviour and might be deprecated in the future.</param>
    /// <returns>Simplified Currently Playing Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    Task<SimplifiedCurrentlyPlaying> GetUserPlaybackCurrentTrackAsync(
        string market = null, List<string> additionalTypes = null);

    /// <summary>
    /// Start/Resume Playback
    /// <para>Scopes: ConnectModifyPlaybackState</para>
    /// </summary>
    /// <param name="contextUri">(Optional) Spotify URI of the context to play. Valid contexts are albums, artists, playlists. Example: "spotify:album:1Je1IMUlBXcx1Fz0WE7oPT"</param>
    /// <param name="uris">(Optional) List of the Spotify track URIs to play. For example: ["spotify:track:4iV5W9uYEdYUVa79Axb7Rh", "spotify:track:1301WleyT98MSxVHPZCA6M"]}</param>
    /// <param name="offsetUri">(Optional) Use either offsetUri or offsetPosition, Indicates from where in the context playback should start. Only available when context_uri corresponds to an album or playlist object, or when the uris parameter is used. “position” is zero based and can’t be negative. Example: 5.</param>
    /// <param name="offsetPosition">(Optional) Use either offsetPosition or offsetUri, Indicates from where in the context playback should start. Only available when context_uri corresponds to an album or playlist object, or when the uris parameter is used. “uri” is a string representing the uri of the item to start at. Example: "spotify:track:1301WleyT98MSxVHPZCA6M"</param>
    /// <param name="position">(Optional) Indicates from what position to start playback.Must be a positive number.Passing in a position that is greater than the length of the track will cause the player to start playing the next song.</param>
    /// <param name="deviceId">(Optional) The id of the device this command is targeting. If not supplied, the user’s currently active device is the target.</param>
    /// <returns>Status Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    Task<Status> UserPlaybackStartResumeAsync(
        string contextUri = null,
        List<string> uris = null,
        int? offsetPosition = null,
        string offsetUri = null,
        int? position = null,
        string deviceId = null);

    /// <summary>
    /// Pause Playback
    /// <para>Scopes: ConnectModifyPlaybackState</para>
    /// </summary>
    /// <param name="deviceId">(Optional) The id of the device this command is targeting. If not supplied, the user’s currently active device is the target.</param>
    /// <returns>Status Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    Task<Status> UserPlaybackPauseAsync(
        string deviceId = null);

    /// <summary>
    /// Skip to Next
    /// <para>Scopes: ConnectModifyPlaybackState</para>
    /// </summary>
    /// <param name="deviceId">(Optional) The id of the device this command is targeting. If not supplied, the user’s currently active device is the target.</param>
    /// <returns>Status Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    Task<Status> UserPlaybackNextTrackAsync(
        string deviceId = null);

    /// <summary>
    /// Skip To Previous
    /// <para>Scopes: ConnectModifyPlaybackState</para>
    /// </summary>
    /// <param name="deviceId">(Optional) The id of the device this command is targeting. If not supplied, the user’s currently active device is the target.</param>
    /// <returns>Status Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    Task<Status> UserPlaybackPreviousTrackAsync(
        string deviceId = null);

    /// <summary>
    /// Seek To Position
    /// <para>Scopes: ConnectModifyPlaybackState</para>
    /// </summary>
    /// <param name="position">(Required) The position in milliseconds to seek to. Must be a positive number. Passing in a position that is greater than the length of the track will cause the player to start playing the next song.</param>
    /// <param name="deviceId">(Optional) The id of the device this command is targeting. If not supplied, the user’s currently active device is the target.</param>
    /// <returns>Status Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
   Task<Status> UserPlaybackSeekTrackAsync(
        int position,
        string deviceId = null);

    /// <summary>
    /// Set Repeat Mode
    /// <para>Scopes: ConnectModifyPlaybackState</para>
    /// </summary>
    /// <param name="state">(Required) track, context or off. track will repeat the current track. context will repeat the current context. off will turn repeat off.</param>
    /// <param name="deviceId">(Optional) The id of the device this command is targeting. If not supplied, the user’s currently active device is the target.</param>
    /// <returns>Status Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    Task<Status> UserPlaybackSetRepeatModeAsync(
        RepeatState state,
        string deviceId = null);

    /// <summary>
    /// Set Playback Volume
    /// <para>Scopes: ConnectModifyPlaybackState</para>
    /// </summary>
    /// <param name="percent">(Required) The volume to set. Must be a value from 0 to 100 inclusive.</param>
    /// <param name="deviceId">(Optional) The id of the device this command is targeting. If not supplied, the user’s currently active device is the target.</param>
    /// <returns>Status Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    Task<Status> UserPlaybackSetVolumeAsync(
        int percent,
        string deviceId = null);

    /// <summary>
    /// Toggle Playback Shuffle
    /// <para>Scopes: ConnectModifyPlaybackState</para>
    /// </summary>
    /// <param name="state">(Required) true : Shuffle user’s playback, false : Do not shuffle user’s playback</param>
    /// <param name="deviceId">(Optional) The id of the device this command is targeting. If not supplied, the user’s currently active device is the target.</param>
    /// <returns>Status Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    Task<Status> UserPlaybackToggleShuffleAsync(
        bool state,
        string deviceId = null);

    /// <summary>
    /// Get Recently Played Tracks
    /// <para>Scopes: ListeningRecentlyPlayed</para>
    /// </summary>
    /// <param name="cursor">(Optional) Limit: The maximum number of items to return. Default: 20. Minimum: 1. Maximum: 50. - After: A Unix timestamp in milliseconds. Returns all items after (but not including) this cursor position. If after is specified, before must not be specified. Before - (Optional) A Unix timestamp in milliseconds. Returns all items before (but not including) this cursor position. If before is specified, after must not be specified.</param>
    /// <returns>Cursor Paging of Play History Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    Task<CursorPaging<PlayHistory>> GetUserRecentlyPlayedTracksAsync(
        Cursor cursor = null);

    /// <summary>
    /// Get the User's Queue
    /// <para>Scopes: ConnectReadCurrentlyPlaying</para>
    /// </summary>
    /// <returns>Queue Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    Task<Queue> GetUserQueueAsync();

    /// <summary>
    /// Add an Item to Playback Queue
    /// <para>Scopes: ConnectModifyPlaybackState</para>
    /// </summary>
    /// <param name="uri">(Required) The uri of the item to add to the queue. Must be a track or an episode uri.</param>
    /// <param name="deviceId">(Optional) The id of the device this command is targeting. If not supplied, the user’s currently active device is the target.</param>
    /// <returns>Status Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    Task<Status> UserPlaybackAddToQueueAsync(
        string uri, string deviceId = null);
    #endregion Player

    #region Markets
    /// <summary>
    /// Get Available Markets
    /// </summary>
    /// <returns>Available Markets</returns>
    Task<AvailableMarkets> GetAvailableMarkets();
    #endregion Markets
}
