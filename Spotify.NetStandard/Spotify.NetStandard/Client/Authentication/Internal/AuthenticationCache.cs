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
        private ImplicitGrant _implicitGrant = null;

        #region Private Methods
        /// <summary>
        /// Map
        /// </summary>
        /// <param name="tokenType">TokenType</param>
        /// <param name="response">AuthenticationResponse</param>
        /// <returns>Access Token</returns>
        private AccessToken Map(TokenType tokenType,
            AuthenticationResponse response) => 
            new AccessToken
            {
                TokenType = tokenType,
                Token = response.AccessToken,
                Refresh = response.RefreshToken,
                Expiration = DateTime.UtcNow.Add(
                TimeSpan.FromSeconds(Convert.ToDouble(
                response.ExpiresIn))),
                Scopes = response.Scope
            };

        /// <summary>
        /// Map
        /// </summary>
        /// <param name="response">ImplicitGrant</param>
        /// <returns>Access Token</returns>
        private AccessToken Map(ImplicitGrant response) => 
            new AccessToken
            {
                TokenType = TokenType.User,
                Token = response.AccessToken,
                Expiration = DateTime.UtcNow.Add(
                    TimeSpan.FromSeconds(Convert.ToDouble(
                    response.ExpiresIn)))
            };
        #endregion Private Methods

        #region Public Methods
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="client">Authentication Client</param>
        /// <param name="clientId">Client Id</param>
        /// <param name="clientSecret">Client Secret</param>
        public AuthenticationCache(
            AuthenticationClient client,
            string clientId, 
            string clientSecret)
        {
            _client = client;
            this._clientId = clientId;
            this._clientSecret = clientSecret;
        }

        /// <summary>
        /// Check And Renew Token Async
        /// </summary>
        /// <param name="tokenType">Token Type</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>Access Token</returns>
        /// <exception cref="AuthAccessTokenRequiredException">AuthAccessTokenRequiredException</exception>
        /// <exception cref="AuthUserTokenRequiredException">AuthUserTokenRequiredException</exception>
        public async Task<AccessToken> CheckAndRenewTokenAsync(
            TokenType tokenType, 
            CancellationToken cancellationToken)
        {
            bool requiresUserToken = (tokenType == TokenType.User);
            if (AccessToken?.Token == null)
            {
                if(requiresUserToken)
                {
                    throw new AuthUserTokenRequiredException();
                }
                AccessToken = await GetAccessTokenAsync(
                    cancellationToken);
            }
            else if(AccessToken?.Refresh != null && 
                (AccessToken.Expiration < DateTime.UtcNow))
            {
                AccessToken = await GetRefreshTokenAsync(
                    AccessToken.Refresh, 
                    AccessToken.TokenType, 
                    cancellationToken);
            }
            else
            {
                bool isUserToken = (AccessToken.TokenType == TokenType.User);
                if(requiresUserToken && !isUserToken)
                {
                    throw new AuthUserTokenRequiredException();
                }
            }
            if(AccessToken?.Token == null || 
                (AccessToken.Expiration < DateTime.UtcNow))
            {
                if (requiresUserToken)
                {
                    throw new AuthUserTokenRequiredException();
                }
                else
                {
                    throw new AuthAccessTokenRequiredException();
                }
            }
            return AccessToken;
        }

        /// <summary>
        /// Get Refresh Token
        /// </summary>
        /// <param name="refreshToken">Refresh Token</param>
        /// <param name="tokenType">Token Type</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>Access Token</returns>
        public async Task<AccessToken> GetRefreshTokenAsync(
            string refreshToken,
            TokenType tokenType,
            CancellationToken cancellationToken)
        {
            var authenticationResponse =
            await _client.RefreshTokenAuthAsync(
                _clientId, 
                _clientSecret, 
                refreshToken, 
                cancellationToken);
            if (authenticationResponse != null)
            {
                AccessToken = Map(tokenType, 
                    authenticationResponse);
            }
            return AccessToken;
        }

        /// <summary>
        /// Get Access Token
        /// </summary>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>Access Token</returns>
        public async Task<AccessToken> GetAccessTokenAsync(
            CancellationToken cancellationToken)
        {
            var authenticationResponse =  
            await _client.ClientCredentialsAuthAsync(
                _clientId, 
                _clientSecret, 
                cancellationToken);
            if (authenticationResponse != null)
            {
                AccessToken = Map(TokenType.Access,
                    authenticationResponse);
            }
            return AccessToken;
        }

        /// <summary>
        /// Get User Token
        /// </summary>
        /// <param name="accessCode">Access Code</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns></returns>
        public async Task<AccessToken> GetUserTokenAsync(
            AccessCode accessCode, 
            CancellationToken cancellationToken)
        {
            var authenticationResponse =
            await _client.AuthorisationCodeAuthAsync(
                _clientId, 
                _clientSecret, 
                accessCode, 
                cancellationToken);
            if (authenticationResponse != null)
            {
                AccessToken = Map(TokenType.User,
                    authenticationResponse);
            }
            return AccessToken;
        }

        /// <summary>
        /// Get Access Token Auth
        /// </summary>
        /// <param name="redirectUri">Redirect Uri</param>
        /// <param name="state">State</param>
        /// <param name="scopes">Scope</param>
        /// <returns>Authentication Uri</returns>
        public Uri GetAccessTokenAuth(
            Uri redirectUri,
            string state,
            string scopes,
            bool showDialog) => 
            _client.AccessTokenAuth(_clientId, scopes,
                state, redirectUri.ToString(), showDialog);

        /// <summary>
        /// Get Access Code Auth
        /// </summary>
        /// <param name="responseUri">Response Uri</param>
        /// <param name="redirectUri">Redirect Uri</param>
        /// <param name="state">State Value</param>
        /// <returns>Access Token</returns>
        /// <exception cref="AuthCodeValueException">AuthCodeValueException</exception>
        /// <exception cref="AuthCodeStateException">AuthCodeStateException</exception>
        public async Task<AccessToken> GetAccessCodeAuthAsync(
            Uri responseUri, 
            Uri redirectUri, 
            string state)
        {
            if (responseUri != null)
            {
                if (responseUri.ToString().Contains(redirectUri.ToString()))
                {
                    if (_accessCode == null || _accessCode.ResponseUri != responseUri)
                    {
                        _accessCode = new AccessCode(
                            responseUri, redirectUri,
                            responseUri.Query.QueryStringAsDictionary());
                        if (state == null || _accessCode.State == state)
                        {
                            if (string.IsNullOrEmpty(_accessCode.Code))
                            {
                                throw new AuthCodeValueException();
                            }
                            else
                            {
                                return await GetUserTokenAsync(
                                    _accessCode, 
                                    new CancellationToken(false));
                            }
                        }
                        else
                        {
                            throw new AuthCodeStateException();
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Get Implicit Grant Auth
        /// </summary>
        /// <param name="redirectUri">Redirect Uri</param>
        /// <param name="state">State</param>
        /// <param name="scopes">Scope</param>
        /// <returns>Authentication Uri</returns>
        public Uri GetImplicitGrantAuth(
            Uri redirectUri,
            string state,
            string scopes,
            bool showDialog) =>
            _client.ImplicitGrantAuth(_clientId, scopes,
                state, redirectUri.ToString(), showDialog);

        /// <summary>
        /// Get Implicit Grant Auth
        /// </summary>
        /// <param name="responseUri">Response Uri</param>
        /// <param name="redirectUri">Redirect Uri</param>
        /// <param name="state">State Value</param>
        /// <returns>Access Token</returns>
        /// <exception cref="AuthTokenValueException">AuthTokenValueException</exception>
        /// <exception cref="AuthTokenStateException">AuthTokenStateException</exception>
        public AccessToken GetImplicitGrantAuth(
            Uri responseUri,
            Uri redirectUri,
            string state)
        {
            if (responseUri != null)
            {
                if (responseUri.ToString().Contains(redirectUri.ToString()))
                {
                    if (_implicitGrant == null || _implicitGrant.ResponseUri != responseUri)
                    {
                        _implicitGrant = new ImplicitGrant(
                            responseUri, redirectUri,
                            responseUri.Fragment.TrimStart('#').QueryStringAsDictionary());
                        if (state == null || _implicitGrant.State == state)
                        {
                            if (string.IsNullOrEmpty(_implicitGrant.AccessToken))
                            {
                                throw new AuthTokenValueException();
                            }
                            else
                            {
                                return Map(_implicitGrant);
                            }
                        }
                        else
                        {
                            throw new AuthTokenStateException();
                        }
                    }
                }
            }
            return null;
        }
        #endregion Public Methods

            /// <summary>
            /// Access Token
            /// </summary>
        public AccessToken AccessToken { get; set; } = null;
    }
}
