using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Occasus.Core.Input
{
    public static class InputManager
    {
        private const int HorizontalTouchSize = 13;
        private const int VerticalTouchSize = 9;
        private const int MaxHorizontalTouchSize = 12;
        private const int MaxVerticalTouchSize = 8;

        private const int MaxHorizontalTouchPosition = 752;
        private const int MaxVerticalTouchPosition = 464;
        private const int HorizontalConstraintSize = 48;
        private const int VerticalConstraintSize = 16;

        private const int TouchTargetSize = 64;

//#if WINDOWS_PHONE
//        private const int HorizontalScreenOffset = 6;
//        private const int VerticalScreenOffset = 4;
//#else
        private const int HorizontalScreenOffset = 10;
        private const int VerticalScreenOffset = 6;
//#endif

        private static Rectangle[,] touchTargets;

        /// <summary>
        /// Gets an array of valid touch targets. This is used primarily to handle 
        /// </summary>
        public static Rectangle[,] TouchTargets
        {
            get
            {
                if (touchTargets == null)
                {
                    touchTargets = new Rectangle[HorizontalTouchSize, VerticalTouchSize];

                    // Add side squares.
                    for (var y = 1; y < MaxVerticalTouchSize; y++)
                    {
                        var yPos = y == 1 ? VerticalConstraintSize : (y * TouchTargetSize) - 48;
                        touchTargets[0, y] = new Rectangle(0, yPos, HorizontalConstraintSize, TouchTargetSize);
                        touchTargets[MaxHorizontalTouchSize, y] = new Rectangle(MaxHorizontalTouchPosition, yPos, HorizontalConstraintSize, TouchTargetSize);
                    }

                    // Add top squares
                    for (var x = 1; x < MaxHorizontalTouchSize; x++)
                    {
                        var xPos = x == 1 ? HorizontalConstraintSize : (x * TouchTargetSize) - 16;
                        touchTargets[x, 0] = new Rectangle(xPos, 0, TouchTargetSize, VerticalConstraintSize);
                        touchTargets[x, MaxVerticalTouchSize] = new Rectangle(xPos, MaxVerticalTouchPosition, TouchTargetSize, VerticalConstraintSize);
                    }

                    // Add corners.
                    touchTargets[0, 0] = new Rectangle(0, 0, HorizontalConstraintSize, VerticalConstraintSize);
                    touchTargets[MaxHorizontalTouchSize, 0] = new Rectangle(MaxHorizontalTouchPosition, 0, HorizontalConstraintSize, VerticalConstraintSize);
                    touchTargets[0, MaxVerticalTouchSize] = new Rectangle(0, MaxVerticalTouchPosition, HorizontalConstraintSize, VerticalConstraintSize);
                    touchTargets[MaxHorizontalTouchSize, MaxVerticalTouchSize] = new Rectangle(MaxHorizontalTouchPosition, MaxVerticalTouchPosition, HorizontalConstraintSize, VerticalConstraintSize);

                    // Fill in everything else.
                    for (var x = 1; x < MaxHorizontalTouchSize; x++)
                    {
                        for (var y = 1; y < MaxVerticalTouchSize; y++)
                        {
                            var xPos = x == 1 ? HorizontalConstraintSize : (x * TouchTargetSize) - 16;
                            var yPos = y == 1 ? VerticalConstraintSize : (y * TouchTargetSize) - 48;
                            touchTargets[x, y] = new Rectangle(xPos, yPos, TouchTargetSize, TouchTargetSize);
                        }
                    }
                }

                return touchTargets;
            }
        }

        /// <summary>
        /// Gets the state of the keyboard.
        /// </summary>
        public static KeyboardState KeyboardState
        {
            get
            {
                return Keyboard.GetState();
            }
        }

        /// <summary>
        /// Gets the state of the game pad.
        /// </summary>
        //public static GamePadState GamePadState
        //{
        //    get
        //    {
        //        return GamePad.GetState(PlayerIndex.One);
        //    }
        //}

        /// <summary>
        /// Gets the movement.
        /// </summary>
        //public static Vector2 Movement
        //{
        //    get
        //    {
        //        return GamePadState.ThumbSticks.Left;
        //    }
        //}

        /// <summary>
        /// Maps the touch target.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="touchTarget">The touch target.</param>
        /// <returns>A Point matching the screen mapped target.</returns>
        public static Point MapTouchTarget(Vector2 position, out Rectangle touchTarget)
        {
            var tileX = (int)position.X / TouchTargetSize;
            var tileY = (int)position.Y / TouchTargetSize;
            var tilePosition = new Rectangle((int)position.X, (int)position.Y, 0, 0);

            // If we don't have the correct position, it'll be because we're offset by some amount (16 pixels in the horizontal direction)
            var targetHit = false;
            touchTarget = TouchTargets[tileX, tileY];
            while (!targetHit)
            {
                if (!touchTarget.Contains(tilePosition))
                {
                    // Check which constraint is out and adjust accordingly.
                    if (tilePosition.X < touchTarget.X)
                    {
                        tileX--;
                    }
                    else if (tilePosition.X > touchTarget.X + touchTarget.Width)
                    {
                        tileX++;
                    }

                    if (tilePosition.Y < touchTarget.Y)
                    {
                        tileY--;
                    }
                    else if (tilePosition.Y > touchTarget.Y)
                    {
                        tileY++;
                    }

                    touchTarget = TouchTargets[tileX, tileY];
                }
                else
                {
                    targetHit = true;
                }
            }

            if (tileX < 0)
            {
                tileX = 0;
            }

            if (tileY < 0)
            {
                tileY = 0;
            }

            return new Point(tileX, tileY);
        }

        public static Point GetMappedScreenPosition(Point worldPosition, Point reference)
        {
            // Reference point is always at the centre of the screen (6,4 on a windows phone), so find the difference here to determine the screen position.
            var x = worldPosition.X - reference.X;
            var y = worldPosition.Y - reference.Y;

            var horizontalPosition = HorizontalScreenOffset + x >= MaxHorizontalTouchSize ? MaxHorizontalTouchSize : HorizontalScreenOffset + x;
            var verticalPosition = VerticalScreenOffset + y >= MaxVerticalTouchSize ? MaxVerticalTouchSize : VerticalScreenOffset + y;

            return new Point(horizontalPosition, verticalPosition);
        }

        public static Point GetMappedWorldPosition(Point worldPosition, Point screenPosition)
        {
            var baseX = worldPosition.X - HorizontalScreenOffset;
            var baseY = worldPosition.Y - VerticalScreenOffset;
            var mappedTilePositionX = baseX + screenPosition.X;
            var mappedTilePositionY = baseY + screenPosition.Y;

            if (mappedTilePositionX < 0)
            {
                mappedTilePositionX = 0;
            }

            if (mappedTilePositionY < 0)
            {
                mappedTilePositionY = 0;
            }

            return new Point(mappedTilePositionX, mappedTilePositionY);
        }
    }
}
