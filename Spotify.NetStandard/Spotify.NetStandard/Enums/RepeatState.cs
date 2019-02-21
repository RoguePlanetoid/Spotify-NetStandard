using System.ComponentModel;

namespace Spotify.NetStandard.Enums
{
    /// <summary>
    /// Repeat State
    /// </summary>
    public enum RepeatState : byte
    {
        [Description("track")]
        Track,
        [Description("context")]
        Context,
        [Description("off")]
        Off,
    }
}
