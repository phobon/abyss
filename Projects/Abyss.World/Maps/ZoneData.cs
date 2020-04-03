using System;
using System.Collections.Generic;
using Abyss.World.Universe;

namespace Abyss.World.Maps
{
    public class ZoneData : IZoneData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ZoneData" /> class.
        /// </summary>
        /// <param name="zoneType">Type of the zone.</param>
        /// <param name="minimumChunkCount">The minimum chunk count.</param>
        /// <param name="maximumChunkCount">The maximum chunk count.</param>
        /// <param name="defaultChunkType">Default type of the chunk.</param>
        /// <param name="structure">The structure.</param>
        public ZoneData(ZoneType zoneType, int minimumChunkCount, int maximumChunkCount, string defaultChunkType, IEnumerable<Tuple<string, int>> structure)
        {
            this.ZoneType = zoneType;
            this.MinimumChunkCount = minimumChunkCount;
            this.MaximumChunkCount = maximumChunkCount;
            this.DefaultChunkType = defaultChunkType;
            this.Structure = structure;
        }

        /// <summary>
        /// Gets the type of the zone.
        /// </summary>
        public ZoneType ZoneType
        {
            get; private set;
        }

        /// <summary>
        /// Gets the minimum chunk count.
        /// </summary>
        public int MinimumChunkCount
        {
            get; private set;
        }

        /// <summary>
        /// Gets the maximum chunk count.
        /// </summary>
        public int MaximumChunkCount
        {
            get; private set;
        }

        /// <summary>
        /// Gets the default type of the chunk.
        /// </summary>
        public string DefaultChunkType
        {
            get; private set;
        }

        /// <summary>
        /// Gets the basic structure of this zone.
        /// </summary>
        public IEnumerable<Tuple<string, int>> Structure
        {
            get; private set;
        }
    }
}
