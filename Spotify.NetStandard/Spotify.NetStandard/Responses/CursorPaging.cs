namespace Spotify.NetStandard.Responses;

/// <summary>
/// Cursor Paging Object
/// </summary>
/// <typeparam name="T">Object Type</typeparam>
[DataContract]
public class CursorPaging<T> : Cursor
{
    /// <summary>
    /// Error Object
    /// </summary>
    [DataMember(Name = "error")]
    public ErrorResponse Error { get; set; }

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
    /// The cursors used to find the next set of items.
    /// </summary>
    [DataMember(Name = "cursors")]
    public Cursor Cursors { get; set; }

    /// <summary>
    /// The total number of items available to return.
    /// </summary>
    [DataMember(Name = "total")]
    public int Total { get; set; }

    /// <summary>
    /// IEnumerable of Type
    /// </summary>
    public IEnumerable<T> ReadOnlyItems => Items;

    /// <summary>
    /// Constructor
    /// </summary>
    public CursorPaging() => 
        Items = new List<T>();
}
