namespace Spotify.NetStandard.Responses;

/// <summary>
/// Queue 
/// </summary>
[DataContract]
public class Queue
{
    /// <summary>
    /// Currently Playing Track or Episode. Can be null
    /// </summary>
    [DataMember(Name = "currently_playing")]
    public Album CurrentlyPlaying { get; set; }

    /// <summary>
    /// Tracks or Episodes in the Queue
    /// </summary>
    [DataMember(Name = "queue")]
    public List<Album> Albums { get; set; }
}
