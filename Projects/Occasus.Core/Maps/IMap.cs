using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Occasus.Core.Entities;
using Occasus.Core.Maps.Tiles;

namespace Occasus.Core.Maps
{
    public interface IMap : IEntity
    {
        /// <summary>
        /// Gets the layout.
        /// </summary>
        IList<Rectangle> Layout { get; }

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
        /// <returns>
        /// An object that contains all of the entities for this particular map.
        /// </returns>
        IDictionary<string, IEnumerable<IEntity>> Generate(params object[] args);

        /// <summary>
        /// Generates a list of view port qualified bounding boxes for tile collision.
        /// </summary>
        /// <param name="viewPort">The view port.</param>
        /// <returns>List of view port qualified bounding boxes.</returns>
        IEnumerable<Rectangle> ViewPortTileBoundingBoxes(Rectangle viewPort);
    }
}
