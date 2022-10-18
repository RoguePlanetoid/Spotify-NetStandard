namespace Spotify.NetStandard.Client.Authentication.Internal
{
    /// <summary>
    /// Authentication Client
    /// </summary>
    internal class AuthenticationClient : SimpleServiceClient
    {
        // Endpoint
        private readonly Uri host_name = new("https://accounts.spotify.com");
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
        private const string token_value = "token";
        private const string redirect_uri = "redirect_uri";
        private const string auth_uri = "/authorize";
        // Url Auth
        private const string authorization_code = "authorization_code";
        private const string refresh_token = "refresh_token";
        private const string show_dialog = "show_dialog";
        // PKCE Auth
        private const string code_challenge_method_type = "S256";
        private const string code_challenge_method = "code_challenge_method";
        private const string code_challenge = "code_challenge";
        private const string code_verifier = "code_verifier";

        /// <summary>
        /// Get Headers
        /// </summary>
        /// <param name="clientId">Client Id</param>
        /// <param name="clientSecret">Client Secret</param>
        /// <returns>Dictionary Of String, String</returns>
        private Dictionary<string, string> GetHeaders(
            string clientId,
            string clientSecret)
        {
            string auth = Convert.ToBase64String(
                Encoding.ASCII.GetBytes($"{clientId}:{clientSecret}"));
            Dictionary<string, string> headers = new()
            {
                { auth_header, $"{auth_basic} {auth}"}
            };
            return headers;
        }

        /// <summary>
        /// Client Credentials - Client Credentials Flow
        /// </summary>
        /// <param name="clientId">Client Id</param>
        /// <param name="clientSecret">Client Secret</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>Authentication Response</returns>
        public Task<AuthenticationResponse> ClientCredentialsAsync(
            string clientId, 
            string clientSecret, 
            CancellationToken cancellationToken)
        {
            var headers = GetHeaders(clientId, clientSecret);
            Dictionary<string, string> request = new()
            {
                { grant_type, client_credentials }
            };
            return PostRequestAsync<Dictionary<string, string>, AuthenticationResponse>(
                host_name,
                token_uri, 
                null, 
                cancellationToken, 
                request, 
                null, 
                headers, 
                true);
        }

        /// <summary>
        /// Refresh Token - Authorisation Code Flow with PKCE
        /// </summary>
        /// <param name="clientId">Client Id</param>
        /// <param name="refreshToken">Refresh Token</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>Authentication Response</returns>
        public Task<AuthenticationResponse> RefreshTokenAsync(
            string clientId,
            string refreshToken,
            CancellationToken cancellationToken)
        {
            var request = new Dictionary<string, string>()
            {
                { grant_type, refresh_token },
                { refresh_token, refreshToken },
                { client_id, clientId }
            };
            return PostRequestAsync<Dictionary<string, string>, AuthenticationResponse>(
                host_name,
                token_uri,
                null,
                cancellationToken,
                request,
                null,
                null,
                true);
        }

        /// <summary>
        /// Implicit Grant Request - Implicit Grant Flow
        /// </summary>
        /// <param name="clientId">Client Id</param>
        /// <param name="scopes">Comma delimited scopes</param>
        /// <param name="state">State</param>
        /// <param name="redirectUrl">Redirect Url</param>
        /// <param name="showDialog">Show Dialog</param>
        /// <returns>Url</returns>
        public Uri GetImplicitGrantRequest(
            string clientId,
            string scopes,
            string state,
            string redirectUrl,
            bool showDialog)
        {
            var request = new Dictionary<string, string>()
            {
                { response_type, token_value },
                { client_id, clientId },
                { scope_value, scopes },
                { state_value, HttpUtility.UrlEncode(state) },
                { redirect_uri, HttpUtility.UrlEncode(redirectUrl) },
                { show_dialog, $"{showDialog}".ToLower() }
            };
            return GetUri(host_name, auth_uri, request);
        }

        /// <summary>
        /// Get Authoristion Code - Authorisation Code Flow with PKCE 
        /// </summary>
        /// <param name="clientId">Client Id</param>
        /// <param name="challenge">Challenge</param>
        /// <param name="scopes">Scopes</param>
        /// <param name="state">State</param>
        /// <param name="redirectUrl">Redirect Uri</param>
        /// <param name="showDialog">Show Dialog</param>
        /// <returns>Uri</returns>
        public Uri GetAuthorisationCodeUri(
            string clientId,
            string challenge,
            string scopes,
            string state,
            string redirectUrl,
            bool showDialog)
        {
            var request = new Dictionary<string, string>()
            {
                { client_id, clientId },
                { response_type, code_value },
                { redirect_uri, HttpUtility.UrlEncode(redirectUrl) },
                { code_challenge_method, code_challenge_method_type },
                { code_challenge, challenge },
                { scope_value, scopes },
                { state_value, HttpUtility.UrlEncode(state) },
                { show_dialog, $"{showDialog}".ToLower() }
            };
            return GetUri(host_name, auth_uri, request);
        }

        /// <summary>
        /// Get Authorisation Code - Authorisation Code Flow with PKCE 
        /// </summary>
        /// <param name="clientId">Client Id</param>
        /// <param name="verifier">Value of this parameter must match the value of the one that your app generated</param>
        /// <param name="accessCode">Access Code</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>Authentication Response</returns>
        public Task<AuthenticationResponse> GetAuthorisationCodeAsync(
            string clientId,
            string verifier,
            AccessCode accessCode,
            CancellationToken cancellationToken)
        {
            var request = new Dictionary<string, string>()
            {
                { client_id, clientId },
                { grant_type, authorization_code },
                { code_value, accessCode.Code },
                { redirect_uri, $"{accessCode.RedirectUri}" },
                { code_verifier, verifier }
            };
            return PostRequestAsync<Dictionary<string, string>, AuthenticationResponse>(
                hostname: host_name,
                relativeUri: token_uri,
                null,
                cancellationToken: cancellationToken,
                request,
                null,
                null,
                true);
        }
    }
}
