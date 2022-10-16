namespace Spotify.NetStandard.Responses;

/// <summary>
/// External Id Object
/// </summary>
[DataContract]
public class ExternalId : Dictionary<string, string>
{
    /// <summary>
    /// International Standard Recording Code
    /// </summary>
    public string Isrc => this.GetValueOrDefault("isrc");

    /// <summary>
    /// International Article Number
    /// </summary>
    public string Ean => this.GetValueOrDefault("ean");

    /// <summary>
    /// Universal Product Code
    /// </summary>
    public string Upc => this.GetValueOrDefault("upc");
}
