using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Xml.Linq;
using Atlasser.Model.Drawing;
using Atlasser.Model.Nodes;
using Atlasser.Model.SpriteSheets;
using Newtonsoft.Json;

namespace Atlasser.Model.ViewModes
{
    public class LoadDataPresenter : ViewMode
    {
        private const string SpriteAtlasNode = "SpriteAtlas";

        private const string SpritesNode = "Sprites";
        private const string SpriteNode = "Sprite";

        private const string ParticlesNode = "Particles";
        private const string ParticleNode = "Particle";

        private const string DoodadsNode = "Doodads";
        private const string DoodadNode = "Doodad";

        private string dataLocation;
        private string spriteSheetLocation;
        private string atlasLocation;

        public LoadDataPresenter(Atlasser parent) 
            : base(parent, Atlasser.DataModeName)
        {
            this.IsAccessible = true;
        }

        public string DataLocation
        {
            get
            {
                return this.dataLocation;
            }

            set
            {
                this.dataLocation = value;
                this.OnPropertyChanged("DataLocation");

                this.LoadData();
            }
        }

        public string SpriteSheetLocation
        {
            get
            {
                return this.spriteSheetLocation;
            }

            set
            {
                this.spriteSheetLocation = value;
                this.OnPropertyChanged("SpriteSheetLocation");
            }
        }

        public string AtlasLocation
        {
            get
            {
                return this.atlasLocation;
            }

            set
            {
                this.atlasLocation = value;
                this.OnPropertyChanged("AtlasLocation");
            }
        }
        
        private void LoadData()
        {
            var atlasserDataStream = new StreamReader(this.DataLocation).ReadToEnd();
            var atlasserData = JsonConvert.DeserializeObject<AtlasserData>(atlasserDataStream);

            this.spriteSheetLocation = atlasserData.SpriteSheetLocation;
            this.atlasLocation = atlasserData.AtlasLocation;
            this.OnPropertyChanged("SpriteSheetLocation");
            this.OnPropertyChanged("AtlasLocation");

            // Load sprite sheets if available.
            if (!string.IsNullOrEmpty(this.SpriteSheetLocation))
            {
                this.LoadSpritesheetData();

                foreach (var v in this.Parent.ViewModes.Values)
                {
                    v.IsAccessible = true;
                }
            }

            // Load atlas data if it exists.
            if (!string.IsNullOrEmpty(this.AtlasLocation))
            {
                this.LoadAtlasData();
            }

            // Load atlas keys if they exist.
            if (atlasserData.AtlasKeys.Any())
            {
                foreach (var k in atlasserData.AtlasKeys)
                {
                    SpriteSheetPresenter.AtlasKeys.Add(k);
                }
            }
        }

        private void LoadSpritesheetData()
        {
            // Load sprite sheet data.
            var spritesheetData = new StreamReader(this.SpriteSheetLocation).ReadToEnd();
            var spriteSheets = JsonConvert.DeserializeObject<List<SpriteSheetData>>(spritesheetData);

            var spriteSheetPresenter = ((SpriteSheetPresenter)this.Parent.ViewModes[Atlasser.SpritesheetModeName]);
            foreach (var s in spriteSheets)
            {
                var qualifiedLocation = Path.GetDirectoryName(this.DataLocation) + s.Location;
                spriteSheetPresenter.SpriteSheets.Add(new SpriteSheet(s.Id, qualifiedLocation, new Point(s.Width, s.Height), new Point(s.SpriteWidth, s.SpriteHeight)));
            }

            spriteSheetPresenter.CurrentSpriteSheet = spriteSheetPresenter.SpriteSheets.First();
        }

        private void LoadAtlasData()
        {
            var doc = XDocument.Load(this.AtlasLocation);
            var spriteAtlasNode = doc.Descendants(SpriteAtlasNode);

            // Load sprite data if it exists.
            foreach (var s in spriteAtlasNode.Descendants(SpritesNode).Descendants(SpriteNode))
            {
                LoadAtlasData(NodeType.Sprite, s);
            }

            // Load animated particle data if it exists.
            foreach (var s in spriteAtlasNode.Descendants(ParticlesNode).Descendants(ParticleNode))
            {
                LoadAtlasData(NodeType.Particle, s);
            }

            // Load doodad data if it exists.
            foreach (var s in spriteAtlasNode.Descendants(DoodadsNode).Descendants(DoodadNode))
            {
                LoadDoodadData(s);
            }
        }

        private void LoadAtlasData(NodeType nodeType, XElement s)
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

            // We have an overall size of a sprite. Individual layers can have sizes as well, but we need something to frame everything in.
            var sizeNode = s.Descendants("Size").First();
            var width = int.Parse(sizeNode.Attribute("x").Value);
            var height = int.Parse(sizeNode.Attribute("y").Value);
            var size = new Point(width, height);

            // Sprites should always have at least one layer. If they don't then wtf m8.
            var layers = new List<Layer>();
            var spriteSheetId = string.Empty;
            foreach (var l in s.Descendants("Layers").Descendants("Layer"))
            {
                // Required layer fields.
                var layerTexture = l.Attribute("texture").Value;
                if (string.IsNullOrEmpty(spriteSheetId))
                {
                    spriteSheetId = layerTexture;
                }

                var layerDepth = (LayerDepth)Enum.Parse(typeof(LayerDepth), l.Attribute("depth").Value);

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
                var spriteX = int.Parse(spriteLocationNode.Attribute("x").Value);
                var spriteY = int.Parse(spriteLocationNode.Attribute("y").Value);

                layers.Add(new Layer(layerTexture, new Point(layerWidth, layerHeight))
                {
                    Id = layerId,
                    X = spriteX,
                    Y = spriteY,
                    Depth = layerDepth
                });
            }

            // Animation states.
            var animations = new List<Animation>();
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
                var frameIndexes = this.ParseFrameIndexes(frames);

                // Animation sprite location.
                var animationSpriteLocationNode = a.Descendants("SpriteLocation").Single();
                var animationX = int.Parse(animationSpriteLocationNode.Attribute("x").Value);
                var animationY = int.Parse(animationSpriteLocationNode.Attribute("y").Value);

                var animation = new Animation
                {
                    Name = animationName,
                    IsLooping = isLooping,
                    LoopLagFrames = loopLagFrames,
                    Speed = speed,
                    X = animationX,
                    Y = animationY,
                    Frames = frames
                };

                //foreach (var f in frameIndexes)
                //{
                //    animation.Frames.Add(f);
                //}

                animations.Add(animation);
            }

            var targetSpriteSheet = ((SpriteSheetPresenter)this.Parent.ViewModes[Atlasser.SpritesheetModeName]).SpriteSheets.Single(o => o.Id.Equals(spriteSheetId, StringComparison.InvariantCultureIgnoreCase));
            switch (nodeType)
            {
                case NodeType.Sprite:
                    var sprite = new Sprite(id, targetSpriteSheet.Id, atlasId, size)
                    {
                        Centre = useCentreAsOrigin,
                        Tile = tileTexture
                    };
                    foreach (var l in layers)
                    {
                        sprite.Layers.Add(l);
                    }

                    foreach (var a in animations)
                    {
                        sprite.Animations.Add(a);
                    }

                    var localSprite = sprite.Layers.First();
                    targetSpriteSheet.AddNode(new Point(localSprite.X, localSprite.Y), sprite);
                    break;
                case NodeType.Particle:
                    var particle = new Particle(id, targetSpriteSheet.Id, atlasId, size)
                    {
                        Centre = useCentreAsOrigin,
                        Tile = tileTexture
                    };

                    foreach (var l in layers)
                    {
                        particle.Layers.Add(l);
                    }

                    foreach (var a in animations)
                    {
                        particle.Animations.Add(a);
                    }

                    var localParticle = particle.Layers.First();
                    targetSpriteSheet.AddNode(new Point(localParticle.X, localParticle.Y), particle);
                    break;
            }
        }


        private void LoadDoodadData(XElement s)
        {
            // Required fields.
            var id = s.Attribute("id").Value;
            var isAnimated = bool.Parse(s.Attribute("isAnimated").Value);

            // Sprite location.
            var spriteLocation = s.Descendants("SpriteLocation").First();
            var x = int.Parse(spriteLocation.Attribute("x").Value);
            var y = int.Parse(spriteLocation.Attribute("y").Value);

            // TODO: FIX THIS PLS
            var doodad = new Doodad(id, "Doodads", new Point(16, 16))
            {
                SpriteLocation = new Point(x, y)
            };

            if (isAnimated)
            {
                var speed = int.Parse(s.Attribute("speed").Value);
                var looplag = int.Parse(s.Attribute("looplag").Value);
                var frames = s.Attribute("frames").Value;
                var frameIndexes = ParseFrameIndexes(frames);

                doodad.IsAnimated = true;
                doodad.Speed = speed;
                doodad.LoopLag = looplag;
                doodad.Frames = frames;
            }

            var targetSpriteSheet = ((SpriteSheetPresenter)this.Parent.ViewModes[Atlasser.SpritesheetModeName]).SpriteSheets.Single(o => o.Id.Equals("doodads", StringComparison.InvariantCultureIgnoreCase));
            targetSpriteSheet.AddNode(doodad.SpriteLocation, doodad);
        }

        private IEnumerable<int> ParseFrameIndexes(string frames)
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
