using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Occasus.Core.Entities;

namespace Occasus.Core.Drawing.Text
{
    public interface ITextElement : IEntity
    {
        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>
        /// The text.
        /// </value>
        string Text { get; set; }

        /// <summary>
        /// Gets or sets the alignment.
        /// </summary>
        /// <value>
        /// The alignment.
        /// </value>
        TextAlignment Alignment { get; set; }

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
        /// Gets or sets the sprite effects.
        /// </summary>
        /// <value>
        /// The sprite effects.
        /// </value>
        SpriteEffects SpriteEffects { get; set; }

        /// <summary>
        /// Gets or sets the font.
        /// </summary>
        /// <value>
        /// The font.
        /// </value>
        SpriteFont Font { get; set; }

        /// <summary>
        /// Measures the specified position.
        /// </summary>
        /// <param name="position">The position.</param>
        void Measure(Vector2 position);
    }
}
