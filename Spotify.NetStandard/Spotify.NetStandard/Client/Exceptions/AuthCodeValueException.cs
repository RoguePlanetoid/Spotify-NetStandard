namespace Spotify.NetStandard.Client.Exceptions
{
    /// <summary>
    /// Auth Code Value Error
    /// </summary>
    public class AuthCodeValueException : AuthException
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">Message</param>
        public AuthCodeValueException(string message) : base(message) { }
    }
}
