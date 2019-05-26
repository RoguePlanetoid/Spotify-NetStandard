namespace Spotify.NetStandard.Requests
{
    /// <summary>
    /// Extension Methods for Scope Class to allow fluent additions of scopes for the API
    /// </summary>
    public static class ScopeExtensions
    {
        /// <summary>
        /// Extension method to add all scopes with "read" in their scope string
        /// </summary>
        /// <param name="scope"></param>
        /// <returns></returns>
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

        /// <summary>
        /// /// Extension method to add all scopes with "modify" in their scope string
        /// </summary>
        /// <param name="scope"></param>
        /// <returns></returns>
        public static Scope AddModifyAllAccess(this Scope scope)
        {
            scope.PlaylistModifyPrivate = true;
            scope.PlaylistModifyPublic = true;
            scope.ConnectModifyPlaybackState = true;
            scope.FollowModify = true;
            scope.LibraryModify = true;

            return scope;
        }

        /// <summary>
        /// Adds all scopes to a scope within the Playlist section of the defined Scopes
        /// </summary>
        /// <param name="scope"></param>
        /// <returns></returns>
        public static Scope AddPlaylistAll(this Scope scope)
        {
            scope.PlaylistReadPrivate = true;
            scope.PlaylistModifyPrivate = true;
            scope.PlaylistModifyPublic = true;
            scope.PlaylistReadCollaborative = true;

            return scope;
        }

        /// <summary>
        /// /// Adds all scopes to a scope within the Spotify Connect section of the defined Scopes
        /// </summary>
        /// <param name="scope"></param>
        /// <returns></returns>
        public static Scope AddSpotifyConnectAll(this Scope scope)
        {
            scope.ConnectModifyPlaybackState = true;
            scope.ConnectReadCurrentlyPlaying = true;
            scope.ConnectReadPlaybackState = true;

            return scope;
        }

        /// <summary>
        /// /// Adds all scopes to a scope within the Listening History section of the defined Scopes
        /// </summary>
        /// <param name="scope"></param>
        /// <returns></returns>
        public static Scope AddListeningHistoryAll(this Scope scope)
        {
            scope.ListeningTopRead = true;
            scope.ListeningRecentlyPlayed = true;

            return scope;
        }

        /// <summary>
        /// /// Adds all scopes to a scope within the Playback section of the defined Scopes
        /// </summary>
        /// <param name="scope"></param>
        /// <returns></returns>
        public static Scope AddPlaybackAll(this Scope scope)
        {
            scope.PlaybackAppRemoteControl = true;
            scope.PlaybackStreaming = true;
            return scope;
        }

        /// <summary>
        /// /// Adds all scopes to a scope within the Users section of the defined Scopes
        /// </summary>
        /// <param name="scope"></param>
        /// <returns></returns>
        public static Scope AddUsersAll(this Scope scope)
        {
            scope.UserReadBirthDate = true;
            scope.UserReadEmail = true;
            scope.UserReadPrivate = true;
            scope.UserGeneratedContentImageUpload = true;

            return scope;
        }

        /// <summary>
        /// /// Adds all scopes to a scope within the Follow section of the defined Scopes
        /// </summary>
        /// <param name="scope"></param>
        /// <returns></returns>
        public static Scope AddFollowAll(this Scope scope)
        {
            scope.FollowRead = true;
            scope.FollowModify = true;

            return scope;
        }

        /// <summary>
        /// /// Adds all scopes to a scope within the Library section of the defined Scopes
        /// </summary>
        /// <param name="scope"></param>
        /// <returns></returns>
        public static Scope AddLibraryAll(this Scope scope)
        {
            scope.LibraryModify = true;
            scope.LibraryRead = true;

            return scope;
        }
    }
}
