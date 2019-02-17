using System.Runtime.Serialization;

namespace Spotify.NetStandard.Responses
{
    /// <summary>
    /// Device Object
    /// </summary>
    [DataContract]
    public class Device
    {
        /// <summary>
        /// The device ID. This may be null.
        /// </summary>
        [DataMember(Name = "id")]
        public string Id { get; set; }

        /// <summary>
        /// If this device is the currently active device.
        /// </summary>
        [DataMember(Name = "is_active")]
        public bool IsActive { get; set; }

        /// <summary>
        /// If this device is currently in a private session.
        /// </summary>
        [DataMember(Name = "is_private_session")]
        public bool IsPrivateSession { get; set; }

        /// <summary>
        /// Whether controlling this device is restricted. If true then no commands will be accepted by this device.
        /// </summary>
        [DataMember(Name = "is_restricted")]
        public bool IsRestricted { get; set; }

        /// <summary>
        /// The name of the device.
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Device type, such as “computer”, “smartphone” or “speaker”.
        /// </summary>
        [DataMember(Name = "type")]
        public string Type { get; set; }

        /// <summary>
        /// The current volume in percent. This may be null.
        /// </summary>
        [DataMember(Name = "volume_percent")]
        public int? Volume { get; set; }
    }
}
