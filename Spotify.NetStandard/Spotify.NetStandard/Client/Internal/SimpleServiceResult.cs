using System.Net;

namespace Spotify.NetStandard.Client.Internal
{
    /// <summary>
    /// Simple Service Result
    /// </summary>
    /// <typeparam name="TResult">Result</typeparam>
    /// <typeparam name="TErrorResult">Error Result</typeparam>
    internal class SimpleServiceResult<TResult, TErrorResult>
    {
        public TResult Result { get; set; }
        public TErrorResult ErrorResult { get; set; }
        public HttpStatusCode HttpStatusCode { get; set; }
    }
}
