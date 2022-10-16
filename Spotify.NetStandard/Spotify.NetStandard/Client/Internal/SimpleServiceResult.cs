namespace Spotify.NetStandard.Client.Internal;

/// <summary>
/// Simple Service Result
/// </summary>
/// <typeparam name="TResult">Result</typeparam>
/// <typeparam name="TErrorResult">Error Result</typeparam>
internal class SimpleServiceResult<TResult, TErrorResult>
{
    /// <summary>
    /// Result
    /// </summary>
    public TResult Result { get; set; }

    /// <summary>
    /// Error Result
    /// </summary>
    public TErrorResult ErrorResult { get; set; }

    /// <summary>
    /// Http Status Code
    /// </summary>
    public HttpStatusCode HttpStatusCode { get; set; }
}
