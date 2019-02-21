using System.Runtime.Serialization;

namespace Spotify.NetStandard.Responses
{
    /// <summary>
    /// Play History Object
    /// </summary>
    [DataContract]
    public class PlayHistory
    {
        /// <summary>
        /// The track the user listened to.
        /// </summary>
        [DataMember(Name = "track")]
        public SimplifiedTrack Track { get; set; }

        /// <summary>
        /// The date and time the track was played. Format yyyy-MM-ddTHH:mm:ss
        /// </summary>
        [DataMember(Name = "played_at")]
        public string PlayedAt { get; set; }

        /// <summary>
        /// The context the track was played from.
        /// </summary>
        [DataMember(Name = "context")]
        public Context Context { get; set; }
    }
}
