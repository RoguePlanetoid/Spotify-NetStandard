namespace Spotify.NetStandard.Requests;

/// <summary>
/// Position URI Request Object
/// </summary>
[DataContract]
public class PositionUriRequest : UriRequest
{
    /// <summary>
    /// Positions for each of the Uris in the playlist, positions are zero-indexed, that is the first item in the playlist has the value 0, the second item 1, and so on
    /// </summary>
    [DataMember(Name = "positions")]
    public List<int> Positions { get; set; }
}
