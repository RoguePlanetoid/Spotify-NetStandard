namespace Spotify.NetStandard.Client.Authentication.Internal;

/// <summary>
/// Authentication Cache
/// </summary>
internal class AuthenticationCache
{
    private const char hash = '#';

    private readonly string _clientId;
    private readonly string _clientSecret;
    private readonly AuthenticationClient _client;
    private AccessCode _accessCode = null;
    private ImplicitGrant _implicitGrant = null;

    /// <summary>
    /// Map
    /// </summary>
    /// <param name="tokenType">TokenType</param>
    /// <param name="response">AuthenticationResponse</param>
    /// <param name="refreshToken">Refresh Token</param>
    /// <returns>Access Token</returns>
    private AccessToken Map(TokenType tokenType,
        AuthenticationResponse response, 
        string refreshToken = null) => 
        new()
        {
            TokenType = tokenType,
            Token = response.AccessToken,
            Refresh = refreshToken ?? response.RefreshToken,
            Expiration = DateTime.UtcNow.Add(
                TimeSpan.FromSeconds(
                    Convert.ToDouble(response.ExpiresIn))),
            Scopes = response.Scope,
            Error = response.Error
        };

    /// <summary>
    /// Map
    /// </summary>
    /// <param name="response">ImplicitGrant</param>
    /// <returns>Access Token</returns>
    private AccessToken Map(ImplicitGrant response) => 
        new()
        {
            TokenType = TokenType.User,
            Token = response.AccessToken,
            Expiration = DateTime.UtcNow.Add(
                TimeSpan.FromSeconds(
                    Convert.ToDouble(response.ExpiresIn))),
            Error = response.Error
        };

    /// <summary>
    /// Get Uri Dictionary
    /// </summary>
    /// <param name="uri">Uri</param>
    /// <returns>Dictionary of Key Values</returns>
    private Dictionary<string, string> GetUriDictionary(Uri uri) => 
        string.IsNullOrEmpty(uri.Fragment)
            ? uri.Query.QueryStringAsDictionary()
            : uri.Fragment.TrimStart(hash)
                .QueryStringAsDictionary();

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="client">Authentication Client</param>
    /// <param name="clientId">Client Id</param>
    /// <param name="clientSecret">Client Secret</param>
    public AuthenticationCache(
        AuthenticationClient client,
        string clientId,
        string clientSecret) =>
        (_client, _clientId, _clientSecret) = 
        (client, clientId, clientSecret);

    /// <summary>
    /// Check And Renew Token
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
        bool requiresUserToken = tokenType == TokenType.User;
        if (AccessToken?.Token == null)
        {
            if(requiresUserToken)
                throw new AuthUserTokenRequiredException();
            AccessToken = await GetClientCredentialsTokenAsync(cancellationToken);
        }
        else if(AccessToken?.Refresh != null && (AccessToken.Expiration < DateTime.UtcNow))
            AccessToken = await GetRefreshTokenAsync(
                AccessToken.Refresh, 
                AccessToken.TokenType, 
                cancellationToken);
        else
        {
            if(requiresUserToken && !(AccessToken.TokenType == TokenType.User))
                throw new AuthUserTokenRequiredException();
        }
        if(AccessToken?.Token == null || (AccessToken.Expiration < DateTime.UtcNow))
        {
            if (requiresUserToken)
                throw new AuthUserTokenRequiredException();
            else
                throw new AuthAccessTokenRequiredException();
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
            await _client.RefreshTokenAsync(
                _clientId,
                refreshToken,
                cancellationToken);
        if (authenticationResponse != null)
            AccessToken = Map(tokenType, 
                authenticationResponse, 
                refreshToken);
        return AccessToken;
    }

    /// <summary>
    /// Get Client Credentials Token - Client Credentials Flow
    /// </summary>
    /// <param name="cancellationToken">Cancellation Token</param>
    /// <returns>Access Token</returns>
    public async Task<AccessToken> GetClientCredentialsTokenAsync(
        CancellationToken cancellationToken)
    {
        var authenticationResponse =  
        await _client.ClientCredentialsAsync(
            _clientId, 
            _clientSecret, 
            cancellationToken);
        if (authenticationResponse != null)
            AccessToken = Map(TokenType.Access, authenticationResponse);
        return AccessToken;
    }

    /// <summary>
    /// Get Implicit Grant Auth - Implicit Grant Flow
    /// </summary>
    /// <param name="redirectUri">Redirect Uri</param>
    /// <param name="state">State</param>
    /// <param name="scopes">Scope</param>
    /// <param name="showDialog">Show Dialog</param>
    /// <returns>Authentication Uri</returns>
    public Uri GetImplicitGrantAuth(
        Uri redirectUri,
        string state,
        string scopes,
        bool showDialog) =>
        _client.GetImplicitGrantRequest(_clientId, scopes,
            state, redirectUri.ToString(), showDialog);

    /// <summary>
    /// Get Implicit Grant Auth - Implicit Grant Flow
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
                if (_implicitGrant?.ResponseUri == null || (_implicitGrant.ResponseUri.ToString() != responseUri.ToString()))
                {
                    _implicitGrant = new ImplicitGrant(
                        responseUri, redirectUri,
                        GetUriDictionary(responseUri));
                    if (state == null || _implicitGrant.State == state)
                        if (string.IsNullOrEmpty(_implicitGrant.AccessToken))
                            throw new AuthTokenValueException(_implicitGrant.Error);
                        else
                        {
                            AccessToken = Map(_implicitGrant);
                            return AccessToken;
                        }
                    else
                        throw new AuthTokenStateException(_implicitGrant.State);
                }
            }
        }
        return null;
    }

    /// <summary>
    /// Get Authorisation Code - Authorization Code Flow with Proof Key for Code Exchange
    /// </summary>
    /// <param name="redirectUri">Redirect Uri</param>
    /// <param name="state">State</param>
    /// <param name="scopes">Scope</param>
    /// <param name="showDialog">Show Dialog</param>
    /// <param name="codeVerifier">Code Verifier</param>
    /// <returns>Authentication Uri</returns>
    public Uri GetAuthorisationCodeUri(
        Uri redirectUri,
        string state,
        string scopes,
        bool showDialog,
        out string codeVerifier)
    {
        var verifierChallenge = new VerifierChallenge();
        codeVerifier = verifierChallenge.Verifier;
        return _client.GetAuthorisationCodeUri(
            _clientId,
            verifierChallenge.Challenge, 
            scopes, 
            state, 
            redirectUri.ToString(),
            showDialog);
    }

    /// <summary>
    /// Get Authorisation Code Token - Authorization Code Flow with Proof Key for Code Exchange
    /// </summary>
    /// <param name="codeVerifier">Code Verifier</param>
    /// <param name="accessCode">Access Code</param>
    /// <param name="cancellationToken">Cancellation Token</param>
    /// <returns>Access Token</returns>
    public async Task<AccessToken> GetAuthorisationCodeTokenAsync(
        string codeVerifier,
        AccessCode accessCode,
        CancellationToken cancellationToken)
    {
        var authenticationResponse =
            await _client.GetAuthorisationCodeAsync(
                _clientId,
                codeVerifier,
                accessCode,
                cancellationToken);
        if (authenticationResponse != null)
            AccessToken = Map(TokenType.User, authenticationResponse);
        return AccessToken;
    }

    /// <summary>
    /// Get Authorisation Code Proof Key for Code Exchange
    /// </summary>
    /// <param name="responseUri">Response Uri</param>
    /// <param name="redirectUri">Request Uri</param>
    /// <param name="state">State</param>
    /// <param name="codeVerifier">Code Verifier</param>
    /// <returns>Access Token</returns>
    public async Task<AccessToken> GetAuthorisationCodeAuthAsync(
        Uri responseUri,
        Uri redirectUri,
        string state,
        string codeVerifier)
    {
        if(string.IsNullOrEmpty(codeVerifier))
            throw new AuthCodeVerifierRequiredException();
        if (responseUri != null)
        {
            if (responseUri.ToString().Contains(redirectUri.ToString()))
            {
                if (_accessCode?.ResponseUri == null || (_accessCode.ResponseUri.ToString() != responseUri.ToString()))
                {
                    _accessCode = new AccessCode(
                        responseUri, redirectUri,
                        responseUri.Query.QueryStringAsDictionary());
                    if (state == null || _accessCode.State == state)
                    {
                        if (string.IsNullOrEmpty(_accessCode.Code))
                            throw new AuthCodeValueException(_accessCode.Error);
                        else
                        {
                            AccessToken = await GetAuthorisationCodeTokenAsync(
                                codeVerifier,
                                _accessCode, 
                                new CancellationToken(false));
                            return AccessToken;
                        }
                    }
                    else
                        throw new AuthCodeStateException(_accessCode.State);
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
