using System.Collections.Generic;
using Occasus.Core.Maps.Definitions;
using Occasus.Core.Maps.Tiles;

namespace Occasus.Core.Maps
{
    public interface IMapChunk
    {
        /// <summary>
        /// Gets the type.
        /// </summary>
        string Type { get; }

        /// <summary>
        /// Gets the layout.
        /// </summary>
        IDictionary<string, IList<IPlatformDefinition>> Layout { get; }

        /// <summary>
        /// Gets the tile map.
        /// </summary>
        ITile[,] TileMap { get; }

        /// <summary>
        /// Gets the doodad map.
        /// </summary>
        ITile[,] DoodadMap { get; }

        IDictionary<string, IList<ISpawnDefinition>> Spawns { get; }
    }
}
