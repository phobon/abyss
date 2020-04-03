using Microsoft.Xna.Framework;
using System;
using Occasus.Core.Maths;

namespace Occasus.Core.Drawing
{
    /// <summary>
    /// A set of helpful methods for working with rectangles.
    /// </summary>
    public static class RectangleExtensions
    {
        /// <summary>
        /// Calculates the signed depth of intersection between two rectangles.
        /// </summary>
        /// <param name="rectA">Rectangle A.</param>
        /// <param name="rectB">Rectangle B.</param>
        /// <returns>
        /// The amount of overlap between two intersecting rectangles. These depth values can be negative depending on which edges the rectangles intersect. This allows callers to determine the correct direction
        /// to push objects in order to resolve collisions. If the rectangles are not intersecting, Vector2.Zero is returned.
        /// </returns>
        public static Vector2 GetIntersectionDepth(this Rectangle rectA, Rectangle rectB)
        {
            // Calculate half sizes.
            var halfWidthA = rectA.Width / 2.0f;
            var halfHeightA = rectA.Height / 2.0f;
            var halfWidthB = rectB.Width / 2.0f;
            var halfHeightB = rectB.Height / 2.0f;

            // Calculate centers.
            var centerA = new Vector2(rectA.Left + halfWidthA, rectA.Top + halfHeightA);
            var centerB = new Vector2(rectB.Left + halfWidthB, rectB.Top + halfHeightB);

            // Calculate current and minimum-non-intersecting distances between centers.
            var distanceX = centerA.X - centerB.X;
            var distanceY = centerA.Y - centerB.Y;
            var minDistanceX = halfWidthA + halfWidthB;
            var minDistanceY = halfHeightA + halfHeightB;

            // If we are not intersecting at all, return (0, 0).
            if (Math.Abs(distanceX) >= minDistanceX || Math.Abs(distanceY) >= minDistanceY)
            {
                return Vector2.Zero;
            }

            // Calculate and return intersection depths.
            var depthX = distanceX > 0 ? minDistanceX - distanceX : -minDistanceX - distanceX;
            var depthY = distanceY > 0 ? minDistanceY - distanceY : -minDistanceY - distanceY;
            return new Vector2(depthX, depthY);
        }

        /// <summary>
        /// Gets the position of the center of the bottom edge of the rectangle.
        /// </summary>
        /// <param name="rect">The rectangle.</param>
        /// <returns>
        /// A Vector2 representing the center of the bottom edge of the rectangle.
        /// </returns>
        public static Vector2 GetBottomCenter(this Rectangle rect)
        {
            return new Vector2(rect.X + (rect.Width / 2.0f), rect.Bottom);
        }

        public static Vector2 GetRandomPoint(this Rectangle rect)
        {
            return new Vector2(MathsHelper.Random(rect.Left, rect.Right), MathsHelper.Random(rect.Top, rect.Bottom));
        }
    }
}
