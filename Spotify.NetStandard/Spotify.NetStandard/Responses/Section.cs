namespace Spotify.NetStandard.Responses;

/// <summary>
/// Section Object
/// </summary>
[DataContract]
public class Section : TimeInterval
{
    /// <summary>
    /// The overall loudness of the section in decibels (dB).
    /// </summary>
    [DataMember(Name = "loudness")]
    public float? Loudness { get; set; }

    /// <summary>
    /// The overall estimated tempo of the section in beats per minute (BPM).  
    /// </summary>
    [DataMember(Name = "tempo")]
    public float? Tempo { get; set; }

    /// <summary>
    /// The confidence, from 0.0 to 1.0, of the reliability of the tempo.
    /// </summary>
    [DataMember(Name = "tempo_confidence")]
    public float? TempoConfidence { get; set; }

    /// <summary>
    /// The estimated overall key of the section. The values in this field ranging from 0 to 11 mapping to pitches using standard Pitch Class notation
    /// </summary>
    [DataMember(Name = "key")]
    public int? Key { get; set; }

    /// <summary>
    /// The confidence, from 0.0 to 1.0, of the reliability of the key.
    /// </summary>
    [DataMember(Name = "key_confidence")]
    public float? KeyConfidence { get; set; }

    /// <summary>
    /// Indicates the modality (major or minor) of a track, the type of scale from which its melodic content is derived.This field will contain a 0 for “minor”, a 1 for “major”, or a -1 for no result.
    /// </summary>
    [DataMember(Name = "mode")]
    public int? Mode { get; set; }

    /// <summary>
    /// The confidence, from 0.0 to 1.0, of the reliability of the mode.
    /// </summary>
    [DataMember(Name = "mode_confidence")]
    public float? ModeConfidence { get; set; }

    /// <summary>
    /// An estimated overall time signature of a track. The time signature (meter) is a notational convention to specify how many beats are in each bar (or measure). The time signature ranges from 3 to 7 indicating time signatures of “3/4”, to “7/4”.
    /// </summary>
    [DataMember(Name = "time_signature")]
    public int? TimeSignature { get; set; }

    /// <summary>
    /// The confidence, from 0.0 to 1.0, of the reliability of the time_signature.
    /// </summary>
    [DataMember(Name = "time_signature_confidence")]
    public float? TimeSignatureConfidence { get; set; }
}
