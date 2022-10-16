namespace Spotify.NetStandard.Responses;

/// <summary>
/// Narrator Object
/// </summary>
[DataContract]
public class Narrator
{
    /// <summary>
    /// The name of the Narrator
    /// </summary>
    [DataMember(Name = "name")]
    public string Label { get; set; }
}
