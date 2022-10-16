namespace Spotify.NetStandard.Client.Exceptions;

/// <summary>
/// Auth Token Expired or Required Error
/// </summary>
public class AuthTokenRequiredException : AuthException { }

/// <summary>
/// Auth User Token Expired or Required Error
/// </summary>
public class AuthUserTokenRequiredException : AuthTokenRequiredException { }

/// <summary>
/// Auth Access Token Expired or Required Error
/// </summary>
public class AuthAccessTokenRequiredException : AuthTokenRequiredException { }
