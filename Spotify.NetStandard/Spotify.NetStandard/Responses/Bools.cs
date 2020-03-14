using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Spotify.NetStandard.Responses
{
    /// <summary>
    /// List of true or false values
    /// </summary>
    [DataContract]
    public class Bools : List<bool>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Bools() { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="errorResponse"></param>
        internal Bools(ErrorResponse errorResponse) => 
            Error = errorResponse;

        /// <summary>
        /// Error Object
        /// </summary>
        [DataMember(Name = "error")]
        public ErrorResponse Error { get; set; }
    }
}
