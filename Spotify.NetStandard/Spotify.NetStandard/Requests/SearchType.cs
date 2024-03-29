﻿namespace Spotify.NetStandard.Requests;

/// <summary>
/// Search Type
/// </summary>
public class SearchType
{
    /// <summary>
    /// Album
    /// </summary>
    [Description("album")]
    public bool? Album { get; set; }

    /// <summary>
    /// Artist
    /// </summary>
    [Description("artist")]
    public bool? Artist { get; set; }

    /// <summary>
    /// Playlist
    /// </summary>
    [Description("playlist")]
    public bool? Playlist { get; set; }

    /// <summary>
    /// Track
    /// </summary>
    [Description("track")]
    public bool? Track { get; set; }

    /// <summary>
    /// Show
    /// </summary>
    [Description("show")]
    public bool? Show { get; set; }

    /// <summary>
    /// Episode
    /// </summary>
    [Description("episode")]
    public bool? Episode { get; set; }

    /// <summary>
    /// Audiobook
    /// </summary>
    [Description("audiobook")]
    public bool? Audiobook { get; set; }
}
