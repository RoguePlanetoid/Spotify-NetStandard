using System.ComponentModel;

namespace Spotify.NetStandard.Enums
{
    /// <summary>
    /// Follow Type
    /// </summary>
    public enum FollowType : byte
    {
        /// <summary>
        /// User
        /// </summary>
        [Description("user")]
        User,
        /// <summary>
        /// Artist
        /// </summary>
        [Description("artist")]
        Artist
    }
}