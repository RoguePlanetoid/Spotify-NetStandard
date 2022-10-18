namespace Spotify.NetStandard.Responses;

/// <summary>
/// Saved Show Object
/// </summary>
public class SavedShow : BaseResponse
{
    /// <summary>
    /// The date and time the show was saved. Timestamps are returned in ISO 8601 format as Coordinated Universal Time (UTC) with a zero offset: YYYY-MM-DDTHH:MM:SSZ. If the time is imprecise (for example, the date/time of an show release), an additional field indicates the precision; see for example, ReleaseDate in a show object.
    /// </summary>
    [DataMember(Name = "added_at")]
    public string AddedAt { get; set; }

    /// <summary>
    /// Information about the show
    /// </summary>
    [DataMember(Name = "show")]
    public SimplifiedShow Show { get; set; }
}
