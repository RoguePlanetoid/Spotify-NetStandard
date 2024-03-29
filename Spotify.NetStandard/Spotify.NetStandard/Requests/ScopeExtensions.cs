﻿namespace Spotify.NetStandard.Requests;

/// <summary>
/// Extension Methods for Scope Class to allow fluent additions of scopes for the API
/// </summary>
public static class ScopeExtensions
{
    /// <summary>
    /// Extension method to add no scopes
    /// </summary>
    /// <param name="scope">Scope</param>
    /// <returns>Scope</returns>
    public static Scope AddNone(this Scope scope)
    {
        scope.PlaylistReadCollaborative = false;
        scope.PlaylistReadPrivate = false;
        scope.ConnectReadCurrentlyPlaying = false;
        scope.ConnectReadPlaybackState = false;
        scope.ListeningTopRead = false;
        scope.PlaybackPositionRead = false;
        scope.ListeningRecentlyPlayed = false;
        scope.UserReadEmail = false;
        scope.UserReadPrivate = false;
        scope.FollowRead = false;
        scope.LibraryRead = false;

        scope.PlaylistModifyPrivate = false;
        scope.PlaylistModifyPublic = false;
        scope.ConnectModifyPlaybackState = false;
        scope.FollowModify = false;
        scope.LibraryModify = false;

        scope.PlaylistReadPrivate = false;
        scope.PlaylistModifyPrivate = false;
        scope.PlaylistModifyPublic = false;
        scope.PlaylistReadCollaborative = false;

        scope.ConnectModifyPlaybackState = false;
        scope.ConnectReadCurrentlyPlaying = false;
        scope.ConnectReadPlaybackState = false;

        scope.ListeningTopRead = false;
        scope.PlaybackPositionRead = false;
        scope.ListeningRecentlyPlayed = false;

        scope.PlaybackAppRemoteControl = false;
        scope.PlaybackStreaming = false;

        scope.UserReadEmail = false;
        scope.UserReadPrivate = false;
        scope.UserGeneratedContentImageUpload = false;

        scope.FollowRead = false;
        scope.FollowModify = false;

        scope.LibraryModify = false;
        scope.LibraryRead = false;
        return scope;
    }

    /// <summary>
    /// Extension method to add all scopes with "read" in their scope string
    /// </summary>
    /// <param name="scope">Scope</param>
    /// <returns>Scope</returns>
    public static Scope AddReadAllAccess(this Scope scope)
    {
        scope.PlaylistReadCollaborative = true;
        scope.PlaylistReadPrivate = true;
        scope.ConnectReadCurrentlyPlaying = true;
        scope.ConnectReadPlaybackState = true;
        scope.ListeningTopRead = true;
        scope.PlaybackPositionRead = true;
        scope.ListeningRecentlyPlayed = true;
        scope.UserReadEmail = true;
        scope.UserReadPrivate = true;
        scope.FollowRead = true;
        scope.LibraryRead = true;
        return scope;
    }

    /// <summary>
    /// Extension method to add all scopes with "modify" in their scope string
    /// </summary>
    /// <param name="scope">Scope</param>
    /// <returns>Scope</returns>
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
    /// <param name="scope">Scope</param>
    /// <returns>Scope</returns>
    public static Scope AddPlaylistAll(this Scope scope)
    {
        scope.PlaylistReadPrivate = true;
        scope.PlaylistModifyPrivate = true;
        scope.PlaylistModifyPublic = true;
        scope.PlaylistReadCollaborative = true;
        return scope;
    }

    /// <summary>
    /// Adds all scopes to a scope within the Spotify Connect section of the defined Scopes
    /// </summary>
    /// <param name="scope">Scope</param>
    /// <returns>Scope</returns>
    public static Scope AddSpotifyConnectAll(this Scope scope)
    {
        scope.ConnectModifyPlaybackState = true;
        scope.ConnectReadCurrentlyPlaying = true;
        scope.ConnectReadPlaybackState = true;
        return scope;
    }

    /// <summary>
    /// Adds all scopes to a scope within the Listening History section of the defined Scopes
    /// </summary>
    /// <param name="scope">Scope</param>
    /// <returns>Scope</returns>
    public static Scope AddListeningHistoryAll(this Scope scope)
    {
        scope.ListeningTopRead = true;
        scope.PlaybackPositionRead = true;
        scope.ListeningRecentlyPlayed = true;

        return scope;
    }

    /// <summary>
    /// Adds all scopes to a scope within the Playback section of the defined Scopes
    /// </summary>
    /// <param name="scope">Scope</param>
    /// <returns>Scope</returns>
    public static Scope AddPlaybackAll(this Scope scope)
    {
        scope.PlaybackAppRemoteControl = true;
        scope.PlaybackStreaming = true;
        return scope;
    }

    /// <summary>
    /// Adds all scopes to a scope within the Users section of the defined Scopes
    /// </summary>
    /// <param name="scope">Scope</param>
    /// <returns>Scope</returns>
    public static Scope AddUsersAll(this Scope scope)
    {
        scope.UserReadEmail = true;
        scope.UserReadPrivate = true;
        scope.UserGeneratedContentImageUpload = true;
        return scope;
    }

    /// <summary>
    /// Adds all scopes to a scope within the Follow section of the defined Scopes
    /// </summary>
    /// <param name="scope">Scope</param>
    /// <returns>Scope</returns>
    public static Scope AddFollowAll(this Scope scope)
    {
        scope.FollowRead = true;
        scope.FollowModify = true;
        return scope;
    }

    /// <summary>
    /// Adds all scopes to a scope within the Library section of the defined Scopes
    /// </summary>
    /// <param name="scope">Scope</param>
    /// <returns>Scope</returns>
    public static Scope AddLibraryAll(this Scope scope)
    {
        scope.LibraryModify = true;
        scope.LibraryRead = true;
        return scope;
    }
}
