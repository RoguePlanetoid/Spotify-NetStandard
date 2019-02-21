using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Spotify.NetStandard.Responses
{
    /// <summary>
    /// Simplified Track Object
    /// </summary>
    [DataContract]
    public class SimplifiedTrack : Content
    {
        /// <summary>
        /// The artists who performed the track. Each artist object includes a link in href to more detailed information about the artist.
        /// </summary>
        [DataMember(Name = "artists")]
        public List<SimplifiedArtist> Artists { get; set; }

        /// <summary>
        /// A list of the countries in which the track can be played, identified by their ISO 3166-1 alpha-2 code.
        /// </summary>
        [DataMember(Name = "available_markets")]
        public List<string> AvailableMarkets { get; set; }

        /// <summary>
        /// The disc number(usually 1 unless the album consists of more than one disc).
        /// </summary>
        [DataMember(Name = "disc_number")]
        public int DiscNumber { get; set; }

        /// <summary>
        /// The track length in milliseconds.
        /// </summary>
        [DataMember(Name = "duration_ms")]
        public long Duration { get; set; }

        /// <summary>
        /// Whether or not the track has explicit lyrics ( true = yes it does; false = no it does not OR unknown).
        /// </summary>
        [DataMember(Name = "explicit")]
        public bool IsExplicit { get; set; }

        /// <summary>
        /// Part of the response when Track Relinking is applied. If true , the track is playable in the given market. Otherwise false.
        /// </summary>
        [DataMember(Name = "is_playable")]
        public bool IsPlayable { get; set; }

        /// <summary>
        /// Part of the response when Track Relinking is applied and is only part of the response if the track linking, in fact, exists
        /// </summary>
        [DataMember(Name = "linked_from")]
        public LinkedTrack LinkedFrom { get; set; }

        /// <summary>
        /// A link to a 30 second preview(MP3 format) of the track.
        /// </summary>
        [DataMember(Name = "preview_url")]
        public string Preview { get; set; }

        /// <summary>
        /// The number of the track. If an album has several discs, the track number is the number on the specified disc.
        /// </summary>
        [DataMember(Name = "track_number")]
        public int TrackNumber { get; set; }
    }
}
