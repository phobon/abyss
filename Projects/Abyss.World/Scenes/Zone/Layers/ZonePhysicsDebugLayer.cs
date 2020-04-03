using Abyss.World.Maps;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Occasus.Core;
using Occasus.Core.Drawing;
using Occasus.Core.Layers;
using Occasus.Core.Scenes;
using System.Linq;

namespace Abyss.World.Scenes.Zone.Layers
{
    /// <summary>
    /// Physics debug layer for the Zone scene.
    /// </summary>
    public class ZonePhysicsDebugLayer : Layer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ZonePhysicsDebugLayer"/> class.
        /// </summary>
        /// <param name="parentScene">The parent scene.</param>
        public ZonePhysicsDebugLayer(IScene parentScene)
            : base(
            parentScene, 
            "Zone Physics Debug Layer", 
            "Physics Debug layer for a zone.", 
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
            var map = this.Parent.TagCache["Map"].FirstOrDefault() as IAbyssMap;
            if (map != null)
            {
                foreach (var b in map.ViewPortTileBoundingBoxes(Monde.GameManager.ViewPort))
                {
                    spriteBatch.DrawBorder(b, Color.Red);
                }
            }
        }
    }
}
