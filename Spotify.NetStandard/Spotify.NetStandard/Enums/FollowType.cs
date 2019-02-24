using System.ComponentModel;

namespace Spotify.NetStandard.Enums
{
    /// <summary>
    /// Follow Type
    /// </summary>
    public enum FollowType : byte
    {
        [Description("user")]
        User,
        [Description("artist")]
        Artist,
    }
}