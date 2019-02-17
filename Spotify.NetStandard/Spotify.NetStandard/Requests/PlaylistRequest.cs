using System.Runtime.Serialization;

namespace Spotify.NetStandard.Requests
{
    /// <summary>
    /// Playlist Request Object
    /// </summary>
    [DataContract]
    public class PlaylistRequest
    {
        /// <summary>
        /// The new name for the playlist, for example "My New Playlist Title" 
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; set; }

        /// <summary>
        /// If true the playlist will be public, if false it will be private. 
        /// </summary>
        [DataMember(Name = "public")]
        public bool? IsPublic { get; set; }

        /// <summary>
        /// If true , the playlist will become collaborative and other users will be able to modify the playlist in their Spotify client. Note: You can only set collaborative to true on non-public playlists. 
        /// </summary>
        [DataMember(Name = "collaborative")]
        public bool? IsCollaborative { get; set; }

        /// <summary>
        /// Value for playlist description as displayed in Spotify Clients and in the Web API. 
        /// </summary>
        [DataMember(Name = "description")]
        public string Description { get; set; }
    }
}
