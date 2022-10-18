namespace Spotify.NetStandard.Responses;

/// <summary>
/// Chapter Object
/// </summary>
public class Chapter : SimplifiedChapter
{
    /// <summary>
    /// Audiobook Object
    /// </summary>
    [DataMember(Name = "audiobook")]
    public SimplifiedAudiobook Audiobook { get; set; }
}
