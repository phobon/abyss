using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Occasus.Core.Assets;
using Occasus.Core.Components.Graphics;
using Occasus.Core.Entities;

namespace Occasus.Core.Drawing.Images
{
    public class Image : GraphicsComponent, IImage
    {
        private Rectangle frameRectangle;

        /// <summary>
        /// Initializes a new instance of the <see cref="Image"/> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        /// <param name="origin">The origin.</param>
        /// <param name="frameSize">Size of the frame.</param>
        /// <param name="imageLayer">The image layers.</param>
        public Image(IEntity parent, string name, string description, Vector2 origin, Vector2 frameSize, IImageLayer imageLayer)
            : base(parent, name, description, origin, frameSize)
        {
            this.Layers = new Dictionary<string, IImageLayer>
            {
                { imageLayer.Id, imageLayer }
            };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Image" /> class.
        /// </summary>
        /// <param name="parent">The entity.</param>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        /// <param name="origin">The origin.</param>
        /// <param name="frameSize">Size of the frame.</param>
        /// <param name="imageLayers">The image layers.</param>
        public Image(IEntity parent, string name, string description, Vector2 origin, Vector2 frameSize, IEnumerable<IImageLayer> imageLayers) 
            : base(parent, name, description, origin, frameSize)
        {
            this.Layers = new Dictionary<string, IImageLayer>(StringComparer.InvariantCultureIgnoreCase);
            foreach (var l in imageLayers)
            {
                this.Layers.Add(l.Id, l);
            }
        }

        /// <summary>
        /// Draws the Engine Component.
        /// </summary>
        /// <param name="gameTime">The game time object.</param>
        /// <param name="spriteBatch">The sprite batch.</param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (this.frameRectangle == Rectangle.Empty)
            {
                this.frameRectangle = new Rectangle(
                                        0,
                                        0,
                                        (int)this.FrameSize.X,
                                        (int)this.FrameSize.Y);
            }

            foreach (var layer in this.Layers.Values)
            {
                spriteBatch.Draw(TextureManager.Textures[layer.TextureId], this.Parent.Transform.Position, frameRectangle, this.Color * this.Opacity, this.Parent.Transform.Rotation, this.Origin, this.Parent.Transform.Scale, this.SpriteEffects, layer.Depth);
            }
        }

        /// <summary>
        /// Gets the layers of this sprite.
        /// </summary>
        public IDictionary<string, IImageLayer> Layers
        {
            get; private set;
        }
    }
}
