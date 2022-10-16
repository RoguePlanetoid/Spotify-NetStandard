namespace Spotify.NetStandard.Responses;

/// <summary>
/// Status Response
/// </summary>
[DataContract]
public class Status : BaseResponse
{
    /// <summary>
    /// Status Code
    /// </summary>
    public HttpStatusCode StatusCode { get; set; }

    /// <summary>
    /// Code
    /// </summary>
    public int Code => (int)StatusCode;

    /// <summary>
    /// Success
    /// </summary>
    public bool Success { get; set; }
}
