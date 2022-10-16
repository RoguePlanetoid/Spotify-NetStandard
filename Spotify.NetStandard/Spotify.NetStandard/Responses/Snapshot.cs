namespace Spotify.NetStandard.Responses;

/// <summary>
/// Snapshot Response Object
/// </summary>
[DataContract]
public class Snapshot : Status
{
    /// <summary>
    /// Can be used to identify playlist version in future requests
    /// </summary>
    [DataMember(Name = "snapshot_id")]
    public string SnapshotId { get; set; }
}
