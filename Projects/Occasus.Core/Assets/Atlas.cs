using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Occasus.Core.Assets.AtlasDefinitions;
using Occasus.Core.Drawing;
using Occasus.Core.Drawing.Animation;
using Occasus.Core.Drawing.Images;
using Occasus.Core.Drawing.ParticleEffects;
using Occasus.Core.Drawing.Sprites;
using Occasus.Core.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Occasus.Core.Maths;
using Occasus.Core.Tiles;

namespace Occasus.Core.Assets
{
    public static class Atlas
    {
        private const string SpriteAtlasNode = "SpriteAtlas";

        private const string SpritesNode = "Sprites";
        private const string SpriteNode = "Sprite";

        private const string ParticlesNode = "Particles";
        private const string ParticleNode = "Particle";

        private const string DoodadsNode = "Doodads";
        private const string DoodadNode = "Doodad";

        private static readonly IDictionary<string, IDictionary<string, ISpriteData>> sprites = new Dictionary<string, IDictionary<string, ISpriteData>>(StringComparer.InvariantCultureIgnoreCase);
        private static readonly IDictionary<string, IDictionary<string, ISpriteData>> animatedParticles = new Dictionary<string, IDictionary<string, ISpriteData>>(StringComparer.InvariantCultureIgnoreCase);

        private static readonly ITile[,] doodads = new ITile[DrawingManager.SpriteSheetWidth, DrawingManager.SpriteSheetHeight];

        private static readonly IDictionary<string, IDictionary<Point, TileAtlasDefinition>> tileData = new Dictionary<string, IDictionary<Point, TileAtlasDefinition>>(StringComparer.InvariantCultureIgnoreCase);
        private static readonly IDictionary<string, IDictionary<string, DoodadAtlasDefinition>> doodadData = new Dictionary<string, IDictionary<string, DoodadAtlasDefinition>>(StringComparer.InvariantCultureIgnoreCase);

        /// <summary>
        /// Gets the sprite.
        /// </summary>
        /// <param name="atlasId">The atlas id.</param>
        /// <param name="id">The id.</param>
        /// <param name="parent">The parent.</param>
        /// <returns>A sprite generated from the atlas id and sprite id.</returns>
        public static ISprite GetSprite(string atlasId, string id, IEntity parent)
        {
            if (!sprites.ContainsKey(atlasId))
            {
                throw new InvalidOperationException(string.Format("Sprite atlas does not contain key: {0}", atlasId));
            }

            var spriteData = sprites[atlasId][id];
            return spriteData.ToSprite(parent);
        }

        public static ISprite GetSprite(string atlasId, string id, IEntity parent, Vector2 origin, Vector2 frameSize)
        {
            if (!sprites.ContainsKey(atlasId))
            {
                throw new InvalidOperationException(string.Format("Sprite atlas does not contain key: {0}", atlasId));
            }

            var spriteData = sprites[atlasId][id];
            return spriteData.ToSprite(parent, origin, frameSize);
        }

        /// <summary>
        /// Gets the animated particle.
        /// </summary>
        /// <param name="atlasId">The atlas identifier.</param>
        /// <param name="id">The identifier.</param>
        /// <param name="effect">The effect.</param>
        /// <param name="position">The position.</param>
        /// <param name="velocity">The velocity.</param>
        /// <param name="initialRotation">The initial rotation.</param>
        /// <param name="rotationSpeed">The rotation speed.</param>
        /// <param name="scale">The scale.</param>
        /// <param name="recycle">if set to <c>true</c> [recycle].</param>
        /// <param name="fadeIn">if set to <c>true</c> [fade in].</param>
        /// <param name="fadeOut">if set to <c>true</c> [fade out].</param>
        /// <param name="frameDelay">The frame delay.</param>
        /// <param name="trackParent">if set to <c>true</c> [track parent].</param>
        /// <param name="shrink">if set to <c>true</c> [shrink].</param>
        /// <returns>An animated particle.</returns>
        /// <exception cref="System.InvalidOperationException"></exception>
        public static IAnimatedParticle GetAnimatedParticle(
            string atlasId, 
            string id, 
            IParticleEffect effect,
            Vector2 position,
            Vector2 velocity,
            float initialRotation,
            float rotationSpeed,
            Vector2 scale,
            bool recycle,
            bool fadeIn,
            bool fadeOut,
            int frameDelay,
            bool trackParent,
            bool shrink)
        {
            if (!animatedParticles.ContainsKey(atlasId))
            {
                throw new InvalidOperationException(string.Format("Animated particle atlas does not contain key: {0}", atlasId));
            }

            var particleData = animatedParticles[atlasId][id];
            return particleData.ToAnimatedParticle(
                effect, 
                position,
                velocity,
                initialRotation, 
                rotationSpeed,
                scale, 
                recycle, 
                fadeIn,
                fadeOut, 
                frameDelay, 
                trackParent,
                shrink);
        }

        /// <summary>
        /// Gets the doodad.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns>A tile at the proper doodad position.</returns>
        public static ITile GetDoodad(int x, int y)
        {
            return doodads[x, y];
        }

        public static IDictionary<Point, IList<int>> GetTileGroupIndexMap(string zone, int maximumGroups)
        {
            var groupIndexMap = new Dictionary<Point, IList<int>>();
            foreach (var groupSize in tileData[zone])
            {
                groupIndexMap.Add(groupSize.Key, new List<int>());

                var totalGroups = maximumGroups <= groupSize.Value.GroupedPoints.Count
                    ? maximumGroups
                    : groupSize.Value.GroupedPoints.Count;

                for (var i = 0; i < totalGroups; i++)
                {
                    var randomGroup = MathsHelper.Random(groupSize.Value.GroupedPoints.Count);
                    while (groupIndexMap[groupSize.Key].Contains(randomGroup))
                    {
                        randomGroup = MathsHelper.Random(groupSize.Value.GroupedPoints.Count);
                    }

                    groupIndexMap[groupSize.Key].Add(randomGroup);
                }
            }

            return groupIndexMap;
        }

        public static int GetRandomTileGroupIndex(string zone, Point size)
        {
            return MathsHelper.Random(tileData[zone][size].GroupedPoints.Count);
        }

        public static Point GetTile(string zone, Point size)
        {
            var randomGroup = MathsHelper.Random(tileData[zone][size].GroupedPoints.Count);
            var group = tileData[zone][size].GroupedPoints[randomGroup];
            return group[MathsHelper.Random(group.Count)];
        }

        public static Point GetTile(string zone, Point size, int group)
        {
            var tileGroup = tileData[zone][size].GroupedPoints[group];
            return tileGroup[MathsHelper.Random(tileGroup.Count)];
        }

        public static IDictionary<string, IList<Point>> GetDoodadGroupIndexMap(string zone, DoodadPlacement placement, int maximumGroups = 2)
        {
            // Compile a list of groups to add doodads to.
            var groups = new List<DoodadAtlasDefinition>();
            foreach (var g in doodadData[zone].Values)
            {
                if (g.Placements.Contains(placement))
                {
                    groups.Add(g);
                }
            }

            var returnGroups = new Dictionary<string, IList<Point>>();
            var count = 0;
            foreach (var g in groups)
            {
                if (MathsHelper.Random() > 50)
                {
                    returnGroups.Add(g.Id, g.Doodads);
                    count++;
                }

                if (count == maximumGroups)
                {
                    break;
                }
            }

            if (returnGroups.Count == 0)
            {
                var firstGroup = groups.First();
                returnGroups.Add(firstGroup.Id, firstGroup.Doodads);
            }

            return returnGroups;
        }

        public static ITile GetDoodad(string zone, DoodadPlacement placement, int point = 0)
        {
            // Compile a list of groups to add doodads to.
            var groups = new List<DoodadAtlasDefinition>();
            foreach (var g in doodadData[zone].Values)
            {
                if (g.Placements.Contains(placement))
                {
                    switch (placement)
                    {
                        case DoodadPlacement.Tile:
                            if (g.Tiles.Contains(point))
                            {
                                groups.Add(g);
                            }
                            break;
                        default:
                            groups.Add(g);
                            break;
                    }
                }
            }

            // Get a random group.
            var randomGroup = groups[MathsHelper.Random(groups.Count)];

            // Get a random doodad from that group.
            var d = randomGroup.Doodads[MathsHelper.Random(randomGroup.Doodads.Count)];
            return doodads[d.X, d.Y];
        }

        /// <summary>
        /// Loads the atlas data.
        /// </summary>
        /// <param name="uri">The URI.</param>
        public static void LoadSpriteData(string uri)
        {
            var stream = TitleContainer.OpenStream(uri);
            var doc = XDocument.Load(stream);

            var spriteAtlasNode = doc.Descendants(SpriteAtlasNode);

            // Load sprite data if it exists.
            foreach (var s in spriteAtlasNode.Descendants(SpritesNode).Descendants(SpriteNode))
            {
                LoadAtlasData(s, sprites);
            }

            // Load animated particle data if it exists.
            foreach (var s in spriteAtlasNode.Descendants(ParticlesNode).Descendants(ParticleNode))
            {
                LoadAtlasData(s, animatedParticles);
            }

            // Load doodad data if it exists.
            foreach (var s in spriteAtlasNode.Descendants(DoodadsNode).Descendants(DoodadNode))
            {
                LoadDoodadData(s);
            }
        }

        public static void LoadDoodadData(string uri)
        {
            var stream = TitleContainer.OpenStream(uri);
            var doc = XDocument.Load(stream);

            var spriteAtlasNode = doc.Descendants("DoodadAtlas");
            foreach (var z in spriteAtlasNode.Descendants("Zone"))
            {
                var zoneId = z.Attribute("id").Value;
                doodadData.Add(zoneId, new Dictionary<string, DoodadAtlasDefinition>());

                foreach (var d in z.Descendants("DoodadDefinition"))
                {
                    var id = d.Attribute("id").Value;

                    var doodadAtlasDefinition = new DoodadAtlasDefinition(id);

                    // Get tile placements.
                    var placementValues = d.Attribute("placement").Value.Split(',');
                    foreach (var p in placementValues)
                    {
                        doodadAtlasDefinition.Placements.Add((DoodadPlacement)Enum.Parse(typeof(DoodadPlacement), p));
                    }

                    // Get tiles this doodad can be placed in if available.
                    if (doodadAtlasDefinition.Placements.Contains(DoodadPlacement.Tile))
                    {
                        var tileValues = d.Attribute("tiles").Value.Split(',');
                        foreach (var t in tileValues)
                        {
                            doodadAtlasDefinition.Tiles.Add(int.Parse(t));
                        }
                    }

                    // Get all of the doodad points.
                    foreach (var t in d.Descendants("Doodad"))
                    {
                        var x = int.Parse(t.Attribute("x").Value);
                        var y = int.Parse(t.Attribute("y").Value);

                        doodadAtlasDefinition.Doodads.Add(new Point(x, y));
                    }

                    doodadData[zoneId].Add(id, doodadAtlasDefinition);
                }
            }
        }

        public static void LoadTileData(string uri)
        {
            var stream = TitleContainer.OpenStream(uri);
            var doc = XDocument.Load(stream);

            var spriteAtlasNode = doc.Descendants("TileAtlas");

            // Load doodad data if it exists.
            foreach (var s in spriteAtlasNode.Descendants("Zone"))
            {
                var zoneId = s.Attribute("id").Value;
                tileData.Add(zoneId, new Dictionary<Point, TileAtlasDefinition>());

                foreach (var d in s.Descendants("TileDefinition"))
                {
                    var width = int.Parse(d.Attribute("w").Value);
                    var height = int.Parse(d.Attribute("h").Value);
                    var size = new Point(width, height);

                    var tileAtlasDefinition = new TileAtlasDefinition(size);

                    foreach (var g in d.Descendants("TileGroup"))
                    {
                        var groupId = int.Parse(g.Attribute("id").Value);
                        tileAtlasDefinition.GroupedPoints.Add(groupId, new List<Point>());

                        foreach (var t in g.Descendants("Tile"))
                        {
                            var x = int.Parse(t.Attribute("x").Value);
                            var y = int.Parse(t.Attribute("y").Value);

                            tileAtlasDefinition.GroupedPoints[groupId].Add(new Point(x, y));
                        }
                    }
                    
                    tileData[zoneId].Add(new KeyValuePair<Point, TileAtlasDefinition>(size, tileAtlasDefinition));
                }
            }
        }

        private static void LoadDoodadData(XElement s)
        {
            // Required fields.
            var id = s.Attribute("id").Value;
            var isAnimated = bool.Parse(s.Attribute("isAnimated").Value);

            // Sprite location.
            var spriteLocation = s.Descendants("SpriteLocation").First();
            var x = int.Parse(spriteLocation.Attribute("x").Value);
            var qualifiedX = x * DrawingManager.TileWidth;
            var y = int.Parse(spriteLocation.Attribute("y").Value);
            var qualifiedY = y * DrawingManager.TileHeight;
            var rec = new Rectangle(qualifiedX, qualifiedY, DrawingManager.TileWidth, DrawingManager.TileHeight);

            if (isAnimated)
            {
                var speed = int.Parse(s.Attribute("speed").Value);
                var looplag = int.Parse(s.Attribute("looplag").Value);
                var frameIndexes = ParseFrameIndexes(s.Attribute("frames").Value);

                doodads[x, y] = new Tile(
                    "Doodads", 
                    frameIndexes, 
                    speed, 
                    9, 
                    rec);
            }
            else
            {
                doodads[x, y] = new Tile("Doodads", 9, rec);
            }
        }

        private static void LoadAtlasData(XElement s, IDictionary<string, IDictionary<string, ISpriteData>> dictionary)
        {
            // Required fields.
            var id = s.Attribute("id").Value;
            var atlasId = s.Attribute("atlas").Value;

            var useCentreAsOrigin = false;
            if (s.Attribute("centre") != null)
            {
                useCentreAsOrigin = bool.Parse(s.Attribute("centre").Value);
            }

            // Optional flags.
            var tileTexture = false;
            if (s.Attribute("tile") != null)
            {
                tileTexture = bool.Parse(s.Attribute("tile").Value);
            }

            var crop = false;
            if (s.Attribute("crop") != null)
            {
                crop = bool.Parse(s.Attribute("crop").Value);
            }

            // We have an overall size of a sprite. Individual layers can have sizes as well, but we need something to frame everything in.
            var sizeNode = s.Descendants("Size").First();
            var width = int.Parse(sizeNode.Attribute("x").Value);
            var height = int.Parse(sizeNode.Attribute("y").Value);
            var size = new Vector2(width, height);

            // Determine origin based on size.
            var origin = useCentreAsOrigin ? size / 2 : Vector2.Zero;

            // Sprites should always have at least one layer. If they don't then wtf m8.
            var layers = new List<IImageLayer>();
            foreach (var l in s.Descendants("Layers").Descendants("Layer"))
            {
                // Required layer fields.
                var layerTexture = l.Attribute("texture").Value;
                var layerDepth = DrawingManager.EntitySpriteDepths[l.Attribute("depth").Value];

                // Optional fields.
                var layerId = string.Empty;
                if (l.Attribute("id") != null)
                {
                    layerId = l.Attribute("id").Value;
                }

                // Size of sprite.
                var layerSizeNode = l.Descendants("Size").Single();
                var layerWidth = int.Parse(layerSizeNode.Attribute("x").Value);
                var layerHeight = int.Parse(layerSizeNode.Attribute("y").Value);

                var spriteLocationNode = l.Descendants("SpriteLocation").Single();
                var spriteX = int.Parse(spriteLocationNode.Attribute("x").Value.ToString());
                var spriteY = int.Parse(spriteLocationNode.Attribute("y").Value.ToString());
                var spriteLocation = new Point(spriteX, spriteY);

                layers.Add(new ImageLayer(
                    layerId,
                    new Rectangle(spriteLocation.X * layerWidth, spriteLocation.Y * layerHeight, layerWidth, layerHeight),
                    null,
                    layerDepth,
                    layerTexture,
                    isVisible: true));
            }

            // Animation states.
            var animations = new List<IAnimation>();
            foreach (var a in s.Descendants("Animations").Descendants("Animation"))
            {
                // Required flags.
                var animationName = a.Attribute("name").Value;
                var speed = int.Parse(a.Attribute("speed").Value);
                var frames = a.Attribute("frames").Value;

                // Optional flags.
                var isLooping = true;
                if (a.Attribute("loop") != null)
                {
                    isLooping = bool.Parse(a.Attribute("loop").Value);
                }

                var loopLagFrames = 0;
                if (a.Attribute("looplag") != null)
                {
                    loopLagFrames = int.Parse(a.Attribute("looplag").Value);
                }

                // Parse the frames value. This can be a single frame, a range, or a comma-separated list.
                var frameIndexes = ParseFrameIndexes(frames);

                // Animation sprite location.
                var animationSpriteLocationNode = a.Descendants("SpriteLocation").Single();
                var animationOrigin = new Point(int.Parse(animationSpriteLocationNode.Attribute("x").Value), int.Parse(animationSpriteLocationNode.Attribute("y").Value));

                var sourceFrame = layers[0].SourceFrame;
                var frameRect = new Rectangle(
                    animationOrigin.X * sourceFrame.Width,
                    animationOrigin.Y * sourceFrame.Height,
                    sourceFrame.Width,
                    sourceFrame.Height);

                animations.Add(
                    new Animation(
                        animationName,
                        animationOrigin,
                        frameRect,
                        frameIndexes,
                        frameDelay: speed,
                        playInFull: false,
                        isLooping: isLooping)
                    {
                        LoopLagFrames = loopLagFrames
                    });
            }

            // Check to ensure the atlas id is created and/or available.
            if (!dictionary.ContainsKey(atlasId))
            {
                dictionary.Add(atlasId, new Dictionary<string, ISpriteData>(StringComparer.InvariantCultureIgnoreCase));
            }

            // Create the sprite data.
            var spriteData = new SpriteData(
                origin,
                layers,
                animations,
                tileTexture,
                crop);

            // Assign the sprite data to the atlas.
            if (!dictionary[atlasId].ContainsKey(id))
            {
                dictionary[atlasId].Add(id, spriteData);
            }
            else
            {
                dictionary[atlasId][id] = spriteData;
            }
        }

        private static IList<int> ParseFrameIndexes(string frames)
        {
            var frameIndexes = new List<int>();
            if (frames.Contains('-'))
            {
                // Format should be 'x-y' and can be in ascending or descending order.
                var frameParse = frames.Split('-');
                var lowerBounds = int.Parse(frameParse[0]);
                var upperBounds = int.Parse(frameParse[1]);
                if (lowerBounds > upperBounds)
                {
                    for (var i = lowerBounds; i > upperBounds; i--)
                    {
                        frameIndexes.Add(i);
                    }
                }
                else
                {
                    for (var i = lowerBounds; i < upperBounds; i++)
                    {
                        frameIndexes.Add(i);
                    }
                }
            }
            else if (frames.Contains(','))
            {
                // Format should be 'a,b,c,d,e,f,..z'. This gives the set order that an animation should execute.
                // This gives us control to hold onto certain frames for more impact, without having to make art changes.
                var frameParse = frames.Split(',');
                foreach (var f in frameParse)
                {
                    if (!f.Equals(","))
                    {
                        frameIndexes.Add(int.Parse(f));
                    }
                }
            }
            else
            {
                frameIndexes.Add(int.Parse(frames));
            }

            return frameIndexes;
        }
    }
}
