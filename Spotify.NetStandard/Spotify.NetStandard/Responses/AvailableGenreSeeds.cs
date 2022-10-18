namespace Spotify.NetStandard.Responses;

/// <summary>
/// Available Genre Seeds Object
/// </summary>
public class AvailableGenreSeeds : BaseResponse
{
    /// <summary>
    /// Genres
    /// </summary>
    [DataMember(Name = "genres")]
    public List<string> Genres { get; set; }
}
