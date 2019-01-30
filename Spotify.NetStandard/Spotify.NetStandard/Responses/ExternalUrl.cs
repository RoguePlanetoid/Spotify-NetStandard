using Spotify.NetStandard.Client.Internal;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Spotify.NetStandard.Responses
{
    /// <summary>
    /// External Url Object
    /// </summary>
    [DataContract]
    public class ExternalUrl : Dictionary<string, string>
    {
        /// <summary>
        /// An external, public URL to the object.
        /// </summary>
        public string Spotify => this.GetValueOrDefault("spotify");
    }
}