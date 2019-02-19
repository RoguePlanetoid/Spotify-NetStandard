using System.Runtime.Serialization;

namespace Spotify.NetStandard.Responses.Internal
{
    /// <summary>
    /// Content Cursor Response
    /// </summary>
    internal class ContentCursorResponse
    {
        /// <summary>
        /// Cursor Paging Object of Artist
        /// </summary>
        [DataMember(Name = "artist")]
        public CursorPaging<Artist> Artists { get; set; }
    }
}
