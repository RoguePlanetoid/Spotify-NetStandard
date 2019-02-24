using System.Runtime.Serialization;

namespace Spotify.NetStandard.Responses
{
    /// <summary>
    /// Currently Playing Object
    /// </summary>
    [DataContract]
    public class CurrentlyPlaying : SimplifiedCurrentlyPlaying
    {
        /// <summary>
        /// The device that is currently active
        /// </summary>
        [DataMember(Name = "device")]
        public Device Device { get; set; }

        /// <summary>
        /// off, track, context
        /// </summary>
        [DataMember(Name = "repeat_state")]
        public string RepeatState { get; set; }

        /// <summary>
        /// If shuffle is on or off
        /// </summary>
        [DataMember(Name = "shuffle_state")]
        public bool SuffleState { get; set; }
    }
}
