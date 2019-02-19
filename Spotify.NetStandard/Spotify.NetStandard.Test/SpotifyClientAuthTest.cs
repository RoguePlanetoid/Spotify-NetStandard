using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Spotify.NetStandard.Client;
using Spotify.NetStandard.Client.Authentication;
using Spotify.NetStandard.Client.Authentication.Enums;
using Spotify.NetStandard.Client.Interfaces;
using Spotify.NetStandard.Requests;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Spotify.NetStandard.Test
{
    [TestClass]
    public class SpotifyClientAuthTest
    {
        private readonly Uri redirect_url = new Uri("https://www.example.org/spotify");
        private const string state = "spotify.state";

        private ISpotifyClient _client = null;
        
        /// <summary>
        /// Initialise Unit Test and Configuration
        /// </summary>
        [TestInitialize]
        public void Init()
        {
            // Configuration
            var configBuilder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            IConfiguration config = configBuilder.Build();
            // Spotify Client Factory
            _client = SpotifyClientFactory.CreateSpotifyClient(
                config["client_id"], config["client_secret"]);
            Assert.IsNotNull(_client);
            // Spotify Client Token
            var accessToken = new AccessToken()
            {
                Token = config["token"],
                Expiration = DateTime.Parse(config["expires"]),
                TokenType = (TokenType)Enum.Parse(typeof(TokenType), config["type"])
            };
            var expired = DateTime.UtcNow > accessToken.Expiration;
            Assert.IsFalse(expired);
            _client.SetToken(accessToken);
        }

        #region Auth
        /// <summary>
        /// Authenticate User
        /// </summary>
        [TestMethod]
        public void Test_AuthUserUri()
        {
            var uri = _client.AuthUser(redirect_url, state,
            new Scope
            {
                UserReadPrivate = true,
                FollowRead = true,
                FollowModify = true,
                PlaylistModifyPublic = true,
                PlaylistModifyPrivate = true,
                UserGeneratedContentImageUpload = true
            });
            Assert.IsNotNull(uri);
        }
        #endregion Auth

        #region Follow
        /// <summary>
        /// Get Following State for Artists/Users
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_AuthLookupFollowingState()
        {
            // "Ariana Grande"
            var result = await _client.AuthLookupFollowingStateAsync(
                new List<string> { "66CXWjxzNUsdJxJ2JdwvnR" }, 
                Enums.FollowType.Artist);
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Check if Users Follow a Playlist
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_AuthLookupUserFollowingPlaylist()
        {
            var result = await _client.AuthLookupUserFollowingPlaylistAsync(
                new List<string> { "jmperezperez" }, "3cEYpjA9oz9GiPac4AsH4n");
            Assert.IsTrue(result.TrueForAll(t => t));
        }

        /// <summary>
        /// Follow Artists or Users
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_AuthFollowArtistAsync()
        {
            // "Ariana Grande"
            var result = await _client.AuthFollowAsync(
                new List<string> { "66CXWjxzNUsdJxJ2JdwvnR" },
                Enums.FollowType.Artist);
            Assert.IsTrue(result.Success);
        }

        /// <summary>
        /// Follow a Playlist
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_AuthFollowPlaylist()
        {
            var result = await _client.AuthFollowPlaylistAsync(
                "37i9dQZF1DWUS3jbm4YExP");
            Assert.IsTrue(result.Success);
        }

        /// <summary>
        /// Get User's Followed Artists
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_AuthLookupFollowedArtists()
        {
            var result = await _client.AuthLookupFollowedArtistsAsync();
            Assert.IsNotNull(result.Items);
        }

        /// <summary>
        /// Unfollow Artists or Users
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_AuthUnfollowArtist()
        {
            // "Ariana Grande"
            var result = await _client.AuthUnfollowAsync(
                new List<string> { "66CXWjxzNUsdJxJ2JdwvnR" },
                Enums.FollowType.Artist);
            Assert.IsTrue(result.Success);
        }

        /// <summary>
        /// Unfollow Playlist
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_AuthUnfollowPlaylist()
        {
            var result = await _client.AuthUnfollowPlaylistAsync(
                "37i9dQZF1DWUS3jbm4YExP");
            Assert.IsTrue(result.Success);
        }
        #endregion Follow

        #region Playlist
        /// <summary>
        /// Add Tracks to a Playlist
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_AuthAddTracksToPlaylist()
        {
            var result = await _client.AuthAddTracksToPlaylistAsync(
                "7D4Epnvxhc8l6NpooJqYXk",
                new UriListRequest()
                {
                  Uris = new List<string>
                  {
                      "spotify:track:2zzdnRWE3z6QP3FoVlnWHO"
                  }
                }, 0);
            Assert.IsTrue(result.Success);
        }

        /// <summary>
        /// Remove Tracks from a Playlist
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_AuthRemoveTracksFromPlaylist()
        {
            var request = new PlaylistTracksRequest()
            {
                Tracks = new List<UriRequest>()
                {
                    new UriRequest()
                    {
                        Uri = "spotify:track:2zzdnRWE3z6QP3FoVlnWHO"
                    }
                }
            };
            var result = await _client.AuthRemoveTracksFromPlaylistAsync(
                "7D4Epnvxhc8l6NpooJqYXk",
                request);
            Assert.IsTrue(result.Success);
        }

        /// <summary>
        /// Get a Playlist Cover Image
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_AuthGetPlaylistCoverImage()
        {
            var result = await _client.AuthGetPlaylistCoverImageAsync(
                "3cEYpjA9oz9GiPac4AsH4n");
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Upload a Custom Playlist Cover Image
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_AuthUploadCustomPlaylistImage()
        {
            string filename = @"C:\Test\PlaylistCover.jpg";
            var file = File.ReadAllBytes(filename);
            var result = await _client.AuthUploadCustomPlaylistImageAsync(
                "1L6ECMsEDXmrp1qfH5htHZ",
                file);
            Assert.IsTrue(result.Success);
        }

        /// <summary>
        /// Get a List of Current User's Playlists 
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_AuthGetListOfCurrentUserPlaylists()
        {
            var result = await _client.AuthLookupUserPlaylistsAsync();
            Assert.IsNotNull(result?.Items);
            Assert.IsTrue(result.Items.Count > 0);
        }

        /// <summary>
        /// Change a Playlist's Details
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_AuthChangePlaylistDetails()
        {
            var result = await _client.AuthChangePlaylistDetailsAsync(
                "7D4Epnvxhc8l6NpooJqYXk", 
                new PlaylistRequest()
            {
                Name = "Spotify.NetStandard",
                Description = "Spotify Client Auth Test"
            });
        }

        /// <summary>
        /// Get a List of a User's Playlists
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_AuthGetListOfUserPlaylists()
        {
            var result = await _client.AuthLookupUserPlaylistsAsync(
                "Spotify");
            Assert.IsNotNull(result?.Items);
            Assert.IsTrue(result.Items.Count > 0);
        }

        /// <summary>
        /// Replace a Playlist's Tracks
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_AuthReplacePlaylistTracks()
        {
            var result = await _client.AuthReplacePlaylistTracksAsync(
                "7D4Epnvxhc8l6NpooJqYXk",
                new UriListRequest()
                {
                    Uris = new List<string>
                  {
                      "spotify:track:2RlgNHKcydI9sayD2Df2xp"
                  }
                });
            Assert.IsTrue(result.Success);
        }

        /// <summary>
        /// Create a Playlist
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_AuthCreatePlaylist()
        {
            var result = await _client.AuthCreatePlaylistAsync(
            "doa31nixtl3kmy7uf8ov88sy0",
            new PlaylistRequest()
            {
                Name = "Spotify.Created",
                Description = "Spotify Client Auth Test"
            });
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Reorder a Playlist's Tracks
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_AuthReorderPlaylistTracks()
        {
            var result = await _client.AuthReorderPlaylistTracksAsync(
            "1L6ECMsEDXmrp1qfH5htHZ", 
            new PlaylistReorderRequest()
            {
                RangeStart = 2,
                RangeLength = 1,
                InsertBefore = 0
            });
            Assert.IsTrue(result.Success);
        }
        #endregion Playlist

        #region Personalisation
        /// <summary>
        /// Get a User's Top Artists
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_AuthLookupUserTopArtists()
        {
            var result = await _client.AuthLookupUserTopArtistsAsync();
            Assert.IsNotNull(result);
            Assert.IsNotNull(result?.Items);
            Assert.IsTrue(result.Items.Count > 0);
        }

        /// <summary>
        /// Get a User's Top Tracks
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_AuthLookupUserTopTracks()
        {
            var result = await _client.AuthLookupUserTopTracksAsync();
            Assert.IsNotNull(result);
            Assert.IsNotNull(result?.Items);
            Assert.IsTrue(result.Items.Count > 0);
        }
        #endregion Personalisation

        #region User Profile
        /// <summary>
        /// Get a User's Profile
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_AuthLookupUserProfile()
        {
            var result = await _client.AuthLookupUserProfileAsync(
                "jmperezperez");
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Get Current User's Profile
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_AuthLookupCurrentUserProfile()
        {
            var result = await _client.AuthLookupUserProfileAsync();
            Assert.IsNotNull(result);
        }
        #endregion User Profile
    }
}
