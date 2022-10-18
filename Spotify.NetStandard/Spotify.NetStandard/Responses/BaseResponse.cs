namespace Spotify.NetStandard.Responses;

/// <summary>
/// Base Response Object
/// </summary>
[DataContract]
public abstract class BaseResponse
{
    /// <summary>
    /// Error Object
    /// </summary>
    [DataMember(Name = "error")]
    public ErrorResponse Error { get; set; }
}
