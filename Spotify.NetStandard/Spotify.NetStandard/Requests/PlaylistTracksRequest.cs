using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Spotify.NetStandard.Requests
{
    /// <summary>
    /// Playlist Tracks Request Object
    /// </summary>
    [DataContract]
    public class PlaylistTracksRequest
    {
        /// <summary>
        /// Spotify URIs of Tracks
        /// </summary>
        [DataMember(Name = "tracks")]
        public List<UriRequest> Tracks { get; set; }

        /// <summary>
        /// The playlist’s snapshot ID against which you want to make the changes
        /// </summary>
        [DataMember(Name = "snapshot_id")]
        public string SnapshotId { get; set; }
    }
}
