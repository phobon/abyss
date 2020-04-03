using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Occasus.Core.Drawing;
using Occasus.Core.Entities;
using Occasus.Core.Physics;
using Occasus.Core.Maps.Tiles;
using System.Collections.Generic;

namespace Occasus.Core.Maps
{
    public abstract class Map : Entity, IMap
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Map" /> class.
        /// </summary>
        protected Map(string name, string description, int width, int height)
            : base(name, description)
        {
            this.Layout = new List<Rectangle>();
            this.Width = width;
            this.Height = height;
        }

        public int Width { get; protected set; }

        public int Height { get; protected set; }

        /// <summary>
        /// Gets the layout.
        /// </summary>
        public IList<Rectangle> Layout { get; }

        /// <summary>
        /// Gets the tile map.
        /// </summary>
        public ITile[,] TileMap { get; private set; }

        /// <summary>
        /// Gets the doodad map.
        /// </summary>
        public ITile[,] DoodadMap { get; private set; }

        /// <summary>
        /// Generates the map with a specified number of areas.
        /// </summary>
        /// <returns>
        /// An object that contains all of the entities for this particular map.
        /// </returns>
        public virtual IDictionary<string, IEnumerable<IEntity>> Generate(params object[] args)
        {
            this.InitializeMaps();
            return new Dictionary<string, IEnumerable<IEntity>>();
        }

        /// <summary>
        /// Generates a list of view port qualified bounding boxes for tile collision.
        /// </summary>
        /// <param name="viewPort">The view port.</param>
        /// <returns>
        /// List of view port qualified bounding boxes.
        /// </returns>
        public IEnumerable<Rectangle> ViewPortTileBoundingBoxes(Rectangle viewPort)
        {
            var boxes = new List<Rectangle>();
            var viewPortTop = viewPort.Top * DrawingManager.TileHeight;
            var viewPortBottom = viewPort.Bottom * DrawingManager.TileHeight;
            foreach (var r in this.Layout)
            {
                if (r.Top >= viewPortTop && r.Bottom <= viewPortBottom)
                {
                    boxes.Add(r);
                }
            }

            return boxes;
        }

        /// <summary>
        /// Draws the Engine Component.
        /// </summary>
        /// <param name="gameTime">The game time object.</param>
        /// <param name="spriteBatch">The sprite batch.</param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for (var y = 0; y < this.TileMap.GetLength(1); y++)
            {
                for (var x = 0; x < this.TileMap.GetLength(0); ++x)
                {
                    // Check whether there is a drawable tile in that position.
                    var tile = this.TileMap[x, y];
                    if (tile != null && tile.TileType != TileCollisitionState.Empty && tile.TileType != TileCollisitionState.Barrier)
                    {
                        tile.Transform.Position = new Vector2(x, y) * DrawingManager.TileWidth;
                        tile.Draw(gameTime, spriteBatch);
                    }

                    // Draw doodads.
                    var doodad = this.DoodadMap[x, y];
                    if (doodad != null && doodad.TileType != TileCollisitionState.Empty)
                    {
                        doodad.Transform.Position = new Vector2(x, y) * DrawingManager.TileHeight;
                        doodad.Draw(gameTime, spriteBatch);
                    }
                }
            }
        }

        protected override void InitializeTags()
        {
            this.Tags.Add("Map");
        }

        protected override void InitializeComponents()
        {
            base.InitializeComponents();
        }

        protected void InitializeMaps()
        {
            // Initialise the tile and doodad maps.
            this.TileMap = new ITile[this.Width, this.Height];
            this.DoodadMap = new ITile[this.Width, this.Height];

            for (var x = 0; x < this.Width; x++)
            {
                for (var y = 0; y < this.Height; y++)
                {
                    this.TileMap[x, y] = null;
                    this.DoodadMap[x, y] = null;
                }
            }
        }
    }
}
