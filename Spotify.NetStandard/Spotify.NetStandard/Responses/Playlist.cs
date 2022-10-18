namespace Spotify.NetStandard.Responses;

/// <summary>
/// Playlist Object
/// </summary>
[DataContract]
public class Playlist : SimplifiedPlaylist
{
    /// <summary>
    /// The playlist description. Only returned for modified, verified playlists, otherwise null.
    /// </summary>
    [DataMember(Name = "description")]
    public string Description { get; set; }

    /// <summary>
    /// Information about the followers of the playlist.
    /// </summary>
    [DataMember(Name = "followers")]
    public Followers Followers { get; set; }
}
