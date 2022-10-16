namespace Spotify.NetStandard.Requests;

/// <summary>
/// URI Request Object
/// </summary>
[DataContract]
public class UriRequest
{
    /// <summary>
    /// Spotify URI
    /// </summary>
    [DataMember(Name = "uri")]
    public string Uri { get; set; }
}
