using System.ComponentModel;

namespace Spotify.NetStandard.Enums
{
    /// <summary>
    /// Time Range
    /// </summary>
    public enum TimeRange : byte
    {
        [Description("long_term")]
        LongTerm,
        [Description("medium_term")]
        MediumTerm,
        [Description("short_term")]
        ShortTerm,
    }
}