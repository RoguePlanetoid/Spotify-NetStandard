namespace Spotify.NetStandard.Responses;

/// <summary>
/// Actions Object
/// </summary>
[DataContract]
public class Actions
{
    /// <summary>
    /// Allows to update the user interface based on which playback actions are available within the current context
    /// </summary>
    [DataMember(Name = "disallows")]
    public Disallows Disallows { get; set; }
}
