using System;
using System.Collections.Generic;
using Abyss.World.Universe;

namespace Abyss.World.Maps
{
    public interface IZoneData
    {
        /// <summary>
        /// Gets the type of the zone.
        /// </summary>
        ZoneType ZoneType { get; }

        /// <summary>
        /// Gets the minimum chunk count.
        /// </summary>
        int MinimumChunkCount { get; }

        /// <summary>
        /// Gets the maximum chunk count.
        /// </summary>
        int MaximumChunkCount { get; }

        /// <summary>
        /// Gets the default type of the chunk.
        /// </summary>
        string DefaultChunkType { get; }

        /// <summary>
        /// Gets the basic structure of this zone.
        /// </summary>
        IEnumerable<Tuple<string, int>> Structure { get; }
    }
}
