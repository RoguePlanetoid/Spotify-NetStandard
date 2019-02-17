using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Spotify.NetStandard.Client;
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
                "decades");
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
                "decades");
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

        #region Playlists API
        /// <summary>
        /// Get a Playlist
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_GetPlaylist()
        {
            var result = await _client.Api.GetPlaylistAsync(
                "37i9dQZF1DXatMjChPKgBk");
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
                "37i9dQZF1DXatMjChPKgBk");
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
        }

        #endregion Playlists API

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
    }
}
