namespace Spotify.NetStandard.Requests
{
    public static class ScopeExtensions
    {
        public static Scope AddReadAllAccess(this Scope scope)
        {
            scope.PlaylistReadCollaborative = true;
            scope.PlaylistReadPrivate = true;
            scope.ConnectReadCurrentlyPlaying = true;
            scope.ConnectReadPlaybackState = true;
            scope.ListeningTopRead = true;
            scope.ListeningRecentlyPlayed = true;
            scope.UserReadBirthDate = true;
            scope.UserReadEmail = true;
            scope.UserReadPrivate = true;
            scope.FollowRead = true;
            scope.LibraryRead = true;

            return scope;
        }

        public static Scope AddModifyAllAccess(this Scope scope)
        {
            scope.PlaylistModifyPrivate = true;
            scope.PlaylistModifyPublic = true;
            scope.ConnectModifyPlaybackState = true;
            scope.FollowModify = true;
            scope.LibraryModify = true;

            return scope;
        }

        public static Scope AddPlaylistAll(this Scope scope)
        {
            scope.PlaylistReadPrivate = true;
            scope.PlaylistModifyPrivate = true;
            scope.PlaylistModifyPublic = true;
            scope.PlaylistReadCollaborative = true;

            return scope;
        }

        public static Scope AddSpotifyConnectAll(this Scope scope)
        {
            scope.ConnectModifyPlaybackState = true;
            scope.ConnectReadCurrentlyPlaying = true;
            scope.ConnectReadPlaybackState = true;

            return scope;
        }

        public static Scope AddListeningHistoryAll(this Scope scope)
        {
            scope.ListeningTopRead = true;
            scope.ListeningRecentlyPlayed = true;

            return scope;
        }

        public static Scope AddPlaybackAll(this Scope scope)
        {
            scope.PlaybackAppRemoteControl = true;
            scope.PlaybackStreaming = true;
            return scope;
        }

        public static Scope AddUsersAll(this Scope scope)
        {
            scope.UserReadBirthDate = true;
            scope.UserReadEmail = true;
            scope.UserReadPrivate = true;
            scope.UserGeneratedContentImageUpload = true;

            return scope;
        }

        public static Scope AddFollowAll(this Scope scope)
        {
            scope.FollowRead = true;
            scope.FollowModify = true;

            return scope;
        }

        public static Scope AddLibraryAll(this Scope scope)
        {
            scope.LibraryModify = true;
            scope.LibraryRead = true;

            return scope;
        }
    }
}
