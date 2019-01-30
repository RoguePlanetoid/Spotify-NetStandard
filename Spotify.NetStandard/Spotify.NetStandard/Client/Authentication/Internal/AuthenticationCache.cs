using Spotify.NetStandard.Client.Authentication.Enums;
using Spotify.NetStandard.Client.Exceptions;
using Spotify.NetStandard.Client.Internal;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Spotify.NetStandard.Client.Authentication.Internal
{
    /// <summary>
    /// Authentication Cache
    /// </summary>
    internal class AuthenticationCache
    {
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly AuthenticationClient _client;
        private AccessCode _accessCode = null;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="client">Authentication Client</param>
        /// <param name="clientId">Client Id</param>
        /// <param name="clientSecret">Client Secret</param>
        public AuthenticationCache(AuthenticationClient client,
        string clientId, string clientSecret)
        {
            _client = client;
            this._clientId = clientId;
            this._clientSecret = clientSecret;
        }

        /// <summary>
        /// Check And Renew Token Async
        /// </summary>
        /// <param name="authType">Auth Type</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>Access Token</returns>
        /// <exception cref="AuthTokenRequiredException">AuthTokenRequiredException</exception>
        public async Task<AccessToken> CheckAndRenewTokenAsync(
        AuthType authType, CancellationToken cancellationToken)
        {
            bool requiresExplicitAuth = authType == AuthType.Explicit;
            if (AccessToken == null || AccessToken.Expiration < DateTime.UtcNow)
            {
                if(requiresExplicitAuth)
                {
                    throw new AuthTokenRequiredException();
                }
                AccessToken = await RenewImplicitTokenAsync(cancellationToken);
            }
            else
            {
                bool isExplictAuth = AccessToken.AuthType == AuthType.Explicit;
                if(requiresExplicitAuth && !isExplictAuth)
                {
                    throw new AuthTokenRequiredException();
                }
            }
            return AccessToken;
        }

        /// <summary>
        /// Renew Implict Token
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>Access Token</returns>
        public async Task<AccessToken> RenewImplicitTokenAsync(
        CancellationToken cancellationToken)
        {
            AuthenticationResponse authenticationResponse =
            await _client.AuthenticateAsync(
            _clientId, _clientSecret, cancellationToken);
            if (authenticationResponse != null)
            {
                AccessToken = new AccessToken
                {
                    AuthType = AuthType.Implicit,
                    Token = authenticationResponse.AccessToken,
                    Expiration = DateTime.UtcNow.Add(
                    TimeSpan.FromSeconds(Convert.ToDouble(
                    authenticationResponse.ExpiresIn)))
                };
            }
            return AccessToken;
        }

        /// <summary>
        /// Renew Explicit Token
        /// </summary>
        /// <param name="accessCode">Access Code</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns></returns>
        public async Task<AccessToken> RenewExplictTokenAsync(AccessCode accessCode, 
        CancellationToken cancellationToken)
        {
            AuthenticationResponse authenticationResponse =
            await _client.AuthenticateAsync(
            _clientId, _clientSecret, accessCode, 
            cancellationToken);
            if (authenticationResponse != null)
            {
                AccessToken = new AccessToken
                {
                    AuthType = AuthType.Explicit,
                    Token = authenticationResponse.AccessToken,
                    Expiration = DateTime.UtcNow.Add(
                    TimeSpan.FromSeconds(Convert.ToDouble(
                    authenticationResponse.ExpiresIn)))
                };
            }
            return AccessToken;
        }

        /// <summary>
        /// Get Auth
        /// </summary>
        /// <param name="redirect">Redirect Uri</param>
        /// <param name="state">State</param>
        /// <param name="scopes">Scopes</param>
        /// <returns>Authentication Uri</returns>
        public Uri GetAuth(Uri redirectUri, string state, string scopes)
        {
            return _client.Authenticate(_clientId, scopes, state, redirectUri.ToString());
        }

        /// <summary>
        /// Get Auth
        /// </summary>
        /// <param name="responseUri">Response Uri</param>
        /// <param name="redirectUri">Redirect Uri</param>
        /// <param name="state">State Value</param>
        /// <returns>Access Token</returns>
        /// <exception cref="AuthValueException">AuthCodeValueException</exception>
        /// <exception cref="AuthStateException">AuthCodeStateException</exception>
        public async Task<AccessToken> GetAuth(Uri responseUri, Uri redirectUri, string state)
        {
            if (responseUri != null)
            {
                if (responseUri.ToString().Contains(redirectUri.ToString()))
                {
                    if (_accessCode == null || _accessCode.ResponseUri != responseUri)
                    {
                        _accessCode = new AccessCode(responseUri, redirectUri,
                        responseUri.Query.QueryStringAsDictionary());
                        if (state == null || _accessCode.State == state)
                        {
                            if (string.IsNullOrEmpty(_accessCode.Code))
                            {
                                throw new AuthValueException();
                            }
                            else
                            {
                                return await RenewExplictTokenAsync(
                                _accessCode, 
                                new CancellationToken(false));
                            }
                        }
                        else
                        {
                            throw new AuthStateException();
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Access Token
        /// </summary>
        public AccessToken AccessToken { get; set; } = null;
    }
}
