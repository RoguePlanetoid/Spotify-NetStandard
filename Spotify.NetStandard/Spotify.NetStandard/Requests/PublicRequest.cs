namespace Spotify.NetStandard.Requests;

/// <summary>
/// Public Request Object
/// </summary>
[DataContract]
class PublicRequest
{
    /// <summary>
    /// Is Public
    /// </summary>
    [DataMember(Name = "public")]
    public bool? IsPublic { get; set; }
}
