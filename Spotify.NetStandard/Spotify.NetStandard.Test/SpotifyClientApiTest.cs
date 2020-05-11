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
using System.Text;
using System.Threading.Tasks;

namespace Spotify.NetStandard.Test
{
    [TestClass]
    public class SpotifyClientApiTest
    {
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

        #region Search API
        /// <summary>
        /// Search for an Item
        /// </summary>
        [TestMethod]
        public async Task Test_SearchForItem()
        {
            var result = await _client.Api.SearchForItemAsync(
                "Moonlight Shadow", 
                new SearchType() { Track = true });
            Assert.IsNotNull(result.Tracks);
            Assert.IsTrue(result.Tracks.Count > 0);
        }
        #endregion Search API

        #region Browse API
        /// <summary>
        /// Get All Categories
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_GetAllCategories()
        {
            var result = await _client.Api.GetAllCategoriesAsync();
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Items.Count > 0);
        }

        /// <summary>
        /// Get a Category
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_GetCategory()
        {
            var result = await _client.Api.GetCategoryAsync(
                "pop");
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Get a Category's Playlists
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_GetCategoryPlaylists()
        {
            var result = await _client.Api.GetCategoryPlaylistsAsync(
                "pop");
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
        }

        /// <summary>
        /// Get Recommendations
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_GetRecommendations()
        {
            var result = await _client.Api.GetRecommendationsAsync(
                seedArtists: new List<string> { "562Od3CffWedyz2BbeYWVn" });
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Tracks.Count > 0);
        }

        /// <summary>
        /// Get Recommendation Genres
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_GetRecommendationGenres()
        {
            var result = await _client.Api.GetRecommendationGenresAsync();
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Genres.Count > 0);
        }

        /// <summary>
        /// Get All New Releases
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_GetAllNewReleases()
        {
            var result = await _client.Api.GetAllNewReleasesAsync();
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Items.Count > 0);
        }

        /// <summary>
        /// Get All Featured Playlists
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_GetAllFeaturedPlaylists()
        {
            var result = await _client.Api.GetAllFeaturedPlaylistsAsync();
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Items.Count > 0);
        }
        #endregion Browse API

        #region Follow API
        /// <summary>
        /// Get Following State for Artists/Users
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_GetFollowingStateForArtistsOrUsers()
        {
            // "Ariana Grande"
            var result = await _client.Api.GetFollowingStateForArtistsOrUsersAsync(
                new List<string> { "66CXWjxzNUsdJxJ2JdwvnR" },
                Enums.FollowType.Artist);
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Check if Users Follow a Playlist
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_CheckUsersFollowingPlaylist()
        {
            var result = await _client.Api.CheckUsersFollowingPlaylistAsync(
                new List<string> { "jmperezperez" }, "3cEYpjA9oz9GiPac4AsH4n");
            Assert.IsTrue(result.TrueForAll(t => t));
        }

        /// <summary>
        /// Follow Artists or Users
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_FollowArtistsOrUsersAsync()
        {
            // "Ariana Grande"
            var result = await _client.Api.FollowArtistsOrUsersAsync(
                new List<string> { "66CXWjxzNUsdJxJ2JdwvnR" },
                Enums.FollowType.Artist);
            Assert.IsTrue(result.Success);
        }

        /// <summary>
        /// Follow a Playlist
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_FollowPlaylist()
        {
            var result = await _client.Api.FollowPlaylistAsync(
                "37i9dQZF1DWUS3jbm4YExP");
            Assert.IsTrue(result.Success);
        }

        /// <summary>
        /// Get User's Followed Artists
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_GetUsersFollowedArtists()
        {
            var result = await _client.Api.GetUsersFollowedArtistsAsync();
            Assert.IsNotNull(result.Items);
        }

        /// <summary>
        /// Unfollow Artists or Users
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_UnfollowArtist()
        {
            // "Ariana Grande"
            var result = await _client.Api.UnfollowArtistsOrUsersAsync(
                new List<string> { "66CXWjxzNUsdJxJ2JdwvnR" },
                Enums.FollowType.Artist);
            Assert.IsTrue(result.Success);
        }

        /// <summary>
        /// Unfollow Playlist
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_UnfollowPlaylist()
        {
            var result = await _client.Api.UnfollowPlaylistAsync(
                "37i9dQZF1DWUS3jbm4YExP");
            Assert.IsTrue(result.Success);
        }
        #endregion Follow API

        #region Playlists API
        /// <summary>
        /// Get a Playlist
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_GetPlaylist()
        {
            var result = await _client.Api.GetPlaylistAsync(
                "3cEYpjA9oz9GiPac4AsH4n");
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Get a Playlist's Tracks
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_GetPlaylistTracks()
        {
            var result = await _client.Api.GetPlaylistTracksAsync(
                "3cEYpjA9oz9GiPac4AsH4n");
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Items.Count > 0);
        }

        /// <summary>
        /// Add Tracks to a Playlist
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_AddTracksToPlaylist()
        {
            var result = await _client.Api.AddTracksToPlaylistAsync(
            "7D4Epnvxhc8l6NpooJqYXk",
            new List<string>
            {
                "spotify:track:2zzdnRWE3z6QP3FoVlnWHO",
            }, 
            0);
            Assert.IsTrue(result.Success);
        }

        /// <summary>
        /// Remove Tracks from a Playlist
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_RemoveTracksFromPlaylist()
        {
            var result = await _client.Api.RemoveTracksFromPlaylistAsync(
            "7D4Epnvxhc8l6NpooJqYXk",
            new List<string>()
            {
                "spotify:track:2zzdnRWE3z6QP3FoVlnWHO"
            });
            Assert.IsTrue(result.Success);
        }

        /// <summary>
        /// Get a Playlist Cover Image
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_GetPlaylistCoverImage()
        {
            var result = await _client.Api.GetPlaylistCoverImageAsync(
                "3cEYpjA9oz9GiPac4AsH4n");
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
        }

        /// <summary>
        /// Upload a Custom Playlist Cover Image
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_UploadCustomPlaylistCoverImage()
        {
            string filename = @"C:\Test\PlaylistCover.jpg";
            var file = File.ReadAllBytes(filename);
            var result = await _client.Api.UploadCustomPlaylistCoverImageAsync(
                "1L6ECMsEDXmrp1qfH5htHZ",
                file);
            Assert.IsTrue(result.Success);
        }

        /// <summary>
        /// Get a List of Current User's Playlists 
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_GetUserPlaylistsAsync()
        {
            var result = await _client.Api.GetUserPlaylistsAsync();
            Assert.IsNotNull(result?.Items);
            Assert.IsTrue(result.Items.Count > 0);
        }

        /// <summary>
        /// Change a Playlist's Details
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_ChangePlaylistDetails()
        {
            var result = await _client.Api.ChangePlaylistDetailsAsync(
            "7D4Epnvxhc8l6NpooJqYXk",
            name: "Spotify.NetStandard",
            description: "Spotify Client Auth Test");
            Assert.IsTrue(result.Success);
        }

        /// <summary>
        /// Get a List of a User's Playlists
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_GetUserPlaylists()
        {
            var result = await _client.Api.GetUserPlaylistsAsync(
                "Spotify");
            Assert.IsNotNull(result?.Items);
            Assert.IsTrue(result.Items.Count > 0);
        }

        /// <summary>
        /// Replace a Playlist's Tracks
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_ReplacePlaylistTracks()
        {
            var result = await _client.Api.ReplacePlaylistTracksAsync(
            "7D4Epnvxhc8l6NpooJqYXk",
            new List<string>
            {
                "spotify:track:2RlgNHKcydI9sayD2Df2xp"
            });
            Assert.IsTrue(result.Success);
        }

        /// <summary>
        /// Create a Playlist
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_CreatePlaylist()
        {
            var result = await _client.Api.CreatePlaylistAsync(
            "doa31nixtl3kmy7uf8ov88sy0",
             name: "Spotify.Created",
             description: "Spotify Client Auth Test");
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Reorder a Playlist's Tracks
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_ReorderPlaylistTracks()
        {
            var result = await _client.Api.ReorderPlaylistTracksAsync(
            "1L6ECMsEDXmrp1qfH5htHZ",
            rangeStart: 2,
            rangeLength: 1,
            insertBefore: 0);
            Assert.IsTrue(result.Success);
        }
        #endregion Playlists API

        #region Library API
        /// <summary>
        /// Check User's Saved Albums
        /// </summary>
        [TestMethod]
        public async Task Test_CheckUserSavedAlbums()
        {
            var result = await _client.Api.CheckUserSavedAlbumsAsync(
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
        public async Task Test_SaveUserTracks()
        {
            var result = await _client.Api.SaveUserTracksAsync(
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
        public async Task Test_RemoveUserAlbums()
        {
            var result = await _client.Api.RemoveUserAlbumsAsync(new List<string>
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
        public async Task Test_SaveUserAlbums()
        {
            var result = await _client.Api.SaveUserAlbumsAsync(new List<string>
            {
                "2C5HYffMBumERQlNfceyrO"
            });
            Assert.IsTrue(result.Success);
        }

        /// <summary>
        /// Remove User's Saved Tracks
        /// </summary>
        [TestMethod]
        public async Task Test_RemoveUserTracks()
        {
            var result = await _client.Api.RemoveUserTracksAsync(
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
        public async Task Test_GetUserSavedAlbums()
        {
            var result = await _client.Api.GetUserSavedAlbumsAsync();
            Assert.IsNotNull(result?.Items);
            Assert.IsTrue(result.Items.Count > 0);
        }

        /// <summary>
        /// Get User's Saved Albums
        /// </summary>
        [TestMethod]
        public async Task Test_GetUserSavedTracks()
        {
            var result = await _client.Api.GetUserSavedTracksAsync();
            Assert.IsNotNull(result?.Items);
            Assert.IsTrue(result.Items.Count > 0);
        }

        /// <summary>
        /// Check User's Saved Tracks
        /// </summary>
        [TestMethod]
        public async Task Test_CheckUserSavedTracks()
        {
            var result = await _client.Api.CheckUserSavedTracksAsync(
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
        public async Task Test_GetUserSavedShowsAsync()
        {
            var result = await _client.Api.GetUserSavedShowsAsync();
            Assert.IsNotNull(result?.Items);
            Assert.IsTrue(result.Items.Count > 0);
        }

        /// <summary>
        /// Check User's Saved Shows
        /// </summary>
        [TestMethod]
        public async Task Test_CheckUserSavedShows()
        {
            var result = await _client.Api.CheckUserSavedShowsAsync(
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
        public async Task Test_SaveUserShows()
        {
            var result = await _client.Api.SaveUserShowsAsync(new List<string>
            {
                "5tz9eGgXtNHmq3WVD3EwYx"
            });
            Assert.IsTrue(result.Success);
        }

        /// <summary>
        /// Remove User's Saved Shows
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_RemoveUserShows()
        {
            var result = await _client.Api.RemoveUserShowsAsync(new List<string>
            {
                "5tz9eGgXtNHmq3WVD3EwYx"
            });
            Assert.IsTrue(result.Success);
        }
        #endregion Library API

        #region Artists API
        /// <summary>
        /// Get Multiple Artists
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_GetMultipleArtists()
        {
            List<string> ids = new List<string>
            {
                "0oSGxfWSnnOXhD2fKuz2Gy", "3dBVyJ7JuOMt4GE9607Qin"
            };
            var result = await _client.Api.GetMultipleArtistsAsync(ids);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count == ids.Count);
        }

        /// <summary>
        /// Get an Artist
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_GetArtist()
        {
            var result = await _client.Api.GetArtistAsync(
                "0OdUWJ0sBjDrqHygGUXeCF");
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Get an Artist's Albums
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_GetArtistAlbums()
        {
            var result = await _client.Api.GetArtistAlbumsAsync(
                "1vCWHaC5f2uS3yhpwWbIA6");
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
        }

        /// <summary>
        /// Get an Artist's Top Tracks
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_GetArtistTopTracks()
        {
            var result = await _client.Api.GetArtistTopTracksAsync(
                "43ZHCT0cAZBISjO8DG9PnE", "GB");
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
        }

        /// <summary>
        /// Get an Artist's Related Artists
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_GetArtistRelatedArtists()
        {
            var result = await _client.Api.GetArtistRelatedArtistsAsync(
                "43ZHCT0cAZBISjO8DG9PnE");
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
        }
        #endregion Artists API

        #region Player API
        /// <summary>
        /// Add an Item to the User's Playback Queue
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_UserPlaybackAddToQueueAsync()
        {
            var result = await _client.Api.UserPlaybackAddToQueueAsync(
                "spotify:track:73SpzrcaHk0RQPFP73vqVR");
            Assert.IsTrue(result.Success);
        }

        /// <summary>
        /// Skip User’s Playback To Next Track
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_UserPlaybackNextTrack()
        {
            var result = await _client.Api.UserPlaybackNextTrackAsync();
            Assert.IsTrue(result.Success);
        }

        /// <summary>
        /// Seek To Position In Currently Playing Track
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_UserPlaybackSeekTrack()
        {
            var timespan = TimeSpan.FromSeconds(5);
            var result = await _client.Api.UserPlaybackSeekTrackAsync(
                (int)timespan.TotalMilliseconds);
            Assert.IsTrue(result.Success);
        }

        /// <summary>
        /// Get a User's Available Devices
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_GetUserPlaybackDevices()
        {
            var result = await _client.Api.GetUserPlaybackDevicesAsync();
            Assert.IsNotNull(result?.Items);
            Assert.IsTrue(result.Items.Count > 0);
        }

        /// <summary>
        /// Toggle Shuffle For User’s Playback
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_UserPlaybackToggleShuffle()
        {
            var result = await _client.Api.UserPlaybackToggleShuffleAsync(
                false);
            Assert.IsTrue(result.Success);
        }

        /// <summary>
        /// Transfer a User's Playback
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_UserPlaybackTransfer()
        {
            var result = await _client.Api.UserPlaybackTransferAsync(
                new List<string>
                {
                   "7fd36d2a1eba3db7b0ca548ae6dfeba9144f283a"
                },
                true);
            Assert.IsTrue(result.Success);
        }

        /// <summary>
        /// Get Current User's Recently Played Tracks
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_GetUserRecentlyPlayedTracks()
        {
            var result = await _client.Api.GetUserRecentlyPlayedTracksAsync();
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Start/Resume a User's Playback
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_UserPlaybackStartResume()
        {
            var result = await _client.Api.UserPlaybackStartResumeAsync(
                contextUri: "spotify:album:3lwu4qs7RJEBRfsDL7aUwu",
                offsetPosition: 3,
                position: 0);
            Assert.IsTrue(result.Success);
        }

        /// <summary>
        /// Set Repeat Mode On User’s Playback
        /// </summary>
        [TestMethod]
        public async Task Test_UserPlaybackSetRepeatMode()
        {
            var result = await _client.Api.UserPlaybackSetRepeatModeAsync(
                Enums.RepeatState.Track);
            Assert.IsTrue(result.Success);
        }

        /// <summary>
        /// Pause a User's Playback
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_UserPlaybackPause()
        {
            var result = await _client.Api.UserPlaybackPauseAsync();
            Assert.IsTrue(result.Success);
        }

        /// <summary>
        /// Skip User’s Playback To Previous Track
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_UserPlaybackPreviousTrack()
        {
            var result = await _client.Api.UserPlaybackPreviousTrackAsync();
            Assert.IsTrue(result.Success);
        }

        /// <summary>
        /// Get Information About The User's Current Playback
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_GetUserPlaybackCurrent()
        {
            var result = await _client.Api.GetUserPlaybackCurrentAsync(
                additionalTypes:
                    new List<string> { "track", "episode" });
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Get the User's Currently Playing Track
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_GetUserPlaybackCurrentTrack()
        {
            var result = await _client.Api.GetUserPlaybackCurrentTrackAsync(
                additionalTypes:
                    new List<string> { "track", "episode" });
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Set Volume For User's Playback
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_UserPlaybackSetVolume()
        {
            var result = await _client.Api.UserPlaybackSetVolumeAsync(50);
            Assert.IsTrue(result.Success);
        }
        #endregion Player API

        #region Personalisation API
        /// <summary>
        /// Get a User's Top Artists
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_GetUserTopArtists()
        {
            var result = await _client.Api.GetUserTopArtistsAsync();
            Assert.IsNotNull(result);
            Assert.IsNotNull(result?.Items);
            Assert.IsTrue(result.Items.Count > 0);
        }

        /// <summary>
        /// Get a User's Top Tracks
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_GetUserTopTracks()
        {
            var result = await _client.Api.GetUserTopTracksAsync();
            Assert.IsNotNull(result?.Items);
            Assert.IsTrue(result.Items.Count > 0);
        }
        #endregion Personalisation API

        #region User Profile API
        /// <summary>
        /// Get a User's Profile
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_LookupUserProfile()
        {
            var result = await _client.Api.GetUserProfileAsync(
                "jmperezperez");
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Get Current User's Profile
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_GetUserProfile()
        {
            var result = await _client.Api.GetUserProfileAsync();
            Assert.IsNotNull(result);
        }
        #endregion User Profile API

        #region Albums API
        /// <summary>
        /// Get Multiple Albums
        /// </summary>
        [TestMethod]
        public async Task Test_GetMultipleAlbums()
        {
            List<string> ids = new List<string>
            {
                "41MnTivkwTO3UUJ8DrqEJJ",
                "6JWc4iAiJ9FjyK0B59ABb4"
            };
            var result = await _client.Api.GetMultipleAlbumsAsync(ids);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count == ids.Count);
        }

        /// <summary>
        /// Get an Album
        /// </summary>
        [TestMethod]
        public async Task Test_GetAlbum()
        {
            var result = await _client.Api.GetAlbumAsync(
                "1ZGxGu4fMROqmZsFSoepeE");
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Get an Album's Tracks
        /// </summary>
        [TestMethod]
        public async Task Test_GetAlbumTracks()
        {
            var result = await _client.Api.GetAlbumTracksAsync(
                "1ZGxGu4fMROqmZsFSoepeE");
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
        }
        #endregion Albums API

        #region Tracks API
        /// <summary>
        /// Get Audio Features for a Track
        /// </summary>
        [TestMethod]
        public async Task Test_GetTrackAudioFeatures()
        {
            var result = await _client.Api.GetTrackAudioFeaturesAsync(
                "1cTZMwcBJT0Ka3UJPXOeeN");
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Get a Track
        /// </summary>
        [TestMethod]
        public async Task Test_GetTrack()
        {
            var result = await _client.Api.GetTrackAsync(
                "1cTZMwcBJT0Ka3UJPXOeeN");
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Get Audio Analysis for a Track
        /// </summary>
        [TestMethod]
        public async Task Test_GetTrackAudioAnalysis()
        {
            var result = await _client.Api.GetTrackAudioAnalysisAsync(
                "1cTZMwcBJT0Ka3UJPXOeeN");
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Get Audio Features for Several Tracks
        /// </summary>
        [TestMethod]
        public async Task Test_GetSeveralTracksAudioFeatures()
        {
            List<string> ids = new List<string>
            {
                "3n3Ppam7vgaVa1iaRUc9Lp",
                "3twNvmDtFQtAd5gMKedhLD"
            };
            var result = await _client.Api.GetSeveralTracksAudioFeaturesAsync(ids);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count == ids.Count);
        }

        /// <summary>
        /// Get Several Tracks
        /// </summary>
        [TestMethod]
        public async Task Test_GetSeveralTracks()
        {
            List<string> ids = new List<string>
            {
                "3n3Ppam7vgaVa1iaRUc9Lp",
                "3twNvmDtFQtAd5gMKedhLD"
            };
            var result = await _client.Api.GetSeveralTracksAsync(ids);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count == ids.Count);
        }
        #endregion Tracks API

        #region Episodes API
        /// <summary>
        /// Get an Episode
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_GetEpisode()
        {
            var result = await _client.Api.GetEpisodeAsync(
                "79hCFrLsRSD7VlDYXcrCVt");
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Get Multiple Episodes
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_GetMultipleEpisodes()
        {
            List<string> ids = new List<string>
            {
                "79hCFrLsRSD7VlDYXcrCVt",
                "6EbtlqXrvhCBic2TpeaalK"
            };
            var result = await _client.Api.GetMultipleEpisodesAsync(ids);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count == ids.Count);
        }
        #endregion Episodes API

        #region Shows API
        /// <summary>
        /// Get a Show
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_GetShow()
        {
            var result = await _client.Api.GetShowAsync(
                "4r157jjrIV0bzS6UxhN07i");
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Get Multiple Shows
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_GetMultipleShows()
        {
            List<string> ids = new List<string>
            {
                "4r157jjrIV0bzS6UxhN07i",
                "2GmNzw8t4uG70rn4XG9zcC"
            };
            var result = await _client.Api.GetMultipleShowsAsync(ids);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count == ids.Count);
        }

        /// <summary>
        /// Get a Show's Episodes
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_GetShowEpisodes()
        {
            var results = await _client.Api.GetShowEpisodesAsync(
                "4r157jjrIV0bzS6UxhN07i");
            Assert.IsNotNull(results);
            Assert.IsTrue(results.Items.Count > 0);
        }
        #endregion Shows API
    }
}
