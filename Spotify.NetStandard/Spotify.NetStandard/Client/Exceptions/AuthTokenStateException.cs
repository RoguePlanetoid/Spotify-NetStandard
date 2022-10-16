namespace Spotify.NetStandard.Client.Exceptions;

/// <summary>
/// Auth Token State Error
/// </summary>
public class AuthTokenStateException : AuthException
{
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="message">Message</param>
    public AuthTokenStateException(string message) : base(message) { }
}
