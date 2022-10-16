namespace Spotify.NetStandard.Responses;

/// <summary>
/// Recommendations Response Object
/// </summary>
[DataContract]
public class RecommendationsResponse : BaseResponse
{
    /// <summary>
    /// An array of recommendation seed objects.
    /// </summary>
    [DataMember(Name = "seeds")]
    public List<RecommendationSeed> Seeds { get; set; }

    /// <summary>
    /// An array of track object (simplified) ordered according to the parameters supplied.
    /// </summary>
    [DataMember(Name = "tracks")]
    public List<SimplifiedTrack> Tracks { get; set; }
}
