namespace Spotify.NetStandard.Enums;

/// <summary>
/// Time Range
/// </summary>
public enum TimeRange : byte
{
    /// <summary>
    /// Calculated from several years of data and including all new data as it becomes available
    /// </summary>
    [Description("long_term")]
    LongTerm,
    /// <summary>
    /// Approximately last 6 months
    /// </summary>
    [Description("medium_term")]
    MediumTerm,
    /// <summary>
    /// Approximately last 4 weeks
    /// </summary>
    [Description("short_term")]
    ShortTerm
}