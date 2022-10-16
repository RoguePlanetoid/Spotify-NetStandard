namespace Spotify.NetStandard.Enums;

/// <summary>
/// Repeat State
/// </summary>
public enum RepeatState : byte
{
    /// <summary>
    /// Will repeat the current track
    /// </summary>
    [Description("track")]
    Track,
    /// <summary>
    /// Will repeat the current context.
    /// </summary>
    [Description("context")]
    Context,
    /// <summary>
    /// Will turn repeat off.
    /// </summary>
    [Description("off")]
    Off
}
