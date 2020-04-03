using System;
using Abyss.World.Entities.Items;
using Abyss.World.Entities.Monsters;
using Abyss.World.Entities.Platforms;
using Abyss.World.Entities.Platforms.Concrete;
using Abyss.World.Entities.Props;
using Abyss.World.Entities.Props.Concrete;
using Abyss.World.Entities.Triggers;
using Abyss.World.Universe;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Occasus.Core.Drawing;
using Occasus.Core.Entities;
using Occasus.Core.Maths;
using Occasus.Core.Physics;
using Occasus.Core.Tiles;
using System.Collections.Generic;
using System.Linq;
using Abyss.World.Entities.Platforms.Concrete.Activated;

namespace Abyss.World.Maps
{
    public class Map : Entity, IMap
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Map" /> class.
        /// </summary>
        public Map()
            : base("Map", "Map in Abyss")
        {
            this.Tags.Add(EntityTags.Map);
            this.Layout = new List<Rectangle>();
            this.Areas = new List<IMapArea>();
        }

        /// <summary>
        /// Gets the depth of this map.
        /// </summary>
        public int MaximumDepth
        {
            get; private set;
        }

        /// <summary>
        /// Gets the layout.
        /// </summary>
        public IList<Rectangle> Layout
        {
            get; private set;
        }

        /// <summary>
        /// Gets the areas.
        /// </summary>
        public IList<IMapArea> Areas
        {
            get; private set;
        }

        /// <summary>
        /// Gets the tile map.
        /// </summary>
        public ITile[,] TileMap
        {
            get; private set;
        }

        /// <summary>
        /// Gets the doodad map.
        /// </summary>
        public ITile[,] DoodadMap
        {
            get; private set;
        }

        /// <summary>
        /// Generates the map with a specified number of areas.
        /// </summary>
        /// <param name="gameMode">Type of the game.</param>
        /// <param name="maximumAreas">The maximum number of areas.</param>
        /// <returns>
        /// An object that contains all of the entities for this particular map.
        /// </returns>
        public IMapEntities Generate(GameMode gameMode, int maximumAreas)
        {
            // Determine the number of random areas in this map; we always start with a normal area and a transition.
            var zones = new List<ZoneType>
            {
                ZoneType.Normal, 
                ZoneType.Transition
            };

            // After each random area, there is a transition area between them. This transition area provides various trans-dimensional challenges that get progressively harder as they go on.
            var randomAreaCount = MathsHelper.Random(3, maximumAreas);
            for (var i = 0; i < randomAreaCount; i++)
            {
                var zoneType = MapData.ZoneTypes.ElementAt(MathsHelper.Random(MapData.ZoneTypes.Count()));
                zones.Add(zoneType);
                zones.Add(ZoneType.Transition);
            }

            // Create a list of chunks for the game world. The first set is a blank set to help the player get more acquainted to the game.
            var chunkList = new List<IMapChunk>();
#if DEBUG
            // Add test stuff.
            var mapChunks = MapFactory.MapChunks[ZoneType.Test];

            // Blank chunks.

            // Beginning of the level
            //chunkList.Add(MapFactory.MapChunks[ZoneType.Tutorial].First(o => o.Type.Equals("begin", StringComparison.InvariantCultureIgnoreCase)));
            //chunkList.AddRange(MapFactory.MapChunks[ZoneType.Tutorial].Where(o => o.Type.Equals("movement", StringComparison.InvariantCultureIgnoreCase)));
            //chunkList.AddRange(MapFactory.GetBlankChunks(3));

            //chunkList.AddRange(mapChunks.Where(o => o.Type.Equals(MapChunkTypes.Moving, StringComparison.InvariantCultureIgnoreCase)));
            //chunkList.AddRange(mapChunks.Where(o => o.Type.Equals(MapChunkTypes.Crumbling, StringComparison.InvariantCultureIgnoreCase)));
            //chunkList.AddRange(mapChunks.Where(o => o.Type.Equals(MapChunkTypes.Crusher, StringComparison.InvariantCultureIgnoreCase)));
            //chunkList.AddRange(mapChunks.Where(o => o.Type.Equals(MapChunkTypes.Icy, StringComparison.InvariantCultureIgnoreCase)));
            //chunkList.AddRange(mapChunks.Where(o => o.Type.Equals(MapChunkTypes.Springy, StringComparison.InvariantCultureIgnoreCase)));
            //chunkList.AddRange(mapChunks.Where(o => o.Type.Equals(MapChunkTypes.Volatile, StringComparison.InvariantCultureIgnoreCase)));
            //chunkList.AddRange(mapChunks.Where(o => o.Type.Equals(MapChunkTypes.Treasure, StringComparison.InvariantCultureIgnoreCase)));
            //chunkList.AddRange(mapChunks.Where(o => o.Type.Equals(MapChunkTypes.Key, StringComparison.InvariantCultureIgnoreCase)));
            //chunkList.AddRange(mapChunks.Where(o => o.Type.Equals(MapChunkTypes.Doodads, StringComparison.InvariantCultureIgnoreCase)));
            //chunkList.AddRange(mapChunks.Where(o => o.Type.Equals(MapChunkTypes.Monsters, StringComparison.InvariantCultureIgnoreCase)));
            //chunkList.AddRange(mapChunks.Where(o => o.Type.Equals("Walker", StringComparison.InvariantCultureIgnoreCase)));
            //chunkList.AddRange(mapChunks.Where(o => o.Type.Equals("Shop", StringComparison.InvariantCultureIgnoreCase)));

            //chunkList.Add(MapFactory.GetMapChunks(ZoneType.Normal, "End").First());
#endif

            // Depending on the game type, create a starting area.
            switch (gameMode)
            {
                case GameMode.Normal:
                    chunkList.AddRange(MapFactory.GetStartingArea());
                    break;
                case GameMode.Daily:
                case GameMode.Speedrun:
                    chunkList.AddRange(MapFactory.GetBlankChunks(1, false));
                    var beginningChunks = MapFactory.MapChunks[ZoneType.Tutorial].Where(o => o.Type.Equals("Begin", StringComparison.InvariantCultureIgnoreCase)).ToList();
                    chunkList.Add(beginningChunks[MathsHelper.Random(beginningChunks.Count)]);
                    chunkList.AddRange(MapFactory.GetBlankChunks(1));
                    break;
            }

            // Build the randomised set of chunks that make up this map.
            foreach (var zone in zones)
            {
                chunkList.AddRange(MapFactory.GetArea(zone));
            }

            // Add an ending chunk to this map.
            chunkList.AddRange(MapFactory.GetBlankChunks(2));
            chunkList.Add(MapFactory.GetMapChunks(ZoneType.Normal, "End").First());
            chunkList.AddRange(MapFactory.GetBlankChunks(1));

            // Determine the maximum depth of the map based on the areas we have build so far.
            this.MaximumDepth += chunkList.Count * MapData.ChunkHeight;

            // Initialise the tile and doodad maps.
            this.TileMap = new ITile[PhysicsManager.MapWidth, this.MaximumDepth];
            this.DoodadMap = new ITile[PhysicsManager.MapWidth, this.MaximumDepth];
            for (var x = 0; x < PhysicsManager.MapWidth; x++)
            {
                for (var y = 0; y < this.MaximumDepth; y++)
                {
                    this.TileMap[x, y] = null;
                    this.DoodadMap[x, y] = null;
                }
            }

            // Compile all of the unique parts of each of the chunks to be processed.
            var currentDepth = 0;
            var chunkDepth = 0;
            var mapEntities = new MapEntities();
            this.Layout.Clear();
            foreach (var c in chunkList)
            {
                var offset = new Vector2(0, chunkDepth * (MapData.ChunkHeight * DrawingManager.TileHeight));
                var connectionId = Guid.NewGuid().ToString();

                // Add all of the layout rectangles (used for collision detection)
                foreach (var r in c.Layout)
                {
                    this.Layout.Add(new Rectangle(r.X + (int)offset.X, r.Y + (int)offset.Y, r.Width, r.Height));
                }

                // Add all of the required entities.
                foreach (var sp in c.SpecialPlatforms)
                {
                    // Clone the platform definition object so we have a discrete instance to play with.
                    var definition = sp.Clone();
                    definition.Position = definition.Position + offset;

                    // For special platforms that have paths, we need to alter their destinations so that they appear in the correct spot.
                    var paths = definition.Path.ToList();
                    definition.Path.Clear();
                    foreach (var p in paths)
                    {
                        definition.Path.Add(p + offset);
                    }

                    // Do some set up to make sure that connected platforms can be connected.
                    var platform = PlatformFactory.GetPlatform(definition);
                    switch (platform.Name)
                    {
                        case PlatformKeys.Key:
                        case PlatformKeys.Gate:
                            var connectedPlatform = (IConnectedPlatform) platform;
                            connectedPlatform.ConnectedId = connectionId;
                            break;
                    }

                    mapEntities.Platforms.Add(platform);
                }

                foreach (var p in c.PropSpawnPoints)
                {
                    var position = new Vector2(p.Item2.X, p.Item2.Y);
                    position += offset;

                    var boundingRectangle = new Rectangle(0, 0, p.Item2.Width, p.Item2.Height);

                    if (p.Item1.Equals("Light"))
                    {
                        mapEntities.Props.Add(PropFactory.GetLight(position, Color.White, 0.8f, 2f));
                    }
                    else if (p.Item1.Equals("Waterfall"))
                    {
                        mapEntities.Props.Add(PropFactory.GetProp(p.Item1, position, boundingRectangle));
                    }
                    else
                    {
                        mapEntities.Props.Add(PropFactory.GetProp(p.Item1, position));
                    }
                }

                foreach (var m in c.MonsterSpawnPoints)
                {
                    var position = m.Position + offset;
                    var path = new List<Vector2>();
                    foreach (var p in m.Path)
                    {
                        path.Add(p + offset);
                    }

                    mapEntities.Monsters.Add(MonsterFactory.GetMonster(m.Name, position, path));
                }

                foreach (var i in c.ItemSpawnPoints)
                {
                    // Items have a 80% chance of spawning, and then are random in nature.
                    if (MathsHelper.Random() > 20)
                    {
                        var position = i + offset;

                        var r = MathsHelper.Random();
                        if (r < 65)
                        {
                            mapEntities.Items.Add(ItemFactory.GetRiftShard("SmallRiftShard", position));
                        }
                        else
                        {
                            mapEntities.Items.Add(ItemFactory.GetRiftShard("LargeRiftShard", position));
                        }
                    }
                }

                foreach (var h in c.Hazards)
                {
                    var position = h + offset;
                    mapEntities.Hazards.Add(PropFactory.GetRandomHarmfulProp(position));
                }

                foreach (var t in c.Triggers)
                {
                    var position = new Vector2(t.Item2.X, t.Item2.Y);
                    position += offset;
                    var boundingRectangle = new Rectangle(0, 0, t.Item2.Width, t.Item2.Height);
                    mapEntities.Triggers.Add(TriggerFactory.GetTrigger(t.Item1, position, boundingRectangle));
                }

                // Process tiles and doodads so they all appear in the correct spot.
                // The depth of each tile/doodad is offset by the depth of a chunk; so it needs a local var.
                var maxDepth = currentDepth + MapData.ChunkHeight;
                var localDepth = 0;
                for (var y = currentDepth; y < maxDepth; y++)
                {
                    for (var x = 0; x < MapData.ChunkWidth; x++)
                    {
                        var tile = c.TileMap[x, localDepth];
                        if (tile != null)
                        {
                            this.TileMap[x, y] = tile;
                        }

                        var doodad = c.DoodadMap[x, localDepth];
                        if (doodad != null)
                        {
                            doodad.Opacity = 0.6f;
                            this.DoodadMap[x, y] = doodad;
                        } 
                    }

                    localDepth++;
                }

                // Update the depth so we can continue to add all the tiles and doodads to the correct locations.
                currentDepth = maxDepth;
                chunkDepth++;
            }

            // Go through the map entities and connect things that need to be connected.
            var riftGates = mapEntities.Platforms.Where(o => o.Name == PlatformKeys.Gate);
            foreach (var riftGate in riftGates)
            {
                var r = (Gate) riftGate;
                var keys = mapEntities.Platforms.Where(o => o.Name == PlatformKeys.Key).ToList();
                foreach (var key in keys)
                {
                    var k = (Key) key;
                    if (k.ConnectedId.Equals(r.ConnectedId))
                    {
                        k.ConnectedGate = r;
                    }
                }
            }

            return mapEntities;
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
            for (var y = GameManager.GameViewPort.Top; y < GameManager.GameViewPort.Bottom; y++)
            {
                for (var x = 0; x < MapData.ChunkWidth; ++x)
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

            base.Draw(gameTime, spriteBatch);
        }
    }
}
