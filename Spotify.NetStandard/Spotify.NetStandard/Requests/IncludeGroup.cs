using System.ComponentModel;

namespace Spotify.NetStandard.Requests
{
    /// <summary>
    /// Include Group
    /// </summary>
    public class IncludeGroup
    {
        /// <summary>
        /// Album
        /// </summary>
        [Description("album")]
        public bool? Album { get; set; }

        /// <summary>
        /// Single
        /// </summary>
        [Description("single")]
        public bool? Single { get; set; }

        /// <summary>
        /// Appears On
        /// </summary>
        [Description("appears_on")]
        public bool? AppearsOn { get; set; }

        /// <summary>
        /// Compliation
        /// </summary>
        [Description("compilation")]
        public bool? Compilation { get; set; }
    }
}
