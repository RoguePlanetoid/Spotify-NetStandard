namespace Spotify.NetStandard.Responses;

/// <summary>
/// Author Object
/// </summary>
[DataContract]
public class Author
{
    /// <summary>
    /// The name of the author
    /// </summary>
    [DataMember(Name = "name")]
    public string Label { get; set; }
}
