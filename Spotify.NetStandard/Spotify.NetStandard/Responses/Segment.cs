namespace Spotify.NetStandard.Responses;

/// <summary>
/// Segment Object
/// </summary>
[DataContract]
public class Segment : TimeInterval
{
    /// <summary>
    /// The onset loudness of the segment in decibels (dB).
    /// </summary>
    [DataMember(Name = "loudness_start")]
    public float? LoudnessStart { get; set; }

    /// <summary>
    /// The peak loudness of the segment in decibels (dB). 
    /// </summary>
    [DataMember(Name = "loudness_max")]
    public float? LoudnessMax { get; set; }

    /// <summary>
    /// The segment-relative offset of the segment peak loudness in seconds. 
    /// </summary>
    [DataMember(Name = "loudness_max_time")]
    public float? LoudnessMaxTime { get; set; }

    /// <summary>
    /// The offset loudness of the segment in decibels (dB).
    /// </summary>
    [DataMember(Name = "loudness_end")]
    public float? LoudnessEnd { get; set; }

    /// <summary>
    /// A “chroma” vector representing the pitch content of the segment, corresponding to the 12 pitch classes C, C#, D to B, with values ranging from 0 to 1 that describe the relative dominance of every pitch in the chromatic scale
    /// </summary>
    [DataMember(Name = "pitches")]
    public List<float> Pitches { get; set; }

    /// <summary>
    /// Timbre is the quality of a musical note or sound that distinguishes different types of musical instruments, or voices.
    /// </summary>
    [DataMember(Name = "timbre")]
    public List<float> Timbre { get; set; }
}
