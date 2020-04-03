using System.Collections.Generic;

using Abyss.World.Universe;

using Microsoft.Xna.Framework;
using Occasus.Core.Entities;
using Occasus.Core.Tiles;

namespace Abyss.World.Maps
{
    public interface IMap : IEntity
    {
        /// <summary>
        /// Gets the depth of this map.
        /// </summary>
        int MaximumDepth { get; }

        /// <summary>
        /// Gets the layout.
        /// </summary>
        IList<Rectangle> Layout { get; }
            
        /// <summary>
        /// Gets the areas.
        /// </summary>
        IList<IMapArea> Areas { get; }

        /// <summary>
        /// Gets the tile map.
        /// </summary>
        ITile[,] TileMap { get; }

        /// <summary>
        /// Gets the doodad map.
        /// </summary>
        ITile[,] DoodadMap { get; }

        /// <summary>
        /// Generates the map with a specified number of areas.
        /// </summary>
        /// <param name="gameMode">The game mode.</param>
        /// <param name="maximumAreas">The maximum number of areas.</param>
        /// <returns>
        /// An object that contains all of the entities for this particular map.
        /// </returns>
        IMapEntities Generate(GameMode gameMode, int maximumAreas);

        /// <summary>
        /// Generates a list of view port qualified bounding boxes for tile collision.
        /// </summary>
        /// <param name="viewPort">The view port.</param>
        /// <returns>List of view port qualified bounding boxes.</returns>
        IEnumerable<Rectangle> ViewPortTileBoundingBoxes(Rectangle viewPort);
    }
}
