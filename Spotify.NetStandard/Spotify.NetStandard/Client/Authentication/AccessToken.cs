using Spotify.NetStandard.Client.Authentication.Enums;
using System;
using System.Runtime.Serialization;

namespace Spotify.NetStandard.Client.Authentication
{
    /// <summary>
    /// Access Token Object
    /// </summary>
    [DataContract]
    public class AccessToken
    {
        /// <summary>
        /// Access Token
        /// </summary>
        [DataMember(Name = "token")]
        public string Token { get; set; }

        /// <summary>
        /// Refresh
        /// </summary>
        [DataMember(Name = "refresh")]
        public string Refresh { get; set; }

        /// <summary>
        /// Token Expiration Date
        /// </summary>
        [DataMember(Name = "expires")]
        public DateTime Expiration { get; set; }

        /// <summary>
        /// Token Type
        /// </summary>
        [DataMember(Name = "type")]
        public TokenType TokenType { get; set; }

        /// <summary>
        /// Scopes
        /// </summary>
        [DataMember(Name = "scopes")]
        public string Scopes { get; set; }

        /// <summary>
        /// Error
        /// </summary>
        [DataMember(Name = "error")]
        public string Error { get; set; }
    }
}
