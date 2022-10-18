namespace Spotify.NetStandard.Responses;

/// <summary>
/// Followers Object
/// </summary>
[DataContract]
public class Followers
{
    /// <summary>
    /// A link to the Web API endpoint providing full details of the followers; null if not available
    /// </summary>
    [DataMember(Name = "href")]
    public string Href { get; set; }

    /// <summary>
    /// The total number of followers.
    /// </summary>
    [DataMember(Name = "total")]
    public int Total { get; set; }
}
