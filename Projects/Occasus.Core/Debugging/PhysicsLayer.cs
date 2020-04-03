using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Occasus.Core.Drawing;
using Occasus.Core.Layers;
using Occasus.Core.Maps;
using Occasus.Core.Scenes;

namespace Occasus.Core.Debugging
{
    internal class PhysicsLayer : Layer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PhysicsLayer"/> class.
        /// </summary>
        /// <param name="parentScene">The parent scene.</param>
        public PhysicsLayer(IScene parentScene)
            : base(
            parentScene,
            "Physics Debug",
            "Physics Debug layer.",
            LayerType.TransformedToCamera,
            4)
        {
        }

        /// <summary>
        /// Draws the Engine Component.
        /// </summary>
        /// <param name="gameTime">The game time object.</param>
        /// <param name="spriteBatch">The sprite batch.</param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);

            // Draw debug for Map.
            var map = this.Parent.TagCache["Map"].FirstOrDefault() as IMap;
            if (map != null)
            {
                //foreach (var b in map.ViewPortTileBoundingBoxes(GameManager.ViewPort))
                //{
                //    spriteBatch.DrawBorder(b, Color.Red);
                //}
            }
        }
    }
}
