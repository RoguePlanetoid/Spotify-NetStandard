namespace Spotify.NetStandard.Responses;

/// <summary>
/// Simplified Audiobook Object
/// </summary>
[DataContract]
public class SimplifiedAudiobook : Content
{
    /// <summary>
    /// The author(s) for the audiobook
    /// </summary>
    [DataMember(Name = "authors")]
    public List<Author> Authors { get; set; }

    /// <summary>
    /// A list of the countries in which the audiobook can be played, identified by their ISO 3166-1 alpha-2 code
    /// </summary>
    [DataMember(Name = "available_markets")]
    public List<string> AvailableMarkets { get; set; }

    /// <summary>
    /// The copyright statements of the audiobook.
    /// </summary>
    [DataMember(Name = "copyrights")]
    public List<Copyright> Copyrights { get; set; }

    /// <summary>
    /// A description of the audiobook. HTML tags are stripped away from this field, use html_description field in case HTML tags are needed.
    /// </summary>
    [DataMember(Name = "description")]
    public string Description { get; set; }

    /// <summary>
    /// A description of the audiobook. This field may contain HTML tags.
    /// </summary>
    [DataMember(Name = "html_description")]
    public string HtmlDescription { get; set; }

    /// <summary>
    /// Whether or not the audiobook has explicit content (true = yes it does; false = no it does not OR unknown).
    /// </summary>
    [DataMember(Name = "explicit")]
    public bool IsExplicit { get; set; }

    /// <summary>
    /// The cover art for the audiobook in various sizes, widest first.
    /// </summary>
    [DataMember(Name = "images")]
    public List<Image> Images { get; set; }

    /// <summary>
    /// A list of the languages used in the audiobook, identified by their ISO 639 code.
    /// </summary>
    [DataMember(Name = "languages")]
    public List<string> Languages { get; set; }

    /// <summary>
    /// The media type of the audiobook.
    /// </summary>
    [DataMember(Name = "media_type")]
    public string MediaType { get; set; }

    /// <summary>
    /// The narrators(s) of the audiobook
    /// </summary>
    [DataMember(Name = "narrators")]
    public List<Narrator> Narrators { get; set; }

    /// <summary>
    /// The publisher of the audiobook.
    /// </summary>
    [DataMember(Name = "publisher")]
    public string Publisher { get; set; }

    /// <summary>
    /// The number of chapters in this audiobook.
    /// </summary>
    [DataMember(Name = "total_chapters")]
    public int? TotalChapters { get; set; }
}
