using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Occasus.Core.Entities;

namespace Occasus.Core.Components.Graphics
{
    public abstract class GraphicsComponent : EntityComponent, IGraphicsComponent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GraphicsComponent" /> class.
        /// </summary>
        /// <param name="parent">The entity.</param>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        /// <param name="origin">The origin.</param>
        /// <param name="frameSize">Size of the frame.</param>
        protected GraphicsComponent(IEntity parent, string name, string description, Vector2 origin, Vector2 frameSize) 
            : base(parent, name, description)
        {
            this.Color = Color.White;
            this.Opacity = 1f;
            this.FrameSize = frameSize;

            this.Origin = origin;
            this.Centre = this.FrameSize / 2;
        }

        /// <summary>
        /// Gets or sets the color.
        /// </summary>
        /// <value>
        /// The color.
        /// </value>
        public Color Color
        {
            get; set;
        }

        /// <summary>
        /// Gets the origin of this sprite.
        /// </summary>
        public Vector2 Origin
        {
            get; private set;
        }

        /// <summary>
        /// Gets the centre of this Graphics component.
        /// </summary>
        public Vector2 Centre
        {
            get; private set;
        }

        /// <summary>
        /// Gets the size of the frame.
        /// </summary>
        public Vector2 FrameSize
        {
            get; private set;
        }

        /// <summary>
        /// Gets or sets the offset for this sprite.
        /// </summary>
        public Vector2 Offset
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the sprite effects.
        /// </summary>
        /// <value>
        /// The sprite effects.
        /// </value>
        public SpriteEffects SpriteEffects
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the opacity.
        /// </summary>
        /// <value>
        /// The opacity.
        /// </value>
        public float Opacity
        {
            get; set;
        }
    }
}
