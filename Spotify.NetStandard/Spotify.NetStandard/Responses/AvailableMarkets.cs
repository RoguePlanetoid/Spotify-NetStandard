namespace Spotify.NetStandard.Responses;

/// <summary>
/// Available Markets Object
/// </summary>
public class AvailableMarkets : BaseResponse
{
    /// <summary>
    /// Get the list of markets where Spotify is available
    /// </summary>
    [DataMember(Name = "markets")]
    public List<string> Markets { get; set; }
}
