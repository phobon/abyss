using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Occasus.Core.Drawing;
using Occasus.Core.Drawing.Lighting;
using Occasus.Core.Entities;
using Occasus.Core.Input;
using Occasus.Core.Maps;

namespace Occasus.Core.Debugging.Components
{
    public sealed class Geometry : DebugEntityComponent
    {
        private readonly IMap map;

        public Geometry(IEntity parent, Color color) 
            : base(parent, "Geometry", "Visual border for geometry")
        {
            this.map = parent as IMap;
            if (map == null)
            {
                throw new ArgumentException("This component can only be added to a map.");
            }

            this.Color = color;
        }

        public Color Color { get; set; }

        public override void Update(GameTime gameTime, IInputState inputState)
        {
            // Don't need to update anything here.
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (var b in map.ViewPortTileBoundingBoxes(Engine<IGameManager<IEntity>>.GameManager.ViewPort))
            {
                spriteBatch.DrawBorder(b, Color.Red);
            }
        }

        protected override void InitializeTags()
        {
            base.InitializeTags();
            this.Tags.Add("Physics");
        }
    }
}
