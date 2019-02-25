using Spotify.NetStandard.Requests;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Spotify.NetStandard.Responses
{
    /// <summary>
    /// Paging Object
    /// </summary>
    /// <typeparam name="T">Object Type</typeparam>
    [DataContract]
    public class Paging<T> : Page
    {
        /// <summary>
        /// A link to the Web API endpoint returning the full result of the request.
        /// </summary>
        [DataMember(Name = "href")]
        public string Href { get; set; }

        /// <summary>
        /// The requested data.
        /// </summary>
        [DataMember(Name = "items")]
        public List<T> Items { get; set; }

        /// <summary>
        /// URL to the next page of items. (null if none)
        /// </summary>
        [DataMember(Name = "next")]
        public string Next { get; set; }

        /// <summary>
        /// URL to the previous page of items. (null if none)
        /// </summary>
        [DataMember(Name = "previous")]
        public string Previous { get; set; }

        /// <summary>
        /// IEnumerable of Type
        /// </summary>
        public IEnumerable<T> ReadOnlyItems => Items;

        /// <summary>
        /// Page
        /// </summary>
        public Page Page { get { return this; } }

        /// <summary>
        /// Constructor
        /// </summary>
        public Paging()
        {
            Items = new List<T>();
        }
    }
}
