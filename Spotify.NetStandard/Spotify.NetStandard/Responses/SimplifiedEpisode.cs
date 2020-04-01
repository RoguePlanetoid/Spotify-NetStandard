using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Spotify.NetStandard.Responses
{
    /// <summary>
    /// Simplified Episode Object
    /// </summary>
    public class SimplifiedEpisode : Content
    {
        /// <summary>
        /// A URL to a 30 second preview (MP3 format) of the episode - null if not available
        /// </summary>
        [DataMember(Name = "audio_preview_url")]
        public string Preview { get; set; }

        /// <summary>
        /// The description of the episode
        /// </summary>
        [DataMember(Name = "description")]
        public string Description { get; set; }

        /// <summary>
        /// The episode length in milliseconds
        /// </summary>
        [DataMember(Name = "duration_ms")]
        public long Duration { get; set; }

        /// <summary>
        /// Whether or not the episode has explicit content ( true = yes it does; false = no it does not OR unknown)
        /// </summary>
        [DataMember(Name = "explicit")]
        public bool IsExplicit { get; set; }

        /// <summary>
        /// The cover art for the episode in various sizes, widest first
        /// </summary>
        [DataMember(Name = "images")]
        public List<Image> Images { get; set; }

        /// <summary>
        /// True if the episode is hosted outside of Spotify's CDN
        /// </summary>
        [DataMember(Name = "is_externally_hosted")]
        public bool IsExternallyHosted { get; set; }

        /// <summary>
        /// True if the episode is playable in the given market. Otherwise false
        /// </summary>
        [DataMember(Name = "is_playable")]
        public bool IsPlayable { get; set; }

        /// <summary>
        /// A list of the languages used in the episode, identified by their ISO 639 code
        /// </summary>
        [DataMember(Name = "languages")]
        public List<string> Languages { get; set; }

        /// <summary>
        /// The date the episode was first released, for example 1981-12-15. Depending on the precision, it might be shown as 1981 or 1981-12
        /// </summary>
        [DataMember(Name = "release_date")]
        public string ReleaseDate { get; set; }

        /// <summary>
        /// The precision with which ReleaseDate value is known: year, month, or day
        /// </summary>
        [DataMember(Name = "release_date_precision")]
        public string ReleaseDatePrecision { get; set; }

        /// <summary>
        /// The user's most recent position in the episode. Set if the supplied access token is a user token and has the scope user-read-playback-position
        /// </summary>
        [DataMember(Name = "resume_point")]
        public ResumePoint ResumePoint { get; set; }
    }
}
