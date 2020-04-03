using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Occasus.Core.Drawing;
using Occasus.Core.Entities;
using Occasus.Core.Input;

namespace Occasus.Core.Debugging.Components
{
    public sealed class Dot : DebugEntityComponent
    {
        private Vector2 position;

        public Dot(IEntity parent, Color color) 
            : base(parent, "Dot", "A coloured dot")
        {
            this.Color = color;
        }

        public Vector2 Offset { get; set; }

        public Color Color { get; set; }

        public override void Update(GameTime gameTime, IInputState inputState)
        {
            this.position = new Vector2(this.Parent.Transform.Position.X + this.Offset.X, this.Parent.Transform.Position.Y + this.Offset.Y);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.DrawBorder(new Rectangle(
                    (int)this.position.X,
                    (int)this.position.Y,
                    1,
                    1),
                    this.Color);
        }

        protected override void InitializeTags()
        {
            base.InitializeTags();
            this.Tags.Add("Physics");
        }
    }
}
