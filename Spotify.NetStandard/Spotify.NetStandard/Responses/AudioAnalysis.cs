namespace Spotify.NetStandard.Responses;

/// <summary>
/// Audio Analysis Object
/// </summary>
[DataContract]
public class AudioAnalysis : BaseResponse
{
    /// <summary>
    /// The time intervals of the bars throughout the track
    /// </summary>
    [DataMember(Name = "bars")]
    public List<TimeInterval> Bars { get; set; }

    /// <summary>
    /// The time intervals of beats throughout the track.
    /// </summary>
    [DataMember(Name = "beats")]
    public List<TimeInterval> Beats { get; set; }

    /// <summary>
    /// Sections are defined by large variations in rhythm or timbre, e.g.chorus, verse, bridge, guitar solo, etc.
    /// </summary>
    [DataMember(Name = "sections")]
    public List<Section> Sections { get; set; }

    /// <summary>
    /// Audio segments attempts to subdivide a song into many segments, with each segment containing a roughly consitent sound throughout its duration.
    /// </summary>
    [DataMember(Name = "segments")]
    public List<Segment> Segments { get; set; }

    /// <summary>
    /// A tatum represents the lowest regular pulse train that a listener intuitively infers from the timing of perceived musical events
    /// </summary>
    [DataMember(Name = "tatums")]
    public List<TimeInterval> Tatums { get; set; }
}
