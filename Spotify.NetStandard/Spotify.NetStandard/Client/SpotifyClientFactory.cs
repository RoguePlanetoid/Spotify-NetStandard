using Spotify.NetStandard.Client.Authentication.Internal;
using Spotify.NetStandard.Client.Interfaces;
using Spotify.NetStandard.Client.Internal;
using System.Collections.Generic;
using System.Net.Http;

namespace Spotify.NetStandard.Client
{
    /// <summary>
    /// Spotify Client Factory
    /// </summary>
    public class SpotifyClientFactory
    {
        #region Private Constants
        private static readonly AuthenticationClient 
            _authenticationClient = new AuthenticationClient();

        private static readonly Dictionary<string, AuthenticationCache> 
            _authenticationCaches =  new Dictionary<string, AuthenticationCache>();
        #endregion Private Constants

        #region Private Methods
        /// <summary>
        /// Get or Add Authenciation Cache
        /// </summary>
        /// <param name="clientId">Spotify Client Id</param>
        /// <param name="clientSecret">Spotify Client Secret</param>
        /// <returns>Authentication Cache</returns>
        private static AuthenticationCache GetOrAddAuthenticationCache(
            string clientId, 
            string clientSecret)
        {
            if (!_authenticationCaches.ContainsKey(clientId))
            {
                var authenticationCache = new AuthenticationCache(
                _authenticationClient, clientId, clientSecret);
                _authenticationCaches[clientId] = authenticationCache;
            }
            return _authenticationCaches[clientId];
        }
        #endregion Private Methods

        #region Public Methods
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
        #endregion Public Methods
    }
}
