namespace Spotify.NetStandard.Client.Exceptions;

/// <summary>
/// Auth Code State Error
/// </summary>
public class AuthCodeStateException : AuthException
{
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="message">Message</param>
    public AuthCodeStateException(string message) : base(message) { }
}
