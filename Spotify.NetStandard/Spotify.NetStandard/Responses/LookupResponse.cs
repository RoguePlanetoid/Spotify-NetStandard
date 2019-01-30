using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Spotify.NetStandard.Responses
{
    /// <summary>
    /// Lookup Response
    /// </summary>
    [DataContract]
    public class LookupResponse : BaseResponse
    {
        /// <summary>
        /// List of Album Object
        /// </summary>
        [DataMember(Name = "albums")]
        public List<Album> Albums { get; set; }

        /// <summary>
        /// List of Artist Object
        /// </summary>
        [DataMember(Name = "artists")]
        public List<Artist> Artists { get; set; }

        /// <summary>
        /// List of Track Object
        /// </summary>
        [DataMember(Name = "tracks")]
        public List<Artist> Tracks { get; set; }

        /// <summary>
        /// List of Audio Feature Object
        /// </summary>
        [DataMember(Name = "audio_features")]
        public List<AudioFeatures> AudioFeatures { get; set; }
    }
}
