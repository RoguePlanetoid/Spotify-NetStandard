using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Spotify.NetStandard.Responses
{
    /// <summary>
    /// Simplified Show Object
    /// </summary>
    public class SimplifiedShow : Content
    {
        /// <summary>
        /// A list of the countries in which the show can be played, identified by their ISO 3166-1 alpha-2 code
        /// </summary>
        [DataMember(Name = "available_markets")]
        public List<string> AvailableMarkets { get; set; }

        /// <summary>
        /// The copyright statements of the show
        /// </summary>
        [DataMember(Name = "copyrights")]
        public List<Copyright> Copyrights { get; set; }

        /// <summary>
        /// A description of the show
        /// </summary>
        [DataMember(Name = "description")]
        public string Description { get; set; }

        /// <summary>
        /// Whether or not the show has explicit content ( true = yes it does; false = no it does not OR unknown)
        /// </summary>
        [DataMember(Name = "explicit")]
        public bool IsExplicit { get; set; }

        /// <summary>
        /// The cover art for the show in various sizes, widest first
        /// </summary>
        [DataMember(Name = "images")]
        public List<Image> Images { get; set; }

        /// <summary>
        /// True if all of the show's episodes are hosted outside of Spotify’s CDN
        /// </summary>
        [DataMember(Name = "is_externally_hosted")]
        public bool IsExternallyHosted { get; set; }

        /// <summary>
        /// A list of the languages used in the show, identified by their ISO 639 code
        /// </summary>
        [DataMember(Name = "languages")]
        public List<string> Languages { get; set; }

        /// <summary>
        /// The media type of the show
        /// </summary>
        [DataMember(Name = "media_type")]
        public string MediaType { get; set; }

        /// <summary>
        /// The publisher of the show
        /// </summary>
        [DataMember(Name = "publisher")]
        public string Publisher { get; set; }
    }
}
