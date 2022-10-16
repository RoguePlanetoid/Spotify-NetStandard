namespace Spotify.NetStandard.Responses;

/// <summary>
/// Simplified Album Object
/// </summary>
[DataContract]
public class SimplifiedAlbum : Content
{
    /// <summary>
    /// The field is present when getting an artist’s albums. Possible values are “album”, “single”, “compilation”, “appears_on”.
    /// </summary>
    [DataMember(Name = "album_group")]
    public string AlbumGroup { get; set; }

    /// <summary>
    /// The type of the album: one of "album" , "single" , or "compilation".
    /// </summary>
    [DataMember(Name = "album_type")]
    public string AlbumType { get; set; }

    /// <summary>
    /// The artists of the album. Each artist object includes a link in href to more detailed information about the artist.
    /// </summary>
    [DataMember(Name = "artists")]
    public List<SimplifiedArtist> Artists { get; set; }

    /// <summary>
    /// The markets in which the album is available: ISO 3166-1 alpha-2 country codes
    /// </summary>
    [DataMember(Name = "available_markets")]
    public List<string> AvailableMarkets { get; set; }

    /// <summary>
    /// The cover art for the album in various sizes, widest first.
    /// </summary>
    [DataMember(Name = "images")]
    public List<Image> Images { get; set; }

    /// <summary>
    /// The total number of tracks
    /// </summary>
    [DataMember(Name = "total_tracks")]
    public int TotalTracks { get; set; }
}
