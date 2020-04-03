using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Occasus.Core.Drawing;
using Occasus.Core.Layers;
using Occasus.Core.Scenes;

namespace Abyss.World.Scenes.GameOver.Layers
{
    public class GameOverInterfaceLayer : Layer
    {
        private const string Instructions = "Tap, Spacebar or Start to go back to the menu";
        private const string StatisticsHeader = "Statistics";
        private const string ScoreHeader = "Score";

        private const string Retry = "Retry";
        private const string BackToMenu = "Back To Menu";

        private static readonly Vector2 gameOverPosition = new Vector2(50f, 350f);
        private static readonly Vector2 instructionsPosition = new Vector2(50f, 400f);

        private static readonly Vector2 statisticsHeaderPosition = new Vector2(50f, 50f);
        private static readonly List<Vector2> statisticsPositions = new List<Vector2>
        {
            new Vector2(50f, 100f),
            new Vector2(50f, 125f),
            new Vector2(50f, 150f),
            new Vector2(50f, 175f),
            new Vector2(50f, 200f),
            new Vector2(50f, 225f)
        };

        private static readonly Vector2[] cursorPositions = new[] { new Vector2(25f, 500f), new Vector2(25f, 540f), new Vector2(25f, 580f) };
        private static readonly Vector2 retryGamePosition = new Vector2(50f, 500f);
        private static readonly Vector2 backToMenuGamePosition = new Vector2(50f, 540f);

        private static readonly Vector2 scoreHeaderPosition = new Vector2(50f, 250f);

        private readonly GameOverScene gameOverScene;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameOverInterfaceLayer"/> class.
        /// </summary>
        /// <param name="parentScene">The parent scene.</param>
        public GameOverInterfaceLayer(IScene parentScene)
            : base(
            parentScene, 
            "Game Over Interface Layer", 
            "Interface layer for the Game Over scene.", 
            LayerType.Interface,
            2)
        {
            this.gameOverScene = (GameOverScene)parentScene;
        }

        /// <summary>
        /// Draws the Engine Component.
        /// </summary>
        /// <param name="gameTime">The game time object.</param>
        /// <param name="spriteBatch">The sprite batch.</param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);

            spriteBatch.DrawString(DrawingManager.Font, GameManager.StatisticManager.GameOverMessage, gameOverPosition, Color.White);
            spriteBatch.DrawString(DrawingManager.Font, Instructions, instructionsPosition, Color.White);

            // Statistics details.
            spriteBatch.DrawString(DrawingManager.Font, StatisticsHeader, statisticsHeaderPosition, Color.White);

            var index = 0;
            spriteBatch.DrawString(DrawingManager.Font, "Depth Travelled: " + GameManager.CurrentDepth, statisticsPositions[index], Color.White);

            index++;
            foreach (var s in GameManager.StatisticManager.Statistics)
            {
                spriteBatch.DrawString(DrawingManager.Font, s.Key + ": " + s.Value, statisticsPositions[index], Color.White);
                index++;
            }

            spriteBatch.DrawString(DrawingManager.Font, "Relics Collected: " + GameManager.StatisticManager.TotalRelicsCollected, statisticsPositions[index], Color.White);
            index++;
            spriteBatch.DrawString(DrawingManager.Font, "Deaths: " + GameManager.StatisticManager.TotalDeaths, statisticsPositions[index], Color.White);

            // Score.
            spriteBatch.DrawString(DrawingManager.Font, ScoreHeader + ": " + GameManager.StatisticManager.TotalScore, scoreHeaderPosition, Color.White);

            // Retry.
            var textColor = this.gameOverScene.MenuIndex == 0 ? Color.Gold : Color.White;
            spriteBatch.DrawString(DrawingManager.Font, Retry, retryGamePosition, textColor);

            // Back to menu.
            textColor = this.gameOverScene.MenuIndex == 1 ? Color.Gold : Color.White;
            spriteBatch.DrawString(DrawingManager.Font, BackToMenu, backToMenuGamePosition, textColor);
            
            // Cursor.
            spriteBatch.DrawString(DrawingManager.Font, ">", cursorPositions[this.gameOverScene.MenuIndex], Color.Gold);
        }
    }
}
