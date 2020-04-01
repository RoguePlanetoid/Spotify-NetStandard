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
