using Spotify.NetStandard.Client.Internal;
using System;
using System.Runtime.Serialization;

namespace Spotify.NetStandard.Responses
{
    /// <summary>
    /// Playlist Track Object
    /// </summary>
    [DataContract]
    public class PlaylistTrack : BaseResponse
    {
        /// <summary>
        /// The date and time the track was added.
        /// </summary>
        [DataMember(Name = "added_at")]
        public DateTime? AddedAt { get; set; }

        /// <summary>
        /// The Spotify user who added the track.
        /// </summary>
        [DataMember(Name = "added_by")]
        public PublicUser AddedBy { get; set; }

        /// <summary>
        /// Whether this track is a local file or not.
        /// </summary>
        [DataMember(Name = "is_local")]
        public bool IsLocal { get; set; }

        /// <summary>
        /// Information about the track or episode.
        /// </summary>
        [DataMember(Name = "track")]
        public object Item { get; set; }

        /// <summary>
        /// Information about the track
        /// </summary>
        [IgnoreDataMember]
        public Track Track => Item.AsType<Track>();

        /// <summary>
        /// Information about the episode
        /// </summary>
        [IgnoreDataMember]
        public Episode Episode => Item.AsType<Episode>();
    }
}
