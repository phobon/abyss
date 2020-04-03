using Abyss.World.Universe;

namespace Abyss.World.Maps
{
    public class MapArea : IMapArea
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MapArea" /> class.
        /// </summary>
        /// <param name="zoneType">Type of the zone.</param>
        /// <param name="depth">The depth.</param>
        public MapArea(
            ZoneType zoneType,
            int depth)
        {
            this.ZoneType = zoneType;
            this.Depth = depth;
        }

        /// <summary>
        /// Gets the type of the zone that relates to this area.
        /// </summary>
        public ZoneType ZoneType
        {
            get; private set;
        }

        /// <summary>
        /// Gets the depth of this area.
        /// </summary>
        public int Depth
        {
            get; private set;
        }
    }
}
