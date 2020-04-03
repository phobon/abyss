namespace Occasus.Core.Drawing.ParticleEffects
{
    public static class ParticleEffectFlag
    {
        /// <summary>
        /// Effect recycles dead pixels.
        /// </summary>
        public const string Recycle = "Recycle";

        /// <summary>
        /// Effect recycles dead particles to the current view port.
        /// </summary>
        public const string RecycleToViewPort = "RecycleToViewPort";
    }
}
