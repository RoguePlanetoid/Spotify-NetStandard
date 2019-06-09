using System;

namespace Spotify.NetStandard.Client.Exceptions
{
    /// <summary>
    /// Auth Exception
    /// </summary>
    public class AuthException : Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public AuthException() : base() { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">Error Message</param>
        public AuthException(string message) : base(message) { }
    }
}
