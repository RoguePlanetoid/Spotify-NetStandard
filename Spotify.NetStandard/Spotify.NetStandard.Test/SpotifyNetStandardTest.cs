using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Spotify.NetStandard.Client;
using Spotify.NetStandard.Client.Interfaces;
using Spotify.NetStandard.Enums;
using Spotify.NetStandard.Responses;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Spotify.NetStandard.Test
{
    [TestClass]
    public class SpotifyNetStandardTest
    {
        private ISpotifyClient _client = null;
        private ContentResponse _content = null;
        private LookupResponse _list = null;

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

        #region Albums
        /// <summary>
        /// Get an Album
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_Album_LookupAsync()
        {
            Album item = await _client.LookupAsync<Album>(
                "1ZGxGu4fMROqmZsFSoepeE", LookupType.Albums);
            Assert.IsNotNull(item);
        }

        /// <summary>
        /// Get an Album's Tracks
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_Album_Tracks_LookupAsync()
        {
            Paging<Track> list = await _client.LookupAsync<Paging<Track>>(
                "1ZGxGu4fMROqmZsFSoepeE", LookupType.AlbumTracks);
            Assert.IsNotNull(list);
            Assert.IsTrue(list.Items.Count > 0);
        }

        /// <summary>
        /// Get Several Albums
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_Albums_LookupAsync()
        {
            List<string> ids = new List<string>
            {
                "41MnTivkwTO3UUJ8DrqEJJ",
                "6JWc4iAiJ9FjyK0B59ABb4"
            };
            _list = await _client.LookupAsync(ids, LookupType.Albums);
            Assert.IsNotNull(_list.Albums);
            Assert.IsTrue(_list.Albums.Count == ids.Count);
        }

        /// <summary>
        /// Search for an album
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_Album_SearchAsync()
        {
            _content = await _client.SearchAsync(
                "Tubular Bells", SearchType.Album);
            Assert.IsNotNull(_content.Albums);
            Assert.IsTrue(_content.Albums.Count > 0);
        }
        #endregion Albums

        #region Artists
        /// <summary>
        /// Get an Artist
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_Artist_LookupAsync()
        {
            Artist item = await _client.LookupAsync<Artist>(
                "0OdUWJ0sBjDrqHygGUXeCF", LookupType.Artists);
            Assert.IsNotNull(item);
        }

        /// <summary>
        /// Get an Artist's Albums
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_Artist_Albums_LookupAsync()
        {
            Paging<Album> list = await _client.LookupAsync<Paging<Album>>(
                "1vCWHaC5f2uS3yhpwWbIA6", LookupType.ArtistAlbums);
            Assert.IsNotNull(list);
            Assert.IsTrue(list.Items.Count > 0);
        }

        /// <summary>
        /// Get an Artist's Top Tracks
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_Artist_TopTracksAsync()
        {
            _list = await _client.GetArtistTopTracksAsync(
                "43ZHCT0cAZBISjO8DG9PnE", "GB");
            Assert.IsNotNull(_list.Tracks);
            Assert.IsTrue(_list.Tracks.Count > 0);
        }

        /// <summary>
        /// Get an Artist's Related Artists
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_Artist_RelatedArtistsAsync()
        {
            _list = await _client.GetArtistRelatedArtistsAsync(
                "43ZHCT0cAZBISjO8DG9PnE");
            Assert.IsNotNull(_list.Artists);
            Assert.IsTrue(_list.Artists.Count > 0);
        }

        /// <summary>
        /// Get Several Artists
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_Artists_LookupAsync()
        {
            List<string> ids = new List<string>
            {
                "0oSGxfWSnnOXhD2fKuz2Gy", "3dBVyJ7JuOMt4GE9607Qin"
            };
            _list = await _client.LookupAsync(ids, LookupType.Artists);
            Assert.IsNotNull(_list.Artists);
            Assert.IsTrue(_list.Artists.Count == ids.Count);
        }

        /// <summary>
        /// Search for an artist
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_Artist_SearchAsync()
        {
            _content = await _client.SearchAsync(
                "Mike Oldfield", SearchType.Artist);
            Assert.IsNotNull(_content.Artists);
            Assert.IsTrue(_content.Artists.Count > 0);
        }
        #endregion Artists

        #region Browse
        /// <summary>
        /// Get a Category
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_Category_LookupAsync()
        {
            Category item = await _client.LookupAsync<Category>(
                "decades", LookupType.Categories);
            Assert.IsNotNull(item);
        }

        /// <summary>
        /// Get a Category's Playlists
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_Category_Playlists_LookupAsync()
        {
            _content = await _client.LookupAsync<ContentResponse>(
                "decades", LookupType.CategoriesPlaylists);
            Assert.IsNotNull(_content.Playlists);
            Assert.IsTrue(_content.Playlists.Count > 0);
        }

        /// <summary>
        /// Get a List of Categories
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_BrowseCategoriesAsync()
        {
            _content = await _client.GetCategoriesAsync();
            Assert.IsNotNull(_content.Categories);
            Assert.IsTrue(_content.Categories.Items.Count > 0);
        }

        /// <summary>
        /// Get a List of Featured Playlists 
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_Playlists_FeaturedAsync()
        {
            _content = await _client.GetFeaturedPlaylistsAsync();
            Assert.IsNotNull(_content.Playlists);
            Assert.IsTrue(_content.Playlists.Items.Count > 0);
        }

        /// <summary>
        /// Get a List of New Releases
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_NewReleasesAsync()
        {
            _content = await _client.GetNewReleasesAsync();
            Assert.IsNotNull(_content.Albums);
            Assert.IsTrue(_content.Albums.Items.Count > 0);
        }

        /// <summary>
        /// Get Recommendations Based on Seeds
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_RecommendationAsync()
        {
            var recommendation = await _client.GetRecommendationsAsync(
                seedArtists: new string[] { "562Od3CffWedyz2BbeYWVn" });
            Assert.IsNotNull(recommendation);
            Assert.IsTrue(recommendation.Tracks.Count > 0);
        }

        /// <summary>
        /// Get Recommendation Genres
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_RecommendationGenreAsync()
        {
            AvailableGenreSeeds genre = await _client.GetRecommendationGenres();
            Assert.IsNotNull(genre);
            Assert.IsTrue(genre.Genres.Count > 0);
        }
        #endregion Browse

        #region Playlists
        /// <summary>
        /// Get a playlist tracks
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_Playlists_Tracks_LookupAsync()
        {
            Paging<PlaylistTrack> list = await _client.LookupAsync<Paging<PlaylistTrack>>(
                "37i9dQZF1DXatMjChPKgBk", LookupType.PlaylistTracks);
            Assert.IsNotNull(list);
            Assert.IsTrue(list.Count > 0);
        }

        /// <summary>
        /// Search for a playlist
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_Playlist_SearchAsync()
        {
            _content = await _client.SearchAsync(
                "Mike Oldfield", SearchType.Playlist);
            Assert.IsNotNull(_content.Playlists);
            Assert.IsTrue(_content.Playlists.Count > 0);
        }
        #endregion Playlists

        #region Tracks
        /// <summary>
        /// Get Audio Features for a Track
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_Track_AudioFeatures_LookupAsync()
        {
            AudioFeatures item = await _client.LookupAsync<AudioFeatures>(
                "1cTZMwcBJT0Ka3UJPXOeeN", LookupType.AudioFeatures);
            Assert.IsNotNull(item);
        }

        /// <summary>
        /// Get a track
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_Track_LookupAsync()
        {
            Track item = await _client.LookupAsync<Track>(
                "1cTZMwcBJT0Ka3UJPXOeeN", LookupType.Albums);
            Assert.IsNotNull(item);
        }

        /// <summary>
        /// Get Audio Analysis for a Track
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_Track_AudioAnalysis_LookupAsync()
        {
            AudioAnalysis item = await _client.LookupAsync<AudioAnalysis>(
                "1cTZMwcBJT0Ka3UJPXOeeN", LookupType.AudioAnalysis);
            Assert.IsNotNull(item);
        }

        /// <summary>
        /// Get Audio Features for Several Tracks
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_Tracks_AudioFeatures_LookupAsync()
        {
            List<string> ids = new List<string>
            {
                "3n3Ppam7vgaVa1iaRUc9Lp", "3twNvmDtFQtAd5gMKedhLD"
            };
            _list = await _client.LookupAsync(ids, LookupType.AudioFeatures);
            Assert.IsNotNull(_list.AudioFeatures);
            Assert.IsTrue(_list.AudioFeatures.Count == ids.Count);
        }

        /// <summary>
        /// Get several tracks
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_Tracks_LookupAsync()
        {
            List<string> ids = new List<string>
            {
                "3n3Ppam7vgaVa1iaRUc9Lp", "3twNvmDtFQtAd5gMKedhLD"
            };
            _list = await _client.LookupAsync(ids, LookupType.Tracks);
            Assert.IsNotNull(_list.Tracks);
            Assert.IsTrue(_list.Tracks.Count == ids.Count);
        }

        /// <summary>
        /// Search for a track
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_Track_SearchAsync()
        {
            _content = await _client.SearchAsync(
                "Moonlight Shadow", SearchType.Track);
            Assert.IsNotNull(_content.Tracks);
            Assert.IsTrue(_content.Tracks.Count > 0);
        }
        #endregion Tracks
    }
}
