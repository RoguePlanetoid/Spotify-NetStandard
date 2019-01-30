using System.ComponentModel;

namespace Spotify.NetStandard.Responses
{
    /// <summary>
    /// Tuneable Track Object
    /// </summary>
    public class TuneableTrack
    {
        /// <summary>
        /// A confidence measure from 0.0 to 1.0 of whether the track is acoustic.
        /// </summary>
        [Description("acousticness")]
        public float? Acousticness { get; set; }

        /// <summary>
        /// Danceability describes how suitable a track is for dancing based on a combination of musical elements including tempo, rhythm stability, beat strength, and overall regularity.
        /// </summary>
        [Description("danceability")]
        public float? Danceability { get; set; }

        /// <summary>
        /// The duration of the track in milliseconds.
        /// </summary>
        [Description("duration_ms")]
        public int? Duration { get; set; }

        /// <summary>
        /// Energy is a measure from 0.0 to 1.0 and represents a perceptual measure of intensity and activity
        /// </summary>
        [Description("energy")]
        public float? Energy { get; set; }

        /// <summary>
        /// Predicts whether a track contains no vocals
        /// </summary>
        [Description("instrumentalness")]
        public float? Instrumentalness { get; set; }

        /// <summary>
        /// The key the track is in. Integers map to pitches using standard Pitch Class notation.
        /// </summary>
        [Description("key")]
        public int? Key { get; set; }

        /// <summary>
        /// Detects the presence of an audience in the recording.
        /// </summary>
        [Description("liveness")]
        public float? Liveness { get; set; }

        /// <summary>
        /// The overall loudness of a track in decibels (dB)
        /// </summary>
        [Description("loudness")]
        public float? Loudness { get; set; }

        /// <summary>
        /// Mode indicates the modality(major or minor) of a track, the type of scale from which its melodic content is derived
        /// </summary>
        [Description("mode")]
        public int? Mode { get; set; }

        /// <summary>
        /// The popularity of the track. The value will be between 0 and 100, with 100 being the most popular.
        /// </summary>
        [Description("popularity")]
        public int? Popularity { get; set; }

        /// <summary>
        /// Speechiness detects the presence of spoken words in a track. 
        /// </summary>
        [Description("speechiness")]
        public float? Speechiness { get; set; }

        /// <summary>
        /// The overall estimated tempo of a track in beats per minute (BPM). 
        /// </summary>
        [Description("tempo")]
        public float? Tempo { get; set; }

        /// <summary>
        /// An estimated overall time signature of a track.
        /// </summary>
        [Description("time_signature")]
        public int? TimeSignature { get; set; }

        /// <summary>
        /// A measure from 0.0 to 1.0 describing the musical positiveness conveyed by a track.
        /// </summary>
        [Description("valence")]
        public float? Valence { get; set; }
    }
}
