﻿using Spotify.NetStandard.Client.Authentication;
using Spotify.NetStandard.Client.Exceptions;
using Spotify.NetStandard.Client.Interfaces;
using Spotify.NetStandard.Enums;
using Spotify.NetStandard.Requests;
using Spotify.NetStandard.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spotify.NetStandard.Client.Internal
{
    /// <summary>
    /// Spotify API
    /// </summary>
    internal class SpotifyApi : ISpotifyApi
    {
        #region Private Members
        private readonly SpotifyClient _client;
        #endregion Private Members

        #region Constructor
        /// <summary>
        /// Spotify API
        /// </summary>
        /// <param name="client">Spotify Client</param>
        public SpotifyApi(SpotifyClient client) => 
            _client = client;
        #endregion Constructor

        #region Authentication
        /// <summary>
        /// Get Authorisation Code Auth Uri - Authorisation Code Flow
        /// </summary>
        /// <param name="redirectUri">Redirect Uri</param>
        /// <param name="state">State</param>
        /// <param name="scope">Scope</param>
        /// <param name="showDialog">(Optional) Whether or not to force the user to approve the app again if they’ve already done so.</param>
        /// <returns>Uri</returns>
        public Uri GetAuthorisationCodeAuthUri(
            Uri redirectUri,
            string state,
            Scope scope,
            bool showDialog = false) => 
            _client.AuthUser(redirectUri, state, scope, showDialog);

        /// <summary>
        /// Get Authorisation Code Auth Token - Authorisation Code Flow
        /// </summary>
        /// <param name="responseUri">Response Uri</param>
        /// <param name="redirectUri">Redirect Uri</param>
        /// <param name="state">State</param>
        /// <returns>AccessToken on Success, Null if Not</returns>
        /// <exception cref="AuthCodeValueException">AuthCodeValueException</exception>
        /// <exception cref="AuthCodeStateException">AuthCodeStateException</exception>
        public async Task<AccessToken> GetAuthorisationCodeAuthTokenAsync(
            Uri responseUri,
            Uri redirectUri,
            string state) =>
                await _client.AuthUserAsync(
                responseUri, redirectUri, state);

        /// <summary>
        /// Get Client Credentials Auth Token - Client Credentials Flow
        /// </summary>
        /// <returns>AccessToken on Success, Null if Not</returns>
        public async Task<AccessToken> GetClientCredentialsAuthTokenAsync() =>
            await _client.AuthAsync();

        /// <summary>
        /// Get Implicit Grant Auth Uri - Implicit Grant Flow
        /// </summary>
        /// <param name="redirectUri">Redirect Uri</param>
        /// <param name="state">State</param>
        /// <param name="scope">Scope</param>
        /// <param name="showDialog">(Optional) Whether or not to force the user to approve the app again if they’ve already done so.</param>
        /// <returns>Uri</returns>
        public Uri GetImplicitGrantAuthUri(
            Uri redirectUri,
            string state,
            Scope scope,
            bool showDialog = false) => 
            _client.AuthUserImplicit(redirectUri, state, scope, showDialog);

        /// <summary>
        /// Get Implicit Grant Auth Token - Implicit Grant Flow
        /// </summary>
        /// <param name="responseUri">Response Uri</param>
        /// <param name="redirectUri">Redirect Uri</param>
        /// <param name="state">State</param>
        /// <returns>AccessToken on Success, Null if Not</returns>
        /// <exception cref="AuthTokenValueException">AuthCodeValueException</exception>
        /// <exception cref="AuthTokenStateException">AuthCodeStateException</exception>
        public AccessToken GetImplicitGrantAuthToken(
            Uri responseUri,
            Uri redirectUri,
            string state) =>
            _client.AuthUserImplicit(responseUri, redirectUri, state);
        #endregion Authentication

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
        /// <exception cref="AuthAccessTokenRequiredException"></exception>
        public async Task<ContentResponse> SearchForItemAsync(
            string query,
            SearchType searchType,
            string market = null,
            bool? external = null,
            Page page = null) => 
                await _client.SearchAsync(query, searchType, market, external, page);
        #endregion Search API

        #region Browse API
        /// <summary>
        /// Get All Categories
        /// </summary>
        /// <param name="country">(Optional) A country: an ISO 3166-1 alpha-2 country code. </param>
        /// <param name="locale">(Optional) The desired language, consisting of an ISO 639-1 language code and an ISO 3166-1 alpha-2 country code, joined by an underscore.</param>
        /// <param name="page">(Optional) Limit: The maximum number of categories to return. Default: 20. Minimum: 1. Maximum: 50. Offset: The index of the first item to return. Default: 0</param>
        /// <returns>Paging List of Category Object</returns>
        /// <exception cref="AuthAccessTokenRequiredException"></exception>
        public async Task<Paging<Category>> GetAllCategoriesAsync(
            string country = null,
            string locale = null,
            Page page = null) =>
                (await _client.LookupAllCategoriesAsync(
                    country, locale, page))?.Categories;

        /// <summary>
        /// Get a Category
        /// </summary>
        /// <param name="categoryId">(Required) The Spotify category ID for the category.</param>
        /// <param name="country">(Optional) A country: an ISO 3166-1 alpha-2 country code. </param>
        /// <param name="locale">(Optional) The desired language, consisting of an ISO 639-1 language code and an ISO 3166-1 alpha-2 country code, joined by an underscore.</param>
        /// <returns>Category Object</returns>
        /// <exception cref="AuthAccessTokenRequiredException"></exception>
        public async Task<Category> GetCategoryAsync(
            string categoryId,
            string country = null,
            string locale = null) =>
                await _client.LookupCategoryAsync(
                    categoryId, country, locale);

        /// <summary>
        /// Get a Category's Playlists
        /// </summary>
        /// <param name="categoryId">(Required) The Spotify category ID for the category.</param>
        /// <param name="country">(Optional) A country: an ISO 3166-1 alpha-2 country code. </param>
        /// <param name="page">(Optional) Limit: The maximum number of items to return. Default: 20. Minimum: 1. Maximum: 50. - Offset: The index of the first item to return. Default: 0</param>
        /// <returns>Paging List of Simplified Playlist Object</returns>
        /// <exception cref="AuthAccessTokenRequiredException"></exception>
        public async Task<Paging<SimplifiedPlaylist>> GetCategoryPlaylistsAsync(
            string categoryId,
            string country = null,
            Page page = null) => 
                (await _client.LookupAsync<ContentResponse>(
                categoryId, LookupType.CategoriesPlaylists, 
                country, page: page))?.Playlists;

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
        public async Task<RecommendationsResponse> GetRecommendationsAsync(
            List<string> seedArtists = null,
            List<string> seedGenres = null,
            List<string> seedTracks = null,
            int? limit = null,
            string market = null,
            TuneableTrack minTuneableTrack = null,
            TuneableTrack maxTuneableTrack = null,
            TuneableTrack targetTuneableTrack = null) =>
                await _client.LookupRecommendationsAsync(
                    seedArtists, seedGenres, seedTracks,
                    limit, market, minTuneableTrack, maxTuneableTrack, 
                    targetTuneableTrack);

        /// <summary>
        /// Get Recommendation Genres
        /// </summary>
        /// <returns>Available Genre Seeds Object</returns>
        /// <exception cref="AuthAccessTokenRequiredException"></exception>
        public async Task<AvailableGenreSeeds> GetRecommendationGenresAsync() =>
            await _client.LookupRecommendationGenres();

        /// <summary>
        /// Get All New Releases
        /// </summary>
        /// <param name="country">(Optional) A country: an ISO 3166-1 alpha-2 country code. </param>
        /// <param name="page">(Optional) Limit: The maximum number of items to return. Default: 20. Minimum: 1. Maximum: 50. - Offset: The index of the first item to return. Default: 0</param>
        /// <returns>Paging List of Album Object</returns>
        /// <exception cref="AuthAccessTokenRequiredException"></exception>
        public async Task<Paging<Album>> GetAllNewReleasesAsync(
            string country = null,
            Page page = null) => 
                (await _client.LookupNewReleasesAsync(
                    country, page))?.Albums;

        /// <summary>
        /// Get All Featured Playlists
        /// </summary>
        /// <param name="country">(Optional) A country: an ISO 3166-1 alpha-2 country code. Provide this parameter if you want the list of returned items to be relevant to a particular country</param>
        /// <param name="locale">(Optional) The desired language, consisting of a lowercase ISO 639-1 language code and an uppercase ISO 3166-1 alpha-2 country code, joined by an underscore</param>
        /// <param name="timeStamp">(Optional) Use this parameter to specify the user’s local time to get results tailored for that specific date and time in the day.</param>
        /// <param name="page">(Optional) Limit: The maximum number of items to return. Default: 20. Minimum: 1. Maximum: 50. - Offset: The index of the first item to return. Default: 0</param>
        /// <returns>Paging List of Simplified Playlist Object</returns>
        /// <exception cref="AuthAccessTokenRequiredException"></exception>
        public async Task<Paging<SimplifiedPlaylist>> GetAllFeaturedPlaylistsAsync(
            string country = null,
            string locale = null,
            DateTime? timeStamp = null,
            Page page = null) => 
                (await _client.LookupFeaturedPlaylistsAsync(
                    country, locale, timeStamp, page))?.Playlists;
        #endregion Browse API

        #region Follow API
        /// <summary>
        /// Get Following State for Artists/Users
        /// <para>Scopes: FollowRead</para>
        /// </summary>
        /// <param name="ids">(Required) List of the artist or the user Spotify IDs to check.</param>
        /// <param name="followType">(Required) Either artist or user.</param>
        /// <returns>List of true or false values</returns>
        public async Task<Bools> GetFollowingStateForArtistsOrUsersAsync(
            List<string> ids,
            FollowType followType) => 
                await _client.AuthLookupFollowingStateAsync(
                    ids, followType);

        /// <summary>
        /// Check if Users Follow a Playlist
        /// <para>Scopes: PlaylistReadPrivate</para>
        /// </summary>
        /// <param name="ids">(Required) List of Spotify User IDs, the ids of the users that you want to check to see if they follow the playlist. Maximum: 5 ids.</param>
        /// <param name="playlistId">(Required) The Spotify ID of the playlist.</param>
        /// <returns>List of true or false values</returns>
        /// <exception cref="AuthUserTokenRequiredException"></exception>
        public async Task<Bools> CheckUsersFollowingPlaylistAsync(
            List<string> ids,
            string playlistId) => 
                await _client.AuthLookupUserFollowingPlaylistAsync(
                    ids, playlistId);

        /// <summary>
        /// Follow Artists or Users
        /// <para>Scopes: FollowModify</para>
        /// </summary>
        /// <param name="ids">(Required) List of the artist or the user Spotify IDs.</param>
        /// <param name="followType">(Required) Either artist or user</param>
        /// <returns>Status Object</returns>
        /// <exception cref="AuthUserTokenRequiredException"></exception>
        public async Task<Status> FollowArtistsOrUsersAsync(
            List<string> ids,
            FollowType followType) => 
                await _client.AuthFollowAsync(
                    ids, followType);

        /// <summary>
        /// Follow a Playlist
        /// <para>Scopes: FollowModify</para>
        /// </summary>
        /// <param name="playlistId">(Required) The Spotify ID of the playlist. Any playlist can be followed, regardless of its public/private status, as long as you know its playlist ID.</param>
        /// <param name="isPublic">(Optional) Defaults to true. If true the playlist will be included in user’s public playlists, if false it will remain private. To be able to follow playlists privately, the user must have granted the playlist-modify-private scope.</param>
        /// <returns>Status Object</returns>
        /// <exception cref="AuthUserTokenRequiredException"></exception>
        public async Task<Status> FollowPlaylistAsync(
            string playlistId,
            bool isPublic = true) =>
                await _client.AuthFollowPlaylistAsync(
                    playlistId, isPublic);

        /// <summary>
        /// Get User's Followed Artists
        /// <para>Scopes: FollowRead</para>
        /// </summary>
        /// <param name="cursor">(Optional) Limit: The maximum number of items to return. Default: 20. Minimum: 1. Maximum: 50. - After: The last artist ID retrieved from the previous request.</param>
        /// <returns>CursorPaging of Artist Object</returns>
        /// <exception cref="AuthUserTokenRequiredException"></exception>
        public async Task<CursorPaging<Artist>> GetUsersFollowedArtistsAsync(
            Cursor cursor = null) => 
                await _client.AuthLookupFollowedArtistsAsync(
                cursor);

        /// <summary>
        /// Unfollow Artists or Users
        /// <para>Scopes: FollowModify</para>
        /// </summary>
        /// <param name="ids">(Required) List of the artist or the user Spotify IDs.</param>
        /// <param name="followType">(Required) Either artist or user</param>
        /// <returns>Status Object</returns>
        /// <exception cref="AuthUserTokenRequiredException"></exception>
        public async Task<Status> UnfollowArtistsOrUsersAsync(
            List<string> ids,
            FollowType followType) =>
                await _client.AuthUnfollowAsync(
                    ids, followType);

        /// <summary>
        /// Unfollow Playlist
        /// <para>Scopes: PlaylistModifyPublic, PlaylistModifyPrivate</para>
        /// </summary>
        /// <param name="playlistId">(Required) The Spotify ID of the playlist that is to be no longer followed.</param>
        /// <returns>Status Object</returns>
        /// <exception cref="AuthUserTokenRequiredException"></exception>
        public async Task<Status> UnfollowPlaylistAsync(
            string playlistId) => 
                await _client.AuthUnfollowPlaylistAsync(
                    playlistId);
        #endregion Follow API

        #region Playlists API
        /// <summary>
        /// Add Tracks to a Playlist
        /// <para>Scopes: PlaylistModifyPublic, PlaylistModifyPrivate</para>
        /// </summary>
        /// <param name="playlistId">The Spotify ID for the playlist.</param>
        /// <param name="uris">(Optional) List of Spotify track URIs to add.</param>
        /// <param name="position">(Optional) The position to insert the tracks, a zero-based index.</param>
        /// <returns>Snapshot Object</returns>
        /// <exception cref="AuthUserTokenRequiredException"></exception>
        public async Task<Snapshot> AddTracksToPlaylistAsync(
            string playlistId,
            List<string> uris = null,
            int? position = null)
        {
            var request = new UriListRequest()
            {
                Uris = uris
            };
            return await _client.AuthAddTracksToPlaylistAsync(
                playlistId, request, position);
        }

        /// <summary>
        /// Get a Playlist
        /// </summary>
        /// <param name="playlistId">(Required) The Spotify ID for the playlist.</param>
        /// <param name="fields">(Optional) Filters for the query: a comma-separated list of the fields to return. If omitted, all fields are returned.</param>
        /// <returns>Playlist Object</returns>
        /// <exception cref="AuthAccessTokenRequiredException"></exception>
        public async Task<Playlist> GetPlaylistAsync(
            string playlistId, string fields = null) => 
            await _client.LookupAsync<Playlist>(
               playlistId, LookupType.Playlist, fields: fields);

        /// <summary>
        /// Remove Tracks from a Playlist
        /// <para>Scopes: PlaylistModifyPublic, PlaylistModifyPrivate</para>
        /// </summary>
        /// <param name="playlistId">(Required) The Spotify ID for the playlist.</param>
        /// <param name="uris">(Required) List of Spotify URIs of the tracks to remove</param>
        /// <param name="snapshotId">(Optional) The playlist’s snapshot ID against which you want to make the changes. The API will validate that the specified tracks exist and in the specified positions and make the changes, even if more recent changes have been made to the playlist.</param>
        /// <returns>Snapshot Object</returns>
        /// <exception cref="AuthUserTokenRequiredException"></exception>
        public async Task<Snapshot> RemoveTracksFromPlaylistAsync(
            string playlistId,
            List<string> uris,
            string snapshotId = null)
        {
            var request = new PlaylistTracksRequest()
            {
                Tracks = uris.Select(uri => 
                    new UriRequest()
                    {
                        Uri = uri
                    }).ToList(),
                SnapshotId = snapshotId
            };
            return await _client.AuthRemoveTracksFromPlaylistAsync(
                playlistId, request);
        }

        /// <summary>
        /// Get a Playlist's Tracks
        /// </summary>
        /// <param name="id">(Required) The Spotify ID for the playlist.</param>
        /// <param name="market">(Optional) An ISO 3166-1 alpha-2 country code or the string from_token</param>
        /// <param name="page">(Optional) Limit: The maximum number of items to return. Default: 100. Minimum: 1. Maximum: 100. - Offset: The index of the first item to return. Default: 0</param>
        /// <param name="fields">(Optional) Filters for the query: a comma-separated list of the fields to return. If omitted, all fields are returned.</param>
        /// <returns>Paging List of Playlist Track Object</returns>
        /// <exception cref="AuthAccessTokenRequiredException"></exception>
        public async Task<Paging<PlaylistTrack>> GetPlaylistTracksAsync(
            string id,
            string market = null,
            Page page = null,
            string fields = null) =>
                await _client.LookupAsync<Paging<PlaylistTrack>>(
                    id, LookupType.PlaylistTracks, market: market, fields: fields, page: page);

        /// <summary>
        /// Get a Playlist Cover Image
        /// </summary>
        /// <param name="playlistId">(Required) The Spotify ID for the playlist.</param>
        /// <returns>List of Image Object</returns>
        /// <exception cref="AuthUserTokenRequiredException"></exception>
        public async Task<List<Image>> GetPlaylistCoverImageAsync(
            string playlistId) => 
                await _client.AuthGetPlaylistCoverImageAsync(
                    playlistId);

        /// <summary>
        /// Upload a Custom Playlist Cover Image
        /// <para>Scopes: UserGeneratedContentImageUpload, PlaylistModifyPublic, PlaylistModifyPrivate</para>
        /// </summary>
        /// <param name="playlistId">(Required) The Spotify ID for the playlist.</param>
        /// <param name="file">(Required) JPEG Image File Bytes</param>
        /// <returns>Status Object</returns>
        /// <exception cref="AuthUserTokenRequiredException"></exception>
        public async Task<Status> UploadCustomPlaylistCoverImageAsync(
            string playlistId,
            byte[] file) => 
                await _client.AuthUploadCustomPlaylistImageAsync(
                    playlistId, file);

        /// <summary>
        /// Get a List of Current User's Playlists
        /// <para>Scopes: PlaylistReadPrivate, PlaylistReadCollaborative</para>
        /// </summary>
        /// <param name="cursor">(Optional) Limit: The maximum number of playlists to return. Default: 20. Minimum: 1. Maximum: 50. - The index of the first playlist to return. Default: 0 (the first object). Maximum offset: 100. Use with limit to get the next set of playlists.</param>
        /// <returns>CursorPaging of Simplified Playlist Object</returns>
        /// <exception cref="AuthUserTokenRequiredException"></exception>
        public async Task<CursorPaging<SimplifiedPlaylist>> GetUserPlaylistsAsync(
            Cursor cursor = null) => 
                await _client.AuthLookupUserPlaylistsAsync(
                cursor);

        /// <summary>
        /// Change a Playlist's Details
        /// <para>Scopes: PlaylistModifyPublic, PlaylistModifyPrivate</para>
        /// </summary>
        /// <param name="playlistId">(Required) The Spotify ID for the playlist.</param>
        /// <param name="name">(Optional) The new name for the playlist, for example "My New Playlist Title"</param>
        /// <param name="isPublic">(Optional) If true the playlist will be public, if false it will be private.</param>
        /// <param name="isCollaborative">(Optional) If true , the playlist will become collaborative and other users will be able to modify the playlist in their Spotify client. Note: You can only set collaborative to true on non-public playlists.</param>
        /// <param name="description">(Optional) Value for playlist description as displayed in Spotify Clients and in the Web API.</param>
        /// <returns>Status Object</returns>
        /// <exception cref="AuthUserTokenRequiredException"></exception>
        public async Task<Status> ChangePlaylistDetailsAsync(
            string playlistId,
            string name = null,
            bool? isPublic = null,
            bool? isCollaborative = null,
            string description = null)
        {
            var request = new PlaylistRequest()
            {
                Name = name,
                IsPublic = isPublic,
                IsCollaborative = isCollaborative,
                Description = description
            };
            return await _client.AuthChangePlaylistDetailsAsync(
                playlistId, request);
        }

        /// <summary>
        /// Get a List of a User's Playlists
        /// <para>Scopes: PlaylistReadPrivate, PlaylistReadCollaborative</para>
        /// </summary>
        /// <param name="userId">(Required) The user’s Spotify user ID.</param>
        /// <param name="cursor">(Optional) Limit: The maximum number of playlists to return. Default: 20. Minimum: 1. Maximum: 50. - Offset: The index of the first playlist to return. Default: 0 (the first object). Maximum offset: 100</param>
        /// <returns>CursorPaging of Simplified Playlist Object</returns>
        /// <exception cref="AuthUserTokenRequiredException"></exception>
        public async Task<CursorPaging<SimplifiedPlaylist>> GetUserPlaylistsAsync(
            string userId,
            Cursor cursor = null) => 
                await _client.AuthLookupUserPlaylistsAsync(
                    userId, cursor);

        /// <summary>
        /// Replace a Playlist's Tracks
        /// <para>Scopes: PlaylistModifyPublic, PlaylistModifyPrivate</para>
        /// </summary>
        /// <param name="playlistId">(Required) The Spotify ID for the playlist.</param>
        /// <param name="uris">(Optional) List of Spotify track URIs.</param>
        /// <exception cref="AuthUserTokenRequiredException"></exception>
        public async Task<Status> ReplacePlaylistTracksAsync(
            string playlistId,
            List<string> uris)
        {
            var request = new UriListRequest()
            {
                Uris = uris
            };
            return await _client.AuthReplacePlaylistTracksAsync(
                playlistId, request);
        }

        /// <summary>
        /// Create a Playlist
        /// <para>Scopes: PlaylistModifyPublic, PlaylistModifyPrivate</para>
        /// </summary>
        /// <param name="userId">(Required) The user’s Spotify user ID.</param>
        /// <param name="name">(Required) The name for the new playlist, for example "Your Coolest Playlist" . This name does not need to be unique; a user may have several playlists with the same name.</param>
        /// <param name="isPublic">(Optional) Defaults to true . If true the playlist will be public, if false it will be private. To be able to create private playlists, the user must have granted the playlist-modify-private scope</param>
        /// <param name="isCollaborative">(Optional) Defaults to false . If true the playlist will be collaborative. Note that to create a collaborative playlist you must also set public to false . To create collaborative playlists you must have granted playlist-modify-private and playlist-modify-public scopes.</param>
        /// <param name="description">(Optional) Value for playlist description as displayed in Spotify Clients and in the Web API.</param>
        /// <returns>Playlist Object</returns>
        /// <exception cref="AuthUserTokenRequiredException"></exception>
        public async Task<Playlist> CreatePlaylistAsync(
            string userId,
            string name,
            bool? isPublic = null,
            bool? isCollaborative = null,
            string description = null)
        {
            var request = new PlaylistRequest()
            {
                Name = name,
                IsPublic = isPublic,
                IsCollaborative = isCollaborative,
                Description = description
            };
            return await _client.AuthCreatePlaylistAsync(
                userId, request);
        }

        /// <summary>
        /// Reorder a Playlist's Tracks
        /// <para>Scopes: PlaylistModifyPublic, PlaylistModifyPrivate</para>
        /// </summary>
        /// <param name="playlistId">The Spotify ID for the playlist.</param>
        /// <param name="rangeStart">(Required) The position of the first track to be reordered.</param>
        /// <param name="insertBefore">(Required) The position where the tracks should be inserted. To reorder the tracks to the end of the playlist, simply set insert_before to the position after the last track.</param>
        /// <param name="rangeLength">(Optional) The amount of tracks to be reordered. Defaults to 1 if not set. The range of tracks to be reordered begins from the range_start position, and includes the range_length subsequent tracks.</param>
        /// <param name="snapshotId">(Optional) The playlist’s snapshot ID against which you want to make the changes.</param>
        /// <returns>Snapshot Object</returns>
        /// <exception cref="AuthUserTokenRequiredException"></exception>
        public async Task<Snapshot> ReorderPlaylistTracksAsync(
            string playlistId,
            int rangeStart,
            int insertBefore,
            int? rangeLength = 1,
            string snapshotId = null)
        {
            var request = new PlaylistReorderRequest()
            {
                RangeStart = rangeStart,
                InsertBefore = insertBefore,
                RangeLength = rangeLength,
                SnapshotId = snapshotId
            };
            return await _client.AuthReorderPlaylistTracksAsync(
                 playlistId, request);
        }
        #endregion Playlists API

        #region Library API
        /// <summary>
        /// Check User's Saved Albums
        /// <para>Scopes: LibraryRead</para>
        /// </summary>
        /// <param name="itemIds">(Required) List of the Spotify IDs for the albums</param>
        /// <returns>List of true or false values</returns>
        /// <exception cref="AuthUserTokenRequiredException"></exception>
        public async Task<Bools> CheckUserSavedAlbumsAsync(
            List<string> itemIds) => 
                await _client.AuthLookupCheckUserSavedAlbumsAsync(
                    itemIds);

        /// <summary>
        /// Save Tracks for User
        /// <para>Scopes: LibraryModify</para>
        /// </summary>
        /// <param name="itemIds">(Required) List of the Spotify IDs for the tracks</param>
        /// <returns>Status Object</returns>
        /// <exception cref="AuthUserTokenRequiredException"></exception>
        public async Task<Status> SaveUserTracksAsync(
            List<string> itemIds) => 
                await _client.AuthSaveUserTracksAsync(
                    itemIds);

        /// <summary>
        /// Remove Albums for Current User
        /// <para>Scopes: LibraryModify</para>
        /// </summary>
        /// <param name="itemIds">(Required) List of the Spotify IDs for the albums</param>
        /// <returns>Status Object</returns>
        /// <exception cref="AuthUserTokenRequiredException"></exception>
        public async Task<Status> RemoveUserAlbumsAsync(
            List<string> itemIds) => 
                await _client.AuthRemoveUserAlbumsAsync(
                    itemIds);

        /// <summary>
        /// Save Albums for Current User
        /// <para>Scopes: LibraryModify</para>
        /// </summary>
        /// <param name="itemIds">(Required) List of the Spotify IDs for the albums</param>
        /// <returns>Status Object</returns>
        /// <exception cref="AuthUserTokenRequiredException"></exception>
        public async Task<Status> SaveUserAlbumsAsync(
            List<string> itemIds) => 
                await _client.AuthSaveUserAlbumsAsync(
                    itemIds);

        /// <summary>
        /// Remove User's Saved Tracks
        /// <para>Scopes: LibraryModify</para>
        /// </summary>
        /// <param name="itemIds">(Required) List of the Spotify IDs for the tracks</param>
        /// <returns>Status Object</returns>
        /// <exception cref="AuthUserTokenRequiredException"></exception>
        public async Task<Status> RemoveUserTracksAsync(
            List<string> itemIds) => 
                await _client.AuthRemoveUserTracksAsync(
                    itemIds);

        /// <summary>
        /// Get User's Saved Albums
        /// <para>Scopes: LibraryRead</para>
        /// </summary>
        /// <param name="market">(Optional) An ISO 3166-1 alpha-2 country code or the string from_token. Provide this parameter if you want to apply Track Relinking.</param>
        /// <param name="cursor">(Optional) Limit: The maximum number of objects to return. Default: 20. Minimum: 1. Maximum: 50. - Offset: The index of the first object to return. Default: 0 (i.e., the first object). Use with limit to get the next set of objects.</param>
        /// <returns>Cursor Paging of Saved Album Object</returns>
        /// <exception cref="AuthUserTokenRequiredException"></exception>
        public async Task<CursorPaging<SavedAlbum>> GetUserSavedAlbumsAsync(
            string market = null,
            Cursor cursor = null) => 
                await _client.AuthLookupUserSavedAlbumsAsync(
                    market, cursor);

        /// <summary>
        /// Get User's Saved Tracks
        /// <para>Scopes: LibraryRead</para>
        /// </summary>
        /// <param name="market">(Optional) An ISO 3166-1 alpha-2 country code or the string from_token. Provide this parameter if you want to apply Track Relinking.</param>
        /// <param name="cursor">(Optional) Limit: The maximum number of objects to return. Default: 20. Minimum: 1. Maximum: 50. - Offset: The index of the first object to return. Default: 0 (i.e., the first object). Use with limit to get the next set of objects.</param>
        /// <returns>Cursor Paging of Saved Track Object</returns>
        /// <exception cref="AuthUserTokenRequiredException"></exception>
        public async Task<CursorPaging<SavedTrack>> GetUserSavedTracksAsync(
            string market = null,
            Cursor cursor = null) => 
                await _client.AuthLookupUserSavedTracksAsync(market,
                    cursor);

        /// <summary>
        /// Check User's Saved Tracks
        /// <para>Scopes: LibraryRead</para>
        /// </summary>
        /// <param name="itemIds">(Required) List of the Spotify IDs for the tracks</param>
        /// <returns>List of true or false values</returns>
        /// <exception cref="AuthUserTokenRequiredException"></exception>
        public async Task<Bools> CheckUserSavedTracksAsync(
            List<string> itemIds) => 
            await _client.AuthLookupCheckUserSavedTracksAsync(
                itemIds);
        #endregion Library API

        #region Artists API
        /// <summary>
        /// Get Multiple Artists
        /// </summary>
        /// <param name="ids">(Required) List of the Spotify IDs for the artists. Maximum: 50 IDs.</param>
        /// <returns>List of Artist Object</returns>
        /// <exception cref="AuthAccessTokenRequiredException"></exception>
        public async Task<List<Artist>> GetMultipleArtistsAsync(
        List<string> ids) =>
            (await _client.LookupAsync(
                ids, LookupType.Artists))?.Artists;

        /// <summary>
        /// Get an Artist
        /// </summary>
        /// <param name="id">(Required) The Spotify ID of the artist.</param>
        /// <returns>Artist Object</returns>
        /// <exception cref="AuthAccessTokenRequiredException"></exception>
        public async Task<Artist> GetArtistAsync(
            string id) =>
                await _client.LookupAsync<Artist>(
                    id, LookupType.Artists);

        /// <summary>
        /// Get an Artist's Albums
        /// </summary>
        /// <param name="id">(Required) The Spotify ID for the artist.</param>
        /// <param name="includeGroup">(Optional) Filters the response. If not supplied, all album types will be returned</param>
        /// <param name="market">(Optional) An ISO 3166-1 alpha-2 country code or the string from_token</param>
        /// <param name="page">(Optional) Limit: The number of album objects to return. Default: 20. Minimum: 1. Maximum: 50. - Offset: The index of the first album to return. Default: 0</param>
        /// <returns>Paging List of Album Object</returns>
        /// <exception cref="AuthAccessTokenRequiredException"></exception>
        public async Task<Paging<Album>> GetArtistAlbumsAsync(
            string id,
            IncludeGroup includeGroup = null,
            string market = null,
            Page page = null) =>
                await _client.LookupArtistAlbumsAsync(
                    id, includeGroup, market, page);

        /// <summary>
        /// Get an Artist's Top Tracks
        /// </summary>
        /// <param name="id">(Required) The Spotify ID for the artist.</param>
        /// <param name="market">(Required) An ISO 3166-1 alpha-2 country code or the string from_token</param>
        /// <returns>List of Track Object</returns>
        /// <exception cref="AuthAccessTokenRequiredException"></exception>
        public async Task<List<Track>> GetArtistTopTracksAsync(
            string id,
            string market) =>
                (await _client.LookupArtistTopTracksAsync(
                    id, market))?.Tracks;

        /// <summary>
        /// Get an Artist's Related Artists
        /// </summary>
        /// <param name="id">(Requird) The Spotify ID for the artist.</param>
        /// <returns>List of Artist Object</returns>
        /// <exception cref="AuthAccessTokenRequiredException"></exception>
        public async Task<List<Artist>> GetArtistRelatedArtistsAsync(
            string id) =>
                (await _client.LookupArtistRelatedArtistsAsync(
                    id))?.Artists;
        #endregion Artists API

        #region Player API
        /// <summary>
        /// Add an Item to the User's Playback Queue
        /// <para>Scopes: ConnectModifyPlaybackState</para>
        /// </summary>
        /// <param name="uri">(Required) The uri of the item to add to the queue. Must be a track or an episode uri.</param>
        /// <param name="deviceId">(Optional) The id of the device this command is targeting. If not supplied, the user’s currently active device is the target.</param>
        /// <returns>Status Object</returns>
        /// <exception cref="AuthUserTokenRequiredException"></exception>
        public async Task<Status> UserPlaybackAddToQueueAsync(
            string uri, string deviceId = null) => 
                await _client.AuthUserPlaybackAddToQueueAsync(uri, deviceId);

        /// <summary>
        /// Skip User’s Playback To Next Track
        /// <para>Scopes: ConnectModifyPlaybackState</para>
        /// </summary>
        /// <param name="deviceId">(Optional) The id of the device this command is targeting. If not supplied, the user’s currently active device is the target.</param>
        /// <returns>Status Object</returns>
        /// <exception cref="AuthUserTokenRequiredException"></exception>
        public async Task<Status> UserPlaybackNextTrackAsync(
            string deviceId = null) => 
                await _client.AuthUserPlaybackNextTrackAsync(
                    deviceId);

        /// <summary>
        /// Seek To Position In Currently Playing Track
        /// <para>Scopes: ConnectModifyPlaybackState</para>
        /// </summary>
        /// <param name="position">(Required) The position in milliseconds to seek to. Must be a positive number. Passing in a position that is greater than the length of the track will cause the player to start playing the next song.</param>
        /// <param name="deviceId">(Optional) The id of the device this command is targeting. If not supplied, the user’s currently active device is the target.</param>
        /// <returns>Status Object</returns>
        /// <exception cref="AuthUserTokenRequiredException"></exception>
        public async Task<Status> UserPlaybackSeekTrackAsync(
            int position,
            string deviceId = null) =>
                await _client.AuthUserPlaybackSeekTrackAsync(
                    position, deviceId);

        /// <summary>
        /// Get a User's Available Devices
        /// <para>Scopes: ConnectReadPlaybackState</para>
        /// </summary>
        /// <returns>Devices Object</returns>
        /// <exception cref="AuthUserTokenRequiredException"></exception>
        public async Task<Devices> GetUserPlaybackDevicesAsync() => 
            await _client.AuthLookupUserPlaybackDevicesAsync();

        /// <summary>
        /// Toggle Shuffle For User’s Playback
        /// <para>Scopes: ConnectModifyPlaybackState</para>
        /// </summary>
        /// <param name="state">(Required) true : Shuffle user’s playback, false : Do not shuffle user’s playback</param>
        /// <param name="deviceId">(Optional) The id of the device this command is targeting. If not supplied, the user’s currently active device is the target.</param>
        /// <returns>Status Object</returns>
        /// <exception cref="AuthUserTokenRequiredException"></exception>
        public async Task<Status> UserPlaybackToggleShuffleAsync(
            bool state,
            string deviceId = null) => 
                await _client.AuthUserPlaybackToggleShuffleAsync(
                  state, deviceId);

        /// <summary>
        /// Transfer a User's Playback
        /// <para>Scopes: ConnectModifyPlaybackState</para>
        /// </summary>
        /// <param name="deviceIds">(Required) List containing the ID of the device on which playback should be started/transferred. Although an array is accepted, only a single device_id is currently supported.</param>
        /// <param name="play">(Optional) true: ensure playback happens on new device. false or not provided: keep the current playback state.</param>
        /// <returns>Status Object</returns>
        /// <exception cref="AuthUserTokenRequiredException"></exception>
        public async Task<Status> UserPlaybackTransferAsync(
             List<string> deviceIds,
             bool? play = null)
        {
            var request = new DevicesRequest()
            {
                DeviceIds = deviceIds,
                Play = play
            };
            return await _client.AuthUserPlaybackTransferAsync(
                request);
        }

        /// <summary>
        /// Get Current User's Recently Played Tracks
        /// <para>Scopes: ListeningRecentlyPlayed</para>
        /// </summary>
        /// <param name="cursor">(Optional) Limit: The maximum number of items to return. Default: 20. Minimum: 1. Maximum: 50. - After: A Unix timestamp in milliseconds. Returns all items after (but not including) this cursor position. If after is specified, before must not be specified. Before - (Optional) A Unix timestamp in milliseconds. Returns all items before (but not including) this cursor position. If before is specified, after must not be specified.</param>
        /// <returns>Cursor Paging of Play History Object</returns>
        /// <exception cref="AuthUserTokenRequiredException"></exception>
        public async Task<CursorPaging<PlayHistory>> GetUserRecentlyPlayedTracksAsync(
            Cursor cursor = null) => 
                await _client.AuthLookupUserRecentlyPlayedTracksAsync(
                    cursor);

        /// <summary>
        /// Start/Resume a User's Playback
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
        public async Task<Status> UserPlaybackStartResumeAsync(
            string contextUri = null,
            List<string> uris = null,
            int? offsetPosition = null,
            string offsetUri = null,
            int? position = null,
            string deviceId = null)
        {
            var request = new PlaybackRequest()
            {
                ContextUri = contextUri,
                Uris = uris,
                Offset = (offsetPosition != null)
                    ? new PositionRequest()
                    {
                        Position = offsetPosition
                    }
                    : (object)new UriRequest()
                    {
                        Uri = offsetUri
                    },
                Position = position               
            };
            return await _client.AuthUserPlaybackStartResumeAsync(
                request, deviceId);
        }

        /// <summary>
        /// Set Repeat Mode On User’s Playback
        /// <para>Scopes: ConnectModifyPlaybackState</para>
        /// </summary>
        /// <param name="state">(Required) track, context or off. track will repeat the current track. context will repeat the current context. off will turn repeat off.</param>
        /// <param name="deviceId">(Optional) The id of the device this command is targeting. If not supplied, the user’s currently active device is the target.</param>
        /// <returns>Status Object</returns>
        /// <exception cref="AuthUserTokenRequiredException"></exception>
        public async Task<Status> UserPlaybackSetRepeatModeAsync(
            RepeatState state,
            string deviceId = null) => 
                await _client.AuthUserPlaybackSetRepeatModeAsync(
                    state, deviceId);

        /// <summary>
        /// Pause a User's Playback
        /// <para>Scopes: ConnectModifyPlaybackState</para>
        /// </summary>
        /// <param name="deviceId">(Optional) The id of the device this command is targeting. If not supplied, the user’s currently active device is the target.</param>
        /// <returns>Status Object</returns>
        /// <exception cref="AuthUserTokenRequiredException"></exception>
        public async Task<Status> UserPlaybackPauseAsync(
            string deviceId = null) => 
                await _client.AuthUserPlaybackPauseAsync(
                    deviceId);

        /// <summary>
        /// Skip User’s Playback To Previous Track
        /// <para>Scopes: ConnectModifyPlaybackState</para>
        /// </summary>
        /// <param name="deviceId">(Optional) The id of the device this command is targeting. If not supplied, the user’s currently active device is the target.</param>
        /// <returns>Status Object</returns>
        /// <exception cref="AuthUserTokenRequiredException"></exception>
        public async Task<Status> UserPlaybackPreviousTrackAsync(
            string deviceId = null) => 
                await _client.AuthUserPlaybackPreviousTrackAsync(
                    deviceId);

        /// <summary>
        /// Get Information About The User's Current Playback
        /// <para>Scopes: ConnectReadPlaybackState</para>
        /// </summary>
        /// <param name="market">(Optional) An ISO 3166-1 alpha-2 country code or the string from_token. Provide this parameter if you want to apply Track Relinking.</param>
        /// <returns>Currently Playing Object</returns>
        /// <exception cref="AuthUserTokenRequiredException"></exception>
        public async Task<CurrentlyPlaying> GetUserPlaybackCurrentAsync(
            string market = null) => 
                await _client.AuthLookupUserPlaybackCurrentAsync(
                    market);

        /// <summary>
        /// Get the User's Currently Playing Track
        /// <para>Scopes: ConnectReadCurrentlyPlaying, ConnectReadPlaybackState</para>
        /// </summary>
        /// <param name="market">(Optional) An ISO 3166-1 alpha-2 country code or the string from_token. Provide this parameter if you want to apply Track Relinking.</param>
        /// <returns>Simplified Currently Playing Object</returns>
        /// <exception cref="AuthUserTokenRequiredException"></exception>
        public async Task<SimplifiedCurrentlyPlaying> GetUserPlaybackCurrentTrackAsync(
            string market = null) => 
                await _client.AuthLookupUserPlaybackCurrentTrackAsync(
                    market);

        /// <summary>
        /// Set Volume For User's Playback
        /// <para>Scopes: ConnectModifyPlaybackState</para>
        /// </summary>
        /// <param name="percent">(Required) The volume to set. Must be a value from 0 to 100 inclusive.</param>
        /// <param name="deviceId">(Optional) The id of the device this command is targeting. If not supplied, the user’s currently active device is the target.</param>
        /// <returns>Status Object</returns>
        /// <exception cref="AuthUserTokenRequiredException"></exception>
        public async Task<Status> UserPlaybackSetVolumeAsync(
            int percent,
            string deviceId = null) => 
                await _client.AuthUserPlaybackSetVolumeAsync(
                    percent, deviceId);
        #endregion Player API

        #region Personalisation API
        /// <summary>
        /// Get a User's Top Artists
        /// <para>Scopes: ListeningTopRead</para>
        /// </summary>
        /// <param name="timeRange">(Optional) Over what time frame the affinities are computed. Long Term: alculated from several years of data and including all new data as it becomes available, Medium Term: (Default) approximately last 6 months, Short Term: approximately last 4 weeks</param>
        /// <param name="cursor">(Optional) Limit: The number of entities to return. Default: 20. Minimum: 1. Maximum: 50. For example - Offset: he index of the first entity to return. Default: 0. Use with limit to get the next set of entities.</param>
        /// <returns>Cursor Paging of Artist Object</returns>
        /// <exception cref="AuthUserTokenRequiredException"></exception>
        public async Task<CursorPaging<Artist>> GetUserTopArtistsAsync(
            TimeRange? timeRange = null,
            Cursor cursor = null) =>
                await _client.AuthLookupUserTopArtistsAsync(
                    timeRange, cursor);

        /// <summary>
        /// Get a User's Top Tracks
        /// <para>Scopes: ListeningTopRead</para>
        /// </summary>
        /// <param name="timeRange">(Optional) Over what time frame the affinities are computed. Long Term: alculated from several years of data and including all new data as it becomes available, Medium Term: (Default) approximately last 6 months, Short Term: approximately last 4 weeks</param>
        /// <param name="cursor">(Optional) Limit: The number of entities to return. Default: 20. Minimum: 1. Maximum: 50. For example - Offset: he index of the first entity to return. Default: 0. Use with limit to get the next set of entities.</param>
        /// <returns>Cursor Paging of Track Object</returns>
        /// <exception cref="AuthUserTokenRequiredException"></exception>
        public async Task<CursorPaging<Track>> GetUserTopTracksAsync(
            TimeRange? timeRange = null,
            Cursor cursor = null) =>
                await _client.AuthLookupUserTopTracksAsync(
                    timeRange, cursor);
        #endregion Personalization API

        #region User Profile API
        /// <summary>
        /// Get a User's Profile
        /// </summary>
        /// <param name="userId">The user’s Spotify user ID.</param>
        /// <returns>Public User Object</returns>
        /// <exception cref="AuthUserTokenRequiredException"></exception>
        public async Task<PublicUser> GetUserProfileAsync(
            string userId) =>
                await _client.AuthLookupUserProfileAsync(
                    userId);

        /// <summary>
        /// Get Current User's Profile
        /// <para>Scopes: UserReadEmail, UserReadPrivate</para>
        /// </summary>
        /// <returns>Private User Object</returns>
        /// <exception cref="AuthUserTokenRequiredException"></exception>
        public async Task<PrivateUser> GetUserProfileAsync() =>
            await _client.AuthLookupUserProfileAsync();
        #endregion User Profile API

        #region Albums API
        /// <summary>
        /// Get Multiple Albums
        /// </summary>
        /// <param name="ids">(Required) List of the Spotify IDs for the albums. Maximum: 20 IDs.</param>
        /// <param name="market">(Optional) An ISO 3166-1 alpha-2 country code or the string from_token. Provide this parameter if you want to apply Track Relinking.</param>
        /// <returns>List of Album Object</returns>
        /// <exception cref="AuthAccessTokenRequiredException"></exception>
        public async Task<List<Album>> GetMultipleAlbumsAsync(
            List<string> ids,
            string market = null) =>
                (await _client.LookupAsync(
                    ids, LookupType.Albums, market))?.Albums;

        /// <summary>
        /// Get an Album
        /// </summary>
        /// <param name="id">(Required) The Spotify ID of the album.</param>
        /// <param name="market">(Optional) The market you’d like to request. An ISO 3166-1 alpha-2 country code.</param>
        /// <returns>Album Object</returns>
        /// <exception cref="AuthAccessTokenRequiredException"></exception>
        public async Task<Album> GetAlbumAsync(
            string id,
            string market = null) =>
                await _client.LookupAsync<Album>(
                    id, LookupType.Albums, market);

        /// <summary>
        /// Get an Album's Tracks
        /// </summary>
        /// <param name="id">(Required) The Spotify ID of the album.</param>      
        /// <param name="market">(Optional) An ISO 3166-1 alpha-2 country code or the string from_token. Provide this parameter if you want to apply Track Relinking.</param>
        /// <param name="page">(Optional) Limit: The maximum number of tracks to return. Default: 20. Minimum: 1. Maximum: 50. - Offset: The index of the first track to return</param>
        /// <returns>Paging of Track Object</returns>
        /// <exception cref="AuthAccessTokenRequiredException"></exception>
        public async Task<Paging<Track>> GetAlbumTracksAsync(
            string id,
            string market = null,
            Page page = null) =>
                await _client.LookupAsync<Paging<Track>>(
                    id, LookupType.AlbumTracks, market, page: page);
        #endregion Albums API

        #region Tracks API
        /// <summary>
        /// Get Audio Features for a Track
        /// </summary>
        /// <param name="id">(Required) The Spotify ID for the track</param>
        /// <returns>Audio Features Object</returns>
        /// <exception cref="AuthAccessTokenRequiredException"></exception>
        public async Task<AudioFeatures> GetTrackAudioFeaturesAsync(
            string id) =>
                await _client.LookupAsync<AudioFeatures>(
                    id, LookupType.AudioFeatures);

        /// <summary>
        /// Get a Track
        /// </summary>
        /// <param name="id">(Required) The Spotify ID for the track.</param>
        /// <param name="market">(Optional) An ISO 3166-1 alpha-2 country code or the string from_token. Provide this parameter if you want to apply Track Relinking.</param>
        /// <returns>Track Object</returns>
        /// <exception cref="AuthAccessTokenRequiredException"></exception>
        public async Task<Track> GetTrackAsync(
            string id,
            string market = null) =>
                await _client.LookupAsync<Track>(
                    id, LookupType.Tracks, market);

        /// <summary>
        /// Get Audio Analysis for a Track
        /// </summary>
        /// <param name="id">(Required) The Spotify ID for the track</param>
        /// <returns>Audio Analysis Object</returns>
        /// <exception cref="AuthAccessTokenRequiredException"></exception>
        public async Task<AudioAnalysis> GetTrackAudioAnalysisAsync(
            string id) => 
                await _client.LookupAsync<AudioAnalysis>(
                    id, LookupType.AudioAnalysis);

        /// <summary>
        /// Get Audio Features for Several Tracks
        /// </summary>
        /// <param name="ids">(Required) List of the Spotify IDs for the tracks. Maximum: 100 IDs.</param>
        /// <returns>List of Audio Features Object</returns>
        /// <exception cref="AuthAccessTokenRequiredException"></exception>
        public async Task<List<AudioFeatures>> GetSeveralTracksAudioFeaturesAsync(
            List<string> ids) =>
                (await _client.LookupAsync(
                    ids, LookupType.AudioFeatures))?.AudioFeatures;

        /// <summary>
        /// Get Several Tracks
        /// </summary>
        /// <param name="ids">(Required) List of the Spotify IDs for the tracks. Maximum: 50 IDs.</param>
        /// <param name="market">(Optional) An ISO 3166-1 alpha-2 country code or the string from_token. Provide this parameter if you want to apply Track Relinking.</param>
        /// <returns>List of Track Object</returns>
        /// <exception cref="AuthAccessTokenRequiredException"></exception>
        public async Task<List<Track>> GetSeveralTracksAsync(
            List<string> ids,
            string market = null) =>
                (await _client.LookupAsync(
                    ids, LookupType.Tracks, market))?.Tracks;
        #endregion Tracks API
    }
}