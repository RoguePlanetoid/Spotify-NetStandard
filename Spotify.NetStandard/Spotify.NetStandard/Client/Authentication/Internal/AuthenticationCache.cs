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

        #region Private Methods
        /// <summary>
        /// Map
        /// </summary>
        /// <param name="tokenType">TokenType</param>
        /// <param name="response">AuthenticationResponse</param>
        /// <returns>Access Token</returns>
        private AccessToken Map(TokenType tokenType,
            AuthenticationResponse response)
        {
            return new AccessToken
            {
                TokenType = tokenType,
                Token = response.AccessToken,
                Refresh = response.RefreshToken,
                Expiration = DateTime.UtcNow.Add(
                TimeSpan.FromSeconds(Convert.ToDouble(
                response.ExpiresIn))),
                Scopes = response.Scope             
            };
        }
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
            else if(AccessToken.Expiration < DateTime.UtcNow)
            {
                AccessToken = await RefreshTokenAsync(
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
                AccessToken.Expiration < DateTime.UtcNow)
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
        /// Refresh Token
        /// </summary>
        /// <param name="refreshToken">Refresh Token</param>
        /// <param name="tokenType">Token Type</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>Access Token</returns>
        public async Task<AccessToken> RefreshTokenAsync(
            string refreshToken,
            TokenType tokenType,
            CancellationToken cancellationToken)
        {
            var authenticationResponse =
            await _client.AuthenticateAsync(
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
            await _client.AuthenticateAsync(
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
            await _client.AuthenticateAsync(
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
        /// Get Auth
        /// </summary>
        /// <param name="redirect">Redirect Uri</param>
        /// <param name="state">State</param>
        /// <param name="scopes">Scope</param>
        /// <returns>Authentication Uri</returns>
        public Uri GetAuth(
            Uri redirectUri,
            string state,
            string scopes) => 
            _client.Authenticate(_clientId, scopes,
                state, redirectUri.ToString());

        /// <summary>
        /// Get Auth
        /// </summary>
        /// <param name="responseUri">Response Uri</param>
        /// <param name="redirectUri">Redirect Uri</param>
        /// <param name="state">State Value</param>
        /// <returns>Access Token</returns>
        /// <exception cref="AuthCodeValueException">AuthAccessCodeException</exception>
        /// <exception cref="AuthCodeStateException">AuthStateException</exception>
        public async Task<AccessToken> GetAuth(
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
        #endregion Public Methods

        /// <summary>
        /// Access Token
        /// </summary>
        public AccessToken AccessToken { get; set; } = null;
    }
}
