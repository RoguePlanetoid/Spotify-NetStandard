using System.ComponentModel;

namespace Spotify.NetStandard.Enums
{
    /// <summary>
    /// Search Type
    /// </summary>
    public enum SearchType : byte
    {
        [Description("album")]
        Album,
        [Description("artist")]
        Artist,
        [Description("playlist")]
        Playlist,
        [Description("track")]
        Track
    }
}
