using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Spotify.NetStandard.Responses
{
    /// <summary>
    /// Simplified Playlist Object
    /// </summary>
    [DataContract]
    public class SimplifiedPlaylist : Content
    {
        /// <summary>
        /// Returns true if the owner allows other users to modify the playlist.
        /// </summary>
        [DataMember(Name = "collaborative")]
        public bool Collaborative { get; set; }

        /// <summary>
        /// Images for the playlist. The array may be empty or contain up to three images. The images are returned by size in descending order.
        /// </summary>
        [DataMember(Name = "images")]
        public List<Image> Images { get; set; }

        /// <summary>
        /// The user who owns the playlist
        /// </summary>
        [DataMember(Name = "owner")]
        public PublicUser Owner { get; set; }

        /// <summary>
        /// The playlist’s public/private status: true the playlist is public, false the playlist is private, null the playlist status is not relevant
        /// </summary>
        [DataMember(Name = "public")]
        public bool? Public { get; set; }

        /// <summary>
        /// The version identifier for the current playlist.
        /// </summary>
        [DataMember(Name = "snapshot_id")]
        public string SnapshotId { get; set; }

        /// <summary>
        /// Information about the tracks of the playlist.
        /// </summary>
        [DataMember(Name = "tracks")]
        public Paging<PlaylistTrack> Tracks { get; set; }
    }
}
