namespace Spotify.NetStandard.Responses;

/// <summary>
/// Context Object
/// </summary>
[DataContract]
public class Context : BaseResponse
{
    /// <summary>
    /// The object type of the object
    /// </summary>
    [DataMember(Name = "type")]
    public string Type { get; set; }

    /// <summary>
    /// A link to the Web API endpoint providing full details of the object
    /// </summary>
    [DataMember(Name = "href")]
    public string Href { get; set; }

    /// <summary>
    /// Known external URLs for this object.
    /// </summary>
    [DataMember(Name = "external_urls")]
    public ExternalUrl ExternalUrls { get; set; }

    /// <summary>
    /// The Spotify URI for the object
    /// </summary>
    [DataMember(Name = "uri")]
    public string Uri { get; set; }
}
