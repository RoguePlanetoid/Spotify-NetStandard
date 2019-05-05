Spotify.NetStandard 
===

Spotify API .NET Standard Library

## AccessToken

Access Token Object

### Expiration

Token Expiration Date

### Refresh

Refresh

### Scopes

Scopes

### Token

Access Token

### TokenType

Token Type


## TokenType

Authentication Token Type

### Access

Access Token

### User

User Token

## AuthAccessTokenRequiredException

Auth Access Token Expired or Required Error


## AuthCodeStateException

Auth Code State Error


## AuthCodeValueException

Auth Code Value Error


## AuthException

Auth Code Error


## AuthTokenRequiredException

Auth Token Expired or Required Error


## AuthUserTokenRequiredException

Auth User Token Expired or Required Error


## ISpotifyApi

Spotify API

### AddTracksToPlaylistAsync(playlistId, uris, position)

Add Tracks to a Playlist 
Scopes: PlaylistModifyPublic, PlaylistModifyPrivate


| Name | Description |
| ---- | ----------- |
| playlistId | *System.String*<br>The Spotify ID for the playlist. |
| uris | *System.Collections.Generic.List{System.String}*<br>(Optional) List of Spotify track URIs to add. |
| position | *System.Nullable{System.Int32}*<br>(Optional) The position to insert the tracks, a zero-based index. |

#### Returns

Snapshot Object

*Spotify.NetStandard.Client.Exceptions.AuthUserTokenRequiredException:* 

### ChangePlaylistDetailsAsync(playlistId, name, isPublic, isCollaborative, description)

Change a Playlist's Details 
Scopes: PlaylistModifyPublic, PlaylistModifyPrivate


| Name | Description |
| ---- | ----------- |
| playlistId | *System.String*<br>(Required) The Spotify ID for the playlist. |
| name | *System.String*<br>(Optional) The new name for the playlist, for example "My New Playlist Title" |
| isPublic | *System.Nullable{System.Boolean}*<br>(Optional) If true the playlist will be public, if false it will be private. |
| isCollaborative | *System.Nullable{System.Boolean}*<br>(Optional) If true , the playlist will become collaborative and other users will be able to modify the playlist in their Spotify client. Note: You can only set collaborative to true on non-public playlists. |
| description | *System.String*<br>(Optional) Value for playlist description as displayed in Spotify Clients and in the Web API. |

#### Returns

Status Object

*Spotify.NetStandard.Client.Exceptions.AuthUserTokenRequiredException:* 

### CheckUserSavedAlbumsAsync(itemIds)

Check User's Saved Albums 
Scopes: LibraryRead


| Name | Description |
| ---- | ----------- |
| itemIds | *System.Collections.Generic.List{System.String}*<br>(Required) List of the Spotify IDs for the albums |

#### Returns

List of true or false values

*Spotify.NetStandard.Client.Exceptions.AuthUserTokenRequiredException:* 

### CheckUserSavedTracksAsync(itemIds)

Check User's Saved Tracks 
Scopes: LibraryRead


| Name | Description |
| ---- | ----------- |
| itemIds | *System.Collections.Generic.List{System.String}*<br>(Required) List of the Spotify IDs for the tracks |

#### Returns

List of true or false values

*Spotify.NetStandard.Client.Exceptions.AuthUserTokenRequiredException:* 

### CheckUsersFollowingPlaylistAsync(ids, playlistId)

Check if Users Follow a Playlist 
Scopes: PlaylistReadPrivate


| Name | Description |
| ---- | ----------- |
| ids | *System.Collections.Generic.List{System.String}*<br>(Required) List of Spotify User IDs, the ids of the users that you want to check to see if they follow the playlist. Maximum: 5 ids. |
| playlistId | *System.String*<br>(Required) The Spotify ID of the playlist. |

#### Returns

List of true or false values

*Spotify.NetStandard.Client.Exceptions.AuthUserTokenRequiredException:* 

### CreatePlaylistAsync(userId, name, isPublic, isCollaborative, description)

Create a Playlist 
Scopes: PlaylistModifyPublic, PlaylistModifyPrivate


| Name | Description |
| ---- | ----------- |
| userId | *System.String*<br>(Required) The user’s Spotify user ID. |
| name | *System.String*<br>(Required) The name for the new playlist, for example "Your Coolest Playlist" . This name does not need to be unique; a user may have several playlists with the same name. |
| isPublic | *System.Nullable{System.Boolean}*<br>(Optional) Defaults to true . If true the playlist will be public, if false it will be private. To be able to create private playlists, the user must have granted the playlist-modify-private scope |
| isCollaborative | *System.Nullable{System.Boolean}*<br>(Optional) Defaults to false . If true the playlist will be collaborative. Note that to create a collaborative playlist you must also set public to false . To create collaborative playlists you must have granted playlist-modify-private and playlist-modify-public scopes. |
| description | *System.String*<br>(Optional) Value for playlist description as displayed in Spotify Clients and in the Web API. |

#### Returns

Playlist Object

*Spotify.NetStandard.Client.Exceptions.AuthUserTokenRequiredException:* 

### FollowArtistsOrUsersAsync(ids, followType)

Follow Artists or Users 
Scopes: FollowModify


| Name | Description |
| ---- | ----------- |
| ids | *System.Collections.Generic.List{System.String}*<br>(Required) List of the artist or the user Spotify IDs. |
| followType | *Spotify.NetStandard.Enums.FollowType*<br>(Required) Either artist or user |

#### Returns

Status Object

*Spotify.NetStandard.Client.Exceptions.AuthUserTokenRequiredException:* 

### FollowPlaylistAsync(playlistId, isPublic)

Follow a Playlist 
Scopes: FollowModify


| Name | Description |
| ---- | ----------- |
| playlistId | *System.String*<br>(Required) The Spotify ID of the playlist. Any playlist can be followed, regardless of its public/private status, as long as you know its playlist ID. |
| isPublic | *System.Boolean*<br>(Optional) Defaults to true. If true the playlist will be included in user’s public playlists, if false it will remain private. To be able to follow playlists privately, the user must have granted the playlist-modify-private scope. |

#### Returns

Status Object

*Spotify.NetStandard.Client.Exceptions.AuthUserTokenRequiredException:* 

### GetAlbumAsync(id, market)

Get an Album

| Name | Description |
| ---- | ----------- |
| id | *System.String*<br>(Required) The Spotify ID of the album. |
| market | *System.String*<br>(Optional) The market you’d like to request. An ISO 3166-1 alpha-2 country code. |

#### Returns

Album Object

*Spotify.NetStandard.Client.Exceptions.AuthAccessTokenRequiredException:* 

### GetAlbumTracksAsync(id, market, page)

Get an Album's Tracks

| Name | Description |
| ---- | ----------- |
| id | *System.String*<br>(Required) The Spotify ID of the album. |
| market | *System.String*<br>(Optional) An ISO 3166-1 alpha-2 country code or the string from_token. Provide this parameter if you want to apply Track Relinking. |
| page | *Spotify.NetStandard.Requests.Page*<br>(Optional) Limit: The maximum number of tracks to return. Default: 20. Minimum: 1. Maximum: 50. - Offset: The index of the first track to return |

#### Returns

Paging of Track Object

*Spotify.NetStandard.Client.Exceptions.AuthAccessTokenRequiredException:* 

### GetAllCategoriesAsync(country, locale, page)

Get All Categories

| Name | Description |
| ---- | ----------- |
| country | *System.String*<br>(Optional) A country: an ISO 3166-1 alpha-2 country code. |
| locale | *System.String*<br>(Optional) The desired language, consisting of an ISO 639-1 language code and an ISO 3166-1 alpha-2 country code, joined by an underscore. |
| page | *Spotify.NetStandard.Requests.Page*<br>(Optional) Limit: The maximum number of categories to return. Default: 20. Minimum: 1. Maximum: 50. Offset: The index of the first item to return. Default: 0 |

#### Returns

Paging List of Category Object

*Spotify.NetStandard.Client.Exceptions.AuthAccessTokenRequiredException:* 

### GetAllFeaturedPlaylistsAsync(country, locale, timeStamp, page)

Get All Featured Playlists

| Name | Description |
| ---- | ----------- |
| country | *System.String*<br>(Optional) A country: an ISO 3166-1 alpha-2 country code. Provide this parameter if you want the list of returned items to be relevant to a particular country |
| locale | *System.String*<br>(Optional) The desired language, consisting of a lowercase ISO 639-1 language code and an uppercase ISO 3166-1 alpha-2 country code, joined by an underscore |
| timeStamp | *System.Nullable{System.DateTime}*<br>(Optional) Use this parameter to specify the user’s local time to get results tailored for that specific date and time in the day. |
| page | *Spotify.NetStandard.Requests.Page*<br>(Optional) Limit: The maximum number of items to return. Default: 20. Minimum: 1. Maximum: 50. - Offset: The index of the first item to return. Default: 0 |

#### Returns

Paging List of Playlist Object

*Spotify.NetStandard.Client.Exceptions.AuthAccessTokenRequiredException:* 

### GetAllNewReleasesAsync(country, page)

Get All New Releases

| Name | Description |
| ---- | ----------- |
| country | *System.String*<br>(Optional) A country: an ISO 3166-1 alpha-2 country code. |
| page | *Spotify.NetStandard.Requests.Page*<br>(Optional) Limit: The maximum number of items to return. Default: 20. Minimum: 1. Maximum: 50. - Offset: The index of the first item to return. Default: 0 |

#### Returns

Paging List of Album Object

*Spotify.NetStandard.Client.Exceptions.AuthAccessTokenRequiredException:* 

### GetArtistAlbumsAsync(id, includeGroup, market, page)

Get an Artist's Albums

| Name | Description |
| ---- | ----------- |
| id | *System.String*<br>(Required) The Spotify ID for the artist. |
| includeGroup | *Spotify.NetStandard.Requests.IncludeGroup*<br>(Optional) Filters the response. If not supplied, all album types will be returned |
| market | *System.String*<br>(Optional) An ISO 3166-1 alpha-2 country code or the string from_token |
| page | *Spotify.NetStandard.Requests.Page*<br>(Optional) Limit: The number of album objects to return. Default: 20. Minimum: 1. Maximum: 50. - Offset: The index of the first album to return. Default: 0 |

#### Returns

Paging List of Album Object

*Spotify.NetStandard.Client.Exceptions.AuthAccessTokenRequiredException:* 

### GetArtistAsync(id)

Get an Artist

| Name | Description |
| ---- | ----------- |
| id | *System.String*<br>(Required) The Spotify ID of the artist. |

#### Returns

Artist Object

*Spotify.NetStandard.Client.Exceptions.AuthAccessTokenRequiredException:* 

### GetArtistRelatedArtistsAsync(id)

Get an Artist's Related Artists

| Name | Description |
| ---- | ----------- |
| id | *System.String*<br>(Requird) The Spotify ID for the artist. |

#### Returns

List of Artist Object

*Spotify.NetStandard.Client.Exceptions.AuthAccessTokenRequiredException:* 

### GetArtistTopTracksAsync(id, market)

Get an Artist's Top Tracks

| Name | Description |
| ---- | ----------- |
| id | *System.String*<br>(Required) The Spotify ID for the artist. |
| market | *System.String*<br>(Required) An ISO 3166-1 alpha-2 country code or the string from_token |

#### Returns

List of Track Object

*Spotify.NetStandard.Client.Exceptions.AuthAccessTokenRequiredException:* 

### GetCategoryAsync(categoryId, country, locale)

Get a Category

| Name | Description |
| ---- | ----------- |
| categoryId | *System.String*<br>(Required) The Spotify category ID for the category. |
| country | *System.String*<br>(Optional) A country: an ISO 3166-1 alpha-2 country code. |
| locale | *System.String*<br>(Optional) The desired language, consisting of an ISO 639-1 language code and an ISO 3166-1 alpha-2 country code, joined by an underscore. |

#### Returns

Category Object

*Spotify.NetStandard.Client.Exceptions.AuthAccessTokenRequiredException:* 

### GetCategoryPlaylistsAsync(categoryId, country, page)

Get a Category's Playlists

| Name | Description |
| ---- | ----------- |
| categoryId | *System.String*<br>(Required) The Spotify category ID for the category. |
| country | *System.String*<br>(Optional) A country: an ISO 3166-1 alpha-2 country code. |
| page | *Spotify.NetStandard.Requests.Page*<br>(Optional) Limit: The maximum number of items to return. Default: 20. Minimum: 1. Maximum: 50. - Offset: The index of the first item to return. Default: 0 |

#### Returns

Paging List of Playlist Object

*Spotify.NetStandard.Client.Exceptions.AuthAccessTokenRequiredException:* 

### GetFollowingStateForArtistsOrUsersAsync(ids, followType)

Get Following State for Artists/Users 
Scopes: FollowRead


| Name | Description |
| ---- | ----------- |
| ids | *System.Collections.Generic.List{System.String}*<br>(Required) List of the artist or the user Spotify IDs to check. |
| followType | *Spotify.NetStandard.Enums.FollowType*<br>(Required) Either artist or user. |

#### Returns

List of true or false values

### GetMultipleAlbumsAsync(ids, market)

Get Multiple Albums

| Name | Description |
| ---- | ----------- |
| ids | *System.Collections.Generic.List{System.String}*<br>(Required) List of the Spotify IDs for the albums. Maximum: 20 IDs. |
| market | *System.String*<br>(Optional) An ISO 3166-1 alpha-2 country code or the string from_token. Provide this parameter if you want to apply Track Relinking. |

#### Returns

List of Album Object

*Spotify.NetStandard.Client.Exceptions.AuthAccessTokenRequiredException:* 

### GetMultipleArtistsAsync(ids)

Get Multiple Artists

| Name | Description |
| ---- | ----------- |
| ids | *System.Collections.Generic.List{System.String}*<br>(Required) List of the Spotify IDs for the artists. Maximum: 50 IDs. |

#### Returns

List of Artist Object

*Spotify.NetStandard.Client.Exceptions.AuthAccessTokenRequiredException:* 

### GetPlaylistAsync(playlistId)

Get a Playlist

| Name | Description |
| ---- | ----------- |
| playlistId | *System.String*<br>(Required) The Spotify ID for the playlist. |

#### Returns

Playlist Object

*Spotify.NetStandard.Client.Exceptions.AuthAccessTokenRequiredException:* 

### GetPlaylistCoverImageAsync(playlistId)

Get a Playlist Cover Image

| Name | Description |
| ---- | ----------- |
| playlistId | *System.String*<br>(Required) The Spotify ID for the playlist. |

#### Returns

List of Image Object

*Spotify.NetStandard.Client.Exceptions.AuthUserTokenRequiredException:* 

### GetPlaylistTracksAsync(id, market, page)

Get a Playlist's Tracks

| Name | Description |
| ---- | ----------- |
| id | *System.String*<br>(Required) The Spotify ID for the playlist. |
| market | *System.String*<br>(Optional) An ISO 3166-1 alpha-2 country code or the string from_token |
| page | *Spotify.NetStandard.Requests.Page*<br>(Optional) Limit: The maximum number of items to return. Default: 100. Minimum: 1. Maximum: 100. - Offset: The index of the first item to return. Default: 0 |

#### Returns

Paging List of Track Object

*Spotify.NetStandard.Client.Exceptions.AuthAccessTokenRequiredException:* 

### GetRecommendationGenresAsync

Get Recommendation Genres

#### Returns

Available Genre Seeds Object

*Spotify.NetStandard.Client.Exceptions.AuthAccessTokenRequiredException:* 

### GetRecommendationsAsync(seedArtists, seedGenres, seedTracks, limit, market, minTuneableTrack, maxTuneableTrack, targetTuneableTrack)

Get Recommendations

| Name | Description |
| ---- | ----------- |
| seedArtists | *System.Collections.Generic.List{System.String}*<br>(Required) List of Spotify IDs for seed artists. Up to 5 seed values may be provided in any combination of seedArtists, seedTracks and seedGenres. |
| seedGenres | *System.Collections.Generic.List{System.String}*<br>(Required) List of any genres in the set of available genre seeds. Up to 5 seed values may be provided in any combination of seedArtists, seedTracks and seedGenres. |
| seedTracks | *System.Collections.Generic.List{System.String}*<br>(Required) List of Spotify IDs for a seed track. Up to 5 seed values may be provided in any combination of seedArtists, seedTracks and seedGenres. |
| limit | *System.Nullable{System.Int32}*<br>(Optional) The target size of the list of recommended tracks. Default: 20. Minimum: 1. Maximum: 100. |
| market | *System.String*<br>(Optional) An ISO 3166-1 alpha-2 country code or the string from_token |
| minTuneableTrack | *Spotify.NetStandard.Requests.TuneableTrack*<br>(Optional) Multiple values. For each tunable track attribute, a hard floor on the selected track attribute’s value can be provided |
| maxTuneableTrack | *Spotify.NetStandard.Requests.TuneableTrack*<br>(Optional) Multiple values. For each tunable track attribute, a hard ceiling on the selected track attribute’s value can be provided. |
| targetTuneableTrack | *Spotify.NetStandard.Requests.TuneableTrack*<br>(Optional) Multiple values. For each of the tunable track attributes (below) a target value may be provided. |

#### Returns

Recommendation Response Object

*Spotify.NetStandard.Client.Exceptions.AuthAccessTokenRequiredException:* 

### GetSeveralTracksAsync(ids, market)

Get Several Tracks

| Name | Description |
| ---- | ----------- |
| ids | *System.Collections.Generic.List{System.String}*<br>(Required) List of the Spotify IDs for the tracks. Maximum: 50 IDs. |
| market | *System.String*<br>(Optional) An ISO 3166-1 alpha-2 country code or the string from_token. Provide this parameter if you want to apply Track Relinking. |

#### Returns

List of Track Object

*Spotify.NetStandard.Client.Exceptions.AuthAccessTokenRequiredException:* 

### GetSeveralTracksAudioFeaturesAsync(ids)

Get Audio Features for Several Tracks

| Name | Description |
| ---- | ----------- |
| ids | *System.Collections.Generic.List{System.String}*<br>(Required) List of the Spotify IDs for the tracks. Maximum: 100 IDs. |

#### Returns

List of Audio Features Object

*Spotify.NetStandard.Client.Exceptions.AuthAccessTokenRequiredException:* 

### GetTrackAsync(id, market)

Get a Track

| Name | Description |
| ---- | ----------- |
| id | *System.String*<br>(Required) The Spotify ID for the track. |
| market | *System.String*<br>(Optional) An ISO 3166-1 alpha-2 country code or the string from_token. Provide this parameter if you want to apply Track Relinking. |

#### Returns

Track Object

*Spotify.NetStandard.Client.Exceptions.AuthAccessTokenRequiredException:* 

### GetTrackAudioAnalysisAsync(id)

Get Audio Analysis for a Track

| Name | Description |
| ---- | ----------- |
| id | *System.String*<br>(Required) The Spotify ID for the track |

#### Returns

Audio Analysis Object

*Spotify.NetStandard.Client.Exceptions.AuthAccessTokenRequiredException:* 

### GetTrackAudioFeaturesAsync(id)

Get Audio Features for a Track

| Name | Description |
| ---- | ----------- |
| id | *System.String*<br>(Required) The Spotify ID for the track |

#### Returns

Audio Features Object

*Spotify.NetStandard.Client.Exceptions.AuthAccessTokenRequiredException:* 

### GetUserPlaybackCurrentAsync(market)

Get Information About The User's Current Playback 
Scopes: ConnectReadPlaybackState


| Name | Description |
| ---- | ----------- |
| market | *System.String*<br>(Optional) An ISO 3166-1 alpha-2 country code or the string from_token. Provide this parameter if you want to apply Track Relinking. |

#### Returns

Currently Playing Object

*Spotify.NetStandard.Client.Exceptions.AuthUserTokenRequiredException:* 

### GetUserPlaybackCurrentTrackAsync(market)

Get the User's Currently Playing Track 
Scopes: ConnectReadCurrentlyPlaying, ConnectReadPlaybackState


| Name | Description |
| ---- | ----------- |
| market | *System.String*<br>(Optional) An ISO 3166-1 alpha-2 country code or the string from_token. Provide this parameter if you want to apply Track Relinking. |

#### Returns

Simplified Currently Playing Object

*Spotify.NetStandard.Client.Exceptions.AuthUserTokenRequiredException:* 

### GetUserPlaybackDevicesAsync

Get a User's Available Devices 
Scopes: ConnectReadPlaybackState


#### Returns

Devices Object

*Spotify.NetStandard.Client.Exceptions.AuthUserTokenRequiredException:* 

### GetUserPlaylistsAsync(cursor)

Get a List of Current User's Playlists 
Scopes: PlaylistReadPrivate, PlaylistReadCollaborative


| Name | Description |
| ---- | ----------- |
| cursor | *Spotify.NetStandard.Requests.Cursor*<br>(Optional) Limit: The maximum number of playlists to return. Default: 20. Minimum: 1. Maximum: 50. - The index of the first playlist to return. Default: 0 (the first object). Maximum offset: 100. Use with limit to get the next set of playlists. |

#### Returns

CursorPaging of Playlist Object

*Spotify.NetStandard.Client.Exceptions.AuthUserTokenRequiredException:* 

### GetUserPlaylistsAsync(userId, cursor)

Get a List of a User's Playlists 
Scopes: PlaylistReadPrivate, PlaylistReadCollaborative


| Name | Description |
| ---- | ----------- |
| userId | *System.String*<br>(Required) The user’s Spotify user ID. |
| cursor | *Spotify.NetStandard.Requests.Cursor*<br>(Optional) Limit: The maximum number of playlists to return. Default: 20. Minimum: 1. Maximum: 50. - Offset: The index of the first playlist to return. Default: 0 (the first object). Maximum offset: 100 |

#### Returns

CursorPaging of Playlist Object

*Spotify.NetStandard.Client.Exceptions.AuthUserTokenRequiredException:* 

### GetUserProfileAsync

Get Current User's Profile 
Scopes: UserReadEmail, UserReadBirthDate, UserReadPrivate


#### Returns

Private User Object

*Spotify.NetStandard.Client.Exceptions.AuthUserTokenRequiredException:* 

### GetUserProfileAsync(userId)

Get a User's Profile

| Name | Description |
| ---- | ----------- |
| userId | *System.String*<br>The user’s Spotify user ID. |

#### Returns

Public User Object

*Spotify.NetStandard.Client.Exceptions.AuthUserTokenRequiredException:* 

### GetUserRecentlyPlayedTracksAsync(cursor)

Get Current User's Recently Played Tracks 
Scopes: ListeningRecentlyPlayed


| Name | Description |
| ---- | ----------- |
| cursor | *Spotify.NetStandard.Requests.Cursor*<br>(Optional) Limit: The maximum number of items to return. Default: 20. Minimum: 1. Maximum: 50. - After: A Unix timestamp in milliseconds. Returns all items after (but not including) this cursor position. If after is specified, before must not be specified. Before - (Optional) A Unix timestamp in milliseconds. Returns all items before (but not including) this cursor position. If before is specified, after must not be specified. |

#### Returns

Cursor Paging of Play History Object

*Spotify.NetStandard.Client.Exceptions.AuthUserTokenRequiredException:* 

### GetUserSavedAlbumsAsync(market, cursor)

Get User's Saved Albums 
Scopes: LibraryRead


| Name | Description |
| ---- | ----------- |
| market | *System.String*<br>(Optional) An ISO 3166-1 alpha-2 country code or the string from_token. Provide this parameter if you want to apply Track Relinking. |
| cursor | *Spotify.NetStandard.Requests.Cursor*<br>(Optional) Limit: The maximum number of objects to return. Default: 20. Minimum: 1. Maximum: 50. - Offset: The index of the first object to return. Default: 0 (i.e., the first object). Use with limit to get the next set of objects. |

#### Returns

Cursor Paging of Saved Album Object

*Spotify.NetStandard.Client.Exceptions.AuthUserTokenRequiredException:* 

### GetUserSavedTracksAsync(market, cursor)

Get User's Saved Tracks 
Scopes: LibraryRead


| Name | Description |
| ---- | ----------- |
| market | *System.String*<br>(Optional) An ISO 3166-1 alpha-2 country code or the string from_token. Provide this parameter if you want to apply Track Relinking. |
| cursor | *Spotify.NetStandard.Requests.Cursor*<br>(Optional) Limit: The maximum number of objects to return. Default: 20. Minimum: 1. Maximum: 50. - Offset: The index of the first object to return. Default: 0 (i.e., the first object). Use with limit to get the next set of objects. |

#### Returns

Cursor Paging of Saved Track Object

*Spotify.NetStandard.Client.Exceptions.AuthUserTokenRequiredException:* 

### GetUsersFollowedArtistsAsync(cursor)

Get User's Followed Artists 
Scopes: FollowRead


| Name | Description |
| ---- | ----------- |
| cursor | *Spotify.NetStandard.Requests.Cursor*<br>(Optional) Limit: The maximum number of items to return. Default: 20. Minimum: 1. Maximum: 50. - After: The last artist ID retrieved from the previous request. |

#### Returns

CursorPaging of Artist Object

*Spotify.NetStandard.Client.Exceptions.AuthUserTokenRequiredException:* 

### GetUserTopArtistsAsync(timeRange, cursor)

Get a User's Top Artists 
Scopes: ListeningTopRead


| Name | Description |
| ---- | ----------- |
| timeRange | *System.Nullable{Spotify.NetStandard.Enums.TimeRange}*<br>(Optional) Over what time frame the affinities are computed. Long Term: alculated from several years of data and including all new data as it becomes available, Medium Term: (Default) approximately last 6 months, Short Term: approximately last 4 weeks |
| cursor | *Spotify.NetStandard.Requests.Cursor*<br>(Optional) Limit: The number of entities to return. Default: 20. Minimum: 1. Maximum: 50. For example - Offset: he index of the first entity to return. Default: 0. Use with limit to get the next set of entities. |

#### Returns

Cursor Paging of Artist Object

*Spotify.NetStandard.Client.Exceptions.AuthUserTokenRequiredException:* 

### GetUserTopTracksAsync(timeRange, cursor)

Get a User's Top Tracks 
Scopes: ListeningTopRead


| Name | Description |
| ---- | ----------- |
| timeRange | *System.Nullable{Spotify.NetStandard.Enums.TimeRange}*<br>(Optional) Over what time frame the affinities are computed. Long Term: alculated from several years of data and including all new data as it becomes available, Medium Term: (Default) approximately last 6 months, Short Term: approximately last 4 weeks |
| cursor | *Spotify.NetStandard.Requests.Cursor*<br>(Optional) Limit: The number of entities to return. Default: 20. Minimum: 1. Maximum: 50. For example - Offset: he index of the first entity to return. Default: 0. Use with limit to get the next set of entities. |

#### Returns

Cursor Paging of Track Object

*Spotify.NetStandard.Client.Exceptions.AuthUserTokenRequiredException:* 

### RemoveTracksFromPlaylistAsync(playlistId, tracks, snapshotId)

Remove Tracks from a Playlist 
Scopes: PlaylistModifyPublic, PlaylistModifyPrivate


| Name | Description |
| ---- | ----------- |
| playlistId | *System.String*<br>(Required) The Spotify ID for the playlist. |
| tracks | *System.Collections.Generic.List{System.String}*<br>(Required) List of Spotify URIs of the tracks to remove |
| snapshotId | *System.String*<br>(Optional) The playlist’s snapshot ID against which you want to make the changes. The API will validate that the specified tracks exist and in the specified positions and make the changes, even if more recent changes have been made to the playlist. |

#### Returns

Snapshot Object

*Spotify.NetStandard.Client.Exceptions.AuthUserTokenRequiredException:* 

### RemoveUserAlbumsAsync(itemIds)

Remove Albums for Current User 
Scopes: LibraryModify


| Name | Description |
| ---- | ----------- |
| itemIds | *System.Collections.Generic.List{System.String}*<br>(Required) List of the Spotify IDs for the albums |

#### Returns

Status Object

*Spotify.NetStandard.Client.Exceptions.AuthUserTokenRequiredException:* 

### RemoveUserTracksAsync(itemIds)

Remove User's Saved Tracks 
Scopes: LibraryModify


| Name | Description |
| ---- | ----------- |
| itemIds | *System.Collections.Generic.List{System.String}*<br>(Required) List of the Spotify IDs for the tracks |

#### Returns

Status Object

*Spotify.NetStandard.Client.Exceptions.AuthUserTokenRequiredException:* 

### ReorderPlaylistTracksAsync(playlistId, rangeStart, insertBefore, rangeLength, snapshotId)

Reorder a Playlist's Tracks 
Scopes: PlaylistModifyPublic, PlaylistModifyPrivate


| Name | Description |
| ---- | ----------- |
| playlistId | *System.String*<br>The Spotify ID for the playlist. |
| rangeStart | *System.Int32*<br>(Required) The position of the first track to be reordered. |
| insertBefore | *System.Int32*<br>(Required) The position where the tracks should be inserted. To reorder the tracks to the end of the playlist, simply set insert_before to the position after the last track. |
| rangeLength | *System.Nullable{System.Int32}*<br>(Optional) The amount of tracks to be reordered. Defaults to 1 if not set. The range of tracks to be reordered begins from the range_start position, and includes the range_length subsequent tracks. |
| snapshotId | *System.String*<br>(Optional) The playlist’s snapshot ID against which you want to make the changes. |

#### Returns

Snapshot Object

*Spotify.NetStandard.Client.Exceptions.AuthUserTokenRequiredException:* 

### ReplacePlaylistTracksAsync(playlistId, uris)

Replace a Playlist's Tracks 
Scopes: PlaylistModifyPublic, PlaylistModifyPrivate


| Name | Description |
| ---- | ----------- |
| playlistId | *System.String*<br>(Required) The Spotify ID for the playlist. |
| uris | *System.Collections.Generic.List{System.String}*<br>(Optional) List of Spotify track URIs. |

*Spotify.NetStandard.Client.Exceptions.AuthUserTokenRequiredException:* 

### SaveUserAlbumsAsync(itemIds)

Save Albums for Current User 
Scopes: LibraryModify


| Name | Description |
| ---- | ----------- |
| itemIds | *System.Collections.Generic.List{System.String}*<br>(Required) List of the Spotify IDs for the albums |

#### Returns

Status Object

*Spotify.NetStandard.Client.Exceptions.AuthUserTokenRequiredException:* 

### SaveUserTracksAsync(itemIds)

Save Tracks for User 
Scopes: LibraryModify


| Name | Description |
| ---- | ----------- |
| itemIds | *System.Collections.Generic.List{System.String}*<br>(Required) List of the Spotify IDs for the tracks |

#### Returns

Status Object

*Spotify.NetStandard.Client.Exceptions.AuthUserTokenRequiredException:* 

### SearchForItemAsync(query, searchType, market, external, page)

Search for an Item

| Name | Description |
| ---- | ----------- |
| query | *System.String*<br>(Required) Search query keywords and optional field filters and operators. |
| searchType | *Spotify.NetStandard.Requests.SearchType*<br>(Required) List of item types to search across. |
| market | *System.String*<br>(Optional) An ISO 3166-1 alpha-2 country code or the string from_token. |
| external | *System.Nullable{System.Boolean}*<br>(Optional) Include any relevant audio content that is hosted externally. |
| page | *Spotify.NetStandard.Requests.Page*<br>(Optional) Limit: Maximum number of results to return. Default: 20 Minimum: 1 Maximum: 50 - Offset: The index of the first track to return |

#### Returns

Content Response

*Spotify.NetStandard.Client.Exceptions.AuthAccessTokenRequiredException:* 

### UnfollowArtistsOrUsersAsync(ids, followType)

Unfollow Artists or Users 
Scopes: FollowModify


| Name | Description |
| ---- | ----------- |
| ids | *System.Collections.Generic.List{System.String}*<br>(Required) List of the artist or the user Spotify IDs. |
| followType | *Spotify.NetStandard.Enums.FollowType*<br>(Required) Either artist or user |

#### Returns

Status Object

*Spotify.NetStandard.Client.Exceptions.AuthUserTokenRequiredException:* 

### UnfollowPlaylistAsync(playlistId)

Unfollow Playlist 
Scopes: PlaylistModifyPublic, PlaylistModifyPrivate


| Name | Description |
| ---- | ----------- |
| playlistId | *System.String*<br>(Required) The Spotify ID of the playlist that is to be no longer followed. |

#### Returns

Status Object

*Spotify.NetStandard.Client.Exceptions.AuthUserTokenRequiredException:* 

### UploadCustomPlaylistCoverImageAsync(playlistId, file)

Upload a Custom Playlist Cover Image 
Scopes: UserGeneratedContentImageUpload, PlaylistModifyPublic, PlaylistModifyPrivate


| Name | Description |
| ---- | ----------- |
| playlistId | *System.String*<br>(Required) The Spotify ID for the playlist. |
| file | *System.Byte[]*<br>(Required) JPEG Image File Bytes |

#### Returns

Status Object

*Spotify.NetStandard.Client.Exceptions.AuthUserTokenRequiredException:* 

### UserPlaybackNextTrackAsync(deviceId)

Skip User’s Playback To Next Track 
Scopes: ConnectModifyPlaybackState


| Name | Description |
| ---- | ----------- |
| deviceId | *System.String*<br>(Optional) The id of the device this command is targeting. If not supplied, the user’s currently active device is the target. |

#### Returns

Status Object

*Spotify.NetStandard.Client.Exceptions.AuthUserTokenRequiredException:* 

### UserPlaybackPauseAsync(deviceId)

Pause a User's Playback 
Scopes: ConnectModifyPlaybackState


| Name | Description |
| ---- | ----------- |
| deviceId | *System.String*<br>(Optional) The id of the device this command is targeting. If not supplied, the user’s currently active device is the target. |

#### Returns

Status Object

*Spotify.NetStandard.Client.Exceptions.AuthUserTokenRequiredException:* 

### UserPlaybackPreviousTrackAsync(deviceId)

Skip User’s Playback To Previous Track 
Scopes: ConnectModifyPlaybackState


| Name | Description |
| ---- | ----------- |
| deviceId | *System.String*<br>(Optional) The id of the device this command is targeting. If not supplied, the user’s currently active device is the target. |

#### Returns

Status Object

*Spotify.NetStandard.Client.Exceptions.AuthUserTokenRequiredException:* 

### UserPlaybackSeekTrackAsync(position, deviceId)

Seek To Position In Currently Playing Track 
Scopes: ConnectModifyPlaybackState


| Name | Description |
| ---- | ----------- |
| position | *System.Int32*<br>(Required) The position in milliseconds to seek to. Must be a positive number. Passing in a position that is greater than the length of the track will cause the player to start playing the next song. |
| deviceId | *System.String*<br>(Optional) The id of the device this command is targeting. If not supplied, the user’s currently active device is the target. |

#### Returns

Status Object

*Spotify.NetStandard.Client.Exceptions.AuthUserTokenRequiredException:* 

### UserPlaybackSetRepeatModeAsync(state, deviceId)

Set Repeat Mode On User’s Playback 
Scopes: ConnectModifyPlaybackState


| Name | Description |
| ---- | ----------- |
| state | *Spotify.NetStandard.Enums.RepeatState*<br>(Required) track, context or off. track will repeat the current track. context will repeat the current context. off will turn repeat off. |
| deviceId | *System.String*<br>(Optional) The id of the device this command is targeting. If not supplied, the user’s currently active device is the target. |

#### Returns

Status Object

*Spotify.NetStandard.Client.Exceptions.AuthUserTokenRequiredException:* 

### UserPlaybackSetVolumeAsync(percent, deviceId)

Set Volume For User's Playback 
Scopes: ConnectModifyPlaybackState


| Name | Description |
| ---- | ----------- |
| percent | *System.Int32*<br>(Required) The volume to set. Must be a value from 0 to 100 inclusive. |
| deviceId | *System.String*<br>(Optional) The id of the device this command is targeting. If not supplied, the user’s currently active device is the target. |

#### Returns

Status Object

*Spotify.NetStandard.Client.Exceptions.AuthUserTokenRequiredException:* 

### UserPlaybackStartResumeAsync(contextUri, uris, offsetUri, offsetPosition, position, deviceId)

Start/Resume a User's Playback 
Scopes: ConnectModifyPlaybackState


| Name | Description |
| ---- | ----------- |
| contextUri | *System.String*<br>(Optional) Spotify URI of the context to play. Valid contexts are albums, artists, playlists. Example: "spotify:album:1Je1IMUlBXcx1Fz0WE7oPT" |
| uris | *System.Collections.Generic.List{System.String}*<br>(Optional) List of the Spotify track URIs to play. For example: ["spotify:track:4iV5W9uYEdYUVa79Axb7Rh", "spotify:track:1301WleyT98MSxVHPZCA6M"]} |
| offsetUri | *System.Nullable{System.Int32}*<br>(Optional) Use either offsetUri or offsetPosition, Indicates from where in the context playback should start. Only available when context_uri corresponds to an album or playlist object, or when the uris parameter is used. “position” is zero based and can’t be negative. Example: 5. |
| offsetPosition | *System.String*<br>(Optional) Use either offsetPosition or offsetUri, Indicates from where in the context playback should start. Only available when context_uri corresponds to an album or playlist object, or when the uris parameter is used. “uri” is a string representing the uri of the item to start at. Example: "spotify:track:1301WleyT98MSxVHPZCA6M" |
| position | *System.Nullable{System.Int32}*<br>(Optional) Indicates from what position to start playback.Must be a positive number.Passing in a position that is greater than the length of the track will cause the player to start playing the next song. |
| deviceId | *System.String*<br>(Optional) The id of the device this command is targeting. If not supplied, the user’s currently active device is the target. |

#### Returns

Status Object

*Spotify.NetStandard.Client.Exceptions.AuthUserTokenRequiredException:* 

### UserPlaybackToggleShuffleAsync(state, deviceId)

Toggle Shuffle For User’s Playback 
Scopes: ConnectModifyPlaybackState


| Name | Description |
| ---- | ----------- |
| state | *System.Boolean*<br>(Required) true : Shuffle user’s playback, false : Do not shuffle user’s playback |
| deviceId | *System.String*<br>(Optional) The id of the device this command is targeting. If not supplied, the user’s currently active device is the target. |

#### Returns

Status Object

*Spotify.NetStandard.Client.Exceptions.AuthUserTokenRequiredException:* 

### UserPlaybackTransferAsync(deviceIds, play)

Transfer a User's Playback 
Scopes: ConnectModifyPlaybackState


| Name | Description |
| ---- | ----------- |
| deviceIds | *System.Collections.Generic.List{System.String}*<br>(Required) List containing the ID of the device on which playback should be started/transferred. Although an array is accepted, only a single device_id is currently supported. |
| play | *System.Nullable{System.Boolean}*<br>(Optional) true: ensure playback happens on new device. false or not provided: keep the current playback state. |

#### Returns

Status Object

*Spotify.NetStandard.Client.Exceptions.AuthUserTokenRequiredException:* 


## ISpotifyClient

Spotify Client

### Api

Spotify API

### AuthAddTracksToPlaylistAsync(playlistId, uris, position)

Add Tracks to a Playlist 
Scopes: PlaylistModifyPublic, PlaylistModifyPrivate


| Name | Description |
| ---- | ----------- |
| playlistId | *System.String*<br>(Required) The Spotify ID for the playlist. |
| uris | *Spotify.NetStandard.Requests.UriListRequest*<br>(Optional) List of Spotify track URIs to add. |
| position | *System.Nullable{System.Int32}*<br>(Optional) The position to insert the tracks, a zero-based index. |

#### Returns

Snapshot Object

*Spotify.NetStandard.Client.Exceptions.AuthUserTokenRequiredException:* 

### AuthChangePlaylistDetailsAsync(playlistId, request)

Change a Playlist's Details 
Scopes: PlaylistModifyPublic, PlaylistModifyPrivate


| Name | Description |
| ---- | ----------- |
| playlistId | *System.String*<br>(Required) The Spotify ID for the playlist. |
| request | *Spotify.NetStandard.Requests.PlaylistRequest*<br>(Optional) Playlist Request Object |

#### Returns

Status Object

*Spotify.NetStandard.Client.Exceptions.AuthUserTokenRequiredException:* 

### AuthCreatePlaylistAsync(userId, request)

Create a Playlist 
Scopes: PlaylistModifyPublic, PlaylistModifyPrivate


| Name | Description |
| ---- | ----------- |
| userId | *System.String*<br>(Required) The user’s Spotify user ID. |
| request | *Spotify.NetStandard.Requests.PlaylistRequest*<br>(Required) Playlist Request |

#### Returns

Playlist Object

*Spotify.NetStandard.Client.Exceptions.AuthUserTokenRequiredException:* 

### AuthFollowAsync(itemIds, followType)

Follow Artists or Users 
Scopes: FollowModify


| Name | Description |
| ---- | ----------- |
| itemIds | *System.Collections.Generic.List{System.String}*<br>(Required) List of the artist or the user Spotify IDs. |
| followType | *Spotify.NetStandard.Enums.FollowType*<br>(Required) Either artist or user |

#### Returns

Status Object

*Spotify.NetStandard.Client.Exceptions.AuthUserTokenRequiredException:* 

### AuthFollowPlaylistAsync(playlistId, isPublic)

Follow a Playlist 
Scopes: FollowModify


| Name | Description |
| ---- | ----------- |
| playlistId | *System.String*<br>(Required) The Spotify ID of the playlist. Any playlist can be followed, regardless of its public/private status, as long as you know its playlist ID. |
| isPublic | *System.Boolean*<br>(Optional) Defaults to true. If true the playlist will be included in user’s public playlists, if false it will remain private. To be able to follow playlists privately, the user must have granted the playlist-modify-private scope. |

#### Returns

Status Object

*Spotify.NetStandard.Client.Exceptions.AuthUserTokenRequiredException:* 

### AuthGetPlaylistCoverImageAsync(playlistId)

Get a Playlist Cover Image

| Name | Description |
| ---- | ----------- |
| playlistId | *System.String*<br>(Required) The Spotify ID for the playlist. |

#### Returns

Image Object

*Spotify.NetStandard.Client.Exceptions.AuthUserTokenRequiredException:* 

### AuthLookupCheckUserSavedAlbumsAsync(itemIds)

Check User's Saved Albums 
Scopes: LibraryRead


| Name | Description |
| ---- | ----------- |
| itemIds | *System.Collections.Generic.List{System.String}*<br>(Required) List of the Spotify IDs for the albums |

#### Returns

List of true or false values

*Spotify.NetStandard.Client.Exceptions.AuthUserTokenRequiredException:* 

### AuthLookupCheckUserSavedTracksAsync(itemIds)

Check User's Saved Tracks 
Scopes: LibraryRead


| Name | Description |
| ---- | ----------- |
| itemIds | *System.Collections.Generic.List{System.String}*<br>(Required) List of the Spotify IDs for the tracks |

#### Returns

List of true or false values

*Spotify.NetStandard.Client.Exceptions.AuthUserTokenRequiredException:* 

### AuthLookupFollowedArtistsAsync(cursor)

Get User's Followed Artists 
Scopes: FollowRead


| Name | Description |
| ---- | ----------- |
| cursor | *Spotify.NetStandard.Requests.Cursor*<br>(Optional) Limit: The maximum number of items to return. Default: 20. Minimum: 1. Maximum: 50. - After: The last artist ID retrieved from the previous request. |

#### Returns

CursorPaging of Artist Object

*Spotify.NetStandard.Client.Exceptions.AuthUserTokenRequiredException:* 

### AuthLookupFollowingStateAsync(itemIds, followType)

Get Following State for Artists/Users 
Scopes: FollowRead


| Name | Description |
| ---- | ----------- |
| itemIds | *System.Collections.Generic.List{System.String}*<br>(Required) List of the artist or the user Spotify IDs to check. |
| followType | *Spotify.NetStandard.Enums.FollowType*<br>(Required) Either artist or user. |

#### Returns

List of true or false values

*Spotify.NetStandard.Client.Exceptions.AuthUserTokenRequiredException:* 

### AuthLookupUserFollowingPlaylistAsync(itemIds, playlistId)

Check if Users Follow a Playlist 
Scopes: PlaylistReadPrivate


| Name | Description |
| ---- | ----------- |
| itemIds | *System.Collections.Generic.List{System.String}*<br>(Required) List of Spotify User IDs, the ids of the users that you want to check to see if they follow the playlist. Maximum: 5 ids. |
| playlistId | *System.String*<br>(Required) The Spotify ID of the playlist. |

#### Returns

List of true or false values

*Spotify.NetStandard.Client.Exceptions.AuthUserTokenRequiredException:* 

### AuthLookupUserPlaybackCurrentAsync(market)

Get Information About The User's Current Playback 
Scopes: ConnectReadPlaybackState


| Name | Description |
| ---- | ----------- |
| market | *System.String*<br>(Optional) An ISO 3166-1 alpha-2 country code or the string from_token. Provide this parameter if you want to apply Track Relinking. |

#### Returns

Currently Playing Object

*Spotify.NetStandard.Client.Exceptions.AuthUserTokenRequiredException:* 

### AuthLookupUserPlaybackCurrentTrackAsync(market)

Get the User's Currently Playing Track 
Scopes: ConnectReadCurrentlyPlaying, ConnectReadPlaybackState


| Name | Description |
| ---- | ----------- |
| market | *System.String*<br>(Optional) An ISO 3166-1 alpha-2 country code or the string from_token. Provide this parameter if you want to apply Track Relinking. |

#### Returns

Simplified Currently Playing Object

*Spotify.NetStandard.Client.Exceptions.AuthUserTokenRequiredException:* 

### AuthLookupUserPlaybackDevicesAsync

Get a User's Available Devices 
Scopes: ConnectReadPlaybackState


#### Returns

Devices Object

*Spotify.NetStandard.Client.Exceptions.AuthUserTokenRequiredException:* 

### AuthLookupUserPlaylistsAsync(cursor)

Get a List of Current User's Playlists 
Scopes: PlaylistReadPrivate, PlaylistReadCollaborative


| Name | Description |
| ---- | ----------- |
| cursor | *Spotify.NetStandard.Requests.Cursor*<br>(Optional) Limit: The maximum number of playlists to return. Default: 20. Minimum: 1. Maximum: 50. - The index of the first playlist to return. Default: 0 (the first object). Maximum offset: 100. Use with limit to get the next set of playlists. |

#### Returns

CursorPaging of Playlist Object

*Spotify.NetStandard.Client.Exceptions.AuthUserTokenRequiredException:* 

### AuthLookupUserPlaylistsAsync(userId, cursor)

Get a List of a User's Playlists 
Scopes: PlaylistReadPrivate, PlaylistReadCollaborative


| Name | Description |
| ---- | ----------- |
| userId | *System.String*<br>(Required) The user’s Spotify user ID. |
| cursor | *Spotify.NetStandard.Requests.Cursor*<br>(Optional) Limit: The maximum number of playlists to return. Default: 20. Minimum: 1. Maximum: 50. - Offset: The index of the first playlist to return. Default: 0 (the first object). Maximum offset: 100 |

#### Returns

CursorPaging of Playlist Object

*Spotify.NetStandard.Client.Exceptions.AuthUserTokenRequiredException:* 

### AuthLookupUserProfileAsync

Get Current User's Profile 
Scopes: UserReadEmail, UserReadBirthDate, UserReadPrivate


#### Returns

Private User Object

*Spotify.NetStandard.Client.Exceptions.AuthUserTokenRequiredException:* 

### AuthLookupUserProfileAsync(userId)

Get a User's Profile

| Name | Description |
| ---- | ----------- |
| userId | *System.String*<br>The user’s Spotify user ID. |

#### Returns

Public User Object

*Spotify.NetStandard.Client.Exceptions.AuthUserTokenRequiredException:* 

### AuthLookupUserRecentlyPlayedTracksAsync(cursor)

Get Current User's Recently Played Tracks 
Scopes: ListeningRecentlyPlayed


| Name | Description |
| ---- | ----------- |
| cursor | *Spotify.NetStandard.Requests.Cursor*<br>(Optional) Limit: The maximum number of items to return. Default: 20. Minimum: 1. Maximum: 50. - After: A Unix timestamp in milliseconds. Returns all items after (but not including) this cursor position. If after is specified, before must not be specified. Before - (Optional) A Unix timestamp in milliseconds. Returns all items before (but not including) this cursor position. If before is specified, after must not be specified. |

#### Returns

Cursor Paging of Play History Object

*Spotify.NetStandard.Client.Exceptions.AuthUserTokenRequiredException:* 

### AuthLookupUserSavedAlbumsAsync(market, cursor)

Get User's Saved Albums 
Scopes: LibraryRead


| Name | Description |
| ---- | ----------- |
| market | *System.String*<br>(Optional) An ISO 3166-1 alpha-2 country code or the string from_token. Provide this parameter if you want to apply Track Relinking. |
| cursor | *Spotify.NetStandard.Requests.Cursor*<br>(Optional) Limit: The maximum number of objects to return. Default: 20. Minimum: 1. Maximum: 50. - Offset: The index of the first object to return. Default: 0 (i.e., the first object). Use with limit to get the next set of objects. |

#### Returns

Cursor Paging of Saved Album Object

*Spotify.NetStandard.Client.Exceptions.AuthUserTokenRequiredException:* 

### AuthLookupUserSavedTracksAsync(market, cursor)

Get User's Saved Tracks 
Scopes: LibraryRead


| Name | Description |
| ---- | ----------- |
| market | *System.String*<br>(Optional) An ISO 3166-1 alpha-2 country code or the string from_token. Provide this parameter if you want to apply Track Relinking. |
| cursor | *Spotify.NetStandard.Requests.Cursor*<br>(Optional) Limit: The maximum number of objects to return. Default: 20. Minimum: 1. Maximum: 50. - Offset: The index of the first object to return. Default: 0 (i.e., the first object). Use with limit to get the next set of objects. |

#### Returns

Cursor Paging of Saved Track Object

*Spotify.NetStandard.Client.Exceptions.AuthUserTokenRequiredException:* 

### AuthLookupUserTopArtistsAsync(timeRange, cursor)

Get a User's Top Artists 
Scopes: ListeningTopRead


| Name | Description |
| ---- | ----------- |
| timeRange | *System.Nullable{Spotify.NetStandard.Enums.TimeRange}*<br>(Optional) Over what time frame the affinities are computed. Long Term: alculated from several years of data and including all new data as it becomes available, Medium Term: (Default) approximately last 6 months, Short Term: approximately last 4 weeks |
| cursor | *Spotify.NetStandard.Requests.Cursor*<br>(Optional) Limit: The number of entities to return. Default: 20. Minimum: 1. Maximum: 50. For example - Offset: he index of the first entity to return. Default: 0. Use with limit to get the next set of entities. |

#### Returns

Cursor Paging of Artist Object

*Spotify.NetStandard.Client.Exceptions.AuthUserTokenRequiredException:* 

### AuthLookupUserTopTracksAsync(timeRange, cursor)

Get a User's Top Tracks 
Scopes: ListeningTopRead


| Name | Description |
| ---- | ----------- |
| timeRange | *System.Nullable{Spotify.NetStandard.Enums.TimeRange}*<br>(Optional) Over what time frame the affinities are computed. Long Term: alculated from several years of data and including all new data as it becomes available, Medium Term: (Default) approximately last 6 months, Short Term: approximately last 4 weeks |
| cursor | *Spotify.NetStandard.Requests.Cursor*<br>(Optional) Limit: The number of entities to return. Default: 20. Minimum: 1. Maximum: 50. For example - Offset: he index of the first entity to return. Default: 0. Use with limit to get the next set of entities. |

#### Returns

Cursor Paging of Track Object

*Spotify.NetStandard.Client.Exceptions.AuthUserTokenRequiredException:* 

### AuthRemoveTracksFromPlaylistAsync(playlistId, request)

Remove Tracks from a Playlist 
Scopes: PlaylistModifyPublic, PlaylistModifyPrivate


| Name | Description |
| ---- | ----------- |
| playlistId | *System.String*<br>(Required) The Spotify ID for the playlist. |
| request | *Spotify.NetStandard.Requests.PlaylistTracksRequest*<br>(Optional) Tracks: An array of objects containing Spotify URIs of the tracks to remove. Snapshot ID : The playlist’s snapshot ID against which you want to make the changes. The API will validate that the specified tracks exist and in the specified positions and make the changes, even if more recent changes have been made to the playlist. |

#### Returns

Snapshot Object

*Spotify.NetStandard.Client.Exceptions.AuthUserTokenRequiredException:* 

### AuthRemoveUserAlbumsAsync(itemIds)

Remove Albums for Current User 
Scopes: LibraryModify


| Name | Description |
| ---- | ----------- |
| itemIds | *System.Collections.Generic.List{System.String}*<br>(Required) List of the Spotify IDs for the albums |

#### Returns

Status Object

*Spotify.NetStandard.Client.Exceptions.AuthUserTokenRequiredException:* 

### AuthRemoveUserTracksAsync(itemIds)

Remove User's Saved Tracks 
Scopes: LibraryModify


| Name | Description |
| ---- | ----------- |
| itemIds | *System.Collections.Generic.List{System.String}*<br>(Required) List of the Spotify IDs for the tracks |

#### Returns

Status Object

*Spotify.NetStandard.Client.Exceptions.AuthUserTokenRequiredException:* 

### AuthReorderPlaylistTracksAsync(playlistId, request)

Reorder a Playlist's Tracks 
Scopes: PlaylistModifyPublic, PlaylistModifyPrivate


| Name | Description |
| ---- | ----------- |
| playlistId | *System.String*<br>(Required) The Spotify ID for the playlist. |
| request | *Spotify.NetStandard.Requests.PlaylistReorderRequest*<br>(Required) Playlist Reorder Request |

#### Returns

Snapshot Object

*Spotify.NetStandard.Client.Exceptions.AuthUserTokenRequiredException:* 

### AuthReplacePlaylistTracksAsync(playlistId, uris)

Replace a Playlist's Tracks 
Scopes: PlaylistModifyPublic, PlaylistModifyPrivate


| Name | Description |
| ---- | ----------- |
| playlistId | *System.String*<br>(Required) The Spotify ID for the playlist. |
| uris | *Spotify.NetStandard.Requests.UriListRequest*<br>(Optional) Uri List Request. |

*Spotify.NetStandard.Client.Exceptions.AuthUserTokenRequiredException:* 

### AuthSaveUserAlbumsAsync(itemIds)

Save Albums for Current User 
Scopes: LibraryModify


| Name | Description |
| ---- | ----------- |
| itemIds | *System.Collections.Generic.List{System.String}*<br>(Required) List of the Spotify IDs for the albums |

#### Returns

Status Object

*Spotify.NetStandard.Client.Exceptions.AuthUserTokenRequiredException:* 

### AuthSaveUserTracksAsync(itemIds)

Save Tracks for User 
Scopes: LibraryModify


| Name | Description |
| ---- | ----------- |
| itemIds | *System.Collections.Generic.List{System.String}*<br>(Required) List of the Spotify IDs for the tracks |

#### Returns

Status Object

*Spotify.NetStandard.Client.Exceptions.AuthUserTokenRequiredException:* 

### AuthUnfollowAsync(itemIds, followType)

Unfollow Artists or Users 
Scopes: FollowModify


| Name | Description |
| ---- | ----------- |
| itemIds | *System.Collections.Generic.List{System.String}*<br>(Required) List of the artist or the user Spotify IDs. |
| followType | *Spotify.NetStandard.Enums.FollowType*<br>(Required) Either artist or user |

#### Returns

Status Object

*Spotify.NetStandard.Client.Exceptions.AuthUserTokenRequiredException:* 

### AuthUnfollowPlaylistAsync(playlistId)

Unfollow Playlist 
Scopes: PlaylistModifyPublic, PlaylistModifyPrivate


| Name | Description |
| ---- | ----------- |
| playlistId | *System.String*<br>(Required) The Spotify ID of the playlist that is to be no longer followed. |

#### Returns

Status Object

*Spotify.NetStandard.Client.Exceptions.AuthUserTokenRequiredException:* 

### AuthUploadCustomPlaylistImageAsync(playlistId, file)

Upload a Custom Playlist Cover Image 
Scopes: UserGeneratedContentImageUpload, PlaylistModifyPublic, PlaylistModifyPrivate


| Name | Description |
| ---- | ----------- |
| playlistId | *System.String*<br>(Required) The Spotify ID for the playlist. |
| file | *System.Byte[]*<br>(Required) JPEG Image File Bytes |

#### Returns

Status Object

*Spotify.NetStandard.Client.Exceptions.AuthUserTokenRequiredException:* 

### AuthUser(redirectUri, state, scope, showDialog)

Auth User - Authorisation Code Flow

| Name | Description |
| ---- | ----------- |
| redirectUri | *System.Uri*<br>Redirect Uri |
| state | *System.String*<br>State |
| scope | *Spotify.NetStandard.Requests.Scope*<br>Scope |
| showDialog | *System.Boolean*<br>(Optional) Whether or not to force the user to approve the app again if they’ve already done so. |

#### Returns

Uri

### AuthUserAsync(responseUri, redirectUri, state)

Auth User - Authorisation Code Flow

| Name | Description |
| ---- | ----------- |
| responseUri | *System.Uri*<br>Response Uri |
| redirectUri | *System.Uri*<br>Redirect Uri |
| state | *System.String*<br>State |

#### Returns

AccessToken on Success, Null if Not

*Spotify.NetStandard.Client.Exceptions.AuthCodeValueException:* AuthCodeValueException

*Spotify.NetStandard.Client.Exceptions.AuthCodeStateException:* AuthCodeStateException

### AuthAsync()

Auth - Client Credentials Flow

#### Returns

AccessToken on Success, Null if Not

### AuthUserImplicit(redirectUri, state, scope, showDialog)

Auth User Implicit - Implicit Grant Flow

| Name | Description |
| ---- | ----------- |
| redirectUri | *System.Uri*<br>Redirect Uri |
| state | *System.String*<br>State |
| scope | *Spotify.NetStandard.Requests.Scope*<br>Scope |
| showDialog | *System.Boolean*<br>(Optional) Whether or not to force the user to approve the app again if they’ve already done so. |

#### Returns

Uri

### AuthUserImplicit(responseUri, redirectUri, state)

Auth User Implicit - Implicit Grant Flow

| Name | Description |
| ---- | ----------- |
| responseUri | *System.Uri*<br>Response Uri |
| redirectUri | *System.Uri*<br>Redirect Uri |
| state | *System.String*<br>State |

#### Returns

AccessToken on Success, Null if Not

*Spotify.NetStandard.Client.Exceptions.AuthTokenValueException:* AuthCodeValueException

*Spotify.NetStandard.Client.Exceptions.AuthTokenStateException:* AuthCodeStateException

### AuthUserPlaybackNextTrackAsync(deviceId)

Skip User’s Playback To Next Track 
Scopes: ConnectModifyPlaybackState


| Name | Description |
| ---- | ----------- |
| deviceId | *System.String*<br>(Optional) The id of the device this command is targeting. If not supplied, the user’s currently active device is the target. |

#### Returns

Status Object

*Spotify.NetStandard.Client.Exceptions.AuthUserTokenRequiredException:* 

### AuthUserPlaybackPauseAsync(deviceId)

Pause a User's Playback 
Scopes: ConnectModifyPlaybackState


| Name | Description |
| ---- | ----------- |
| deviceId | *System.String*<br>(Optional) The id of the device this command is targeting. If not supplied, the user’s currently active device is the target. |

#### Returns

Status Object

*Spotify.NetStandard.Client.Exceptions.AuthUserTokenRequiredException:* 

### AuthUserPlaybackPreviousTrackAsync(deviceId)

Skip User’s Playback To Previous Track 
Scopes: ConnectModifyPlaybackState


| Name | Description |
| ---- | ----------- |
| deviceId | *System.String*<br>(Optional) The id of the device this command is targeting. If not supplied, the user’s currently active device is the target. |

#### Returns

Status Object

*Spotify.NetStandard.Client.Exceptions.AuthUserTokenRequiredException:* 

### AuthUserPlaybackSeekTrackAsync(position, deviceId)

Seek To Position In Currently Playing Track 
Scopes: ConnectModifyPlaybackState


| Name | Description |
| ---- | ----------- |
| position | *System.Int32*<br>(Required) The position in milliseconds to seek to. Must be a positive number. Passing in a position that is greater than the length of the track will cause the player to start playing the next song. |
| deviceId | *System.String*<br>(Optional) The id of the device this command is targeting. If not supplied, the user’s currently active device is the target. |

#### Returns

Status Object

*Spotify.NetStandard.Client.Exceptions.AuthUserTokenRequiredException:* 

### AuthUserPlaybackSetRepeatModeAsync(state, deviceId)

Set Repeat Mode On User’s Playback 
Scopes: ConnectModifyPlaybackState


| Name | Description |
| ---- | ----------- |
| state | *Spotify.NetStandard.Enums.RepeatState*<br>(Required) track, context or off. track will repeat the current track. context will repeat the current context. off will turn repeat off. |
| deviceId | *System.String*<br>(Optional) The id of the device this command is targeting. If not supplied, the user’s currently active device is the target. |

#### Returns

Status Object

*Spotify.NetStandard.Client.Exceptions.AuthUserTokenRequiredException:* 

### AuthUserPlaybackSetVolumeAsync(percent, deviceId)

Set Volume For User's Playback 
Scopes: ConnectModifyPlaybackState


| Name | Description |
| ---- | ----------- |
| percent | *System.Int32*<br>(Required) The volume to set. Must be a value from 0 to 100 inclusive. |
| deviceId | *System.String*<br>(Optional) The id of the device this command is targeting. If not supplied, the user’s currently active device is the target. |

#### Returns

Status Object

*Spotify.NetStandard.Client.Exceptions.AuthUserTokenRequiredException:* 

### AuthUserPlaybackStartResumeAsync(request, deviceId)

Start/Resume a User's Playback 
Scopes: ConnectModifyPlaybackState


| Name | Description |
| ---- | ----------- |
| request | *Spotify.NetStandard.Requests.PlaybackRequest*<br>(Optional) Playback Request Object |
| deviceId | *System.String*<br>(Optional) The id of the device this command is targeting. If not supplied, the user’s currently active device is the target. |

#### Returns

Status Object

*Spotify.NetStandard.Client.Exceptions.AuthUserTokenRequiredException:* 

### AuthUserPlaybackToggleShuffleAsync(state, deviceId)

Toggle Shuffle For User’s Playback 
Scopes: ConnectModifyPlaybackState


| Name | Description |
| ---- | ----------- |
| state | *System.Boolean*<br>(Required) true : Shuffle user’s playback, false : Do not shuffle user’s playback |
| deviceId | *System.String*<br>(Optional) The id of the device this command is targeting. If not supplied, the user’s currently active device is the target. |

#### Returns

Status Object

*Spotify.NetStandard.Client.Exceptions.AuthUserTokenRequiredException:* 

### AuthUserPlaybackTransferAsync(request)

Transfer a User's Playback 
Scopes: ConnectModifyPlaybackState


| Name | Description |
| ---- | ----------- |
| request | *Spotify.NetStandard.Requests.DevicesRequest*<br>(Required) Devices Request Object |

#### Returns

Status Object

*Spotify.NetStandard.Client.Exceptions.AuthUserTokenRequiredException:* 

### GetToken

Get Access Token

#### Returns

Access Token

### LookupAllCategoriesAsync(country, locale, page)

Lookup All Categories

| Name | Description |
| ---- | ----------- |
| country | *System.String*<br>(Optional) A country: an ISO 3166-1 alpha-2 country code. |
| locale | *System.String*<br>(Optional) The desired language, consisting of a lowercase ISO 639-1 language code and an uppercase ISO 3166-1 alpha-2 country code, joined by an underscore |
| page | *Spotify.NetStandard.Requests.Page*<br>(Optional) Limit: The maximum number of categories to return. Default: 20. Minimum: 1. Maximum: 50. - Offset: The index of the first item to return. Default: 0 |

#### Returns

Content Response

*Spotify.NetStandard.Client.Exceptions.AuthAccessTokenRequiredException:* 

### LookupArtistAlbumsAsync(itemId, includeGroup, market, page)

Lookup Artist's Albums

| Name | Description |
| ---- | ----------- |
| itemId | *System.String*<br>(Required) The Spotify ID for the artist. |
| includeGroup | *Spotify.NetStandard.Requests.IncludeGroup*<br>(Optional) Filters the response. If not supplied, all album types will be returned |
| market | *System.String*<br>(Optional) An ISO 3166-1 alpha-2 country code |
| page | *Spotify.NetStandard.Requests.Page*<br>(Optional) Limit: The number of album objects to return. Default: 20. Minimum: 1. Maximum: 50 - Offset: The index of the first album to return. Default: 0 (i.e., the first album). |

#### Returns

Paging List of Album

*Spotify.NetStandard.Client.Exceptions.AuthAccessTokenRequiredException:* 

### LookupArtistRelatedArtistsAsync(itemId)

Lookup Artist's Related Artists

| Name | Description |
| ---- | ----------- |
| itemId | *System.String*<br>(Required) The Spotify ID for the artist. |

#### Returns

Lookup Response

*Spotify.NetStandard.Client.Exceptions.AuthAccessTokenRequiredException:* 

### LookupArtistTopTracksAsync(itemId, market)

Lookup Artist's Top Tracks

| Name | Description |
| ---- | ----------- |
| itemId | *System.String*<br>(Required) The Spotify ID for the artist. |
| market | *System.String*<br>(Required) A country: an ISO 3166-1 alpha-2 country code or the string from_token |

#### Returns

Lookup Response

*Spotify.NetStandard.Client.Exceptions.AuthAccessTokenRequiredException:* 

### LookupAsync(itemIds, lookupType, market, page)

Lookup

| Name | Description |
| ---- | ----------- |
| itemIds | *System.Collections.Generic.List{System.String}*<br>(Required) List of Spotify ID for the items |
| lookupType | *Spotify.NetStandard.Enums.LookupType*<br>(Required) Lookup Item Type |
| market | *System.String*<br>(Optional) ISO 3166-1 alpha-2 country code or the string from_token |
| page | *Spotify.NetStandard.Requests.Page*<br>(Optional) Limit: The maximum number of items to return - Offset: The index of the first item to return |

#### Returns

Lookup Response

*Spotify.NetStandard.Client.Exceptions.AuthAccessTokenRequiredException:* 

### LookupAsync\`\`1(itemId, lookupType, market, key, value, page)

Lookup

#### Type Parameters

- TResponse - Response Type

| Name | Description |
| ---- | ----------- |
| itemId | *System.String*<br>(Required) The Spotify ID for the album. |
| lookupType | *Spotify.NetStandard.Enums.LookupType*<br>(Required) Item Type |
| market | *System.String*<br>(Optional) ISO 3166-1 alpha-2 country code or the string from_token |
| key | *System.String*<br>(Optional) Query Parameter Key |
| value | *System.String*<br>(Optional) Query Parameter Value |
| page | *Spotify.NetStandard.Requests.Page*<br>(Optional) Limit: The maximum number of items to return - Offset: The index of the first item to return |

#### Returns

Lookup Response by Type

*Spotify.NetStandard.Client.Exceptions.AuthAccessTokenRequiredException:* 

### LookupCategoryAsync(categoryId, country, locale)

Lookup Category

| Name | Description |
| ---- | ----------- |
| categoryId | *System.String*<br>The Spotify category ID for the category. |
| country | *System.String*<br>(Optional) A country: an ISO 3166-1 alpha-2 country code. |
| locale | *System.String*<br>(Optional) The desired language, consisting of an ISO 639-1 language code and an ISO 3166-1 alpha-2 country code, joined by an underscore. |

#### Returns

Category Object

*Spotify.NetStandard.Client.Exceptions.AuthAccessTokenRequiredException:* 

### LookupFeaturedPlaylistsAsync(country, locale, timestamp, page)

Lookup Featured Playlists

| Name | Description |
| ---- | ----------- |
| country | *System.String*<br>(Optional) A country: an ISO 3166-1 alpha-2 country code. |
| locale | *System.String*<br>(Optional) The desired language, consisting of a lowercase ISO 639-1 language code and an uppercase ISO 3166-1 alpha-2 country code, joined by an underscore |
| timestamp | *System.Nullable{System.DateTime}*<br>(Optional) Use this parameter to specify the user’s local time to get results tailored for that specific date and time in the day. |
| page | *Spotify.NetStandard.Requests.Page*<br>(Optional) Limit: The maximum number of items to return. Default: 20. Minimum: 1. Maximum: 50. - Offset: The index of the first item to return. Default: 0 |

#### Returns

Content Response

*Spotify.NetStandard.Client.Exceptions.AuthAccessTokenRequiredException:* 

### LookupNewReleasesAsync(country, page)

Lookup New Releases

| Name | Description |
| ---- | ----------- |
| country | *System.String*<br>(Optional) A country: an ISO 3166-1 alpha-2 country code. |
| page | *Spotify.NetStandard.Requests.Page*<br>(Optional) Limit: The maximum number of items to return. Default: 20. Minimum: 1. Maximum: 50. - Offset: The index of the first item to return. Default: 0 |

#### Returns

Content Response

*Spotify.NetStandard.Client.Exceptions.AuthAccessTokenRequiredException:* 

### LookupRecommendationGenres

Lookup Recommendation Genres

#### Returns

Available Genre Seeds Object

*Spotify.NetStandard.Client.Exceptions.AuthAccessTokenRequiredException:* 

### LookupRecommendationsAsync(seedArtists, seedGenres, seedTracks, limit, market, minTuneableTrack, maxTuneableTrack, targetTuneableTrack)

Lookup Recommendations

| Name | Description |
| ---- | ----------- |
| seedArtists | *System.Collections.Generic.List{System.String}*<br>List of Spotify IDs for seed artists |
| seedGenres | *System.Collections.Generic.List{System.String}*<br>List of any genres in the set of available genre seeds |
| seedTracks | *System.Collections.Generic.List{System.String}*<br>List of Spotify IDs for a seed track |
| limit | *System.Nullable{System.Int32}*<br>The target size of the list of recommended tracks. Default: 20. Minimum: 1. Maximum: 100. |
| market | *System.String*<br>An ISO 3166-1 alpha-2 country code |
| minTuneableTrack | *Spotify.NetStandard.Requests.TuneableTrack*<br>Multiple values. For each tunable track attribute, a hard floor on the selected track attribute’s value can be provided |
| maxTuneableTrack | *Spotify.NetStandard.Requests.TuneableTrack*<br>Multiple values. For each tunable track attribute, a hard ceiling on the selected track attribute’s value can be provided. |
| targetTuneableTrack | *Spotify.NetStandard.Requests.TuneableTrack*<br>Multiple values. For each of the tunable track attributes a target value may be provided. |

#### Returns

Recommendation Response Object

*Spotify.NetStandard.Client.Exceptions.AuthAccessTokenRequiredException:* 

### NavigateAsync\`\`1(paging, navigateType)

Navigate

#### Type Parameters

- TResponse - Response Type

| Name | Description |
| ---- | ----------- |
| paging | *Spotify.NetStandard.Responses.Paging{\`\`0}*<br>Paging Object |
| navigateType | *Spotify.NetStandard.Enums.NavigateType*<br>Navigate Type |

#### Returns

Content Response

*Spotify.NetStandard.Client.Exceptions.AuthAccessTokenRequiredException:* 

### SearchAsync(query, searchType, country, external, page)

Search

| Name | Description |
| ---- | ----------- |
| query | *System.String*<br>(Required) Search Query |
| searchType | *Spotify.NetStandard.Requests.SearchType*<br>(Required) Search results include hits from all the specified item types. |
| country | *System.String*<br>(Optional) An ISO 3166-1 alpha-2 country code or the string from_token |
| external | *System.Nullable{System.Boolean}*<br>(Optional) Include any relevant audio content that is hosted externally. |
| page | *Spotify.NetStandard.Requests.Page*<br>(Optional) Limit: The maximum number of items to return - Offset: The index of the first item to return |

#### Returns

Content Response

*Spotify.NetStandard.Client.Exceptions.AuthAccessTokenRequiredException:* 

### SetToken(value)

Set Access Token

| Name | Description |
| ---- | ----------- |
| value | *Spotify.NetStandard.Client.Authentication.AccessToken*<br>Access Token |

## SpotifyClientFactory

Spotify Client Factory

### CreateSpotifyClient(clientId, clientSecret)

Create Spotify Client

| Name | Description |
| ---- | ----------- |
| clientId | *System.String*<br>Spotify Client Id |
| clientSecret | *System.String*<br>Spotify Client Secret |

#### Returns

Spotify Client

## FollowType

Follow Type

### Artist

Artist

### User

User


## LookupType

Lookup Type

### Albums

Albums

### AlbumTracks

Album Tracks

### ArtistAlbums

Artist Albums

### Artists

Artists

### AudioAnalysis

Audio Analysis

### AudioFeatures

Audio Features

### Categories

Categories

### CategoriesPlaylists

Category Playlists

### Playlist

Playlists

### PlaylistTracks

Playlist Tracks

### Tracks

Tracks


## NavigateType

Navigate Type

### Next

Navigate Next

### None

None

### Previous

Navigate Previous


## RepeatState

Repeat State

### Context

Will repeat the current context.

### Off

Will turn repeat off.

### Track

Will repeat the current track


## TimeRange

Time Range

### LongTerm

Calculated from several years of data and including all new data as it becomes available

### MediumTerm

Approximately last 6 months

### ShortTerm

Approximately last 4 weeks


## Cursor

Cursor Object

### After

The cursor to use as key to find the next page of items.

### Before

The cursor to use as key to find the previous page of items.

### Limit

The maximum number of items in the response (as set in the query or by default).


## DevicesRequest

Devices Request Object

### DeviceIds

(Required) List containing the ID of the device on which playback should be started/transferred. Although an array is accepted, only a single id is currently supported.

### Play

(Optional) true: ensure playback happens on new device. false or not provided: keep the current playback state.


## IncludeGroup

Include Group

### Album

Album

### AppearsOn

Appears On

### Compilation

Compliation

### Single

Single


## Page

Page

### Count

Page Count

### Current

Get / Set Current Page

### Limit

The maximum number of items in the response (as set in the query or by default).

### Offset

The offset of the items returned (as set in the query or by default).

### Total

The total number of items available to return.


## PlaybackRequest

Playback Request Object

### ContextUri

(Optional) Spotify URI of the context to play. Valid contexts are albums, artists, playlists. Example: spotify:album:1Je1IMUlBXcx1Fz0WE7oPT

### Offset

(Optional) Indicates from where in the context playback should start. Only available when ContextUri corresponds to an album or playlist object, or when the uris parameter is used. “position” is zero based and can’t be negative. Example: PositionRequest with Position = 5 or a UriRequest with Uri representing the uri of the item to start at. Example: UriRequest with Uri = "spotify:track:1301WleyT98MSxVHPZCA6M"

### Position

(Optional) Indicates from what position to start playback. Must be a positive number. Passing in a position that is greater than the length of the track will cause the player to start playing the next song.

### Uris

(Optional) A JSON array of the Spotify track URIs to play. Example: spotify:track:4iV5W9uYEdYUVa79Axb7Rh, spotify:track:1301WleyT98MSxVHPZCA6M


## PlaylistReorderRequest

Playlist Reorder Request Object

### InsertBefore

(Required) The position where the tracks should be inserted. To reorder the tracks to the end of the playlist, simply set insert_before to the position after the last track.

### RangeLength

(Optional) The amount of tracks to be reordered. Defaults to 1 if not set. The range of tracks to be reordered begins from the RangeStart position, and includes the RangeLength subsequent tracks.

### RangeStart

(Required) The position of the first track to be reordered.

### SnapshotId

(Optional) The playlist’s snapshot ID against which you want to make the changes.


## PlaylistRequest

Playlist Request Object

### Description

(Optional) Value for playlist description as displayed in Spotify Clients and in the Web API.

### IsCollaborative

(Optional) If true , the playlist will become collaborative and other users will be able to modify the playlist in their Spotify client. Note: You can only set collaborative to true on non-public playlists.

### IsPublic

(Optional) If true the playlist will be public, if false it will be private.

### Name

The new name for the playlist, for example "My New Playlist Title"


## PlaylistTracksRequest

Playlist Tracks Request Object

### SnapshotId

The playlist’s snapshot ID against which you want to make the changes

### Tracks

Spotify URIs of Tracks


## PositionRequest

Position Request Object

### Position

Position


## PublicRequest

Public Request Object

### IsPublic

Is Public


## Scope

Authorisation Scopes

### ConnectModifyPlaybackState

Pause a User's Playback 
Required For

Seek To Position In Currently Playing Track, Set Repeat Mode On User’s Playback, Set Volume For User's Playback

Skip User’s Playback To Next Track, Skip User’s Playback To Previous Track, Start/Resume a User's Playback

Toggle Shuffle For User’s Playback Transfer a User's Playback


### ConnectReadCurrentlyPlaying

Read access to a user’s currently playing track 
Required For

Get the User's Currently Playing Track


### ConnectReadPlaybackState

Read access to a user’s player state. 
Required For

Get a User's Available Devices, Get Information About The User's Current Playback, Get the User's Currently Playing Track


### FollowModify

Write/delete access to the list of artists and other users that the user follows. 
Required For

Follow Artists or Users, Unfollow Artists or Users


### FollowRead

Read access to the list of artists and other users that the user follows. 
Required For

Check if Current User Follows Artists or Users, Get User's Followed Artists


### LibraryModify

Write/delete access to a user's "Your Music" library. 
Required For

Remove Albums for Current User, Remove User's Saved Tracks, Save Albums for Current User Save Tracks for User


### LibraryRead

Read access to a user's "Your Music" library. 
Required For

Check User's Saved Albums Check User's Saved Tracks, Get Current User's Saved Albums Get a User's Saved Tracks


### ListeningRecentlyPlayed

Read access to a user’s recently played tracks 
Required For

Get Current User's Recently Played Tracks


### ListeningTopRead

Read access to a user's top artists and tracks. 
Required For

Get a User's Top Artists and Tracks


### PlaybackAppRemoteControl

Remote control playback of Spotify.

### PlaybackStreaming

Control playback of a Spotify track. The user must have a Spotify Premium account.

### PlaylistModifyPrivate

Write access to a user's private playlists. 
Required For

Follow a Playlist, Unfollow a Playlist, Add Tracks to a Playlist

Change a Playlist's Details, Create a Playlist, Remove Tracks from a Playlist

Reorder a Playlist's Tracks, Replace a Playlist's Tracks, Upload a Custom Playlist Cover Image


### PlaylistModifyPublic

Write access to a user's public playlists. 
Required For

Follow a Playlist, Unfollow a Playlist, Add Tracks to a Playlist

Change a Playlist's Details, Create a Playlist, Remove Tracks from a Playlist

Reorder a Playlist's Tracks, Replace a Playlist's Tracks, Upload a Custom Playlist Cover Image


### PlaylistReadCollaborative

Include collaborative playlists when requesting a user's playlists. 
Required For

Get a List of Current User's Playlists, Get a List of a User's Playlists


### PlaylistReadPrivate

Read access to user's private playlists. 
Required For

Check if Users Follow a Playlist, Get a List of Current User's Playlists, Get a List of a User's Playlists


### UserGeneratedContentImageUpload

User Generated Content Image Upload 
Required For

Upload a Custom Playlist Cover Image


### UserReadBirthDate

Read access to the user's birthdate. 
Required For

Get Current User's Profile


### UserReadEmail

Read access to user’s email address. 
Required For

Get Current User's Profile


### UserReadPrivate

Read access to user’s subscription details (type of user account). 
Required For

Search for an Item, Get Current User's Profile



## SearchType

Search Type

### Album

Album

### Artist

Artist

### Playlist

Playlist

### Track

Track


## TuneableTrack

Tuneable Track Object

### Acousticness

A confidence measure from 0.0 to 1.0 of whether the track is acoustic.

### Danceability

Danceability describes how suitable a track is for dancing based on a combination of musical elements including tempo, rhythm stability, beat strength, and overall regularity.

### Duration

The duration of the track in milliseconds.

### Energy

Energy is a measure from 0.0 to 1.0 and represents a perceptual measure of intensity and activity

### Instrumentalness

Predicts whether a track contains no vocals

### Key

The key the track is in. Integers map to pitches using standard Pitch Class notation.

### Liveness

Detects the presence of an audience in the recording.

### Loudness

The overall loudness of a track in decibels (dB)

### Mode

Mode indicates the modality(major or minor) of a track, the type of scale from which its melodic content is derived

### Popularity

The popularity of the track. The value will be between 0 and 100, with 100 being the most popular.

### Speechiness

Speechiness detects the presence of spoken words in a track.

### Tempo

The overall estimated tempo of a track in beats per minute (BPM).

### TimeSignature

An estimated overall time signature of a track.

### Valence

A measure from 0.0 to 1.0 describing the musical positiveness conveyed by a track.


## UriListRequest

URI List Request Object

### Uris

URIs


## UriRequest

URI Request Object

### Uri

Spotify URI


## Album

Album Object

### Artists

The artists of the album. Each artist object includes a link in href to more detailed information about the artist.

### Copyrights

The copyright statements of the album.

### ExternalId

Known external IDs for the album.

### Genres

A list of the genres used to classify the album. For example: "Prog Rock" , "Post-Grunge"

### Label

The label for the album.

### Popularity

The popularity of the album. The value will be between 0 and 100, with 100 being the most popular

### ReleaseDate

The date the album was first released, for example 1981. Depending on the precision, it might be shown as 1981-12 or 1981-12-15

### ReleaseDatePrecision

The precision with which release_date value is known: year , month , or day.

### Tracks

The tracks of the album.


## Artist

Artist Object

### Followers

Information about the followers of the artist.

### Genres

A list of the genres the artist is associated with. For example: "Prog Rock" , "Post-Grunge".

### Images

Images of the artist in various sizes, widest first.

### Popularity

The popularity of the artist. The value will be between 0 and 100, with 100 being the most popular.


## AudioAnalysis

Audio Analysis Object

### Bars

The time intervals of the bars throughout the track

### Beats

The time intervals of beats throughout the track.

### Sections

Sections are defined by large variations in rhythm or timbre, e.g.chorus, verse, bridge, guitar solo, etc.

### Segments

Audio segments attempts to subdivide a song into many segments, with each segment containing a roughly consitent sound throughout its duration.

### Tatums

A tatum represents the lowest regular pulse train that a listener intuitively infers from the timing of perceived musical events


## AudioFeatures

Audio Features Object

### Acousticness

A confidence measure from 0.0 to 1.0 of whether the track is acoustic.

### AnalysisUrl

An HTTP URL to access the full audio analysis of this track.

### Danceability

Danceability describes how suitable a track is for dancing based on a combination of musical elements including tempo, rhythm stability, beat strength, and overall regularity.

### Duration

The duration of the track in milliseconds.

### Energy

Energy is a measure from 0.0 to 1.0 and represents a perceptual measure of intensity and activity

### Instrumentalness

Predicts whether a track contains no vocals

### Key

The key the track is in. Integers map to pitches using standard Pitch Class notation.

### Liveness

Detects the presence of an audience in the recording.

### Loudness

The overall loudness of a track in decibels (dB)

### Mode

Mode indicates the modality(major or minor) of a track, the type of scale from which its melodic content is derived

### Speechiness

Speechiness detects the presence of spoken words in a track.

### Tempo

The overall estimated tempo of a track in beats per minute (BPM).

### TimeSignature

An estimated overall time signature of a track.

### TrackHref

A link to the Web API endpoint providing full details of the track.

### Valence

A measure from 0.0 to 1.0 describing the musical positiveness conveyed by a track.


## AvailableGenreSeeds

Available Genre Seeds Object

### Genres

Genres


## BaseResponse

Base Response Object

### Error

Error Object


## Category

Category Object

### Images

The category icon, in various sizes.


## Content

Content

### Id

The base-62 identifier that you can find at the end of the Spotify URI for the object

### Name

The name of the content


## ContentResponse

Content Response

### Albums

Paging Object of Albums

### Artists

Paging Object of Artists

### Categories

Paging Object of Category

### Playlists

Paging Object of Playlists

### Tracks

Paging Object of Tracks


## Context

Context Object

### ExternalUrls

Known external URLs for this object.

### Href

A link to the Web API endpoint providing full details of the object

### Type

The object type of the object

### Uri

The Spotify URI for the object


## Copyright

Copyright Object

### Text

The copyright text for this album.

### Type

The type of copyright: C = the copyright, P = the sound recording (performance) copyright.


## CurrentlyPlaying

Currently Playing Object

### Device

The device that is currently active

### RepeatState

off, track, context

### SuffleState

If shuffle is on or off


## CursorPaging\`1

Cursor Paging Object

#### Type Parameters

- T - Object Type

### Constructor

Constructor

### After

The cursor to use as key to find the next page of items.

### Cursors

The cursors used to find the next set of items.

### Href

A link to the Web API endpoint returning the full result of the request.

### Items

The requested data.

### Next

URL to the next page of items. (null if none)

### ReadOnlyItems

IEnumerable of Type

### Total

The total number of items available to return.


## Device

Device Object

### Id

The device ID. This may be null.

### IsActive

If this device is the currently active device.

### IsPrivateSession

If this device is currently in a private session.

### IsRestricted

Whether controlling this device is restricted. If true then no commands will be accepted by this device.

### Name

The name of the device.

### Type

Device type, such as “computer”, “smartphone” or “speaker”.

### Volume

The current volume in percent. This may be null.


## Devices

Devices Object

### Items

A list of 0..n Device objects.


## ErrorResponse

Error Object

### Message

A short description of the cause of the error.

### StatusCode

The HTTP status code


## ExternalId

External Id Object

### Ean

International Article Number

### Isrc

International Standard Recording Code

### Upc

Universal Product Code


## ExternalUrl

External Url Object

### Spotify

An external, public URL to the object.


## Followers

Followers Object

### Href

A link to the Web API endpoint providing full details of the followers; null if not available

### Total

The total number of followers.


## Image

Image Object

### Height

The image height in pixels. If unknown: null or not returned.

### Url

The source URL of the image.

### Width

The image width in pixels. If unknown: null or not returned.


## ContentCursorResponse

Content Cursor Response

### Artists

Cursor Paging Object of Artist


## LinkedTrack

Linked Track Object


## LookupResponse

Lookup Response

### Albums

List of Album Object

### Artists

List of Artist Object

### AudioFeatures

List of Audio Feature Object

### Tracks

List of Track Object


## Paging\`1

Paging Object

#### Type Parameters

- T - Object Type

### Constructor

Constructor

### Href

A link to the Web API endpoint returning the full result of the request.

### Items

The requested data.

### Next

URL to the next page of items. (null if none)

### Page

Page

### Previous

URL to the previous page of items. (null if none)

### ReadOnlyItems

IEnumerable of Type


## PlayHistory

Play History Object

### Context

The context the track was played from.

### PlayedAt

The date and time the track was played. Format yyyy-MM-ddTHH:mm:ss

### Track

The track the user listened to.


## Playlist

Playlist Object

### Collaborative

true if the owner allows other users to modify the playlist.

### Description

The playlist description. Only returned for modified, verified playlists, otherwise null.

### Followers

Information about the followers of the playlist.

### Images

Images for the playlist.The array may be empty or contain up to three images. The images are returned by size in descending order.

### Owner

The user who owns the playlist

### Public

The playlist’s public/private status: true the playlist is public, false the playlist is private, null the playlist status is not relevant

### SnapshotId

The version identifier for the current playlist.

### Tracks

Information about the tracks of the playlist.


## PlaylistTrack

Playlist Track Object

### AddedAt

The date and time the track was added.

### AddedBy

The Spotify user who added the track.

### IsLocal

Whether this track is a local file or not.

### Track

Information about the track.


## PrivateUser

Private User Object

### BirthDate

The user’s date-of-birth.This field is only available when the current user has granted access to the user-read-birthdate scope.

### Country

The country of the user, as set in the user’s account profile.An ISO 3166-1 alpha-2 country code.This field is only available when the current user has granted access to the user-read-private scope.

### Email

The user’s email address, as entered by the user when creating their account. his field is only available when the current user has granted access to the user-read-email scope


## PublicUser

Public User Object

### DisplayName

The name displayed on the user’s profile. null if not available.

### Followers

Information about the followers of this user.

### Images

The user’s profile image.

### Product

The user’s Spotify subscription level: “premium”, “free”, etc. This field is only available when the current user has granted access to the user-read-private scope.


## RecommendationSeed

Recommendation Seed Object

### AfterFilteringSize

The number of tracks available after min_* and max_* filters have been applied.

### AfterRelinkingSize

The number of tracks available after relinking for regional availability.

### InitialPoolSize

The number of recommended tracks available for this seed.


## RecommendationsResponse

Recommendations Response Object

### Seeds

An array of recommendation seed objects.

### Tracks

An array of track object (simplified) ordered according to the parameters supplied.


## SavedAlbum

Saved Album Object

### AddedAt

The date and time the album was saved Timestamps are returned in ISO 8601 format as Coordinated Universal Time (UTC) with a zero offset: YYYY-MM-DDTHH:MM:SSZ. If the time is imprecise (for example, the date/time of an album release), an additional field indicates the precision; see for example, release_date in an album object.

### Album

Information about the album.


## SavedTrack

Saved Track Object

### AddedAt

The date and time the track was saved. Timestamps are returned in ISO 8601 format as Coordinated Universal Time (UTC) with a zero offset: YYYY-MM-DDTHH:MM:SSZ. If the time is imprecise (for example, the date/time of an album release), an additional field indicates the precision; see for example, release_date in an album object.

### Track

Information about the track.


## Section

Section Object

### Key

The estimated overall key of the section. The values in this field ranging from 0 to 11 mapping to pitches using standard Pitch Class notation

### KeyConfidence

The confidence, from 0.0 to 1.0, of the reliability of the key.

### Loudness

The overall loudness of the section in decibels (dB).

### Mode

Indicates the modality (major or minor) of a track, the type of scale from which its melodic content is derived.This field will contain a 0 for “minor”, a 1 for “major”, or a -1 for no result.

### ModeConfidence

The confidence, from 0.0 to 1.0, of the reliability of the mode.

### Tempo

The overall estimated tempo of the section in beats per minute (BPM).

### TempoConfidence

The confidence, from 0.0 to 1.0, of the reliability of the tempo.

### TimeSignature

An estimated overall time signature of a track. The time signature (meter) is a notational convention to specify how many beats are in each bar (or measure). The time signature ranges from 3 to 7 indicating time signatures of “3/4”, to “7/4”.

### TimeSignatureConfidence

The confidence, from 0.0 to 1.0, of the reliability of the time_signature.


## Segment

Segment Object

### LoudnessEnd

The offset loudness of the segment in decibels (dB).

### LoudnessMax

The peak loudness of the segment in decibels (dB).

### LoudnessMaxTime

The segment-relative offset of the segment peak loudness in seconds.

### LoudnessStart

The onset loudness of the segment in decibels (dB).

### Pitches

A “chroma” vector representing the pitch content of the segment, corresponding to the 12 pitch classes C, C#, D to B, with values ranging from 0 to 1 that describe the relative dominance of every pitch in the chromatic scale

### Timbre

Timbre is the quality of a musical note or sound that distinguishes different types of musical instruments, or voices.


## SimplifiedAlbum

Simplified Album Object

### AlbumGroup

The field is present when getting an artist’s albums. Possible values are “album”, “single”, “compilation”, “appears_on”.

### AlbumType

The type of the album: one of "album" , "single" , or "compilation".

### Artists

The artists of the album. Each artist object includes a link in href to more detailed information about the artist.

### AvailableMarkets

The markets in which the album is available: ISO 3166-1 alpha-2 country codes

### Images

The cover art for the album in various sizes, widest first.


## SimplifiedArtist

Simplified Artist Object


## SimplifiedCurrentlyPlaying

Simplified Currently Playing Object

### Context

A Context Object. Can be null

### IsPlaying

If something is currently playing, return true.

### Progress

Progress into the currently playing track. Can be null.

### TimeStamp

Unix Millisecond Timestamp when data was fetched

### Track

The currently playing track. Can be null.

### Type

The object type of the currently playing item. Can be one of track, episode, ad or unknown.


## SimplifiedTrack

Simplified Track Object

### Artists

The artists who performed the track. Each artist object includes a link in href to more detailed information about the artist.

### AvailableMarkets

A list of the countries in which the track can be played, identified by their ISO 3166-1 alpha-2 code.

### DiscNumber

The disc number(usually 1 unless the album consists of more than one disc).

### Duration

The track length in milliseconds.

### IsExplicit

Whether or not the track has explicit lyrics ( true = yes it does; false = no it does not OR unknown).

### IsPlayable

Part of the response when Track Relinking is applied. If true , the track is playable in the given market. Otherwise false.

### LinkedFrom

Part of the response when Track Relinking is applied and is only part of the response if the track linking, in fact, exists

### Preview

A link to a 30 second preview(MP3 format) of the track.

### TrackNumber

The number of the track. If an album has several discs, the track number is the number on the specified disc.


## Snapshot

Snapshot Response Object

### SnapshotId

Can be used to identify playlist version in future requests


## Status

Status Response

### Code

Code

### Success

Success


## TimeInterval

Time Interval Object

### Confidence

The reliability confidence, from 0.0 to 1.0

### Duration

The duration in seconds

### Start

The starting point in seconds.


## Track

Track Object

### Album

The album on which the track appears.The album object includes a link in href to full information about the album.

### Artists

The artists who performed the track. Each artist object includes a link in href to more detailed information about the artist.

### ExternalId

Known external IDs for the track.

### Popularity

The popularity of the track.The value will be between 0 and 100, with 100 being the most popular.

### Restrictions

Part of the response when Track Relinking is applied, the original track is not available in the given market


## TrackRestriction

Track Restriction Object

### Reason

Contains the reason why the track is not available e.g. market
