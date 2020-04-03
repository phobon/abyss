using Abyss.World.Entities.Monsters;
using Abyss.World.Entities.Platforms;
using Abyss.World.Universe;
using Microsoft.Xna.Framework;
using Occasus.Core.Assets;
using Occasus.Core.Assets.AtlasDefinitions;
using Occasus.Core.Drawing;
using Occasus.Core.Maths;
using Occasus.Core.Physics;
using Occasus.Core.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Abyss.World.Maps
{
    public static class MapFactory
    {
        private const int BackgroundOffset = 13;
        private const int MapChunkHeight = 17;

        private static IDictionary<ZoneType, IList<IMapChunk>> mapChunks;
        private static IDictionary<ZoneType, IZoneData> zoneDataReference;

        public static IDictionary<ZoneType, IZoneData> ZoneDataReference
        {
            get
            {
                return zoneDataReference ?? (zoneDataReference = new Dictionary<ZoneType, IZoneData>());
            }
        }

        public static IDictionary<ZoneType, IList<IMapChunk>> MapChunks
        {
            get
            {
                if (mapChunks == null)
                {
                    mapChunks = new Dictionary<ZoneType, IList<IMapChunk>>
                        {
                            { ZoneType.Normal, new List<IMapChunk>() },
                            //{ ZoneType.Fire, new List<IMapChunk>() },
                            //{ ZoneType.Ice, new List<IMapChunk>() },
                            //{ ZoneType.Electric, new List<IMapChunk>() },
                            //{ ZoneType.Shadow, new List<IMapChunk>() },
                            //{ ZoneType.Void, new List<IMapChunk>() },
                            { ZoneType.Transition, new List<IMapChunk>() },
                            { ZoneType.Tutorial, new List<IMapChunk>() },
                            { ZoneType.Test, new List<IMapChunk>() }
                        };
                }

                return mapChunks;
            }
        }

        public static IEnumerable<IMapChunk> GetBlankChunks(int count, bool hasSpawnPoints = true)
        {
            var chunks = new List<IMapChunk>();
            for (var i = 0; i < count; i++)
            {
                var chunk = mapChunks[ZoneType.Transition].First();
                if (!hasSpawnPoints)
                {
                    chunk.MonsterSpawnPoints.Clear();
                    chunk.ItemSpawnPoints.Clear();
                }

                chunks.Add(chunk);
            }

            return chunks;
        }

        public static IEnumerable<IMapChunk> GetStartingArea()
        {
            // Add some empty chunks.
            var chunks = new List<IMapChunk>();
            chunks.AddRange(GetBlankChunks(2, false));

            // Add tutorial chunks.
            chunks.AddRange(MapChunks[ZoneType.Tutorial].Where(o => o.Type.Equals("Movement", StringComparison.InvariantCultureIgnoreCase)));
            chunks.AddRange(MapChunks[ZoneType.Tutorial].Where(o => o.Type.Equals("DimensionShift", StringComparison.InvariantCultureIgnoreCase)));
            chunks.AddRange(MapChunks[ZoneType.Tutorial].Where(o => o.Type.Equals("Key", StringComparison.InvariantCultureIgnoreCase)));

            // Beginning chunk.
            var beginningChunks = MapChunks[ZoneType.Tutorial].Where(o => o.Type.Equals("Begin", StringComparison.InvariantCultureIgnoreCase)).ToList();
            chunks.Add(beginningChunks[MathsHelper.Random(beginningChunks.Count)]);

            return chunks;
        }

        public static IEnumerable<IMapChunk> GetArea(ZoneType key)
        {
            // TODO: This is gross, change.
            if (!mapChunks.ContainsKey(key))
            {
                return new List<IMapChunk>();
            }

            var zoneData = ZoneDataReference[key];

            // Determine a random number of chunks.
            var chunkCount = MathsHelper.Random(zoneData.MinimumChunkCount, zoneData.MaximumChunkCount);
            
            // Create a new area based on the structure found in the zone data object.
            var chunks = new List<IMapChunk>();
            
            // TODO: Always start with a specific type of chunk.
            
            var structureIndex = 0;
            for (var i = 0; i < chunkCount; i++)
            {
                // Determine whether we go with the type of chunk specified in the structure, or with the default.
                var structure = zoneData.Structure.ElementAt(structureIndex);
                var selectedChunks = MathsHelper.Random() < structure.Item2 
                    ? mapChunks[key].Where(o => o.Type.Equals(structure.Item1, StringComparison.InvariantCultureIgnoreCase)).ToList() 
                    : mapChunks[key].Where(o => o.Type.Equals(zoneData.DefaultChunkType, StringComparison.InvariantCultureIgnoreCase)).ToList();

                // Determine a random chunk to add to the list.
                var index = MathsHelper.Random(selectedChunks.Count);
                chunks.Add(selectedChunks[index]);

                structureIndex++;
                if (structureIndex == zoneData.Structure.Count())
                {
                    structureIndex = 0;
                }
            }

            // TODO: Always end an area with a particular type of chunk.

            return chunks;
        }

        public static IEnumerable<IMapChunk> GetMapChunks(ZoneType key, string chunkType)
        {
            if (!mapChunks.ContainsKey(key))
            {
                return null;
            }

            return mapChunks[key].Where(o => o.Type.Equals(chunkType, StringComparison.InvariantCultureIgnoreCase)).ToList();
        }

        public static void LoadContent()
        {
            LoadMapChunks();
            LoadZoneData();
        }

        private static void LoadMapChunks()
        {
            // Here is where we load all of the data we need.
            var keys = MapChunks.Keys.ToList();
            foreach (var key in keys)
            {
                var stream = TitleContainer.OpenStream("Content\\Data\\Level\\" + key + ".xml");
                var doc = XDocument.Load(stream);
                var chunks = new List<IMapChunk>();
                foreach (var chunk in doc.Descendants("Chunks").Descendants("Chunk"))
                {
                    var newChunk = new MapChunk(chunk.Attribute("type").Value);

                    // Create a couple of collections here to help us with entity placement later in the process.
                    var layoutCells = new List<Point>();
                    var groundedCells = new List<Point>();

                    // Layout rectangles.
                    foreach (var t in chunk.Descendants("Layout").Descendants("rect"))
                    {
                        var x = int.Parse(t.Attribute("x").Value);
                        var y = int.Parse(t.Attribute("y").Value);
                        var width = int.Parse(t.Attribute("w").Value);
                        var height = int.Parse(t.Attribute("h").Value);
                        newChunk.Layout.Add(new Rectangle(x, y, width, height));

                        // This is a bit of weird code to help us later when we add items and monsters.
                        var leftCell = x / DrawingManager.TileWidth;
                        var topCell = y / DrawingManager.TileHeight;
                        var verticalCells = topCell + (height / DrawingManager.TileHeight);
                        var horizontalCells = leftCell + (width / DrawingManager.TileWidth);

                        // Add a grounded cell, this is used when we add items and monsters later.
                        // We're adding the grounded cell to the layout cell collection as well so that we don't spawn a flying monster in a cell 
                        // that a grounded monster could be.
                        var groundedCellPosition = topCell - 1;
                        for (var h = leftCell; h < horizontalCells; h++)
                        {
                            var groundedCell = new Point(h, groundedCellPosition);
                            groundedCells.Add(groundedCell);
                            layoutCells.Add(groundedCell);
                        }

                        for (var v = topCell; v < verticalCells; v++)
                        {
                            for (var h = leftCell; h < horizontalCells; h++)
                            {
                                // Add a layout cell, this is something to help us when we add flying monsters.
                                layoutCells.Add(new Point(h, v));
                            }
                        }
                    }
                    
                    // Load background geometry.
                    LoadGeometry(chunk, newChunk, "Normal", "Background", BackgroundOffset);

                    // Load foreground geometry.
                    var doodadMap = LoadGeometry(chunk, newChunk, "Normal", "Foreground", 0);

                    // Load tile-based doodads.
                    for (var y = 0; y < MapChunkHeight; y++)
                    {
                        for (var x = 0; x < PhysicsManager.MapWidth; x++)
                        {
                            if (doodadMap[x, y] == 0)
                            {
                                continue;
                            }

                            if (MathsHelper.Random() > 25)
                            {
                                newChunk.DoodadMap[x, y] = Atlas.GetDoodad("Normal", DoodadPlacement.Tile, doodadMap[x, y]);
                            }
                        }
                    }

                    // Load doodads that appear above the ground.
                    var randomDoodadMap = Atlas.GetDoodadGroupIndexMap("Normal", DoodadPlacement.Above);
                    var aboveDoodadMap = new int[PhysicsManager.MapWidth, MapChunkHeight];
                    var aboveDoodadNode = chunk.Descendants("UpDoodad").First().Value;
                    var count = 0;
                    for (var y = 0; y < MapChunkHeight; y++)
                    {
                        for (var x = 0; x < PhysicsManager.MapWidth; x++)
                        {
                            if (aboveDoodadNode[count] == '\n')
                            {
                                count++;
                            }

                            if (aboveDoodadNode[count] == '1')
                            {
                                aboveDoodadMap[x, y] = 1;
                            }

                            count++;
                        }
                    }

                    for (var y = 0; y < MapChunkHeight; y++)
                    {
                        for (var x = 0; x < PhysicsManager.MapWidth; x++)
                        {
                            if (aboveDoodadMap[x, y] == 0)
                            {
                                continue;
                            }

                            if (MathsHelper.Random() > 35)
                            {
                                var doodadGroup = randomDoodadMap.Values.ElementAt(MathsHelper.Random(randomDoodadMap.Count));
                                var doodad = doodadGroup[MathsHelper.Random(doodadGroup.Count)];
                                newChunk.DoodadMap[x, y] = Atlas.GetDoodad(doodad.X, doodad.Y);
                            }
                        }
                    }

                    randomDoodadMap = Atlas.GetDoodadGroupIndexMap("Normal", DoodadPlacement.Below);
                    var belowDoodadMap = new int[PhysicsManager.MapWidth, MapChunkHeight];
                    var belowDoodadNode = chunk.Descendants("DownDoodad").First().Value;
                    count = 0;
                    for (var y = 0; y < MapChunkHeight; y++)
                    {
                        for (var x = 0; x < PhysicsManager.MapWidth; x++)
                        {
                            if (belowDoodadNode[count] == '\n')
                            {
                                count++;
                            }

                            if (belowDoodadNode[count] == '1')
                            {
                                belowDoodadMap[x, y] = 1;
                            }

                            count++;
                        }
                    }

                    for (var y = 0; y < MapChunkHeight; y++)
                    {
                        for (var x = 0; x < PhysicsManager.MapWidth; x++)
                        {
                            if (belowDoodadMap[x, y] == 0)
                            {
                                continue;
                            }

                            if (MathsHelper.Random() > 35)
                            {
                                var doodadGroup = randomDoodadMap.Values.ElementAt(MathsHelper.Random(randomDoodadMap.Count));
                                var doodad = doodadGroup[MathsHelper.Random(doodadGroup.Count)];
                                newChunk.DoodadMap[x, y] = Atlas.GetDoodad(doodad.X, doodad.Y);
                            }
                        }
                    }

                    // TODO: Load above and below doodads based on bitstrings in XML.

                    // Determine geometry placement.
                    //foreach (var t in chunk.Descendants("Geometry").Descendants("tile"))
                    //{
                    //    var tileData = new Tile(
                    //        "Zones", 
                    //        PhysicsConstants.Collidable, 
                    //        new Rectangle(
                    //            int.Parse(t.Attribute("tx").Value) * DrawingManager.TileDimension,
                    //            int.Parse(t.Attribute("ty").Value) * DrawingManager.TileDimension,
                    //            DrawingManager.TileDimension,
                    //            DrawingManager.TileDimension));

                    //    var x = int.Parse(t.Attribute("x").Value);
                    //    var y = int.Parse(t.Attribute("y").Value);
                    //    newChunk.TileMap[x, y] = tileData;
                    //}

                    // Determine doodads placement.
                    //foreach (var t in chunk.Descendants("Doodads").Descendants("tile"))
                    //{
                    //    var spriteX = int.Parse(t.Attribute("tx").Value);
                    //    var spriteY = int.Parse(t.Attribute("ty").Value);

                    //    var x = int.Parse(t.Attribute("x").Value);
                    //    var y = int.Parse(t.Attribute("y").Value);
                    //    newChunk.DoodadMap[x, y] = Atlas.GetDoodad(spriteX, spriteY);
                    //}

                    // Add special platforms.
                    foreach (var t in chunk.Descendants("Platforms").Descendants())
                    {
                        if (t.Name == "node")
                        {
                            continue;
                        }

                        var position = new Vector2(
                            int.Parse(t.Attribute("x").Value),
                            int.Parse(t.Attribute("y").Value));
                        var width = int.Parse(t.Attribute("width").Value);
                        var height = int.Parse(t.Attribute("height").Value);
                        var size = new Rectangle(0, 0, width, height);

                        var platformDefinition = new PlatformDefinition(t.Name.LocalName, position, size);

                        // Add any nodes as required.
                        var nodes = t.Descendants("node");
                        foreach (var n in nodes)
                        {
                            var nodePosition = new Vector2(
                                int.Parse(n.Attribute("x").Value),
                                int.Parse(n.Attribute("y").Value));
                            platformDefinition.Path.Add(nodePosition);
                        }

                        // Add a path node to reset to.
                        platformDefinition.Path.Add(position);

                        // TODO: This is kind of janky - think of a better way to do this.
                        if (t.HasAttributes && t.Attribute("Warp") != null)
                        {
                            platformDefinition.Parameters.Add("Warp", int.Parse(t.Attribute("Warp").Value));
                        }

                        newChunk.SpecialPlatforms.Add(platformDefinition);
                    }

                    // Props spawn points. These are determined by the map itself.
                    foreach (var t in chunk.Descendants("Props").Descendants())
                    {
                        var x = int.Parse(t.Attribute("x").Value);
                        var y = int.Parse(t.Attribute("y").Value);

                        var width = DrawingManager.TileWidth;
                        var height = DrawingManager.TileHeight;
                        if (t.Attribute("height") != null)
                        {
                            height = int.Parse(t.Attribute("height").Value);
                        }

                        if (t.Attribute("width") != null)
                        {
                            width = int.Parse(t.Attribute("width").Value);
                        }

                        newChunk.PropSpawnPoints.Add(Tuple.Create(t.Name.LocalName, new Rectangle(x, y, width, height)));
                    }

                    // Trigger placement. These are determined by the map itself.
                    foreach (var t in chunk.Descendants("Triggers").Descendants())
                    {
                        var x = int.Parse(t.Attribute("x").Value);
                        var y = int.Parse(t.Attribute("y").Value);
                        var width = int.Parse(t.Attribute("width").Value);
                        var height = int.Parse(t.Attribute("height").Value);
                        newChunk.Triggers.Add(Tuple.Create(t.Name.LocalName, new Rectangle(x, y, width, height)));
                    }
                    
                    // Hazard spawn points.
                    foreach (var t in chunk.Descendants("Hazards").Descendants())
                    {
                        var position = new Vector2(
                            int.Parse(t.Attribute("x").Value),
                            int.Parse(t.Attribute("y").Value));
                        newChunk.Hazards.Add(position);
                    }

                    // If we have a map chunk of type "begin", don't do anything here.
                    if (key == ZoneType.Tutorial || newChunk.Type.Equals("Begin", StringComparison.InvariantCultureIgnoreCase))
                    {
                        chunks.Add(newChunk);
                        continue;
                    }

                    // The grand entity placement algorithm.

                    // Item spawn points.
                    // Iterate through the grounded positions and place an item spawn point based on some chance. It doesn't matter if monsters overlap.
                    foreach (var g in groundedCells)
                    {
                        if (MathsHelper.Random() < 70)
                        {
                            continue;
                        }

                        newChunk.ItemSpawnPoints.Add(new Vector2(g.X * DrawingManager.TileWidth, g.Y * DrawingManager.TileHeight));
                    }

                    // Idler spawn points. 
                    // Iterate through the grounded positions and place an idler monster spawn point. These should not overlap with other monsters.
                    var monsterSpawns = new List<Point>();
                    foreach (var g in groundedCells)
                    {
                        if (MathsHelper.Random() < 80)
                        {
                            continue;
                        }

                        newChunk.MonsterSpawnPoints.Add(new MonsterSpawnDefinition("Idler", new Vector2(g.X * DrawingManager.TileWidth, g.Y * DrawingManager.TileHeight), new List<Vector2>()));
                        monsterSpawns.Add(g);
                    }

                    // Clean up the potential grounded cells list.
                    foreach (var m in monsterSpawns)
                    {
                        groundedCells.Remove(m);
                    }

                    monsterSpawns.Clear();

                    // Walker spawn points.
                    // This requires a bit of special processing here because we need to determine its path. Walkers can spawn on any platform but limit this to 1 per platform.
                    foreach (var r in newChunk.Layout)
                    {
                        // We can't have walkers on platforms that are only 1 cell in width.
                        if (r.Width == DrawingManager.TileWidth)
                        {
                            continue;
                        }

                        if (MathsHelper.Random() < 80)
                        {
                            continue;
                        }

                        var verticalSpawnPosition = r.Top - DrawingManager.TileHeight;

                        // Determine a horizontal spawn position. This is either on the far left or the far right of the platform.
                        var path = new List<Vector2>();
                        Vector2 spawn;
                        if (MathsHelper.Random() > 50)
                        {
                            // Spawn on the right.
                            spawn = new Vector2(r.Right - DrawingManager.TileWidth, verticalSpawnPosition);
                            path.Add(new Vector2(r.Left, verticalSpawnPosition));
                        }
                        else
                        {
                            // Spawn on the left.
                            spawn = new Vector2(r.Left, verticalSpawnPosition);
                            path.Add(new Vector2(r.Right - DrawingManager.TileWidth, verticalSpawnPosition));
                        }

                        newChunk.MonsterSpawnPoints.Add(new MonsterSpawnDefinition("Walker", spawn, path));

                        // Update the monster spawns list.
                        var x = (int)spawn.X / DrawingManager.TileWidth;
                        var y = (int)spawn.Y / DrawingManager.TileHeight;
                        monsterSpawns.Add(new Point(x, y));
                    }

                    // Clean up the potential grounded cells list.
                    foreach (var m in monsterSpawns)
                    {
                        groundedCells.Remove(m);
                    }

                    monsterSpawns.Clear();

                    // Flying enemy spawn points.
                    // Flying enemies can spawn wherever a platform isn't. Floaters are more likely than flyers.
                    var flyingCount = MathsHelper.Random(6);
                    for (var i = 0; i < flyingCount; i++)
                    {
                        if (MathsHelper.Random() < 80)
                        {
                            continue;
                        }

                        var p = new Point(MathsHelper.Random(PhysicsManager.MapWidth), MathsHelper.Random(PhysicsManager.MapHeight));
                        while (layoutCells.Contains(p))
                        {
                            p = new Point(MathsHelper.Random(PhysicsManager.MapWidth), MathsHelper.Random(PhysicsManager.MapHeight));
                        }

                        var spawn = new Vector2(p.X * DrawingManager.TileWidth, p.Y * DrawingManager.TileHeight);
                        if (MathsHelper.Random() > 60)
                        {
                            var path = new List<Vector2>();

                            // Get a list of waypoints.
                            var waypoints = new List<Point>();
                            for (var z = 0; z < PhysicsManager.MapWidth; z++)
                            {
                                waypoints.Add(new Point(z, p.Y));
                            }

                            // Going left, determine how where we can travel before we have to turn back.
                            var index = p.X - 1;
                            var distance = 0;
                            if (index >= 0)
                            {
                                // We can actually go left. Check whether there's a platform blocking it.
                                while (!layoutCells.Contains(waypoints[index]) && index > 0)
                                {
                                    distance++;
                                    index--;
                                }

                                if (distance > 0)
                                {
                                    path.Add(new Vector2(index * DrawingManager.TileWidth, p.Y * DrawingManager.TileHeight));
                                }
                            }

                            // Now go right and see how we go.
                            index = p.X + 1;
                            distance = 0;
                            if (index < PhysicsManager.MapWidth)
                            {
                                // We can actually go right. Check whether there's a platform blocking it.
                                while (!layoutCells.Contains(waypoints[index]) && index < PhysicsManager.MapWidth - 1)
                                {
                                    distance++;
                                    index++;
                                }

                                if (distance > 0)
                                {
                                    path.Add(new Vector2(index * DrawingManager.TileWidth, p.Y * DrawingManager.TileHeight));
                                }
                            }

                            newChunk.MonsterSpawnPoints.Add(new MonsterSpawnDefinition("Flyer", spawn, path));
                        }
                        else
                        {
                            // Just a regular floater enemy.
                            newChunk.MonsterSpawnPoints.Add(new MonsterSpawnDefinition("Floater", spawn, new List<Vector2>()));
                        }
                    }

                    chunks.Add(newChunk);
                }

                mapChunks[key] = chunks;
            }
        }

        private static int[,] LoadGeometry(XElement chunk, MapChunk newChunk, string zoneType, string layoutType, int offset)
        {
            // Set up for auto tiling.
            var layoutMap = new int[PhysicsManager.MapWidth, MapChunkHeight];
            var groupIndexMap = Atlas.GetTileGroupIndexMap(zoneType, 1);

            var foreground = chunk.Descendants(layoutType).First().Value;
            var count = 0;
            for (var y = 0; y < MapChunkHeight; y++)
            {
                for (var x = 0; x < PhysicsManager.MapWidth; x++)
                {
                    if (foreground[count] == '\n')
                    {
                        count++;
                    }

                    if (foreground[count] == '1')
                    {
                        layoutMap[x, y] = 1;
                    }

                    count++;
                }
            }

            for (var y = 0; y < MapChunkHeight; y++)
            {
                for (var x = 0; x < PhysicsManager.MapWidth; x++)
                {
                    // If the cell is empty, just continue.
                    if (layoutMap[x, y] == 0 || newChunk.TileMap[x, y] != null)
                    {
                        continue;
                    }

                    // Don't check left or above for valid cells.
                    var width = 1;
                    var height = 1;

                    // Check the cell to the right to see if there's a tile there.
                    if (x + 1 < PhysicsManager.MapWidth)
                    {
                        if (layoutMap[x + 1, y] != 0 && newChunk.TileMap[x + 1, y] == null)
                        {
                            if (MathsHelper.Random() > 60)
                            {
                                width++;
                            }
                        }
                    }

                    // Check the cell below to see if there's a tile there.
                    if (y + 1 < MapChunkHeight)
                    {
                        if (layoutMap[x, y + 1] != 0 && newChunk.TileMap[x, y + 1] == null)
                        {
                            if (MathsHelper.Random() > 60)
                            {
                                height++;
                            }
                        }
                    }

                    // Determine tile types so we can use this for future stuff.
                    if (width == 1)
                    {
                        if (height == 2)
                        {
                            layoutMap[x, y] = 2;
                            layoutMap[x, y + 1] = 3;
                        }
                        else
                        {
                            layoutMap[x, y] = 1;
                        }
                    }
                    else if (width == 2)
                    {
                        if (height == 1)
                        {
                            layoutMap[x, y] = 4;
                            layoutMap[x + 1, y] = 5;
                        }
                        else
                        {
                            layoutMap[x, y] = 6;
                            layoutMap[x + 1, y] = 7;
                            layoutMap[x, y + 1] = 8;
                            layoutMap[x + 1, y + 1] = 9;
                        }
                    }

                    // Get our starting tile position.
                    var size = new Point(width, height);
                    var tileStartingPosition = Atlas.GetTile("Normal", size, groupIndexMap[size][MathsHelper.Random(groupIndexMap[size].Count)]);
                    tileStartingPosition.Y += offset;

                    var horizontalStep = 0;
                    var verticalStep = 0;
                    for (var layoutY = y; layoutY < (y + height); layoutY++)
                    {
                        for (var layoutX = x; layoutX < (x + width); layoutX++)
                        {
                            if (newChunk.TileMap[layoutX, layoutY] == null)
                            {
                                var tileData = new Tile(
                                    "Zones",
                                    TileCollisitionState.Collidable,
                                    new Rectangle(
                                        (tileStartingPosition.X + horizontalStep) * DrawingManager.TileWidth,
                                        (tileStartingPosition.Y + verticalStep) * DrawingManager.TileHeight,
                                        DrawingManager.TileWidth,
                                        DrawingManager.TileHeight));

                                newChunk.TileMap[layoutX, layoutY] = tileData;
                            }

                            horizontalStep++;
                        }

                        horizontalStep = 0;
                        verticalStep++;
                    }
                }
            }

            return layoutMap;
        }

        private static void LoadZoneData()
        {
            var stream = TitleContainer.OpenStream("Content\\Data\\zonedata.xml");
            var doc = XDocument.Load(stream);

            foreach (var zoneData in doc.Descendants("Zones").Descendants("ZoneData"))
            {
                var zoneType = (ZoneType)Enum.Parse(typeof (ZoneType), zoneData.Attribute("zone").Value);
                var minChunkCount = int.Parse(zoneData.Attribute("minimumChunkCount").Value);
                var maxChunkCount = int.Parse(zoneData.Attribute("maximumChunkCount").Value);

                var zoneStructure = zoneData.Descendants("Structure").Single();
                var defaultChunkType = zoneStructure.Attribute("default").Value;

                var structure = new List<Tuple<string, int>>();
                foreach (var chunkType in zoneStructure.Descendants())
                {
                    // Parse each of the nodes here into chunk types and determine the chance they have to appear.
                    var chance = chunkType.Attribute("chance") != null
                        ? int.Parse(chunkType.Attribute("chance").Value)
                        : 100;

                    structure.Add(new Tuple<string, int>(chunkType.Name.ToString(), chance));
                }

                // Finally, add the zone data to the reference list.
                ZoneDataReference.Add(zoneType, new ZoneData(zoneType, minChunkCount, maxChunkCount, defaultChunkType, structure));
            }
        }
    }
}
