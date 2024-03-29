# Spotify.NetStandard

Spotify API .NET Standard Library

## Documentation and Source Code

Project Documentation and Source Code can be found at [https://github.com/RoguePlanetoid/Spotify-NetStandard](https://github.com/RoguePlanetoid/Spotify-NetStandard)

## NuGet

To add to your project from [nuget.org](https://www.nuget.org/packages/Spotify.NetStandard/) use:
```
Install-Package Spotify.NetStandard
```

## Example

```c#
using Spotify.NetStandard.Client;
using Spotify.NetStandard.Requests;

var client = SpotifyClientFactory
    .CreateSpotifyClient(
    "client-id","client-secret");
var page = new Page() { Limit = 10 };
var browse = await client.LookupNewReleasesAsync(page: page);
foreach (var album in browse.Albums.Items)
{
    ...
}
```

## Client Id and Client Secret

You can get a "client-id" and "client-secret" from [developer.spotify.com/dashboard](https://developer.spotify.com/dashboard/) by signing in with your Spotify Id then creating an Application.

## Change Log

### Version 2.1.0

- Restored Authorisation Code Flow

### Version 2.0.0

- Refactored Code, Added User Episodes, Audiobook, Chapter and Queue, Updated Authorisation Code Flow with Code Verifier using Proof Key for Code Exchange (PKCE), Removed Authorisation Code Flow without Code Verifier and Original Proof Key for Code Exchange (PKCE) Flow

### Version 1.9.0

- Added External HttpClient Support and Uno Platform Compatibility

### Version 1.8.0

- Updated Authentication Flow with PKCE for Refresh Token

### Version 1.7.7

- Fixed issue with Authentication Cache Response Uri

### Version 1.7.6

- Fixed issue with Resume Point not returning Resume Position Correctly

### Version 1.7.5

- Added Authorization Code Flow with Proof Key for Code Exchange (PKCE) for API

### Version 1.7.0

- Added Authorization Code Flow with Proof Key for Code Exchange (PKCE), Updated Track Restrictions and Newtonsoft.Json

### Version 1.6.6

- Fixed Track or Episode identification issue with Playlist Track and Currently Playing Objects

### Version 1.6.5

- Updated Get a Playlist and Get a Playlist's Items to support Additional Types

### Version 1.6.0

- Updated Remove Tracks from Playlist to support Positions and Added Paging Method

### Version 1.5.5

- Fixed issue with Market and Country being used correctly

### Version 1.5.0

- Added Save Shows for Current User, Get User's Saved Shows, Remove User's Saved Shows, Get an Episode, Get Multiple Episodes, Get a Show, Get Multiple Shows and Get a Show's Episodes plus PlaybackPositionRead Scope. Updated Get the User's Currently Playing Track, Get Information About The User's Current Playback and Search for an Item

### Version 1.2.0

- Added Add an Item to the User's Playback Queue, Updated Get a Playlist and Get Playlist's Tracks to support Fields Parameter and Fixed Check/Lookup Methods Return Error Status Correctly

### Version 1.1.5

- Added Actions Object, Disallows Object, Simplified Playlist Object and updated related methods

### Version 1.1.4

- Fixed Token Storage and Get Playlist Tracks

### Version 1.1.3

- Removed User Birthdate Value and Scope

### Version 1.1.2

- Fixed and Improved Authentication Exceptions including minor Client Changes

### Version 1.1.1

- Fixed Cursor and Paging Navigation

### Version 1.1.0

- Added Authenticated Get Methods
- Fixed Issue with Cursor Responses

### Version 1.0.2

- Fixed Extension Methods

### Version 1.0.1

- Added Multi Scope Helpers by [parkeradam](https://github.com/parkeradam)

### Version 1.0.0

- Initial Release
