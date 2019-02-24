using System.Runtime.Serialization;

namespace Spotify.NetStandard.Responses
{
    /// <summary>
    /// Audio Features Object
    /// </summary>
    public class AudioFeatures : Content
    {
        /// <summary>
        /// A confidence measure from 0.0 to 1.0 of whether the track is acoustic.
        /// </summary>
        [DataMember(Name = "acousticness")]
        public float? Acousticness { get; set; }

        /// <summary>
        /// An HTTP URL to access the full audio analysis of this track.
        /// </summary>
        [DataMember(Name = "analysis_url")]
        public string AnalysisUrl { get; set; }

        /// <summary>
        /// Danceability describes how suitable a track is for dancing based on a combination of musical elements including tempo, rhythm stability, beat strength, and overall regularity.
        /// </summary>
        [DataMember(Name = "danceability")]
        public float? Danceability { get; set; }

        /// <summary>
        /// The duration of the track in milliseconds.
        /// </summary>
        [DataMember(Name = "duration_ms")]
        public long? Duration { get; set; }

        /// <summary>
        /// Energy is a measure from 0.0 to 1.0 and represents a perceptual measure of intensity and activity
        /// </summary>
        [DataMember(Name = "energy")]
        public float? Energy { get; set; }

        /// <summary>
        /// Predicts whether a track contains no vocals
        /// </summary>
        [DataMember(Name = "instrumentalness")]
        public float? Instrumentalness { get; set; }

        /// <summary>
        /// The key the track is in. Integers map to pitches using standard Pitch Class notation.
        /// </summary>
        [DataMember(Name = "key")]
        public int? Key { get; set; }

        /// <summary>
        /// Detects the presence of an audience in the recording.
        /// </summary>
        [DataMember(Name = "liveness")]
        public float? Liveness { get; set; }

        /// <summary>
        /// The overall loudness of a track in decibels (dB)
        /// </summary>
        [DataMember(Name = "loudness")]
        public float? Loudness { get; set; }

        /// <summary>
        /// Mode indicates the modality(major or minor) of a track, the type of scale from which its melodic content is derived
        /// </summary>
        [DataMember(Name = "mode")]
        public int? Mode { get; set; }

        /// <summary>
        /// Speechiness detects the presence of spoken words in a track. 
        /// </summary>
        [DataMember(Name = "speechiness")]
        public float? Speechiness { get; set; }

        /// <summary>
        /// The overall estimated tempo of a track in beats per minute (BPM). 
        /// </summary>
        [DataMember(Name = "tempo")]
        public float? Tempo { get; set; }

        /// <summary>
        /// An estimated overall time signature of a track.
        /// </summary>
        [DataMember(Name = "time_signature")]
        public int? TimeSignature { get; set; }

        /// <summary>
        /// A link to the Web API endpoint providing full details of the track.
        /// </summary>
        [DataMember(Name = "track_href")]
        public string TrackHref { get; set; }

        /// <summary>
        /// A measure from 0.0 to 1.0 describing the musical positiveness conveyed by a track.
        /// </summary>
        [DataMember(Name = "valence")]
        public float? Valence { get; set; }
    }
}
