using System.Runtime.Serialization;

namespace Spotify.NetStandard.Responses
{
    /// <summary>
    /// Disallows Object
    /// </summary>
    [DataContract]
    public class Disallows
    {
        /// <summary>
        /// Interrupting playback not allowed?
        /// </summary>
        [DataMember(Name = "interrupting_playback")]
        public bool IsInterruptingPlaybackNotAllowed { get; set; }

        /// <summary>
        /// Pausing not allowed?
        /// </summary>
        [DataMember(Name = "pausing")]
        public bool IsPausingNotAllowed { get; set; }

        /// <summary>
        /// Resuming not allowed?
        /// </summary>
        [DataMember(Name = "resuming")]
        public bool IsResumingNotAllowed { get; set; }

        /// <summary>
        /// Seeking not allowed? Will be set to true while playing an ad track
        /// </summary>
        [DataMember(Name = "seeking")]
        public bool IsSeekingNotAllowed { get; set; }

        /// <summary>
        /// Skipping next not allowed? Will be set to true while playing an ad track
        /// </summary>
        [DataMember(Name = "skipping_next")]
        public bool IsSkippingNextNotAllowed { get; set; }

        /// <summary>
        /// Skipping previous not allowed? Will be set to true while playing an ad track
        /// </summary>
        [DataMember(Name = "skipping_prev")]
        public bool IsSkippingPrevNotAllowed { get; set; }

        /// <summary>
        /// Toggling repeat context not allowed?
        /// </summary>
        [DataMember(Name = "toggling_repeat_context")]
        public bool IsTogglingRepeatContextNotAllowed { get; set; }

        /// <summary>
        /// Toggling shuffle not allowed?
        /// </summary>
        [DataMember(Name = "toggling_shuffle")]
        public bool IsTogglingShuffleNotAllowed { get; set; }

        /// <summary>
        /// Toggling repeat track not allowed?
        /// </summary>
        [DataMember(Name = "toggling_repeat_track")]
        public bool IsTogglingRepeatTrackNotAllowed { get; set; }

        /// <summary>
        /// Transferring playback not allowed?
        /// </summary>
        [DataMember(Name = "transferring_playback")]
        public bool IsTransferringPlaybackNotAllowed { get; set; }
    }
}
