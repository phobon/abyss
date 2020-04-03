using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Occasus.Core.Components.Graphics
{
    public interface IGraphicsComponent : IEntityComponent
    {
        /// <summary>
        /// Gets or sets the color.
        /// </summary>
        /// <value>
        /// The color.
        /// </value>
        Color Color { get; set; }

        /// <summary>
        /// Gets the origin of this Graphics component.
        /// </summary>
        Vector2 Origin { get; }

        /// <summary>
        /// Gets the centre of this Graphics component.
        /// </summary>
        Vector2 Centre { get; }

        /// <summary>
        /// Gets the size of the frame.
        /// </summary>
        Vector2 FrameSize { get; }

        /// <summary>
        /// Gets or sets the offset for this Graphics component.
        /// </summary>
        Vector2 Offset { get; set; }

        /// <summary>
        /// Gets or sets the sprite effects.
        /// </summary>
        /// <value>
        /// The sprite effects.
        /// </value>
        SpriteEffects SpriteEffects { get; set; }

        /// <summary>
        /// Gets or sets the opacity.
        /// </summary>
        /// <value>
        /// The opacity.
        /// </value>
        float Opacity { get; set; }
    }
}
