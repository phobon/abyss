using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Occasus.Core.Assets;
using Occasus.Core.Entities;

namespace Occasus.Core.Drawing.Lighting
{
    public class PointLight : LightSource
    {
        private readonly Vector2 origin;
        private float scalePulse;
        private float scaleStep = 0.005f;

        /// <summary>
        /// Initializes a new instance of the <see cref="PointLight" /> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="intensity">The initial intensity.</param>
        /// <param name="initialScale">The initial scale.</param>
        /// <param name="color">The color.</param>
        public PointLight(IEntity parent, float intensity, float initialScale, Color color)
            : base(
            parent, 
            "Point Light",
            "A light cast by a single point in space that has no volume.", 
            LightSourceType.Point)
        {
            this.Intensity = intensity;
            this.Color = color;

            this.Scale = initialScale;
            origin = new Vector2(TextureManager.Textures["PointLight"].Width / 2, TextureManager.Textures["PointLight"].Height / 2);

            this.Flags[EngineFlag.DeferredBegin] = true;
        }

        /// <summary>
        /// Updates the Engine Component.
        /// </summary>
        /// <param name="gameTime">The game time object.</param>
        /// <param name="inputState">The current input state.</param>
        public override void Update(GameTime gameTime, Input.IInputState inputState)
        {
            this.Scale += scaleStep;
            this.scalePulse += scaleStep;

            if (this.scalePulse >= 0.5f)
            {
                scaleStep = -0.005f;
            }
            else if (this.scalePulse <= 0f)
            {
                scaleStep = 0.005f;
            }
        }

        /// <summary>
        /// Draws the Engine Component.
        /// </summary>
        /// <param name="gameTime">The game time object.</param>
        /// <param name="spriteBatch">The sprite batch.</param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var position = this.Parent.Transform.Position + new Vector2(this.Parent.Collider.BoundingBox.Center.X, this.Parent.Collider.BoundingBox.Center.Y);
            spriteBatch.Draw(TextureManager.Textures["PointLight"], position, null, this.Color * this.Intensity, 0f, origin, this.Scale, SpriteEffects.None, 0f);
        }
    }
}
