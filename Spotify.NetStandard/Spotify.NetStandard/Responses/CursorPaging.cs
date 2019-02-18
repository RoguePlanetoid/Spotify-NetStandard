﻿using Spotify.NetStandard.Requests;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Spotify.NetStandard.Responses
{
    /// <summary>
    /// Cursor Paging Object
    /// </summary>
    /// <typeparam name="T">Object Type</typeparam>
    [DataContract]
    public class CursorPaging<T> : Cursor
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
        /// The cursors used to find the next set of items.
        /// </summary>
        [DataMember(Name = "cursors")]
        public Cursor Cursors { get; set; }

        /// <summary>
        /// The total number of items available to return.
        /// </summary>
        [DataMember(Name = "total")]
        public int Total { get; set; }

        public IEnumerable<T> ReadOnlyItems => Items;

        /// <summary>
        /// The cursor to use as key to find the next page of items.
        /// </summary>
        public new Cursor After => Cursors;

        public CursorPaging()
        {
            Items = new List<T>();
        }
    }
}