using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Spotify.NetStandard.Responses
{
    /// <summary>
    /// Album Object
    /// </summary>
    [DataContract]
    public class Album : SimplifiedAlbum
    {
        /// <summary>
        /// The copyright statements of the album.
        /// </summary>
        [DataMember(Name = "copyrights")]
        public List<Copyright> Copyrights { get; set; }

        /// <summary>
        /// Known external IDs for the album.
        /// </summary>
        [DataMember(Name = "external_ids")]
        public ExternalId ExternalId { get; set; }

        /// <summary>
        /// A list of the genres used to classify the album. For example: "Prog Rock" , "Post-Grunge"
        /// </summary>
        [DataMember(Name = "genres")]
        public List<string> Genres { get; set; }

        /// <summary>
        /// The label for the album.
        /// </summary>
        [DataMember(Name = "label")]
        public string Label { get; set; }

        /// <summary>
        /// The popularity of the album. The value will be between 0 and 100, with 100 being the most popular
        /// </summary>
        [DataMember(Name = "popularity")]
        public int Popularity { get; set; }

        /// <summary>
        /// The date the album was first released, for example 1981. Depending on the precision, it might be shown as 1981-12 or 1981-12-15
        /// </summary>
        [DataMember(Name = "release_date")]
        public string ReleaseDate { get; set; }

        /// <summary>
        /// The precision with which ReleaseDate value is known: year , month , or day.
        /// </summary>
        [DataMember(Name = "release_date_precision")]
        public string ReleaseDatePrecision { get; set; }

        /// <summary>
        /// The tracks of the album.
        /// </summary>
        [DataMember(Name = "tracks")]
        public Paging<SimplifiedTrack> Tracks { get; set; }
    }
}
