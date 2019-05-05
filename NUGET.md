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