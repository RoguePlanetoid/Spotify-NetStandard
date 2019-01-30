using System;
using System.Collections.Generic;

namespace Spotify.NetStandard.Client.Authentication.Internal
{
    /// <summary>
    /// Access Code
    /// </summary>
    internal class AccessCode
    {
        private const string error = "error";
        private const string state = "state";
        private const string code = "code";

        /// <summary>
        /// An authorization code that can be exchanged for an access token.
        /// </summary>
        public string Code { get; set; }

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
        /// <param name="queryDict">QueryString Dictionary</param>
        public AccessCode(Uri responseUri, Uri redirectUri, Dictionary<string, string> queryDict)
        {
            try
            {
                ResponseUri = responseUri;
                RedirectUri = redirectUri;
                if (queryDict != null)
                {
                    if (queryDict.ContainsKey(code)) Code = queryDict[code];
                    if (queryDict.ContainsKey(state)) State = queryDict[state];
                    if (queryDict.ContainsKey(error)) Error = queryDict[error];
                }
            }
            finally { }
        }
    }
}
