namespace Spotify.NetStandard.Responses;

/// <summary>
/// Show Object
/// </summary>
public class Show : SimplifiedShow 
{
    /// <summary>
    /// A list of the show’s episodes.
    /// </summary>
    [DataMember(Name = "episodes")]
    public Paging<SimplifiedEpisode> Episodes { get; set;  }
}
