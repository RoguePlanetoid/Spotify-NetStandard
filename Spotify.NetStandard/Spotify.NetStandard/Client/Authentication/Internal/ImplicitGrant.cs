using System;
using System.Collections.Generic;

namespace Spotify.NetStandard.Client.Authentication.Internal
{
    /// <summary>
    /// Implicit Grant
    /// </summary>
    internal class ImplicitGrant
    {
        private const string error = "error";
        private const string state = "state";
        private const string access_token = "access_token";
        private const string token_type = "token_type";
        private const string expires_in = "expires_in";

        /// <summary>
        /// An access token that can be provided in subsequent calls, for example to Spotify Web API services.
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// Value: “Bearer”
        /// </summary>
        public string TokenType { get; set; }

        /// <summary>
        /// The time period (in seconds) for which the access token is valid.
        /// </summary>
        public string ExpiresIn { get; set; }

        /// <summary>
        /// The value of the state parameter supplied in the request.
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// The reason authorization failed, for example: “access_denied”
        /// </summary>
        public string Error { get; set; }

        /// <summary>
        /// An authorization Uri
        /// </summary>
        public Uri ResponseUri { get; set; }

        /// <summary>
        /// Redirect Uri
        /// </summary>
        public Uri RedirectUri { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="responseUri">An authorization Uri</param>
        /// <param name="redirectUri">Redirect Uri</param>
        /// <param name="dictionary">QueryString Dictionary</param>
        public ImplicitGrant(
            Uri responseUri, 
            Uri redirectUri, 
            Dictionary<string, string> dictionary)
        {
            try
            {
                ResponseUri = responseUri;
                RedirectUri = redirectUri;
                if (dictionary != null)
                {
                    if (dictionary.ContainsKey(access_token)) AccessToken = dictionary[access_token];
                    if (dictionary.ContainsKey(token_type)) TokenType = dictionary[token_type];
                    if (dictionary.ContainsKey(expires_in)) ExpiresIn = dictionary[expires_in];
                    if (dictionary.ContainsKey(state)) State = dictionary[state];
                    if (dictionary.ContainsKey(error)) Error = dictionary[error];
                }
            }
            finally { }
        }
    }
}
