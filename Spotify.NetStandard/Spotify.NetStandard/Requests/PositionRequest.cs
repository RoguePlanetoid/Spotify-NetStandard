using System.Runtime.Serialization;

namespace Spotify.NetStandard.Requests
{
    /// <summary>
    /// Position Request Object
    /// </summary>
    [DataContract]
    public class PositionRequest
    {
        /// <summary>
        /// Position
        /// </summary>
        [DataMember(Name = "position")]
        public int? Position { get; set; }
    }
}
