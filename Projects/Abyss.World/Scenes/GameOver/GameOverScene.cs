using System;
using Abyss.World.Scenes.GameOver.Layers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

using Occasus.Core;
using Occasus.Core.Audio;
using Occasus.Core.Input;
using Occasus.Core.Scenes;

namespace Abyss.World.Scenes.GameOver
{
    public class GameOverScene : Scene
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GameOverScene"/> class.
        /// </summary>
        public GameOverScene()
            : base("Game Over", "Game over scene in Abyss.")
        {
        }

        /// <summary>
        /// Gets the menu index.
        /// </summary>
        public int MenuIndex
        {
            get; private set;
        }

        /// <summary>
        /// Handles the input for this scene.
        /// </summary>
        /// <param name="inputState">State of the input.</param>
        public override void HandleInput(IInputState inputState)
        {
            PlayerIndex playerIndex;
            if (inputState.IsNewKeyPress(Keys.Down, PlayerIndex.One, out playerIndex) || inputState.IsNewButtonPress(Buttons.DPadDown, PlayerIndex.One, out playerIndex) || inputState.IsNewButtonPress(Buttons.LeftThumbstickDown, PlayerIndex.One, out playerIndex))
            {
                // Decrement the menu index; clamping as necessary.
                var newIndex = MenuIndex + 1;
                MenuIndex = Math.Min(1, newIndex);

                AudioManager.Play("menubeep");
            }
            if (inputState.IsNewKeyPress(Keys.Up, PlayerIndex.One, out playerIndex) || inputState.IsNewButtonPress(Buttons.DPadUp, PlayerIndex.One, out playerIndex) || inputState.IsNewButtonPress(Buttons.LeftThumbstickUp, PlayerIndex.One, out playerIndex))
            {
                // Increment the menu index; clamping as necessary.
                var newIndex = MenuIndex - 1;
                MenuIndex = Math.Max(0, newIndex);

                AudioManager.Play("menubeep");
            }
            else if (inputState.IsNewKeyPress(Keys.Space, PlayerIndex.One, out playerIndex) || inputState.IsNewButtonPress(Buttons.Start, PlayerIndex.One, out playerIndex))
            {
                switch (this.MenuIndex)
                {
                    case 0:
                        break;
                    case 1:
                        Monde.ActivateScene("Menu");
                        return;
                }

                Monde.ActivateScene("Zone");
            }

            while (TouchPanel.IsGestureAvailable)
            {
                var gs = TouchPanel.ReadGesture();
                switch (gs.GestureType)
                {
                    case GestureType.Tap:
                        Monde.ActivateScene("Menu");
                        break;
                }
            }
        }

        /// <summary>
        /// Adds layers to this scene.
        /// </summary>
        public override void AddLayers()
        {
            this.Layers.Add("Interface", new GameOverInterfaceLayer(this));
        }
    }
}
