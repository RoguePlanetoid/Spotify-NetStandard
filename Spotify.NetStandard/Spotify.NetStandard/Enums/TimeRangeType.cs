using System.ComponentModel;

namespace Spotify.NetStandard.Enums
{
    /// <summary>
    /// Follow Type
    /// </summary>
    public enum TimeRangeType : byte
    {
        [Description("long_term")]
        LongTerm,
        [Description("medium_term")]
        MediumTerm,
        [Description("short_term")]
        ShortTerm,
    }
}