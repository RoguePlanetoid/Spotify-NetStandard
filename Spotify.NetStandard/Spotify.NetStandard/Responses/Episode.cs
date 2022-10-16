namespace Spotify.NetStandard.Responses;

/// <summary>
/// Episode Object
/// </summary>
public class Episode : SimplifiedEpisode 
{
    /// <summary>
    /// The show on which the episode belongs.
    /// </summary>
    [DataMember(Name = "show")]
    public SimplifiedShow Show { get; set; }
}
