namespace Spotify.NetStandard.Responses;

/// <summary>
/// Track Object
/// </summary>
[DataContract]
public class Track : SimplifiedTrack
{
    /// <summary>
    /// The album on which the track appears.The album object includes a link in href to full information about the album.
    /// </summary>
    [DataMember(Name = "album")]
    public SimplifiedAlbum Album { get; set; }

    /// <summary>
    /// Known external IDs for the track.
    /// </summary>
    [DataMember(Name = "external_ids")]
    public ExternalId ExternalId { get; set; }

    /// <summary>
    /// The popularity of the track. The value will be between 0 and 100, with 100 being the most popular.
    /// </summary>
    [DataMember(Name = "popularity")]
    public int Popularity { get; set; }

    /// <summary>
    /// Part of the response when Track Relinking is applied, the original track is not available in the given market
    /// </summary>
    [DataMember(Name = "restrictions")]
    public TrackRestriction Restrictions { get; set; }
}
