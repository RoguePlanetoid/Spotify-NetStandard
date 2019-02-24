using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Spotify.NetStandard.Requests
{
    /// <summary>
    /// Devices Request Object
    /// </summary>
    [DataContract]
    public class DevicesRequest
    {
        /// <summary>
        /// (Required) List containing the ID of the device on which playback should be started/transferred. Although an array is accepted, only a single id is currently supported.
        /// </summary>
        [DataMember(Name = "device_ids")]
        public List<string> DeviceIds { get; set; }

        /// <summary>
        /// (Optional) true: ensure playback happens on new device. false or not provided: keep the current playback state.
        /// </summary>
        [DataMember(Name = "play")]
        public bool? Play { get; set; }
    }
}
