using System.Runtime.Serialization;

namespace Spotify.NetStandard.Requests
{
    /// <summary>
    /// Cursor Object
    /// </summary>
    [DataContract]
    public class Cursor
    {
        /// <summary>
        /// The cursor to use as key to find the next page of items.
        /// </summary>
        [DataMember(Name = "after")]
        public string After { get; set; }

        /// <summary>
        /// The cursor to use as key to find the previous page of items.
        /// </summary>
        [DataMember(Name = "before")]
        public string Before { get; set; }

        /// <summary>
        /// The maximum number of items in the response (as set in the query or by default).
        /// </summary>
        [DataMember(Name = "limit")]
        public int? Limit { get; set; }
    }
}
