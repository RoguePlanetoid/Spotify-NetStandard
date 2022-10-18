namespace Spotify.NetStandard.Responses;

/// <summary>
/// Lookup Response
/// </summary>
[DataContract]
public class LookupResponse : BaseResponse
{
    /// <summary>
    /// List of Album Object
    /// </summary>
    [DataMember(Name = "albums")]
    public List<Album> Albums { get; set; }

    /// <summary>
    /// List of Artist Object
    /// </summary>
    [DataMember(Name = "artists")]
    public List<Artist> Artists { get; set; }

    /// <summary>
    /// List of Track Object
    /// </summary>
    [DataMember(Name = "tracks")]
    public List<Track> Tracks { get; set; }

    /// <summary>
    /// List of Audio Feature Object
    /// </summary>
    [DataMember(Name = "audio_features")]
    public List<AudioFeatures> AudioFeatures { get; set; }

    /// <summary>
    /// List of Episode Object
    /// </summary>
    [DataMember(Name = "episodes")]
    public List<Episode> Episodes { get; set; }

    /// <summary>
    /// List of Show Object
    /// </summary>
    [DataMember(Name = "shows")]
    public List<Show> Shows { get; set; }

    /// <summary>
    /// List of Audiobook Object
    /// </summary>
    [DataMember(Name = "audiobooks")]
    public List<Audiobook> Audiobooks { get; set; }

    /// <summary>
    /// List of Chapter Object
    /// </summary>
    [DataMember(Name = "chapters")]
    public List<Chapter> Chapters { get; set; }
}
