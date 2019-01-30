using System.Runtime.Serialization;

namespace Spotify.NetStandard.Responses
{
    /// <summary>
    /// Content
    /// </summary>
    [DataContract]
    public abstract class Content : Context
    {
        /// <summary>
        /// The base-62 identifier that you can find at the end of the Spotify URI for the object
        /// </summary>
        [DataMember(Name = "id")]
        public string Id { get; set; }

        /// <summary>
        /// The name of the content
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; set; }
    }
}
