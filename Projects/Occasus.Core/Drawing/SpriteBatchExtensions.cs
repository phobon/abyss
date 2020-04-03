using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Occasus.Core.Drawing
{
    public static class SpriteBatchExtensions
    {
        private static Texture2D pen;
        private static Texture2D Pen
        {
            get
            {
                if (pen == null)
                {
                    pen = new Texture2D(DrawingManager.GraphicsDevice, 1, 1);
                    pen.SetData(new Color[] { Color.White });
                }

                return pen;
            }
        }

        public static void DrawLine(this SpriteBatch s, Texture2D colourTexture, Vector2 start, Vector2 end)
        {
        }

        public static void DrawRectangle(this SpriteBatch s, Rectangle rectangle, Color colour)
        {
            s.Draw(Pen, rectangle, colour);
        }

        public static void DrawBorder(this SpriteBatch s, Rectangle rectangle, Color colour, int thickness = 1)
        {
            // Draw top line.
            s.Draw(Pen, new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, thickness), colour);

            // Draw left line.
            s.Draw(Pen, new Rectangle(rectangle.X, rectangle.Y, thickness, rectangle.Height), colour);

            // Draw right line.
            s.Draw(Pen, new Rectangle(rectangle.X + rectangle.Width, rectangle.Y, thickness, rectangle.Height), colour);

            // Draw bottom line.
            s.Draw(Pen, new Rectangle(rectangle.X, rectangle.Y + rectangle.Height, rectangle.Width + 1, thickness), colour);
        }
    }
}
