namespace Spotify.NetStandard.Responses;

/// <summary>
/// Audiobook Object
/// </summary>
[DataContract]
public class Audiobook : SimplifiedAudiobook
{
    /// <summary>
    /// The chapters of the audiobook.
    /// </summary>
    [DataMember(Name = "chapters")]
    public Paging<SimplifiedChapter> Chapters { get; set; }
}