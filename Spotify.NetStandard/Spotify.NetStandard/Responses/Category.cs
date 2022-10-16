namespace Spotify.NetStandard.Responses;

/// <summary>
/// Category Object
/// </summary>
[DataContract]
public class Category : Content
{
    /// <summary>
    /// The category icon, in various sizes.
    /// </summary>
    [DataMember(Name = "icons")]
    public List<Image> Images { get; set; }
}
