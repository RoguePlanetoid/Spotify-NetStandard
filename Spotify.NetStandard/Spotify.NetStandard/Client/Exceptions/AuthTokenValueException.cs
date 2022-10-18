namespace Spotify.NetStandard.Client.Exceptions;

/// <summary>
/// Auth Token Value Error
/// </summary>
public class AuthTokenValueException : AuthException
{
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="message">Message</param>
    public AuthTokenValueException(string message) : base(message) { }
}
