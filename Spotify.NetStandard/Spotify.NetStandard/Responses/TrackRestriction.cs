namespace Spotify.NetStandard.Responses;

/// <summary>
/// Track Restriction Object
/// </summary>
[DataContract]
public class TrackRestriction
{
    /// <summary>
    /// Contains the reason why the track is not available e.g. market
    /// </summary>
    [DataMember(Name = "reason")]
    public string Reason { get; set; }
}
