namespace Spotify.NetStandard.Responses;

/// <summary>
/// Artist Object
/// </summary>
[DataContract]
public class Artist : SimplifiedArtist
{
    /// <summary>
    /// Information about the followers of the artist.
    /// </summary>
    [DataMember(Name = "followers")]
    public Followers Followers { get; set; }

    /// <summary>
    /// A list of the genres the artist is associated with. For example: "Prog Rock" , "Post-Grunge".
    /// </summary>
    [DataMember(Name = "genres")]
    public List<string> Genres { get; set; }

    /// <summary>
    /// Images of the artist in various sizes, widest first.
    /// </summary>
    [DataMember(Name = "images")]
    public List<Image> Images { get; set; }

    /// <summary>
    /// The popularity of the artist. The value will be between 0 and 100, with 100 being the most popular.
    /// </summary>
    [DataMember(Name = "popularity")]
    public int Popularity { get; set; }
}
