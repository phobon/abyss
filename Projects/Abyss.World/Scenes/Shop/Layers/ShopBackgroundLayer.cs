using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Occasus.Core.Assets;
using Occasus.Core.Drawing;
using Occasus.Core.Input;
using Occasus.Core.Layers;
using Occasus.Core.Scenes;

namespace Abyss.World.Scenes.Shop.Layers
{
    public class ShopBackgroundLayer : Layer
    {
        private const int MidCoefficient = 4;
        private const int TopCoefficient = 2;

        // This is parallax scrolling layer, so we need several different rects to draw from.
        private static readonly Rectangle baseTextureRect = new Rectangle(0, 0, (int)DrawingManager.BaseResolutionWidth, (int)DrawingManager.BaseResolutionHeight);
        private static readonly Rectangle midLayerTextureRect = new Rectangle((int)DrawingManager.BaseResolutionWidth, 0, (int)DrawingManager.BaseResolutionWidth, (int)DrawingManager.BaseResolutionHeight);
        private static readonly Rectangle topLayerTextureRect = new Rectangle((int)DrawingManager.BaseResolutionWidth * 2, 0, (int)DrawingManager.BaseResolutionWidth, (int)DrawingManager.BaseResolutionHeight);

        private float previousDepth;
        private int midOffset;
        private int topOffset;

        public ShopBackgroundLayer(IScene parentScene)
            : base(
            parentScene, 
            "Shop Background Layer", 
            "Background layer for a shop.", 
            LayerType.Static,
            0)
        {
        }

        /// <summary>
        /// Updates the Engine Component.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        /// <param name="inputState">The current input state.</param>
        public override void Update(GameTime gameTime, IInputState inputState)
        {
            // Determine offset for each of the layers.
            var depthOffset = GameManager.Player.Transform.Position.Y - this.previousDepth;
            if (!depthOffset.Equals(0f))
            {
                var d = (int)depthOffset;
                midOffset += d / MidCoefficient;
                topOffset += d / TopCoefficient;
            }

            // Cache the previous depth for the next call.
            this.previousDepth = GameManager.Player.Transform.Position.Y;

            base.Update(gameTime, inputState);
        }

        /// <summary>
        /// Draws the Engine Component.
        /// </summary>
        /// <param name="gameTime">The game time object.</param>
        /// <param name="spriteBatch">The sprite batch.</param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var c = DrawingManager.AmbientLightColor * 0.2f;

            // Draw base layer.
            spriteBatch.Draw(TextureManager.Textures["NormalBackground"], Vector2.Zero, baseTextureRect, c);

            // Draw mid layer.
            var midRect = midLayerTextureRect;
            midRect.Y = midOffset;
            spriteBatch.Draw(TextureManager.Textures["NormalBackground"], Vector2.Zero, midRect, c);

            // Draw top layer.
            var topRect = topLayerTextureRect;
            topRect.Y = topOffset;
            spriteBatch.Draw(TextureManager.Textures["NormalBackground"], Vector2.Zero, topRect, c);

            base.Draw(gameTime, spriteBatch);
        }
    }
}
