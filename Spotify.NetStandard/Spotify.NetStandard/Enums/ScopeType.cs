using System.ComponentModel;

namespace Spotify.NetStandard.Enums
{
    /// <summary>
    /// Scope Type
    /// </summary>
    public enum ScopeType : byte
    {
        // Library
        [Description("user-library-read")]
        LibraryRead,
        [Description("user-library-modify")]
        LibraryModify,
        // Playlists
        [Description("playlist-read-private")]
        PlaylistReadPrivate,
        [Description("playlist-modify-public")]
        PlaylistModifyPublic,
        [Description("playlist-modify-private")]
        PlaylistModifyPrivate,
        [Description("playlist-read-collaborative")]
        PlaylistReadCollaborative,
        // Listening History 
        [Description("user-read-recently-played")]
        ListeningRecentlyPlayed,
        [Description("user-top-read")]
        ListeningTopRead,
        // Users 
        [Description("user-read-private")]
        UserReadPrivate,
        [Description("user-read-email")]
        UserReadEmail,
        [Description("user-read-birthdate")]
        UserReadBirthDate,
        // Playback
        [Description("streaming")]
        PlaybackStreaming,
        [Description("app-remote-control")]
        PlaybackAppRemoteControl,
        // Spotify Connect 
        [Description("user-modify-playback-state")]
        ConnectModifyPlaybackState,
        [Description("user-read-currently-playing")]
        ConnectReadCurrentlyPlaying,
        [Description("user-read-playback-state")]
        ConnectReadPlaybackState,
        // Follow
        [Description("user-follow-modify")]
        FollowModify,
        [Description("user-follow-read")]
        FollowRead
    }
}
