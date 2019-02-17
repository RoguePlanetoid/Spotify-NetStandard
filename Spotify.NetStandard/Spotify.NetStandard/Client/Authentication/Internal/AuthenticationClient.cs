using Spotify.NetStandard.Client.Internal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Spotify.NetStandard.Client.Authentication.Internal
{
    /// <summary>
    /// Authentication Client
    /// </summary>
    internal class AuthenticationClient : SimpleServiceClient
    {
        // Token Auth
        private const string auth_header = "Authorization";
        private const string auth_basic = "Basic";
        private const string grant_type = "grant_type";
        private const string client_credentials = "client_credentials";
        private const string token_uri = "/api/token";
        // Get Auth
        private const string response_type = "response_type";
        private const string code_value = "code";
        private const string client_id = "client_id";
        private const string scope_value = "scope";
        private const string state_value = "state";
        private const string redirect_uri = "redirect_uri";
        private const string auth_uri = "/authorize";
        // Url Auth
        private const string authorization_code = "authorization_code";
        // Endpoint
        private readonly Uri host_name = new Uri("https://accounts.spotify.com");

        /// <summary>
        /// Authenticate
        /// </summary>
        /// <param name="clientId">Client Id</param>
        /// <param name="clientSecret">Client Secret</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>Authentication Response</returns>
        public Task<AuthenticationResponse> AuthenticateAsync(
            string clientId, 
            string clientSecret, 
            CancellationToken cancellationToken)
        {
            string auth = Convert.ToBase64String(
            Encoding.ASCII.GetBytes($"{clientId}:{clientSecret}"));
            Dictionary<string, string> request = new Dictionary<string, string>()
            {
                { grant_type, client_credentials }
            };
            Dictionary<string, string> headers = new Dictionary<string, string>()
            {
                { auth_header, $"{auth_basic} {auth}"}
            };

            return PostRequestAsync<Dictionary<string, string>, AuthenticationResponse>(host_name,
                token_uri, null, cancellationToken, request, null, headers, true);
        }

        /// <summary>
        /// Authenticate
        /// </summary>
        /// <param name="clientId">Client Id</param>
        /// <param name="clientSecret">Client Secret</param>
        /// <param name="accessCode">Access Code</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>Authentication Response</returns>
        public async Task<AuthenticationResponse> AuthenticateAsync(
            string clientId, 
            string clientSecret, 
            AccessCode accessCode, 
            CancellationToken cancellationToken)
        {
            string auth = Convert.ToBase64String(
            Encoding.ASCII.GetBytes($"{clientId}:{clientSecret}"));
            Dictionary<string, string> request = new Dictionary<string, string>()
            {
                { grant_type, authorization_code },
                { code_value, accessCode.Code },
                { redirect_uri, accessCode.RedirectUri.ToString() }
            };
            Dictionary<string, string> headers = new Dictionary<string, string>()
            {
                { auth_header, $"{auth_basic} {auth}"}
            };
            return await PostRequestAsync<Dictionary<string, string>, AuthenticationResponse>(host_name,
                token_uri, null, cancellationToken, request, null, headers, true);
        }

        /// <summary>
        /// Authenticate
        /// </summary>
        /// <param name="clientId">Client Id</param>
        /// <param name="scopes">Comma delimited scopes</param>
        /// <param name="state">State</param>
        /// <param name="redirectUrl">Redirect Url</param>
        /// <returns>Url</returns>
        public Uri Authenticate(
            string clientId, 
            string scopes, 
            string state, 
            string redirectUrl)
        {
            Dictionary<string, string> request = new Dictionary<string, string>()
            {
                { response_type, code_value },
                { client_id, clientId },
                { scope_value, scopes },
                { state_value, HttpUtility.UrlEncode(state) },
                { redirect_uri, HttpUtility.UrlEncode(redirectUrl) }
            };
            return GetUri(host_name, auth_uri, request);
        }
    }
}
