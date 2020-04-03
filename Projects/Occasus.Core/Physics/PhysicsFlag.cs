namespace Occasus.Core.Physics
{
    public enum PhysicsFlag
    {
        /// <summary>
        /// Entity is grounded.
        /// </summary>
        Grounded,

        /// <summary>
        /// Entity reacts to physics.
        /// </summary>
        ReactsToPhysics,

        /// <summary>
        /// Entity reacts to gravity.
        /// </summary>
        ReactsToGravity,

        /// <summary>
        /// Entity collides with the environment.
        /// </summary>
        CollidesWithEnvironment
    }
}
