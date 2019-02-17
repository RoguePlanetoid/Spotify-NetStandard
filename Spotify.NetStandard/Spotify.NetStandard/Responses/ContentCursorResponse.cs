using System.Runtime.Serialization;

namespace Spotify.NetStandard.Responses
{
    /// <summary>
    /// Content Cursor Response
    /// </summary>
    public class ContentCursorResponse
    {
        /// <summary>
        /// Cursor Paging Object of Artist
        /// </summary>
        [DataMember(Name = "artist")]
        public CursorPaging<Artist> Artists { get; set; }
    }
}
