using System.Collections;

using Abyss.World.Scenes.Menu.Layers.Interface;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Occasus.Core.Components.Logic;
using Occasus.Core.Drawing;
using Occasus.Core.Layers;
using Occasus.Core.Scenes;

namespace Abyss.World.Scenes.Menu.Layers
{
    public class MenuInterfaceLayer : Layer
    {
        private const string Instructions = "Tap, Spacebar or Start to start";

        private const string NormalGame = "Normal";
        private const string DailyChallengeGame = "Daily";
        private const string SpeedrunnerGame = "Speedrun";

        private static readonly Vector2 gameVersionPosition = new Vector2(382f, 420f);
        private static readonly Vector2 instructionsPosition = new Vector2(50f, 670f);

        private static readonly Vector2 normalMenuPosition = new Vector2(50f, 500f);
        private static readonly Vector2 dailyChallengeMenuPosition = new Vector2(50f, 540f);
        private static readonly Vector2 speedRunnerMenuPosition = new Vector2(50f, 580f);

        private static readonly Vector2[] cursorPositions = new[] { new Vector2(25f, 500f), new Vector2(25f, 540f), new Vector2(25f, 580f) };

        private readonly string version;

        private readonly LogoElement logo;

        private readonly MenuScene menuScene;

        private float menuItemOpacity;

        /// <summary>
        /// Initializes a new instance of the <see cref="MenuInterfaceLayer"/> class.
        /// </summary>
        /// <param name="parentScene">The parent scene.</param>
        public MenuInterfaceLayer(IScene parentScene)
            : base(
            parentScene, 
            "Menu Interface Layer", 
            "Interface layer for the Menu scene.", 
            LayerType.Interface,
            2)
        {
            this.version = string.Format("Version: {0}", System.Reflection.Assembly.GetEntryAssembly().GetName().Version);
            this.menuScene = (MenuScene)parentScene;
            this.logo = new LogoElement();
        }

        /// <summary>
        /// Performs any animations, state logic or operations required when this engine component begins.
        /// </summary>
        public override void Begin()
        {
            base.Begin();
            CoroutineManager.Add(this.BeginEffect());
        }

        /// <summary>
        /// Draws the Engine Component.
        /// </summary>
        /// <param name="gameTime">The game time object.</param>
        /// <param name="spriteBatch">The sprite batch.</param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);

#if DEBUG
            spriteBatch.DrawString(DrawingManager.Font, this.version, gameVersionPosition, Color.DarkGray * this.menuItemOpacity);
#endif
            spriteBatch.DrawString(DrawingManager.Font, Instructions, instructionsPosition, Color.Gray * this.menuItemOpacity);

            // Normal game.
            var textColor = this.menuScene.MenuIndex == 0 ? Color.Gold : Color.White;
            spriteBatch.DrawString(DrawingManager.Font, NormalGame, normalMenuPosition, textColor * this.menuItemOpacity);

            // Daily challenge game.
            textColor = this.menuScene.MenuIndex == 1 ? Color.Gold : Color.White;
            spriteBatch.DrawString(DrawingManager.Font, DailyChallengeGame, dailyChallengeMenuPosition, textColor * this.menuItemOpacity);

            // Speedrunner game.
            textColor = this.menuScene.MenuIndex == 2 ? Color.Gold : Color.White;
            spriteBatch.DrawString(DrawingManager.Font, SpeedrunnerGame, speedRunnerMenuPosition, textColor * this.menuItemOpacity);

            // Cursor.
            spriteBatch.DrawString(DrawingManager.Font, ">", cursorPositions[this.menuScene.MenuIndex], Color.Gold * this.menuItemOpacity);
        }

        protected override void InitializeEntityCache()
        {
            // Add the logo element.
            this.AddEntity(this.logo);
            base.InitializeEntityCache();
        }

        private IEnumerator BeginEffect()
        {
            yield return Coroutines.Pause(90);

            var elapsedFrames = 0;
            var totalFrames = 60;
            while (elapsedFrames <= totalFrames)
            {
                this.menuItemOpacity = (float)elapsedFrames / (float)totalFrames;
                yield return null;
                elapsedFrames++;
            }

            this.menuItemOpacity = 1f;
        }
    }
}
