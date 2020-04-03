namespace Occasus.Core.Drawing.Animation
{
    public enum AnimationFlag
    {
        /// <summary>
        /// Animation is static; this means the animation only has a single frame.
        /// </summary>
        Static,

        /// <summary>
        /// Animation loops continuously until the next animation, or it is manually stopped.
        /// </summary>
        Looping,

        /// <summary>
        /// Animation should be played in full before the next animation starts.
        /// </summary>
        PlayInFull,

        /// <summary>
        /// Animation should hold its last frame before ending or looping.
        /// </summary>
        HoldEnd
    }
}
