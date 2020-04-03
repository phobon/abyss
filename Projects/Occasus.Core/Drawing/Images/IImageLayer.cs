using Microsoft.Xna.Framework;

namespace Occasus.Core.Drawing.Images
{
    public interface IImageLayer
    {
        /// <summary>
        /// Gets the id.
        /// </summary>
        string Id { get; }
        
        /// <summary>
        /// Gets or sets a value indicating whether this particular layer is visible or not.
        /// </summary>
        /// <value><c>True</c> if this layer is visible; otherwise, <c>false</c>.
        /// </value>
        bool IsVisible { get; set; }

        /// <summary>
        /// Gets or sets the color.
        /// </summary>
        /// <value>
        /// The color.
        /// </value>
        Color Color { get; set; }

        /// <summary>
        /// Gets or sets the opacity.
        /// </summary>
        /// <value>
        /// The opacity.
        /// </value>
        float Opacity { get; set; }

        /// <summary>
        /// Gets the source frame.
        /// </summary>
        Rectangle SourceFrame { get; }

        /// <summary>
        /// Gets or sets the destination frame.
        /// </summary>
        Rectangle? DestinationFrame { get; set; }

        /// <summary>
        /// Gets the depth.
        /// </summary>
        float Depth { get; }

        /// <summary>
        /// Gets the texture id.
        /// </summary>
        string TextureId { get; }
    }
}
