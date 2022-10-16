namespace Spotify.NetStandard.Client.Internal;

/// <summary>
/// Helper Methods
/// </summary>
internal static class Helpers
{
    /// <summary>
    /// Get Playlist Tracks Request
    /// </summary>
    /// <param name="uris">Uris</param>
    /// <param name="uriPositions">(Optional) Uri Positions</param>
    /// <param name="snapshotId">(Optional) Snapshot Id</param>
    /// <returns></returns>
    public static PlaylistTracksRequest GetPlaylistTracksRequest(
        List<string> uris, 
        List<List<int>> uriPositions = null, 
        string snapshotId = null)
    {
        var request = new PlaylistTracksRequest()
        {
            Tracks = uris.Select(uri =>
                new PositionUriRequest()
                {
                    Uri = uri
                }).ToList(),
            SnapshotId = snapshotId,
        };
        for (int i = 0; i < request.Tracks.Count(); i++)
        {
            if (uriPositions?.Count > 0 && i < uriPositions.Count)
                request.Tracks[i].Positions = uriPositions[i];
        }
        return request;
    }
}
