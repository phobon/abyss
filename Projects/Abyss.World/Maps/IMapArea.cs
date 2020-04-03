using Abyss.World.Universe;

namespace Abyss.World.Maps
{
    public interface IMapArea
    {
        /// <summary>
        /// Gets the type of the zone that relates to this area.
        /// </summary>
        ZoneType ZoneType { get; }

        /// <summary>
        /// Gets the depth of this area.
        /// </summary>
        int Depth { get; }
    }
}
