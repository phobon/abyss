using System;
using Abyss.World.Entities.Items;
using Abyss.World.Entities.Monsters;
using Abyss.World.Entities.Platforms;
using Abyss.World.Entities.Platforms.Concrete;
using Abyss.World.Entities.Props;
using Abyss.World.Entities.Triggers;
using Abyss.World.Universe;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Occasus.Core.Drawing;
using Occasus.Core.Entities;
using Occasus.Core.Maths;
using Occasus.Core.Physics;
using System.Collections.Generic;
using System.Linq;
using Abyss.World.Entities.Platforms.Concrete.Activated;
using Occasus.Core;
using Occasus.Core.Maps;
using Occasus.Core.Maps.Definitions;

namespace Abyss.World.Maps
{
    public class AbyssMap : Map, IAbyssMap
    {
        public const string Platforms = "Platforms";
        public const string Hazards = "Hazards";
        public const string Props = "Props";
        public const string Items = "Items";
        public const string Monsters = "Monsters";
        public const string Triggers = "Triggers";

        /// <summary>
        /// Initializes a new instance of the <see cref="Map" /> class.
        /// </summary>
        public AbyssMap()
            : base("Map", "Map in Abyss", PhysicsManager.MapWidth, MapData.ChunkHeight)
        {
        }

        /// <summary>
        /// Generates the map with a specified number of areas.
        /// </summary>
        /// <returns>
        /// An object that contains all of the entities for this particular map.
        /// </returns>
        public override IDictionary<string, IEnumerable<IEntity>> Generate(params object[] args)
        {
            if (args == null)
            {
                throw new ArgumentNullException(nameof(args));
            }

            if (args.Length == 1)
            {
                throw new ArgumentException("Must provide: GameMode, MaximumAreas");
            }
            
            var gameMode = (GameMode)args[0];
            var maximumAreas = (int)args[1];

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
            this.Height += chunkList.Count * MapData.ChunkHeight;

            // Initialise the tile and doodad maps.
            this.InitializeMaps();

            // Compile all of the unique parts of each of the chunks to be processed.
            var currentDepth = 0;
            var chunkDepth = 0;
            var mapEntities = new Dictionary<string, IEnumerable<IEntity>>();
            this.Layout.Clear();

            var platforms = new List<IPlatform>();
            var props = new List<IProp>();
            var monsters = new List<IMonster>();
            var items = new List<IItem>();
            var hazards = new List<IEntity>();
            var triggers = new List<ITrigger>();
            foreach (var c in chunkList)
            {
                var offset = new Vector2(0, chunkDepth * (MapData.ChunkHeight * DrawingManager.TileHeight));
                var connectionId = Guid.NewGuid().ToString();

                // Add all of the layout rectangles (used for collision detection)
                foreach (var r in c.Layout[MapChunkData.NormalPlatforms])
                {
                    var rect = new Rectangle((int) r.Position.X + (int) offset.X, (int) r.Position.Y + (int) offset.Y, r.Size.Width, r.Size.Height);
                    this.Layout.Add(rect);
                }

                // Add all of the required entities.
                foreach (var sp in c.Layout[MapChunkData.SpecialPlatforms])
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

                    platforms.Add(platform);
                }
                
                foreach (IAreaSpawnDefinition p in c.Spawns[MapChunkData.PropSpawns])
                {
                    var position = new Vector2(p.Area.X, p.Area.Y);
                    position += offset;

                    var boundingRectangle = new Rectangle(0, 0, p.Area.Width, p.Area.Height);

                    if (p.Name.Equals("Light"))
                    {
                        props.Add(PropFactory.GetLight(position, Color.White, 0.8f, 2f));
                    }
                    else if (p.Name.Equals("Waterfall"))
                    {
                        props.Add(PropFactory.GetProp(p.Name, position, boundingRectangle));
                    }
                    else
                    {
                        props.Add(PropFactory.GetProp(p.Name, position));
                    }
                }

                foreach (IActorSpawnDefinition m in c.Spawns[MapChunkData.MonsterSpawns])
                {
                    var position = m.Position + offset;
                    var path = new List<Vector2>();
                    foreach (var p in m.Path)
                    {
                        path.Add(p + offset);
                    }

                    monsters.Add(MonsterFactory.GetMonster(m.Name, position, path));
                }
                
                foreach (var i in c.Spawns[MapChunkData.ItemSpawns])
                {
                    // Items have a 80% chance of spawning, and then are random in nature.
                    if (MathsHelper.Random() > 20)
                    {
                        var position = i.Position + offset;

                        var r = MathsHelper.Random();
                        if (r < 65)
                        {
                            items.Add(ItemFactory.GetRiftShard("SmallRiftShard", position));
                        }
                        else
                        {
                            items.Add(ItemFactory.GetRiftShard("LargeRiftShard", position));
                        }
                    }
                }

                foreach (var h in c.Spawns[MapChunkData.HazardSpawns])
                {
                    var position = h.Position + offset;
                    hazards.Add(PropFactory.GetRandomHarmfulProp(position));
                }
                
                foreach (IAreaSpawnDefinition t in c.Spawns[MapChunkData.TriggerSpawns])
                {
                    var position = new Vector2(t.Area.X, t.Area.Y);
                    position += offset;
                    var boundingRectangle = new Rectangle(0, 0, t.Area.Width, t.Area.Height);
                    triggers.Add(TriggerFactory.GetTrigger(t.Name, position, boundingRectangle));
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

            mapEntities.Add(Platforms, platforms);
            mapEntities.Add(Props, props);
            mapEntities.Add(Monsters, monsters);
            mapEntities.Add(Items, items);
            mapEntities.Add(Hazards, hazards);
            mapEntities.Add(Triggers, triggers);

            // Go through the map entities and connect things that need to be connected.
            var riftGates = platforms.Where(o => o.Name == PlatformKeys.Gate);
            foreach (var riftGate in riftGates)
            {
                var r = (Gate) riftGate;
                var keys = platforms.Where(o => o.Name == PlatformKeys.Key).ToList();
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
        /// Draws the Engine Component.
        /// </summary>
        /// <param name="gameTime">The game time object.</param>
        /// <param name="spriteBatch">The sprite batch.</param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for (var y = Monde.GameManager.ViewPort.Top; y < Monde.GameManager.ViewPort.Bottom; y++)
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
        }
    }
}
