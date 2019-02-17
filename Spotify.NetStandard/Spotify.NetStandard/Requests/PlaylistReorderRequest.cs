using System.Runtime.Serialization;

namespace Spotify.NetStandard.Requests
{
    /// <summary>
    /// Playlist Reorder Request Object
    /// </summary>
    [DataContract]
    public class PlaylistReorderRequest
    {
        /// <summary>
        /// (Required) The position of the first track to be reordered. 
        /// </summary>
        [DataMember(Name = "range_start")]
        public int RangeStart { get; set; }

        /// <summary>
        /// (Optional) The position where the tracks should be inserted. To reorder the tracks to the end of the playlist, simply set insert_before to the position after the last track.
        /// </summary>
        [DataMember(Name = "insert_before")]
        public int? InsertBefore { get; set; }

        /// <summary>
        /// (Optional) The amount of tracks to be reordered. Defaults to 1 if not set. The range of tracks to be reordered begins from the RangeStart position, and includes the RangeLength subsequent tracks.
        /// </summary>
        [DataMember(Name = "range_length")]
        public int? RangeLength { get; set; }

        /// <summary>
        /// The playlist’s snapshot ID against which you want to make the changes.
        /// </summary>
        [DataMember(Name = "snapshot_id")]
        public string SnapshotId { get; set; }
    }
}
