using Spotify.NetStandard.Client.Authentication.Internal;
using Spotify.NetStandard.Client.Interfaces;
using Spotify.NetStandard.Client.Internal;
using System.Collections.Generic;

namespace Spotify.NetStandard.Client
{
    /// <summary>
    /// Spotify Client Factory
    /// </summary>
    public class SpotifyClientFactory
    {
        private static readonly AuthenticationClient 
            _authenticationClient = new AuthenticationClient();

        private static readonly Dictionary<string, AuthenticationCache> 
            _authenticationCaches =  new Dictionary<string, AuthenticationCache>();

        /// <summary>
        /// Get or Add Authenciation Cache
        /// </summary>
        /// <param name="clientId">Spotify Client Id</param>
        /// <param name="clientSecret">Spotify Client Secret</param>
        /// <returns>Authentication Cache</returns>
        private static AuthenticationCache GetOrAddAuthenticationCache(
            string clientId, string clientSecret)
        {
            if (!_authenticationCaches.ContainsKey(clientId))
            {
                AuthenticationCache authenticationCache = new AuthenticationCache(
                _authenticationClient, clientId, clientSecret);
                _authenticationCaches[clientId] = authenticationCache;
            }
            return _authenticationCaches[clientId];
        }

        /// <summary>
        /// Create Spotify Client
        /// </summary>
        /// <param name="clientId">Spotify Client Id</param>
        /// <param name="clientSecret">Spotify Client Secret</param>
        /// <returns>Spotify Client</returns>
        public static ISpotifyClient CreateSpotifyClient(
            string clientId, 
            string clientSecret)
        {
            return new SpotifyClient(GetOrAddAuthenticationCache(
            clientId, clientSecret));
        }
    }
}
