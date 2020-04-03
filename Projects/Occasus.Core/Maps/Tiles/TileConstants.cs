using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Occasus.Core.Physics;

namespace Occasus.Core.Maps.Tiles
{
    public static class TileConstants
    {
        /// <summary>
        /// Gets an empty tile.
        /// </summary>
        public static ITile EmptyTile => new Tile(string.Empty, new List<int> { 0 }, 0, TileCollisitionState.Empty, Rectangle.Empty);
    }
}
