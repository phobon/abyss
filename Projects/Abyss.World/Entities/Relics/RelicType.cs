namespace Abyss.World.Entities.Relics
{
    public enum RelicType
    {
        /// <summary>
        /// Effects of this relic apply at all times.
        /// </summary>
        Passive,

        /// <summary>
        /// Effects of this relic only apply when it is activated.
        /// </summary>
        Active,

        /// <summary>
        /// Effects of this relic are cosmetic in nature.
        /// </summary>
        Cosmetic

        /// <summary>
        /// Effects of this relic only last until the player dies in the Abyss.
        /// </summary>
        //Transient,

        /// <summary>
        /// Effects of this relic are temporary; only lasting for a set amount of time before they disappear.
        /// </summary>
        //PowerUp,

        /// <summary>
        /// Effects of this relic are permanent. They persist between existences in the Abyss.
        /// </summary>
        //Persistent,

        /// <summary>
        /// These relics grant the player access to new worlds and new environments.
        /// </summary>
        //Key
    }
}
