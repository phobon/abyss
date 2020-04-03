using Microsoft.Xna.Framework;

namespace Occasus.Core.Drawing
{
    public static class Vector2Extensions
    {
        public static Vector2 SimpleTransform(this Vector2 v, int x, int y)
        {
            return new Vector2(v.X * x, v.Y * y);
        }
    }
}
