using System.Runtime.Serialization;

namespace Spotify.NetStandard.Responses
{
    /// <summary>
    /// Copyright Object
    /// </summary>
    [DataContract]
    public class Copyright
    {
        /// <summary>
        /// The copyright text for this album.
        /// </summary>
        [DataMember(Name = "text")]
        public string Text { get; set; }

        /// <summary>
        /// The type of copyright: C = the copyright, P = the sound recording (performance) copyright.
        /// </summary>
        [DataMember(Name = "type")]
        public string Type { get; set; }
    }
}
