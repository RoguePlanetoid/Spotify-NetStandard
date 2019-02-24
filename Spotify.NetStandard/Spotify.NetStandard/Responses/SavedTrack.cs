using System.Runtime.Serialization;

namespace Spotify.NetStandard.Responses
{
    /// <summary>
    /// Saved Track Object
    /// </summary>
    [DataContract]
    public class SavedTrack
    {
        /// <summary>
        /// The date and time the track was saved. Timestamps are returned in ISO 8601 format as Coordinated Universal Time (UTC) with a zero offset: YYYY-MM-DDTHH:MM:SSZ. If the time is imprecise (for example, the date/time of an album release), an additional field indicates the precision; see for example, release_date in an album object.
        /// </summary>
        [DataMember(Name = "added_at")]
        public string AddedAt { get; set; }

        /// <summary>
        /// Information about the track.
        /// </summary>
        [DataMember(Name = "track")]
        public Track Track { get; set; }
    }
}
