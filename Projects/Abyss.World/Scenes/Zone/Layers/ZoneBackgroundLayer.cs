using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Occasus.Core.Assets;
using Occasus.Core.Drawing;
using Occasus.Core.Input;
using Occasus.Core.Layers;
using Occasus.Core.Scenes;

namespace Abyss.World.Scenes.Zone.Layers
{
    /// <summary>
    /// Background layer for the Zone scene.
    /// </summary>
    public class ZoneBackgroundLayer : Layer
    {
        private const float MidCoefficient = 0.8f;
        private const float TopCoefficient = 0.5f;

        // This is parallax scrolling layer, so we need several different rects to draw from.
        private static readonly Rectangle baseTextureRect = new Rectangle(0, 0, (int)DrawingManager.BaseResolutionWidth, (int)DrawingManager.BaseResolutionHeight);
        private static readonly Rectangle midLayerTextureRect = new Rectangle((int)DrawingManager.BaseResolutionWidth, 0, (int)DrawingManager.BaseResolutionWidth, (int)DrawingManager.BaseResolutionHeight);
        private static readonly Rectangle topLayerTextureRect = new Rectangle((int)DrawingManager.BaseResolutionWidth * 2, 0, (int)DrawingManager.BaseResolutionWidth, (int)DrawingManager.BaseResolutionHeight);

        private float previousDepth;
        private float midOffset;
        private float topOffset;

        /// <summary>
        /// Initializes a new instance of the <see cref="ZoneBackgroundLayer"/> class.
        /// </summary>
        /// <param name="parentScene">The parent scene.</param>
        public ZoneBackgroundLayer(IScene parentScene)
            : base(
            parentScene, 
            "Zone Background Layer", 
            "Background layer for a zone.", 
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
            var depthOffset = Monde.GameManager.Player.Transform.Position.Y - this.previousDepth;
            if (!depthOffset.Equals(0f))
            {
                var d = (int)depthOffset;
                midOffset += d - MidCoefficient;
                topOffset += d - TopCoefficient;
            }

            // Cache the previous depth for the next call.
            this.previousDepth = Monde.GameManager.Player.Transform.Position.Y;

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
            midRect.Y = (int)midOffset;
            spriteBatch.Draw(TextureManager.Textures["NormalBackground"], Vector2.Zero, midRect, c);

            // Draw top layer.
            var topRect = topLayerTextureRect;
            topRect.Y = (int)topOffset;
            spriteBatch.Draw(TextureManager.Textures["NormalBackground"], Vector2.Zero, topRect, c);

            base.Draw(gameTime, spriteBatch);
        }
    }
}
