using System.Runtime.Serialization;

namespace Spotify.NetStandard.Responses
{
    /// <summary>
    /// Status Response
    /// </summary>
    [DataContract]
    public class Status
    {
        /// <summary>
        /// Code
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// Success
        /// </summary>
        public bool Success { get; set; }
    }
}
