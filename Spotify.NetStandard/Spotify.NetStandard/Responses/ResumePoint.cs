using System.Runtime.Serialization;

namespace Spotify.NetStandard.Responses
{
    /// <summary>
    /// Resume Point Object
    /// </summary>
    [DataContract]
    public class ResumePoint
    {
        /// <summary>
        /// Whether or not the episode has been fully played by the user
        /// </summary>
        [DataMember(Name = "fully_played")]
        public bool FullyPlayed { get; set; }

        /// <summary>
        /// The user’s most recent position in the episode in milliseconds
        /// </summary>
        [DataMember(Name = "resume_position_ms")]
        public long ResumePosition { get; set; }
    }
}
