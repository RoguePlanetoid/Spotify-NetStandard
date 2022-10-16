namespace Spotify.NetStandard.Client.Authentication.Internal;

/// <summary>
/// Authentication Response
/// </summary>
[DataContract]
internal class AuthenticationResponse
{
    /// <summary>
    /// An access token that can be provided in subsequent calls
    /// </summary>
    [DataMember(Name = "access_token")]
    public string AccessToken { get; set; }

    /// <summary>
    /// How the access token may be used: always “Bearer”.
    /// </summary>
    [DataMember(Name = "token_type")]
    public string TokenType { get; set; }

    /// <summary>
    /// A space-separated list of scopes which have been granted for this access token
    /// </summary>
    [DataMember(Name = "scope", EmitDefaultValue = false)]
    public string Scope { get; set; }

    /// <summary>
    /// The time period (in seconds) for which the access token is valid.
    /// </summary>
    [DataMember(Name = "expires_in", EmitDefaultValue = false)]
    public int ExpiresIn { get; set; }

    /// <summary>
    /// A token that can be sent to the Spotify Accounts service in place of an authorization code
    /// </summary>
    [DataMember(Name = "refresh_token")]
    public string RefreshToken { get; set; }

    /// <summary>
    /// The reason authorisation failed, for example: “access_denied”.
    /// </summary>
    [DataMember(Name = "error")]
    public string Error { get; set; }
}
