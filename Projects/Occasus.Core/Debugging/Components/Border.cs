using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Occasus.Core.Drawing;
using Occasus.Core.Drawing.Lighting;
using Occasus.Core.Entities;
using Occasus.Core.Input;

namespace Occasus.Core.Debugging.Components
{
    public sealed class Border : DebugEntityComponent
    {
        public Border(IEntity parent, Color color) 
            : base(parent, "Border", "Visual border")
        {
            this.Color = color;
        }

        public Color Color { get; set; }

        public override void Update(GameTime gameTime, IInputState inputState)
        {
            // Don't need to update anything here.
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.DrawBorder(this.Parent.Collider.QualifiedBoundingBox, this.Color);
        }

        protected override void InitializeTags()
        {
            base.InitializeTags();
            this.Tags.Add("Physics");
        }
    }
}
