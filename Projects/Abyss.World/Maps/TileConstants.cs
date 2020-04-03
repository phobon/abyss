using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Occasus.Core.Physics;
using Occasus.Core.Tiles;

namespace Abyss.World.Maps
{
    /// <summary>
    /// Constants related to Tiles in Abyss.
    /// </summary>
    public static class TileConstants
    {
        /// <summary>
        /// Gets an empty tile.
        /// </summary>
        public static ITile EmptyTile
        {
            get
            {
                return new Tile(string.Empty, new List<int> { 0 }, 0, TileCollisitionState.Empty, Rectangle.Empty);
            }
        }
    }
}
