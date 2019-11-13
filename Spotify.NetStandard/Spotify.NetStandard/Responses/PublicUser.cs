using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Spotify.NetStandard.Responses
{
    /// <summary>
    /// Public User Object
    /// </summary>
    [DataContract]
    public class PublicUser : Content
    {
        /// <summary>
        /// The name displayed on the user’s profile. null if not available.
        /// </summary>
        [DataMember(Name = "display_name")]
        public string DisplayName { get; set; }

        /// <summary>
        /// Information about the followers of this user.
        /// </summary>
        [DataMember(Name = "followers")]
        public Followers Followers { get; set; }

        /// <summary>
        /// The user’s profile image.
        /// </summary>
        [DataMember(Name = "images")]
        public List<Image> Images { get; set; }
    }
}
