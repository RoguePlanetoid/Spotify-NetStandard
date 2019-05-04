using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Spotify.NetStandard.Responses
{
    /// <summary>
    /// Devices Object
    /// </summary>
    [DataContract]
    public class Devices : BaseResponse
    {
        /// <summary>
        /// A list of 0..n Device objects.
        /// </summary>
        [DataMember(Name = "devices")]
        public List<Device> Items { get; set; }
    }
}
