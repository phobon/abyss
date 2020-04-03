using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Occasus.Core.Assets;
using Occasus.Core.Camera;
using Occasus.Core.Components.Logic;
using Occasus.Core.Drawing;
using Occasus.Core.Drawing.Shaders;
using Occasus.Core.Input;
using Occasus.Core.Scenes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Occasus.Core.Physics;

namespace Occasus.Core
{
    public abstract class Engine : Game
    {
        private const string EngineDataUri = "Content\\enginedata.json";

        private const string EngineActivateSceneKey = "Engine_ActivateScene";

        private static IScene currentScene;
        private static IDictionary<string, IScene> scenes;

        // Fixed timestep calculation.
        private static float framerate;
        private static int framerateChangeDelay;
        private static float targetFramerate;

        private static IScene previousScene;

#if DEBUG
        private static IDebugger debugger;
        private const string TitleFormat = "Abyss: fps [{0}] - entities [{1}]";
#endif

        private readonly GraphicsDeviceManager graphics;

        /// <summary>
        /// Initializes a new instance of the <see cref="Engine" /> class.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="initialFramerate">The framerate.</param>
        protected Engine(int width, int height, int initialFramerate)
        {
            scenes = new Dictionary<string, IScene>();

            this.graphics = new GraphicsDeviceManager(this);
            this.Content.RootDirectory = "Content";

            this.graphics.PreferredBackBufferWidth = width;
            this.graphics.PreferredBackBufferHeight = height;

            this.IsFixedTimeStep = true;
            Framerate = initialFramerate;
            TargetElapsedTime = TimeSpan.FromSeconds(1.0 / (double)initialFramerate);
        }

        /// <summary>
        /// Occurs when the engine framerate changes.
        /// </summary>
        public static event FramerateChangedEventHandler FramerateChanged;

        /// <summary>
        /// Gets the time between frames.
        /// </summary>
        public static float DeltaTime
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the framerate.
        /// </summary>
        public static float Framerate
        {
            get
            {
                return framerate;
            }

            private set
            {
                if (framerate.Equals(value))
                {
                    return;
                }

                framerate = value;

                DeltaTime = 1.0f / framerate;
                framerateChangeDelay = 0;

                OnFramerateChanged((int)framerate);
            }
        }

        /// <summary>
        /// Gets a dictionary containing all of the scenes in this Engine.
        /// </summary>
        public IDictionary<string, IScene> Scenes
        {
            get
            {
                return scenes;
            }
        }

        /// <summary>
        /// Gets the current scene.
        /// </summary>
        public IScene CurrentScene
        {
            get
            {
                return currentScene;
            }
        }

        /// <summary>
        /// Gets an object that contains input state data.
        /// </summary>
        public IInputState InputState
        {
            get;
            private set;
        }

#if DEBUG
        public static IDebugger Debugger
        {
            get { return debugger ?? (debugger = new Debugger()); }
        }
#endif

        /// <summary>
        /// Activates the scene.
        /// </summary>
        /// <param name="sceneId">The scene identifier.</param>
        public static void ActivateScene(string sceneId)
        {
            CoroutineManager.Add(EngineActivateSceneKey, ActivateSceneEffect(sceneId));
        }

        /// <summary>
        /// Changes the framerate with an optional delay measured in frames.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="frameDelay">The frame delay.</param>
        public static void ChangeFramerate(float target, int frameDelay)
        {
            if (target.Equals(framerate))
            {
                return;
            }

            // TODO: This is fucking DUMB :(
            targetFramerate = target;
            if (frameDelay == 0)
            {
                framerateChangeDelay = 1;
            }
        }

        public static void LoadEngineData()
        {
            var data = GetEngineData();
            PhysicsManager.LoadPhysicsData(data["Physics"]);
            DrawingManager.LoadDrawingData(data["Drawing"]);
        }

        /// <summary>
        /// Called when the GameComponent needs to be updated. Override this method with component-specific update code.
        /// </summary>
        /// <param name="gameTime">Time elapsed since the last call to Update</param>
        protected override void Update(GameTime gameTime)
        {
            // Update the camera.
            DrawingManager.Camera.Update(gameTime, null);

            // Handle input.
            this.InputState.Update();

            // Handle framerate change delay if it exists.
            if (framerateChangeDelay > 0)
            {
                framerateChangeDelay--;
                if (framerateChangeDelay == 0)
                {
                    Framerate = targetFramerate;
                    TargetElapsedTime = TimeSpan.FromSeconds(1.0 / (double)framerate);
                }
            }

#if DEBUG
            // Update debugger.
            Debugger.Update(gameTime, this.InputState);
#endif

            // Update the CurrentScene if it has an EngineFlag.Active.
            if (this.CurrentScene != null && this.CurrentScene.Flags[EngineFlag.Active])
            {
                this.CurrentScene.Update(gameTime, this.InputState);
            }

            // Update the coroutine manager.
            CoroutineManager.Update();

            // If the cached previous scene is not null, we can end it here outside of the regular Update loop.
            if (previousScene != null)
            {
                previousScene.End();
                previousScene = null;
            }
        }

        /// <summary>
        /// Called when the game component needs to be drawn. Override this method with component-specific drawing code. Reference page contains links to related conceptual articles.
        /// </summary>
        /// <param name="gameTime">Time passed since the last call to Draw.</param>
        protected override void Draw(GameTime gameTime)
        {
            if (this.CurrentScene != null && this.CurrentScene.Flags[EngineFlag.Visible])
            {
                // Draw the game area of the active screen
                this.CurrentScene.Draw(gameTime, null);
            }

            // Draw the camera here. Generally this will do nothing until there's a screen flash or similar.
            DrawingManager.Camera.Draw(gameTime, DrawingManager.SpriteBatch);

#if DEBUG
            Window.Title = string.Format(TitleFormat, Debugger.Framerate, Debugger.EntityCount);
#endif
        }

        /// <summary>
        /// Initializes the component. Override this method to load any non-graphics resources and query for any required services.
        /// </summary>
        protected override void Initialize()
        {
            // Set up the relevant Drawing assets.
            DrawingManager.GraphicsDevice = this.GraphicsDevice;
            DrawingManager.ContentManager = this.Content;

            // Set up input state management.
            this.InputState = new InputState();

            // Initialize the camera.
            //DrawingManager.Camera = new Camera2D(null, DrawingManager.ScreenWidth, DrawingManager.ScreenHeight);
            DrawingManager.Camera = new Camera2D(null, DrawingManager.BaseResolutionWidth, DrawingManager.BaseResolutionHeight);
            DrawingManager.Camera.Initialize();

            // Add and initialize all of the scenes.
            this.AddScenes();

            // Set the initial gameplay screen.
            ActivateScene(this.Scenes.First().Key);

            this.AddShaders();
            foreach (var s in ShaderManager.SupportedShaders.Values)
            {
                s.Initialize();
            }

            base.Initialize();
        }

        /// <summary>
        /// Called when graphics resources need to be loaded. Override this method to load any component-specific graphics resources.
        /// </summary>
        protected override void LoadContent()
        {
            // Load a new spritebatch that will be used to draw contents from the backbuffer.
            DrawingManager.SpriteBatch = new SpriteBatch(DrawingManager.GraphicsDevice);

            // Load the font that will be used to display text.
            DrawingManager.Font = DrawingManager.ContentManager.Load<SpriteFont>("Graphics/Fonts/pixelhud");

            // Load the basic lighting shader and textures.
            DrawingManager.LightingShader = DrawingManager.ContentManager.Load<Effect>("Effects/Lighting");
            TextureManager.Textures.Add("PointLight", DrawingManager.ContentManager.Load<Texture2D>("Graphics/Lighting/pointlight"));

            // Load a new RenderTarget for any shaders that we want to render.
            DrawingManager.DeferredLightingRenderTarget = DrawingManager.CloneRenderTarget(DrawingManager.GraphicsDevice);
            DrawingManager.ColorMapRenderTarget = DrawingManager.CloneRenderTarget(DrawingManager.GraphicsDevice);
            DrawingManager.ShaderRenderTarget = DrawingManager.CloneRenderTarget(DrawingManager.GraphicsDevice);
            DrawingManager.ScaleRenderTarget = DrawingManager.CloneRenderTarget(DrawingManager.GraphicsDevice);

            // Load engine data. Theoretically, we can edit this file on the fly.
            LoadEngineData();
        }

        /// <summary>
        /// Adds Scenes to this engine instance.
        /// </summary>
        protected abstract void AddScenes();

        /// <summary>
        /// Adds Shaders to this engine instance.
        /// </summary>
        protected abstract void AddShaders();

        private static IEnumerator ActivateSceneEffect(string sceneId)
        {
            // Fade the camera out.
            DrawingManager.Camera.FadeOut(30);
            yield return Coroutines.Pause(30);

            // Cache the previous scene so that we can remove it at a later point.
            previousScene = currentScene;
            if (previousScene != null)
            {
                previousScene.Suspend();
            }

            // Clear the coroutine manager.
            CoroutineManager.Clear();

            // Get the new screen, set it in the correct container and activate it.
            currentScene = scenes[sceneId];
            currentScene.Initialize();
            currentScene.Begin();

            // Fade the camera in.
            DrawingManager.Camera.FadeIn(45);
        }

        private static void OnFramerateChanged(int newFramerate)
        {
            var handler = FramerateChanged;
            if (handler != null)
            {
                handler(new FramerateChangedEventArgs(newFramerate));
            }
        }

        private static JToken GetEngineData()
        {
            var stream = TitleContainer.OpenStream(EngineDataUri);
            return JToken.ReadFrom(new JsonTextReader(new StreamReader(stream)));
        }
    }
}
