using System.Runtime.Serialization;

namespace Spotify.NetStandard.Responses
{
    /// <summary>
    /// Private User Object
    /// </summary>
    [DataContract]
    public class PrivateUser : PublicUser
    {
        /// <summary>
        /// The country of the user, as set in the user’s account profile.An ISO 3166-1 alpha-2 country code.This field is only available when the current user has granted access to the user-read-private scope.
        /// </summary>
        [DataMember(Name = "country")]
        public string Country { get; set; }

        /// <summary>
        /// The user’s email address, as entered by the user when creating their account. his field is only available when the current user has granted access to the user-read-email scope
        /// </summary>
        [DataMember(Name = "email")]
        public string Email { get; set; }
    }
}
