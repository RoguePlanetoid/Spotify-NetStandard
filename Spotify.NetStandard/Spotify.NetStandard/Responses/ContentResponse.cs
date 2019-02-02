using System.Runtime.Serialization;

namespace Spotify.NetStandard.Responses
{
    /// <summary>
    /// Content Response
    /// </summary>
    [DataContract]
    public class ContentResponse : BaseResponse
    {
        /// <summary>
        /// Paging Object of Albums
        /// </summary>
        [DataMember(Name = "albums")]
        public Paging<Album> Albums { get; set; }

        /// <summary>
        /// Paging Object of Category
        /// </summary>
        [DataMember(Name = "categories")]
        public Paging<Category> Categories { get; set; }

        /// <summary>
        /// Paging Object of Artists
        /// </summary>
        [DataMember(Name = "artists")]
        public Paging<Artist> Artists { get; set; }

        /// <summary>
        /// Paging Object of Playlists
        /// </summary>
        [DataMember(Name = "playlists")]
        public Paging<Playlist> Playlists { get; set; }

        /// <summary>
        /// Paging Object of Tracks
        /// </summary>
        [DataMember(Name = "tracks")]
        public Paging<Track> Tracks { get; set; }
    }
}
