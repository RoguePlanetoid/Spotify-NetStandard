namespace Spotify.NetStandard.Client;

/// <summary>
/// Spotify Client Factory
/// </summary>
public class SpotifyClientFactory
{
    private static readonly AuthenticationClient _authenticationClient = new(); 
    private static readonly Dictionary<string, AuthenticationCache> _authenticationCache = new();

    /// <summary>
    /// Get or Add Authentication Cache
    /// </summary>
    /// <param name="clientId">Spotify Client Id</param>
    /// <param name="clientSecret">Spotify Client Secret</param>
    /// <returns>Authentication Cache</returns>
    private static AuthenticationCache GetOrAddAuthenticationCache(
        string clientId, 
        string clientSecret)
    {
        if (!_authenticationCache.ContainsKey(clientId))
        {
            var authenticationCache = new AuthenticationCache(
                _authenticationClient, clientId, clientSecret);
            _authenticationCache[clientId] = authenticationCache;
        }
        return _authenticationCache[clientId];
    }

    /// <summary>
    /// Create Spotify Client
    /// </summary>
    /// <param name="clientId">(Required) Spotify Client Id</param>
    /// <param name="clientSecret">(Optional) Spotify Client Secret</param>
    /// <returns>Spotify Client</returns>
    public static ISpotifyClient CreateSpotifyClient(
        string clientId,
        string clientSecret = null) => 
            new SpotifyClient(GetOrAddAuthenticationCache(clientId, clientSecret));

    /// <summary>
    /// Create Spotify Client
    /// </summary>
    /// <param name="httpClient">(Required) Http Client</param>
    /// <param name="clientId">(Required) Spotify Client Id</param>
    /// <param name="clientSecret">(Optional) Spotify Client Secret</param>
    /// <returns>Spotify Client</returns>
    public static ISpotifyClient CreateSpotifyClient(
        HttpClient httpClient,
        string clientId,
        string clientSecret = null) =>
            new SpotifyClient(GetOrAddAuthenticationCache(clientId, clientSecret), httpClient);
}
