using System;

using Microsoft.Xna.Framework;

namespace Occasus.Core.Drawing.Images
{
    public class ImageLayer : IImageLayer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImageLayer" /> class.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="sourceFrame">The source frame.</param>
        /// <param name="destinationFrame">The destination rectangle.</param>
        /// <param name="layerDepth">The layer depth.</param>
        /// <param name="spriteTextureType">Type of the sprite texture.</param>
        public ImageLayer(
            string id,
            Rectangle sourceFrame,
            Rectangle? destinationFrame,
            float layerDepth,
            string spriteTextureType)
            : this(id, sourceFrame, destinationFrame, layerDepth, spriteTextureType, true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageLayer" /> class.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="sourceFrame">The source frame.</param>
        /// <param name="destinationFrame">The destination rectangle.</param>
        /// <param name="layerDepth">The layer depth.</param>
        /// <param name="spriteTextureType">Type of the sprite texture.</param>
        /// <param name="isVisible">if set to <c>true</c> [is visible].</param>
        public ImageLayer(
            string id,
            Rectangle sourceFrame,
            Rectangle? destinationFrame,
            float layerDepth, 
            string spriteTextureType,
            bool isVisible)
        {
            if (string.IsNullOrEmpty(id))
            {
                id = Guid.NewGuid().ToString();
            }

            this.Id = id;
            this.SourceFrame = sourceFrame;
            this.IsVisible = isVisible;
            this.DestinationFrame = destinationFrame;
            this.Depth = layerDepth;
            this.TextureId = spriteTextureType.ToLower();

            this.Color = Color.White;
            this.Opacity = 1f;
        }

        /// <summary>
        /// Gets the id.
        /// </summary>
        public string Id
        {
            get; private set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this particular layer is visible or not.
        /// </summary>
        /// <value>
        ///   <c>True</c> if this layer is visible; otherwise, <c>false</c>.
        /// </value>
        public bool IsVisible
        {
            get; set;
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
        /// Gets or sets the opacity.
        /// </summary>
        /// <value>
        /// The opacity.
        /// </value>
        public float Opacity
        {
            get; set;
        }

        /// <summary>
        /// Gets the source frame.
        /// </summary>
        public Rectangle SourceFrame
        {
            get; private set;
        }

        /// <summary>
        /// Gets or sets the destination frame.
        /// </summary>
        public Rectangle? DestinationFrame
        {
            get; set;
        }

        /// <summary>
        /// Gets the layer depth.
        /// </summary>
        public float Depth
        {
            get; private set;
        }

        /// <summary>
        /// Gets the name of the sprite texture.
        /// </summary>
        public string TextureId
        {
            get; private set;
        }
    }
}
