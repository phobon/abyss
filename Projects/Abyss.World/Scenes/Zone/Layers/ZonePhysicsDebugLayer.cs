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
            var map = this.Parent.TagCache["Map"].FirstOrDefault() as IMap;
            if (map != null)
            {
                foreach (var b in map.ViewPortTileBoundingBoxes(GameManager.GameViewPort))
                {
                    spriteBatch.DrawBorder(b, Color.Red);
                }
            }

            // Draw debug for Player.
            spriteBatch.DrawBorder(GameManager.Player.Collider.QualifiedBoundingBox, Color.Green);

            // Draw debug for Monsters.
            if (this.Parent.TagCache.ContainsKey("Monster"))
            {
                foreach (var e in this.Parent.TagCache["Monster"])
                {
                    if (e.Flags[EngineFlag.Active])
                    {
                        spriteBatch.DrawBorder(e.Collider.QualifiedBoundingBox, Color.Green);
                    }
                }
            }
            
            // Draw debug for Items.
            if (this.Parent.TagCache.ContainsKey("Item"))
            {
                foreach (var e in this.Parent.TagCache["Item"])
                {
                    if (e.Flags[EngineFlag.Active] && e.Collider != null)
                    {
                        spriteBatch.DrawBorder(e.Collider.QualifiedBoundingBox, Color.Yellow);
                    }
                }
            }

            // Draw debug for Props.
            foreach (var e in this.Parent.TagCache["Prop"])
            {
                if (e.Flags[EngineFlag.Active])
                {
                    spriteBatch.DrawBorder(e.Collider.QualifiedBoundingBox, Color.Blue);
                }
            }

            // Draw debug for Triggers.
            if (this.Parent.TagCache.ContainsKey("Trigger"))
            {
                foreach (var e in this.Parent.TagCache["Trigger"])
                {
                    if (e.Flags[EngineFlag.Active])
                    {
                        spriteBatch.DrawBorder(e.Collider.QualifiedBoundingBox, Color.Plum);
                    }
                }
            }

            // Draw debug for Dimensional tiles.
            if (this.Parent.TagCache.ContainsKey(EntityTags.InterdimensionalPlatform))
            {
                foreach (var e in this.Parent.TagCache[EntityTags.InterdimensionalPlatform])
                {
                    if (e.Flags[EngineFlag.Active])
                    {
                        spriteBatch.DrawBorder(e.Collider.QualifiedBoundingBox, Color.Pink);
                    }
                }
            }
        }
    }
}
