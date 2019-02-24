using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Spotify.NetStandard.Requests
{
    /// <summary>
    /// Playback Request Object
    /// </summary>
    [DataContract]
    public class PlaybackRequest
    {
        /// <summary>
        /// (Optional) Spotify URI of the context to play. Valid contexts are albums, artists, playlists. Example: spotify:album:1Je1IMUlBXcx1Fz0WE7oPT
        /// </summary>
        [DataMember(Name = "context_uri")]
        public string ContextUri { get; set; }

        /// <summary>
        /// (Optional) A JSON array of the Spotify track URIs to play. Example: spotify:track:4iV5W9uYEdYUVa79Axb7Rh, spotify:track:1301WleyT98MSxVHPZCA6M
        /// </summary>
        [DataMember(Name = "uris")]
        public List<string> Uris { get; set; }

        /// <summary>
        /// (Optional) Indicates from where in the context playback should start. Only available when ContextUri corresponds to an album or playlist object, or when the uris parameter is used. “position” is zero based and can’t be negative. Example: PositionRequest with Position = 5 or a UriRequest with Uri representing the uri of the item to start at. Example: UriRequest with Uri = "spotify:track:1301WleyT98MSxVHPZCA6M"
        /// </summary>
        [DataMember(Name = "offset")]
        public object Offset { get; set; }

        /// <summary>
        /// (Optional) Indicates from what position to start playback. Must be a positive number. Passing in a position that is greater than the length of the track will cause the player to start playing the next song.
        /// </summary>
        [DataMember(Name = "position_ms")]
        public long? Position { get; set; }
    }
}
