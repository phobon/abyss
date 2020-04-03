namespace Occasus.Core.Drawing.ParticleEffects
{
    public enum ParticleFlag
    {
        /// <summary>
        /// The particle fades into view. This is a linear function.
        /// </summary>
        FadeIn,

        /// <summary>
        /// The particle fades away. This is a linear function.
        /// </summary>
        FadeOut,

        /// <summary>
        /// The particle shrinks over its lifespan. This is a linear function.
        /// </summary>
        Shrink,

        /// <summary>
        /// The particle rotates over its lifespan. This is a linear function.
        /// </summary>
        Rotate,

        /// <summary>
        /// The particle is recycled after it expires.
        /// </summary>
        Recycle,

        /// <summary>
        /// The particle tracks the parent entities position.
        /// </summary>
        TrackParent
    }
}
