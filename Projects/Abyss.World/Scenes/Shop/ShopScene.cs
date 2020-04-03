using Abyss.World.Scenes.Shop.Layers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Occasus.Core;
using Occasus.Core.Drawing;
using Occasus.Core.Input;
using Occasus.Core.Maths;
using Occasus.Core.Scenes;
using System;

namespace Abyss.World.Scenes.Shop
{
    public class ShopScene : Scene
    {
        private const string BackgroundLayerKey = "Background";
        private const string InterfaceLayerKey = "Interface";

        public ShopScene()
            : base("Shop", "Shop scene in Abyss.")
        {
        }

        public override void HandleInput(IInputState inputState)
        {
            PlayerIndex playerIndex;

#if DEBUG
            // Check whether we should quit the game.
            if (inputState.IsNewKeyPress(Keys.Q, PlayerIndex.One, out playerIndex))
            {
                Engine.ActivateScene("GameOver");
            }
            
            if (inputState.IsNewKeyPress(Keys.Enter, PlayerIndex.One, out playerIndex))
            {
                // Shake the camera.
                DrawingManager.Camera.Shake(10f, 2f, Ease.QuadIn);
            }

            // Set ambient light value.
            if (inputState.IsNewKeyPress(Keys.OemPlus, PlayerIndex.One, out playerIndex))
            {
                DrawingManager.AmbientLightValue = Math.Min(DrawingManager.AmbientLightValue += 0.05f, 1f);
            }

            if (inputState.IsNewKeyPress(Keys.OemMinus, PlayerIndex.One, out playerIndex))
            {
                DrawingManager.AmbientLightValue = Math.Max(DrawingManager.AmbientLightValue -= 0.05f, 0f);
            }

            if (inputState.IsNewKeyPress(Keys.F5, PlayerIndex.One, out playerIndex))
            {
                GameManager.LightsOut();
            }

            if (inputState.IsNewKeyPress(Keys.F6, PlayerIndex.One, out playerIndex))
            {
                GameManager.LightsOn();
            }

            if (inputState.IsNewKeyPress(Keys.F7, PlayerIndex.One, out playerIndex))
            {
                Engine.ChangeFramerate(30f, 0);
            }

            if (inputState.IsNewKeyPress(Keys.F8, PlayerIndex.One, out playerIndex))
            {
                Engine.ChangeFramerate(60f, 0);
            }
#endif

            // Handle player input.
            GameManager.Player.HandleInput(inputState);
        }

        public override void AddLayers()
        {
            this.Layers.Add(BackgroundLayerKey, new ShopBackgroundLayer(this));
            this.Layers.Add(InterfaceLayerKey, new ShopInterfaceLayer(this));
        }
    }
}
