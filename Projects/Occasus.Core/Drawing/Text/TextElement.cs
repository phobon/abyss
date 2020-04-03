using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Occasus.Core.Entities;

namespace Occasus.Core.Drawing.Text
{
    public class TextElement : Entity, ITextElement
    {
        private TextAlignment alignment;
        private SpriteFont font;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="TextElement" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        /// <param name="text">The text.</param>
        /// <param name="textAlignment">The text alignment.</param>
        /// <param name="initialPosition">The initial position.</param>
        public TextElement(string name, string description, string text, TextAlignment textAlignment, Vector2 initialPosition)
            : base(name, description)
        {
            this.Text = text;
            this.alignment = textAlignment;
            this.Transform.Position = initialPosition;

            this.Color = Color.White;
            this.Opacity = 1f;
            this.SpriteEffects = SpriteEffects.None;
             this.font = DrawingManager.Font;
        }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>
        /// The text.
        /// </value>
        public string Text
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the alignment.
        /// </summary>
        /// <value>
        /// The alignment.
        /// </value>
        public TextAlignment Alignment
        {
            get
            {
                return this.alignment;
            }

            set
            {
                this.alignment = value;

                // Re-measure this text element based on the new alignment.
                this.Measure(this.Transform.Position);
            }
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
        /// Gets or sets the font.
        /// </summary>
        /// <value>
        /// The font.
        /// </value>
        public SpriteFont Font
        {
            get
            {
                return this.font;
            }

            set
            {
                if (this.font == value)
                {
                    return;
                }

                this.font = value;

                // Re-measure with the new font.
                this.Measure(this.Transform.Position);
            }
        }

        /// <summary>
        /// Measures the specified position.
        /// </summary>
        /// <param name="position">The position.</param>
        public void Measure(Vector2 position)
        {
            var textSize = this.Font.MeasureString(this.Text);
            switch (this.Alignment)
            {
                case TextAlignment.Center:
                    // If this is aligned to the Center, make sure the position is in the middle.
                    var textCenter = textSize * 0.5f;
                    this.Transform.Position = new Vector2(position.X - textCenter.X, this.Transform.Position.Y);
                    break;
                case TextAlignment.Right:
                    // If this is aligned to the Right, make sure the position is all the way to the right.
                    this.Transform.Position = new Vector2(position.X - textSize.X, this.Transform.Position.Y);
                    break;
            }
        }

        /// <summary>
        /// Draws the Engine Component.
        /// </summary>
        /// <param name="gameTime">The game time object.</param>
        /// <param name="spriteBatch">The sprite batch.</param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);

            spriteBatch.DrawString(this.Font, this.Text, this.Transform.Position, this.Color * this.Opacity, this.Transform.Rotation, Vector2.Zero, this.Transform.Scale, this.SpriteEffects, 0f);
        }

        /// <summary>
        /// Initializes the Engine Component.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            // Take an initial measurement.
            this.Measure(this.Transform.Position);
        }
    }
}
