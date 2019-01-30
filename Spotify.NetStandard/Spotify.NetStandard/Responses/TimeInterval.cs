using System.Runtime.Serialization;

namespace Spotify.NetStandard.Responses
{
    /// <summary>
    /// Time Interval Object
    /// </summary>
    [DataContract]
    public class TimeInterval
    {
        /// <summary>
        /// The starting point in seconds.
        /// </summary>
        [DataMember(Name = "start")]
        public float? Start { get; set; }

        /// <summary>
        /// The duration in seconds
        /// </summary>
        [DataMember(Name = "duration")]
        public float? Duration { get; set; }

        /// <summary>
        /// The reliability confidence, from 0.0 to 1.0
        /// </summary>
        [DataMember(Name = "confidence")]
        public float? Confidence { get; set; }
    }
}
