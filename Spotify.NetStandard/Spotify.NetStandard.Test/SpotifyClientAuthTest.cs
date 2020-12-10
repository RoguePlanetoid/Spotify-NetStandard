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
                Refresh = config["refresh"],
                Expiration = DateTime.Parse(config["expires"]),
                TokenType = (TokenType)Enum.Parse(typeof(TokenType), config["type"])
            };
            var expired = DateTime.UtcNow > accessToken.Expiration;
            //Assert.IsFalse(expired);
            _client.SetToken(accessToken);
        }

        #region Authenticate
        /// <summary>
        /// Authenticate User
        /// </summary>
        [TestMethod]
        public void Test_AuthUser_Request()
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
            StringAssert.Contains(uri.ToString(), "user-read-private");
            StringAssert.Contains(uri.ToString(), "user-follow-read");
            StringAssert.Contains(uri.ToString(), "user-follow-modify");
            StringAssert.Contains(uri.ToString(), "playlist-modify-public");
            StringAssert.Contains(uri.ToString(), "playlist-modify-private");
            StringAssert.Contains(uri.ToString(), "ugc-image-upload");
        }

        /// <summary>
        /// Refresh Token
        /// </summary>
        [TestMethod]
        public async Task Test_RefreshToken()
        {
            var token = await _client.RefreshToken();
            Assert.IsNotNull(token);
        }

        /// <summary>
        /// Auth Access Request
        /// </summary>
        [TestMethod]
        public void Test_AuthUserImplicit_Request()
        {
            var uri = _client.AuthUserImplicit(redirect_url, state, new Scope
            {
                UserReadPrivate = true,
                FollowRead = true,
                FollowModify = true,
                PlaylistModifyPublic = true,
                PlaylistModifyPrivate = true,
                UserGeneratedContentImageUpload = true,
                PlaybackPositionRead = true,
            });
            Assert.IsNotNull(uri);
        }

        /// <summary>
        /// Auth
        /// </summary>
        [TestMethod]
        public async Task Test_AuthAsync()
        {
            var token = await _client.AuthAsync();
            Assert.IsNotNull(token);
        }

        /// <summary>
        /// Test Refresh
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_TestRefresh()
        {
            var oldToken = _client.GetToken();
            var newToken = await _client.RefreshToken(oldToken);
            Assert.AreNotEqual(oldToken.Token, newToken.Token);
        }

        /// <summary>
        /// Test Auth User Code
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public void Test_AuthUserCode_Uri()
        {
            var uri = _client.AuthUserCode(
                redirect_url, 
                state, new Scope
                {
                    UserReadPrivate = true,
                    FollowRead = true,
                    FollowModify = true,
                    PlaylistModifyPublic = true,
                    PlaylistModifyPrivate = true,
                    UserGeneratedContentImageUpload = true,
                    PlaybackPositionRead = true,
                });
            Assert.IsNotNull(uri);
        }
        #endregion Authenticate

        #region Follow
        /// <summary>
        /// Get Following State for Artists/Users
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_AuthLookupFollowingState_Artist()
        {
            // "Ariana Grande"
            var result = await _client.AuthLookupFollowingStateAsync(
                new List<string> { "66CXWjxzNUsdJxJ2JdwvnR" },
                Enums.FollowType.Artist);
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Get Following State for Artists/Users
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_AuthLookupFollowingState_User()
        {
            var result = await _client.AuthLookupFollowingStateAsync(
                new List<string> { "spotify" },
                Enums.FollowType.User);
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
                "5V4ZnvICxETxIFFHoLEcur",
                new UriListRequest()
                {
                    Uris = new List<string>
                  {
                      "spotify:episode:0QKbvamKUnMbHuEbIRZaG9"
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
                Tracks = new List<PositionUriRequest>()
                {
                    new PositionUriRequest()
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
            Assert.IsTrue(result.Count > 0);
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
        public async Task Test_AuthLookupUserPlaylists()
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
            Assert.IsTrue(result.Success);
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

        #region Library
        /// <summary>
        /// Check User's Saved Albums
        /// </summary>
        [TestMethod]
        public async Task Test_AuthLookupCheckUserSavedAlbums()
        {
            var result = await _client.AuthLookupCheckUserSavedAlbumsAsync(
                new List<string>
                {
                    "01w0dp5OVOgviPw2HHXB3M"
                });
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Save Tracks for User
        /// </summary>
        [TestMethod]
        public async Task Test_AuthUserSaveTracks()
        {
            var result = await _client.AuthSaveUserTracksAsync(
            new List<string>
            {
                "2XWjPtKdi5sucFYtVav07d"
            });
            Assert.IsTrue(result.Success);
        }

        /// <summary>
        /// Remove Albums for Current User
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_AuthRemoveUserAlbums()
        {
            var result = await _client.AuthRemoveUserAlbumsAsync(new List<string>
            {
                "2C5HYffMBumERQlNfceyrO"
            });
            Assert.IsTrue(result.Success);
        }

        /// <summary>
        /// Save Albums for Current User
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_AuthSaveUserAlbums()
        {
            var result = await _client.AuthSaveUserAlbumsAsync(new List<string>
            {
                "2C5HYffMBumERQlNfceyrO"
            });
            Assert.IsTrue(result.Success);
        }

        /// <summary>
        /// Remove User's Saved Tracks
        /// </summary>
        [TestMethod]
        public async Task Test_AuthUserRemoveTracks()
        {
            var result = await _client.AuthRemoveUserTracksAsync(
            new List<string>
            {
                "2XWjPtKdi5sucFYtVav07d"
            });
            Assert.IsTrue(result.Success);
        }

        /// <summary>
        /// Get User's Saved Albums
        /// </summary>
        [TestMethod]
        public async Task Test_AuthLookupUserSavedAlbums()
        {
            var result = await _client.AuthLookupUserSavedAlbumsAsync();
            Assert.IsNotNull(result?.Items);
            Assert.IsTrue(result.Items.Count > 0);
        }

        /// <summary>
        /// Get User's Saved Albums
        /// </summary>
        [TestMethod]
        public async Task Test_AuthLookupUserSavedTracks()
        {
            var result = await _client.AuthLookupUserSavedTracksAsync();
            Assert.IsNotNull(result?.Items);
            Assert.IsTrue(result.Items.Count > 0);
        }

        /// <summary>
        /// Check User's Saved Tracks
        /// </summary>
        [TestMethod]
        public async Task Test_AuthLookupCheckUserSavedTracks()
        {
            var result = await _client.AuthLookupCheckUserSavedTracksAsync(
                new List<string>
                {
                    "1gWFMuLlQDN18GJDtSy049"
                });
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Get User's Saved Shows
        /// </summary>
        [TestMethod]
        public async Task Test_AuthLookupUserSavedShows()
        {
            var result = await _client.AuthLookupUserSavedShowsAsync();
            Assert.IsNotNull(result?.Items);
            Assert.IsTrue(result.Items.Count > 0);
        }

        /// <summary>
        /// Check User's Saved Shows
        /// </summary>
        [TestMethod]
        public async Task Test_AuthLookupCheckUserSavedShows()
        {
            var result = await _client.AuthLookupCheckUserSavedShowsAsync(
                new List<string>
                {
                    "4r157jjrIV0bzS6UxhN07i"
                });
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Save Shows for Current User
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_AuthSaveUserShows()
        {
            var result = await _client.AuthSaveUserShowsAsync(new List<string>
            {
                "5tz9eGgXtNHmq3WVD3EwYx"
            });
            Assert.IsTrue(result.Success);
        }

        /// <summary>
        /// Remove Shows for Current User
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_AuthRemoveUserShows()
        {
            var result = await _client.AuthRemoveUserShowsAsync(new List<string>
            {
                "5tz9eGgXtNHmq3WVD3EwYx"
            });
            Assert.IsTrue(result.Success);
        }
        #endregion Library

        #region Player
        /// <summary>
        /// Add an Item to the User's Playback Queue
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_AuthUserPlaybackAddToQueueAsync()
        {
            var result = await _client.AuthUserPlaybackAddToQueueAsync(
                "spotify:track:73SpzrcaHk0RQPFP73vqVR");
            Assert.IsTrue(result.Success);
        }

        /// <summary>
        /// Skip User’s Playback To Next Track
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_AuthUserPlaybackNextTrack()
        {
            var result = await _client.AuthUserPlaybackNextTrackAsync();
            Assert.IsTrue(result.Success);
        }

        /// <summary>
        /// Seek To Position In Currently Playing Track
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_AuthUserPlaybackSeekTrack()
        {
            var timespan = TimeSpan.FromSeconds(5);
            var result = await _client.AuthUserPlaybackSeekTrackAsync(
                (int)timespan.TotalMilliseconds);
            Assert.IsTrue(result.Success);
        }

        /// <summary>
        /// Get a User's Available Devices
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_AuthLookupUserPlaybackDevices()
        {
            var result = await _client.AuthLookupUserPlaybackDevicesAsync();
            Assert.IsNotNull(result?.Items);
            Assert.IsTrue(result.Items.Count > 0);
        }

        /// <summary>
        /// Toggle Shuffle For User’s Playback
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_AuthUserPlaybackToggleShuffle()
        {
            var result = await _client.AuthUserPlaybackToggleShuffleAsync(
                false);
            Assert.IsTrue(result.Success);
        }

        /// <summary>
        /// Transfer a User's Playback
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_AuthUserPlaybackTransfer()
        {
            var result = await _client.AuthUserPlaybackTransferAsync(
                new DevicesRequest()
            {
                DeviceIds = new List<string>
                {
                    "074aabdf5bcba93cb3fb8395151adae1cabbb63b"
                },
                Play = true
            });
            Assert.IsTrue(result.Success);
        }

        /// <summary>
        /// Get Current User's Recently Played Tracks
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_AuthLookupUserRecentlyPlayedTracks()
        {
            var result = await _client.AuthLookupUserRecentlyPlayedTracksAsync();
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Start/Resume a User's Playback
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_AuthUserPlaybackStartResume()
        {
            var result = await _client.AuthUserPlaybackStartResumeAsync(
                new PlaybackRequest()
                {
                    ContextUri = "spotify:album:3lwu4qs7RJEBRfsDL7aUwu",
                    Offset = new PositionRequest()
                    {
                        Position = 3
                    },
                    Position = 0
                },
                null);
            Assert.IsTrue(result.Success);
        }

        /// <summary>
        /// Set Repeat Mode On User’s Playback
        /// </summary>
        [TestMethod]
        public async Task Test_AuthUserPlaybackSetRepeatMode()
        {
            var result = await _client.AuthUserPlaybackSetRepeatModeAsync(
                Enums.RepeatState.Track);
            Assert.IsTrue(result.Success);
        }

        /// <summary>
        /// Pause a User's Playback
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_AuthUserPlaybackPause()
        {
            var result = await _client.AuthUserPlaybackPauseAsync();
            Assert.IsTrue(result.Success);
        }

        /// <summary>
        /// Skip User’s Playback To Previous Track
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_AuthUserPlaybackPreviousTrack()
        {
            var result = await _client.AuthUserPlaybackPreviousTrackAsync();
            Assert.IsTrue(result.Success);
        }

        /// <summary>
        /// Get Information About The User's Current Playback
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_AuthLookupUserPlaybackCurrent()
        {
            var result = await _client.AuthLookupUserPlaybackCurrentAsync(
                additionalTypes:
                    new List<string> { "track", "episode" });
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Get the User's Currently Playing Track
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_AuthLookupUserPlaybackCurrentTrack()
        {
            var result = await _client.AuthLookupUserPlaybackCurrentTrackAsync(
                additionalTypes:
                    new List<string> { "track", "episode" });
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Set Volume For User's Playback
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_AuthUserPlaybackSetVolume()
        {
            var result = await _client.AuthUserPlaybackSetVolumeAsync(50);
            Assert.IsTrue(result.Success);
        }
        #endregion Player

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
