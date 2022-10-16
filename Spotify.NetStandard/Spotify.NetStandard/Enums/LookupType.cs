namespace Spotify.NetStandard.Enums;

/// <summary>
/// Lookup Type
/// </summary>
public enum LookupType : byte
{
    /// <summary>
    /// Categories
    /// </summary>
    [Description("browse/categories")]
    Categories,
    /// <summary>
    /// Category Playlists
    /// </summary>
    [Description("browse/categories_playlists")]
    CategoriesPlaylists,
    /// <summary>
    /// Artists
    /// </summary>
    [Description("artists")]
    Artists,
    /// <summary>
    /// Albums
    /// </summary>
    [Description("albums")]
    Albums,
    /// <summary>
    /// Tracks
    /// </summary>
    [Description("tracks")]
    Tracks,
    /// <summary>
    /// Album Tracks
    /// </summary>
    [Description("albums_tracks")]
    AlbumTracks,
    /// <summary>
    /// Artist Albums
    /// </summary>
    [Description("artists_albums")]
    ArtistAlbums,
    /// <summary>
    /// Playlists
    /// </summary>
    [Description("playlists")]
    Playlist,
    /// <summary>
    /// Playlist Tracks
    /// </summary>
    [Description("playlists_tracks")]
    PlaylistTracks,
    /// <summary>
    /// Audio Features
    /// </summary>
    [Description("audio-features")]
    AudioFeatures,
    /// <summary>
    /// Audio Analysis
    /// </summary>
    [Description("audio-analysis")]
    AudioAnalysis,
    /// <summary>
    /// Episodes
    /// </summary>
    [Description("episodes")]
    Episodes,
    /// <summary>
    /// Shows
    /// </summary>
    [Description("shows")]
    Shows,
    /// <summary>
    /// Show Episodes
    /// </summary>
    [Description("shows_episodes")]
    ShowEpisodes,
    /// <summary>
    /// Audiobooks
    /// </summary>
    [Description("audiobooks")]
    Audiobooks,
    /// <summary>
    /// Audiobook Chapters
    /// </summary>
    [Description("audiobooks_chapters")]
    AudiobookChapters,
    /// <summary>
    /// Chapters
    /// </summary>
    [Description("chapters")]
    Chapters
}
