using System;
using System.Runtime.Serialization;

namespace Spotify.NetStandard.Requests
{
    /// <summary>
    /// Page
    /// </summary>
    [DataContract]
    public class Page
    {
        private int _page;

        /// <summary>
        /// The total number of items available to return.
        /// </summary>
        [DataMember(Name = "total")]
        public int Total { get; set; }

        /// <summary>
        /// The offset of the items returned (as set in the query or by default).
        /// </summary>
        [DataMember(Name = "offset")]
        public int Offset { get; set; }

        /// <summary>
        /// The maximum number of items in the response (as set in the query or by default).
        /// </summary>
        [DataMember(Name = "limit")]
        public int Limit { get; set; }

        /// <summary>
        /// Page Count
        /// </summary>
        public int Count
        {
            get { return (int)Math.Ceiling((double)Total / Limit); }
        }

        /// <summary>
        /// Get / Set Current Page
        /// </summary>
        public int Current
        {
            get
            {
                _page = (int)Math.Ceiling((double)Total / Offset);
                return _page <= 0 ? 1 : _page;
            }
            set
            {
                _page = (value < 1) ? 1 : (value > Count) ? Count : value;
                Offset = (_page - 1) * Limit;
            }
        }
    }
}
