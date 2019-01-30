using System.Runtime.Serialization;

namespace Spotify.NetStandard.Responses
{
    /// <summary>
    /// Recommendation Seed Object
    /// </summary>
    [DataContract]
    public class RecommendationSeed : Content
    {
        /// <summary>
        /// The number of tracks available after min_* and max_* filters have been applied.
        /// </summary>
        [DataMember(Name = "afterFilteringSize")]
        public int AfterFilteringSize { get; set; }

        /// <summary>
        /// The number of tracks available after relinking for regional availability.
        /// </summary>
        [DataMember(Name = "afterRelinkingSize")]
        public int AfterRelinkingSize { get; set; }

        /// <summary>
        /// The number of recommended tracks available for this seed.
        /// </summary>
        [DataMember(Name = "initialPoolSize")]
        public int InitialPoolSize { get; set; }
    }
}
