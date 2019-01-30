using Spotify.NetStandard.Enums;

namespace Spotify.NetStandard.Requests
{
    /// <summary>
    /// Navigate By Request
    /// </summary>
    public class NavigateBy
    {
        /// <summary>
        /// Navigation Direction
        /// </summary>
        public NavigateType Direction { get; set; } = NavigateType.None;
    }
}
