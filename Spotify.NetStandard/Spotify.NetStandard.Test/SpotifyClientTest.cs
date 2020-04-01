using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Spotify.NetStandard.Client;
using Spotify.NetStandard.Client.Authentication;
using Spotify.NetStandard.Client.Authentication.Enums;
using Spotify.NetStandard.Client.Interfaces;
using Spotify.NetStandard.Enums;
using Spotify.NetStandard.Requests;
using Spotify.NetStandard.Responses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Spotify.NetStandard.Test
{
    [TestClass]
    public class SpotifyClientTest
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

        #region Albums
        /// <summary>
        /// Get an Album
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_Lookup_Album()
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
        public async Task Test_Lookup_AlbumTracks()
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
        public async Task Test_Lookup_Albums()
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
        public async Task Test_Search_Album()
        {
            _content = await _client.SearchAsync(
                "Tubular Bells", new SearchType { Album = true });
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
        public async Task Test_Lookup_Artist()
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
        public async Task Test_Lookup_ArtistAlbums()
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
        public async Task Test_Lookup_ArtistTopTracks()
        {
            _list = await _client.LookupArtistTopTracksAsync(
                "43ZHCT0cAZBISjO8DG9PnE", "GB");
            Assert.IsNotNull(_list.Tracks);
            Assert.IsTrue(_list.Tracks.Count > 0);
        }

        /// <summary>
        /// Get an Artist's Related Artists
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_Lookup_ArtistRelatedArtists()
        {
            _list = await _client.LookupArtistRelatedArtistsAsync(
                "43ZHCT0cAZBISjO8DG9PnE");
            Assert.IsNotNull(_list.Artists);
            Assert.IsTrue(_list.Artists.Count > 0);
        }

        /// <summary>
        /// Get Several Artists
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_Lookup_Artists()
        {
            List<string> ids = new List<string>
            {
                "0oSGxfWSnnOXhD2fKuz2Gy",
                "3dBVyJ7JuOMt4GE9607Qin"
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
        public async Task Test_Search_Artist()
        {
            _content = await _client.SearchAsync(
                "Mike Oldfield", new SearchType { Artist = true });
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
        public async Task Test_Lookup_Category()
        {
            Category item = await _client.LookupAsync<Category>(
                "pop", LookupType.Categories);
            Assert.IsNotNull(item);
        }

        /// <summary>
        /// Get a Category's Playlists
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_Lookup_CategoryPlaylists()
        {
            _content = await _client.LookupAsync<ContentResponse>(
                "pop", LookupType.CategoriesPlaylists);
            Assert.IsNotNull(_content.Playlists);
            Assert.IsTrue(_content.Playlists.Count > 0);
        }

        /// <summary>
        /// Get a List of Categories
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_Lookup_Categories()
        {
            _content = await _client.LookupAllCategoriesAsync();
            Assert.IsNotNull(_content.Categories);
            Assert.IsTrue(_content.Categories.Items.Count > 0);
        }

        /// <summary>
        /// Get a List of Featured Playlists 
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_Lookup_FeaturedPlaylists()
        {
            _content = await _client.LookupFeaturedPlaylistsAsync();
            Assert.IsNotNull(_content.Playlists);
            Assert.IsTrue(_content.Playlists.Items.Count > 0);
        }

        /// <summary>
        /// Get a List of New Releases
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_Lookup_NewReleases()
        {
            _content = await _client.LookupNewReleasesAsync();
            Assert.IsNotNull(_content.Albums);
            Assert.IsTrue(_content.Albums.Items.Count > 0);
        }

        /// <summary>
        /// Get Recommendations Based on Seeds
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_Lookup_Recommendation()
        {
            var recommendation = await _client.LookupRecommendationsAsync(
                seedArtists: new List<string> { "562Od3CffWedyz2BbeYWVn" });
            Assert.IsNotNull(recommendation);
            Assert.IsTrue(recommendation.Tracks.Count > 0);
        }

        /// <summary>
        /// Get Recommendation Genres
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_Lookup_RecommendationGenres()
        {
            AvailableGenreSeeds genre = await _client.LookupRecommendationGenres();
            Assert.IsNotNull(genre);
            Assert.IsTrue(genre.Genres.Count > 0);
        }
        #endregion Browse

        #region Playlists
        /// <summary>
        /// Get a Playlist
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_Lookup_Playlist()
        {
            Playlist playlist = await _client.LookupAsync<Playlist>(
                "37i9dQZF1DXatMjChPKgBk", LookupType.Playlist);
            Assert.IsNotNull(playlist);
        }

        /// <summary>
        /// Get a playlist tracks
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_Lookup_PlaylistsTracks()
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
        public async Task Test_Search_Playlist()
        {
            _content = await _client.SearchAsync(
                "Mike Oldfield", new SearchType { Playlist = true });
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
        public async Task Test_Lookup_Track_AudioFeatures()
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
        public async Task Test_Lookup_Track()
        {
            Track item = await _client.LookupAsync<Track>(
                "1cTZMwcBJT0Ka3UJPXOeeN", LookupType.Tracks);
            Assert.IsNotNull(item);
        }

        /// <summary>
        /// Get Audio Analysis for a Track
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_Lookup_Track_AudioAnalysis()
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
        public async Task Test_Lookup_Tracks_AudioFeatures()
        {
            List<string> ids = new List<string>
            {
                "3n3Ppam7vgaVa1iaRUc9Lp",
                "3twNvmDtFQtAd5gMKedhLD"
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
        public async Task Test_Lookup_Tracks()
        {
            List<string> ids = new List<string>
            {
                "3n3Ppam7vgaVa1iaRUc9Lp",
                "3twNvmDtFQtAd5gMKedhLD"
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
        public async Task Test_Search_Track()
        {
            _content = await _client.SearchAsync(
                "Moonlight Shadow", new SearchType() { Track = true });
            Assert.IsNotNull(_content.Tracks);
            Assert.IsTrue(_content.Tracks.Count > 0);
        }
        #endregion Tracks

        #region Episodes
        /// <summary>
        /// Get an Episode
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_Lookup_Episode()
        {
            Episode item = await _client.LookupAsync<Episode>(
                "79hCFrLsRSD7VlDYXcrCVt", LookupType.Episodes);
            Assert.IsNotNull(item);
        }

        /// <summary>
        /// Get Multiple Episodes
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_Lookup_Episodes()
        {
            List<string> ids = new List<string>
            {
                "79hCFrLsRSD7VlDYXcrCVt",
                "6EbtlqXrvhCBic2TpeaalK"
            };
            _list = await _client.LookupAsync(ids, LookupType.Episodes);
            Assert.IsNotNull(_list.Episodes);
            Assert.IsTrue(_list.Episodes.Count == ids.Count);
        }

        /// <summary>
        /// Search for an Episode
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_Search_Episode()
        {
            _content = await _client.SearchAsync(
                "Andrew Jackson", new SearchType() { Episode = true });
            Assert.IsNotNull(_content.Episodes);
            Assert.IsTrue(_content.Episodes.Count > 0);
        }
        #endregion Episodes

        #region Shows
        /// <summary>
        /// Get a Show
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_Lookup_Show()
        {
            Show item = await _client.LookupAsync<Show>(
                "4r157jjrIV0bzS6UxhN07i", LookupType.Shows);
            Assert.IsNotNull(item);
        }

        /// <summary>
        /// Get Multiple Shows
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_Lookup_Shows()
        {
            List<string> ids = new List<string>
            {
                "4r157jjrIV0bzS6UxhN07i",
                "2GmNzw8t4uG70rn4XG9zcC"
            };
            _list = await _client.LookupAsync(ids, LookupType.Shows);
            Assert.IsNotNull(_list.Shows);
            Assert.IsTrue(_list.Shows.Count == ids.Count);
        }

        /// <summary>
        /// Get a Show's Episodes
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_Lookup_ShowEpisodes()
        {
            Paging<SimplifiedEpisode> list = await _client.LookupAsync<Paging<SimplifiedEpisode>>(
                "4r157jjrIV0bzS6UxhN07i", LookupType.ShowEpisodes);
            Assert.IsNotNull(list);
            Assert.IsTrue(list.Items.Count > 0);
        }

        /// <summary>
        /// Search for a Show
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Test_Search_Show()
        {
            _content = await _client.SearchAsync(
                "Famous Fates", new SearchType() { Show = true });
            Assert.IsNotNull(_content.Shows);
            Assert.IsTrue(_content.Shows.Count > 0);
        }
        #endregion Shows
    }
}
