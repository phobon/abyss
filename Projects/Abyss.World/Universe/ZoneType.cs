namespace Abyss.World.Universe
{
    /// <summary>
    /// Represents the type of Zone an area is associated with.
    /// </summary>
    public enum ZoneType
    {
        /// <summary>
        /// The common zone.
        /// </summary>
        Common = -1,

        /// <summary>
        /// The normal zone.
        /// </summary>
        Normal = 0,

        /// <summary>
        /// The fire zone.
        /// </summary>
        Fire = 2,

        /// <summary>
        /// The ice zone.
        /// </summary>
        Ice = 3,

        /// <summary>
        /// The electric zone.
        /// </summary>
        Electric = 1,

        /// <summary>
        /// The shadow zone.
        /// </summary>
        Shadow = 4,

        /// <summary>
        /// The void zone.
        /// </summary>
        Void = 5,
        
        /// <summary>
        /// The transition between zones.
        /// </summary>
        Transition = 6,

        /// <summary>
        /// A series of tutorial chunks that help new players figure out how to play.
        /// </summary>
        Tutorial = 50,

        /// <summary>
        /// A series of testing chunks to test new features.
        /// </summary>
        Test = 99
    }
}
