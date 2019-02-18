using Spotify.NetStandard.Enums;
using Spotify.NetStandard.Requests;
using Spotify.NetStandard.Responses;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Spotify.NetStandard.Client.Interfaces
{
    public interface ISpotifyApi
    {
        #region Search API
        /// <summary>
        /// Search for an Item
        /// </summary>
        /// <param name="query">(Required) Search query keywords and optional field filters and operators.</param>
        /// <param name="searchType">(Required) List of item types to search across.</param>
        /// <param name="market">(Optional) An ISO 3166-1 alpha-2 country code or the string from_token.</param>
        /// <param name="external">(Optional) Include any relevant audio content that is hosted externally. </param>
        /// <param name="page">(Optional) Limit: Maximum number of results to return. Default: 20 Minimum: 1 Maximum: 50 - Offset: The index of the first track to return</param>
        /// <returns>Content Response</returns>
        Task<ContentResponse> SearchForItemAsync(
            string query,
            SearchType searchType,
            string market = null,
            bool? external = null,
            Page page = null);
        #endregion Search API

        #region Browse API
        /// <summary>
        /// Get All Categories
        /// </summary>
        /// <param name="country">(Optional) A country: an ISO 3166-1 alpha-2 country code. </param>
        /// <param name="locale">(Optional) The desired language, consisting of an ISO 639-1 language code and an ISO 3166-1 alpha-2 country code, joined by an underscore.</param>
        /// <param name="page">(Optional) Limit: The maximum number of categories to return. Default: 20. Minimum: 1. Maximum: 50. Offset: The index of the first item to return. Default: 0</param>
        /// <returns>Paging List of Category Object</returns>
        Task<Paging<Category>> GetAllCategoriesAsync(
            string country = null,
            string locale = null,
            Page page = null);

        /// <summary>
        /// Get a Category
        /// </summary>
        /// <param name="categoryId">(Required) The Spotify category ID for the category.</param>
        /// <param name="country">(Optional) A country: an ISO 3166-1 alpha-2 country code. </param>
        /// <param name="locale">(Optional) The desired language, consisting of an ISO 639-1 language code and an ISO 3166-1 alpha-2 country code, joined by an underscore.</param>
        /// <returns>Category Object</returns>
        Task<Category> GetCategoryAsync(
            string categoryId,
            string country = null,
            string locale = null);

        /// <summary>
        /// Get a Category's Playlists
        /// </summary>
        /// <param name="categoryId">(Required) The Spotify category ID for the category.</param>
        /// <param name="country">(Optional) A country: an ISO 3166-1 alpha-2 country code. </param>
        /// <param name="page">(Optional) Limit: The maximum number of items to return. Default: 20. Minimum: 1. Maximum: 50. - Offset: The index of the first item to return. Default: 0</param>
        /// <returns>Paging List of Playlist Object</returns>
        Task<Paging<Playlist>> GetCategoryPlaylistsAsync(
            string categoryId,
            string country = null,
            Page page = null);

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
        Task<RecommendationsResponse> GetRecommendationsAsync(
            List<string> seedArtists = null,
            List<string> seedGenres = null,
            List<string> seedTracks = null,
            int? limit = null,
            string market = null,
            TuneableTrack minTuneableTrack = null,
            TuneableTrack maxTuneableTrack = null,
            TuneableTrack targetTuneableTrack = null);

        /// <summary>
        /// Get Recommendation Genres
        /// </summary>
        /// <returns>Available Genre Seeds Object</returns>
        Task<AvailableGenreSeeds> GetRecommendationGenresAsync();

        /// <summary>
        /// Get All New Releases
        /// </summary>
        /// <param name="country">(Optional) A country: an ISO 3166-1 alpha-2 country code. </param>
        /// <param name="page">(Optional) Limit: The maximum number of items to return. Default: 20. Minimum: 1. Maximum: 50. - Offset: The index of the first item to return. Default: 0</param>
        /// <returns>Paging List of Album Object</returns>
        Task<Paging<Album>> GetAllNewReleasesAsync(
            string country = null,
            Page page = null);

        /// <summary>
        /// Get All Featured Playlists
        /// </summary>
        /// <param name="country">(Optional) A country: an ISO 3166-1 alpha-2 country code. Provide this parameter if you want the list of returned items to be relevant to a particular country</param>
        /// <param name="locale">(Optional) The desired language, consisting of a lowercase ISO 639-1 language code and an uppercase ISO 3166-1 alpha-2 country code, joined by an underscore</param>
        /// <param name="timeStamp">(Optional) Use this parameter to specify the user’s local time to get results tailored for that specific date and time in the day.</param>
        /// <param name="page">(Optional) Limit: The maximum number of items to return. Default: 20. Minimum: 1. Maximum: 50. - Offset: The index of the first item to return. Default: 0</param>
        /// <returns>Paging List of Playlist Object</returns>
        Task<Paging<Playlist>> GetAllFeaturedPlaylistsAsync(
            string country = null,
            string locale = null,
            DateTime? timeStamp = null,
            Page page = null);
        #endregion Browse API

        #region Follow API
        /// <summary>
        /// Get Following State for Artists/Users
        /// </summary>
        /// <param name="ids">(Required) List of the artist or the user Spotify IDs to check.</param>
        /// <param name="followType">Type: either artist or user.</param>
        /// <returns>List of true or false values</returns>
        /// <exception cref="AuthTokenRequiredException"></exception>
        Task<List<bool>> GetFollowingStateForArtistsOrUsersAsync(
            List<string> ids,
            FollowType followType);

        /// <summary>
        /// Check if Users Follow a Playlist
        /// </summary>
        /// <param name="ids">(Required) List of Spotify User IDs ; the ids of the users that you want to check to see if they follow the playlist. Maximum: 5 ids.</param>
        /// <param name="playlistId">The Spotify ID of the playlist.</param>
        /// <returns>List of true or false values</returns>
        /// <exception cref="AuthTokenRequiredException"></exception>
        Task<List<bool>> CheckUsersFollowingPlaylistAsync(
            List<string> ids,
            string playlistId);

        /// <summary>
        /// Follow Artists or Users
        /// </summary>
        /// <param name="ids">(Required) List of the artist or the user Spotify IDs.</param>
        /// <param name="followType">Either artist or user</param>
        /// <returns>Status Object</returns>
        /// <exception cref="AuthTokenRequiredException"></exception>
        Task<Status> FollowArtistsOrUsersAsync(
            List<string> ids,
            FollowType followType);

        /// <summary>
        /// Follow a Playlist
        /// </summary>
        /// <param name="playlistId">(Required) The Spotify ID of the playlist. Any playlist can be followed, regardless of its public/private status, as long as you know its playlist ID.</param>
        /// <param name="isPublic">(Optional) Defaults to true. If true the playlist will be included in user’s public playlists, if false it will remain private. To be able to follow playlists privately, the user must have granted the playlist-modify-private scope.</param>
        /// <returns>Status Object</returns>
        /// <exception cref="AuthTokenRequiredException"></exception>
        Task<Status> FollowPlaylistAsync(
            string playlistId,
            bool isPublic = true);

        /// <summary>
        /// Get User's Followed Artists
        /// </summary>
        /// <param name="cursor">(Optional) Cursor</param>
        /// <returns>Cursor Paging of Artist Object</returns>
        /// <exception cref="AuthTokenRequiredException"></exception>
        Task<CursorPaging<Artist>> GetUsersFollowedArtistsAsync(
            Cursor cursor = null);

        /// <summary>
        /// Unfollow Artists or Users
        /// </summary>
        /// <param name="ids">(Required) List of the artist or the user Spotify IDs.</param>
        /// <param name="followType">Either artist or user</param>
        /// <returns>Status Object</returns>
        /// <exception cref="AuthTokenRequiredException"></exception>
        Task<Status> UnfollowArtistsOrUsersAsync(
            List<string> ids,
            FollowType followType);

        /// <summary>
        /// Unfollow Playlist
        /// </summary>
        /// <param name="playlistId">(Required) The Spotify ID of the playlist that is to be no longer followed.</param>
        /// <returns>Status Object</returns>
        /// <exception cref="AuthTokenRequiredException"></exception>
        Task<Status> UnfollowPlaylistAsync(
            string playlistId);
        #endregion Follow API

        #region Playlists API
        /// <summary>
        /// Add Tracks to a Playlist
        /// </summary>
        /// <param name="playlistId">The Spotify ID for the playlist.</param>
        /// <param name="uris">(Optional) List of Spotify track URIs to add.</param>
        /// <param name="position">(Optional) The position to insert the tracks, a zero-based index.</param>
        /// <returns>Snapshot Object</returns>
        /// <exception cref="AuthTokenRequiredException"></exception>
        Task<Snapshot> AddTracksToPlaylistAsync(
            string playlistId,
            List<string> uris = null,
            int? position = null);

        /// <summary>
        /// Get a Playlist
        /// </summary>
        /// <param name="playlistId">(Required) The Spotify ID for the playlist.</param>
        /// <returns>Playlist Object</returns>
        Task<Playlist> GetPlaylistAsync(string playlistId);

        /// <summary>
        /// Remove Tracks from a Playlist
        /// </summary>
        /// <param name="playlistId">(Required) The Spotify ID for the playlist.</param>
        /// <param name="tracks">(Required) List of Spotify URIs of the tracks to remove</param>
        /// <param name="snapshotId">(Optional) The playlist’s snapshot ID against which you want to make the changes. The API will validate that the specified tracks exist and in the specified positions and make the changes, even if more recent changes have been made to the playlist.</param>
        /// <returns>Snapshot Object</returns>
        /// <exception cref="AuthTokenRequiredException"></exception>
        Task<Snapshot> RemoveTracksFromPlaylistAsync(
            string playlistId,
            List<string> tracks,
            string snapshotId = null);

        /// <summary>
        /// Get a Playlist's Tracks
        /// </summary>
        /// <param name="id">(Required) The Spotify ID for the playlist.</param>
        /// <param name="market">(Optional) An ISO 3166-1 alpha-2 country code or the string from_token</param>
        /// <param name="page">(Optional) Limit: The maximum number of items to return. Default: 100. Minimum: 1. Maximum: 100. - Offset: The index of the first item to return. Default: 0</param>
        /// <returns>Paging List of Track Object</returns>
        Task<Paging<Track>> GetPlaylistTracksAsync(
            string id,
            string market = null,
            Page page = null);

        /// <summary>
        /// Get a Playlist Cover Image
        /// </summary>
        /// <param name="playlistId">(Require) The Spotify ID for the playlist.</param>
        /// <returns>Image Object</returns>
        /// <exception cref="AuthTokenRequiredException"></exception>
        Task<Image> GetPlaylistCoverImageAsync(
            string playlistId);

        /// <summary>
        /// Upload a Custom Playlist Cover Image
        /// </summary>
        /// <param name="playlistId">The Spotify ID for the playlist.</param>
        /// <param name="file">JPEG File Bytes</param>
        /// <returns>Status Object</returns>
        Task<Status> UploadCustomPlaylistCoverImageAsync(
            string playlistId,
            byte[] file);

        /// <summary>
        /// Get a List of Current User's Playlists
        /// </summary>
        /// <param name="cursor">(Optional) Limit: The maximum number of playlists to return. Default: 20. Minimum: 1. Maximum: 50. - Offset: The index of the first playlist to return. Default: 0 (the first object). Maximum offset: 100</param>
        /// <returns>CursorPaging of Playlist Object</returns>
        /// <exception cref="AuthTokenRequiredException"></exception>
        Task<CursorPaging<Playlist>> GetUserPlaylistsAsync(
            Cursor cursor = null);

        /// <summary>
        /// Change a Playlist's Details
        /// </summary>
        /// <param name="playlistId">(Required) The Spotify ID for the playlist.</param>
        /// <param name="name">(Optional) The new name for the playlist, for example "My New Playlist Title"</param>
        /// <param name="isPublic">(Optional) If true the playlist will be public, if false it will be private.</param>
        /// <param name="isCollaborative">(Optional) If true , the playlist will become collaborative and other users will be able to modify the playlist in their Spotify client. Note: You can only set collaborative to true on non-public playlists.</param>
        /// <param name="description">(Optional) Value for playlist description as displayed in Spotify Clients and in the Web API.</param>
        /// <returns>Status Object</returns>
        /// <exception cref="AuthTokenRequiredException"></exception>
        Task<Status> ChangePlaylistDetailsAsync(
            string playlistId,
            string name = null,
            bool? isPublic = null,
            bool? isCollaborative = null,
            string description = null);

        /// <summary>
        /// Get a List of a User's Playlists
        /// </summary>
        /// <param name="userId">(Required) The user’s Spotify user ID.</param>
        /// <param name="cursor">(Optional) Limit: The maximum number of playlists to return. Default: 20. Minimum: 1. Maximum: 50. - Offset: The index of the first playlist to return. Default: 0 (the first object). Maximum offset: 100</param>
        /// <returns>CursorPaging of Playlist Object</returns>
        /// <exception cref="AuthTokenRequiredException"></exception>
        Task<CursorPaging<Playlist>> GetUserPlaylistsAsync(
            string userId,
            Cursor cursor = null);

        /// <summary>
        /// Replace a Playlist's Tracks
        /// </summary>
        /// <param name="playlistId">(Required) The Spotify ID for the playlist.</param>
        /// <param name="uris">(Optional) List of Spotify track URIs.</param>
        /// <exception cref="AuthTokenRequiredException"></exception>
        Task<Status> ReplacePlaylistTracksAsync(
            string playlistId,
            List<string> uris);

        /// <summary>
        /// Create a Playlist
        /// </summary>
        /// <param name="userId">(Required) The user’s Spotify user ID.</param>
        /// <param name="name">(Required) The name for the new playlist, for example "Your Coolest Playlist" . This name does not need to be unique; a user may have several playlists with the same name.</param>
        /// <param name="isPublic">(Optional) Defaults to true . If true the playlist will be public, if false it will be private. To be able to create private playlists, the user must have granted the playlist-modify-private scope</param>
        /// <param name="isCollaborative">(Optional) Defaults to false . If true the playlist will be collaborative. Note that to create a collaborative playlist you must also set public to false . To create collaborative playlists you must have granted playlist-modify-private and playlist-modify-public scopes.</param>
        /// <param name="description">(Optional) Value for playlist description as displayed in Spotify Clients and in the Web API.</param>
        /// <returns>Playlist Object</returns>
        /// <exception cref="AuthTokenRequiredException"></exception>
        Task<Playlist> CreatePlaylistAsync(
            string userId,
            string name,
            bool? isPublic = null,
            bool? isCollaborative = null,
            string description = null);

        /// <summary>
        /// Reorder a Playlist's Tracks
        /// </summary>
        /// <param name="playlistId">The Spotify ID for the playlist.</param>
        /// <param name="rangeStart">(Required) The position of the first track to be reordered.</param>
        /// <param name="insertBefore">(Required) The position where the tracks should be inserted. To reorder the tracks to the end of the playlist, simply set insert_before to the position after the last track.</param>
        /// <param name="rangeLength">(Optional) The amount of tracks to be reordered. Defaults to 1 if not set. The range of tracks to be reordered begins from the range_start position, and includes the range_length subsequent tracks.</param>
        /// <param name="snapshotId">The playlist’s snapshot ID against which you want to make the changes.</param>
        /// <returns>Snapshot Object</returns>
        /// <exception cref="AuthTokenRequiredException"></exception>
        Task<Snapshot> ReorderPlaylistTracksAsync(
            string playlistId,
            int rangeStart,
            int insertBefore,
            int? rangeLength,
            string snapshotId = null);
        #endregion Playlists API

        #region User Profile API
        /// <summary>
        /// Get a User's Profile
        /// </summary>
        /// <param name="userId">The user’s Spotify user ID.</param>
        /// <returns>Public User Object</returns>
        /// <exception cref="AuthTokenRequiredException"></exception>
        Task<PublicUser> GetUserProfileAsync(
            string userId);

        /// <summary>
        /// Get Current User's Profile
        /// </summary>
        /// <returns>Private User Object</returns>
        /// <exception cref="AuthTokenRequiredException"></exception>
        Task<PrivateUser> GetUserProfileAsync();
        #endregion User Profile API

        #region Artists API
        /// <summary>
        /// Get Multiple Artists
        /// </summary>
        /// <param name="ids">(Required) List of the Spotify IDs for the artists. Maximum: 50 IDs.</param>
        /// <returns>List of Artist Object</returns>
        Task<List<Artist>> GetMultipleArtistsAsync(
        List<string> ids);

        /// <summary>
        /// Get an Artist
        /// </summary>
        /// <param name="id">(Required) The Spotify ID of the artist.</param>
        /// <returns>Artist Object</returns>
        Task<Artist> GetArtistAsync(
            string id);

        /// <summary>
        /// Get an Artist's Albums
        /// </summary>
        /// <param name="id">(Required) The Spotify ID for the artist.</param>
        /// <param name="includeGroup">(Optional) Filters the response. If not supplied, all album types will be returned</param>
        /// <param name="market">(Optional) An ISO 3166-1 alpha-2 country code or the string from_token</param>
        /// <param name="page">(Optional) Limit: The number of album objects to return. Default: 20. Minimum: 1. Maximum: 50. - Offset: The index of the first album to return. Default: 0</param>
        /// <returns>Paging List of Album Object</returns>
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
        Task<List<Track>> GetArtistTopTracksAsync(
            string id,
            string market);

        /// <summary>
        /// Get an Artist's Related Artists
        /// </summary>
        /// <param name="id">(Requird) The Spotify ID for the artist.</param>
        /// <returns>List of Artist Object</returns>
        Task<List<Artist>> GetArtistRelatedArtistsAsync(
            string id);
        #endregion Artists API

        #region Albums API
        /// <summary>
        /// Get Multiple Albums
        /// </summary>
        /// <param name="ids">(Required) List of the Spotify IDs for the albums. Maximum: 20 IDs.</param>
        /// <param name="market">(Optional) An ISO 3166-1 alpha-2 country code or the string from_token. Provide this parameter if you want to apply Track Relinking.</param>
        /// <returns>List of Album Object</returns>
        Task<List<Album>> GetMultipleAlbumsAsync(
            List<string> ids,
            string market = null);

        /// <summary>
        /// Get an Album
        /// </summary>
        /// <param name="id">(Required) The Spotify ID of the album.</param>
        /// <param name="market">(Optional) The market you’d like to request. An ISO 3166-1 alpha-2 country code.</param>
        /// <returns>Album Object</returns>
        Task<Album> GetAlbumAsync(
            string id,
            string market = null);

        /// <summary>
        /// Get an Album's Tracks
        /// </summary>
        /// <param name="id">(Required) The Spotify ID of the album.</param>      
        /// <param name="market">(Optional) An ISO 3166-1 alpha-2 country code or the string from_token. Provide this parameter if you want to apply Track Relinking.</param>
        /// <param name="page">(Optional) Limit: The maximum number of tracks to return. Default: 20. Minimum: 1. Maximum: 50. - Offset: The index of the first track to return</param>
        /// <returns>Paging of Track Object</returns>
        Task<Paging<Track>> GetAlbumTracksAsync(
            string id,
            string market = null,
            Page page = null);
        #endregion Albums API

        #region Tracks API
        /// <summary>
        /// Get Audio Features for a Track
        /// </summary>
        /// <param name="id">(Required) The Spotify ID for the track</param>
        /// <returns>Audio Features Object</returns>
        Task<AudioFeatures> GetTrackAudioFeaturesAsync(
            string id);

        /// <summary>
        /// Get a Track
        /// </summary>
        /// <param name="id">(Required) The Spotify ID for the track.</param>
        /// <param name="market">(Optional) An ISO 3166-1 alpha-2 country code or the string from_token. Provide this parameter if you want to apply Track Relinking.</param>
        /// <returns>Track Object</returns>
        Task<Track> GetTrackAsync(
            string id,
            string market = null);

        /// <summary>
        /// Get Audio Analysis for a Track
        /// </summary>
        /// <param name="id">(Required) The Spotify ID for the track</param>
        /// <returns>Audio Analysis Object</returns>
        Task<AudioAnalysis> GetTrackAudioAnalysisAsync(
            string id);

        /// <summary>
        /// Get Audio Features for Several Tracks
        /// </summary>
        /// <param name="ids">(Required) List of the Spotify IDs for the tracks. Maximum: 100 IDs.</param>
        /// <returns>List of Audio Features Object</returns>
        Task<List<AudioFeatures>> GetSeveralTracksAudioFeaturesAsync(
            List<string> ids);

        /// <summary>
        /// Get Several Tracks
        /// </summary>
        /// <param name="ids">(Required) List of the Spotify IDs for the tracks. Maximum: 50 IDs.</param>
        /// <param name="market">(Optional) An ISO 3166-1 alpha-2 country code or the string from_token. Provide this parameter if you want to apply Track Relinking.</param>
        /// <returns>List of Track Object</returns>
        Task<List<Track>> GetSeveralTracksAsync(
            List<string> ids,
            string market = null);
        #endregion Tracks API
    }
}
