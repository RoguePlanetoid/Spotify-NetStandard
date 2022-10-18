namespace Spotify.NetStandard.Responses;

/// <summary>
/// Image Object
/// </summary>
[DataContract]
public class Image
{
    /// <summary>
    /// The image height in pixels. If unknown: null or not returned.
    /// </summary>
    [DataMember(Name = "height")]
    public int? Height { get; set; }

    /// <summary>
    /// The source URL of the image.
    /// </summary>
    [DataMember(Name = "url")]
    public string Url { get; set; }

    /// <summary>
    /// The image width in pixels. If unknown: null or not returned.
    /// </summary>
    [DataMember(Name = "width")]
    public int? Width { get; set; }
}
