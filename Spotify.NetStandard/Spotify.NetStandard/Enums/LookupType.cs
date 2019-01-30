using System.ComponentModel;

namespace Spotify.NetStandard.Enums
{
    /// <summary>
    /// Lookup Type
    /// </summary>
    public enum LookupType : byte
    {
        [Description("browse/categories")]
        Categories,
        [Description("browse/categories_playlists")]
        CategoriesPlaylists,
        [Description("artists")]
        Artists,
        [Description("albums")]
        Albums,
        [Description("tracks")]
        Tracks,
        [Description("albums_tracks")]
        AlbumTracks,
        [Description("artists_albums")]
        ArtistAlbums,
        [Description("playlists_tracks")]
        PlaylistTracks,
        [Description("audio-features")]
        AudioFeatures,
        [Description("audio-analysis")]
        AudioAnalysis
    }
}
