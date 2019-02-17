using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Spotify.NetStandard.Requests
{
    /// <summary>
    /// URI List Request Object
    /// </summary>
    [DataContract]
    public class UriListRequest
    {
        /// <summary>
        /// URIs
        /// </summary>
        [DataMember(Name = "uris")]
        public List<string> Uris { get; set; }
    }
}
