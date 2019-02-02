using System.Runtime.Serialization;

namespace Spotify.NetStandard.Responses
{
    /// <summary>
    /// Error Object
    /// </summary>
    [DataContract]
    public class ErrorResponse
    {
        /// <summary>
        /// The HTTP status code
        /// </summary>
        [DataMember(Name = "status")]
        public int StatusCode { get; set; }

        /// <summary>
        /// A short description of the cause of the error. 
        /// </summary>
        [DataMember(Name = "message")]
        public string Message { get; set; }
    }
}
