using System.Runtime.Serialization;

namespace Spotify.NetStandard.Responses
{
    /// <summary>
    /// Simplified Currently Playing Object
    /// </summary>
    [DataContract]
    public class SimplifiedCurrentlyPlaying : BaseResponse
    {
        /// <summary>
        /// A Context Object. Can be null
        /// </summary>
        [DataMember(Name = "context")]
        public Context Context { get; set; }

        /// <summary>
        /// Unix Millisecond Timestamp when data was fetched
        /// </summary>
        [DataMember(Name = "timestamp")]
        public long TimeStamp { get; set; }

        /// <summary>
        /// Progress into the currently playing track. Can be null.
        /// </summary>
        [DataMember(Name = "progress_ms")]
        public long? Progress { get; set; }

        /// <summary>
        /// If something is currently playing, return true.
        /// </summary>
        [DataMember(Name = "is_playing")]
        public bool IsPlaying { get; set; }

        /// <summary>
        /// The currently playing track. Can be null.
        /// </summary>
        [DataMember(Name = "item")]
        public Track Track { get; set; }

        /// <summary>
        /// The object type of the currently playing item. Can be one of track, episode, ad or unknown.
        /// </summary>
        [DataMember(Name = "currently_playing_type")]
        public string Type { get; set; }
    }
}
