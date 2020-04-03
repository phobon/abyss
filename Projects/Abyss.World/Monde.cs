using Abyss.World.Drawing.Shaders;
using Abyss.World.Maps;
using Abyss.World.Phases;
using Abyss.World.Scenes.GameOver;
using Abyss.World.Scenes.Menu;
using Abyss.World.Scenes.Zone;
using Microsoft.Xna.Framework;
using Occasus.Core;
using Occasus.Core.Assets;
using Occasus.Core.Audio;
using Occasus.Core.Drawing.Shaders;
using Occasus.Core.Physics;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Abyss.World
{
    public class Monde : Engine<AbyssGameManager>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Monde"/> class.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="framerate">The framerate.</param>
        public Monde(int width, int height, int framerate) 
            : base(width, height, framerate)
        {
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        protected override void Initialize()
        {
            GameManager = new AbyssGameManager();
            GameManager.Initialize();

            AudioManager.MasterVolume = 1f;

            // Initialise asset managers.
            //TextureManager.Initialise();
            //AudioManager.Initialise();
            //Atlas.Initialise();

            // Initialise game-specific managers.
            PhaseManager.Initialize();
            //MapManager.Initialise();

            base.Initialize();
        }

        /// <summary>
        /// Adds Scenes to this engine instance.
        /// </summary>
        protected override void AddScenes()
        {
            this.Scenes.Add("Menu", new MenuScene());
            this.Scenes.Add("Zone", new ZoneScene());
            this.Scenes.Add("GameOver", new GameOverScene());
        }

        protected override void AddShaders()
        {
            // Add supported shader effects.
            ShaderManager.SupportedShaders.Add("Mask", new MaskEffect());
            ShaderManager.SupportedShaders.Add("Desaturate", new DesaturateEffect());
            ShaderManager.SupportedShaders.Add("Sepia", new SepiaEffect());
            ShaderManager.SupportedShaders.Add("FlipVertical", new FlipVerticalEffect());
            ShaderManager.SupportedShaders.Add("FlipHorizontal", new FlipHorizontalEffect());
            ShaderManager.SupportedShaders.Add("Ripple", new RippleEffect());
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();
            
            // Load content for game-specific managers.
            PhaseManager.LoadContent();

            // Load textures.
            TextureManager.LoadTextureData("Content\\Data\\textureatlas.xml");

            // Load sprite and tile atlas.
            Atlas.LoadSpriteData("Content\\Data\\spriteatlas.xml");
            Atlas.LoadTileData("Content\\Data\\tileatlas.xml");
            Atlas.LoadDoodadData("Content\\Data\\doodadatlas.xml");

            // Load sound data.
            AudioManager.LoadSoundData("Content\\Data\\soundatlas.xml");
            
            // Load map data.
            MapFactory.LoadContent();

            // Load shaders.
            foreach (var s in ShaderManager.SupportedShaders.Values)
            {
                s.LoadContent();
            }

            // Load game data.
            this.LoadGameData();
        }

        private void LoadGameData()
        {
            var stream = TitleContainer.OpenStream("Content\\Data\\actordata.xml");
            var doc = XDocument.Load(stream);

            foreach (var actor in doc.Descendants("ActorData").Descendants("Actor"))
            {
                var actorName = actor.Attribute("name").Value;
                
                // Parse movement speeds.
                var movement = actor.Descendants("Movement").Single();
                var movementSpeeds = new Dictionary<string, float>
                {
                    { ActorSpeed.Slow, float.Parse(movement.Attribute(ActorSpeed.Slow).Value) },
                    { ActorSpeed.Normal, float.Parse(movement.Attribute(ActorSpeed.Normal).Value) },
                    { ActorSpeed.Fast, float.Parse(movement.Attribute(ActorSpeed.Fast).Value) }
                };

                PhysicsManager.ActorMovementSpeeds.Add(actorName, movementSpeeds);

                // Parse fall speeds.
                var fall = actor.Descendants("Fall").Single();
                var fallSpeeds = new Dictionary<string, float>
                {
                    { ActorSpeed.Slow, float.Parse(fall.Attribute(ActorSpeed.Slow).Value) },
                    { ActorSpeed.Normal, float.Parse(fall.Attribute(ActorSpeed.Normal).Value) },
                    { ActorSpeed.Fast, float.Parse(fall.Attribute(ActorSpeed.Fast).Value) }
                };

                PhysicsManager.ActorFallSpeeds.Add(actorName, fallSpeeds);
            }

            // Set up any constants we want to use in this game.
            PhysicsManager.GroundDragFactor = PhysicsManager.BaseGroundDragFactor;
        }
    }
}
