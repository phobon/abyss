using System.Globalization;
using Microsoft.Xna.Framework;
using Occasus.Core.Drawing;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Occasus.Core.Physics
{
    public static class TileCollisitionState
    {
        /// <summary>
        /// Tile is empty and provides no collision.
        /// </summary> 
        public const int Empty = -1;

        /// <summary>
        /// Tile is collidable.
        /// </summary>
        public const int Collidable = 0;

        /// <summary>
        /// Tile is collidable but is not drawn.
        /// </summary>
        public const int Barrier = 1;
    }

    public static class ActorSpeed
    {
        /// <summary>
        /// Actor is moving at a slow speed.
        /// </summary>
        public const string Slow = "slow";

        /// <summary>
        /// Actor is moving at normal speed.
        /// </summary>
        public const string Normal = "normal";
        
        /// <summary>
        /// Actor is moving at a fast speed.
        /// </summary>
        public const string Fast = "fast";
    }

    public static class PhysicsManager
    {
        private const string MapWidthKey = "MapWidth";
        private const string MapHeightKey = "MapHeight";
        private const string BaseActorSpeedKey = "BaseActorSpeed";
        private const string BaseMovementAccelerationKey = "BaseMovementAcceleration";
        private const string BaseGravityAccelerationKey = "BaseGravityAcceleration";
        private const string BaseGroundDragFactorKey = "BaseGroundDragFactor";
        private const string BaseAirDragFactorKey = "BaseAirDragFactor";

#if DEBUG
        private const string HorizontalVelocityDebugKey = "Horizontal Velocity";
#endif

        private static IDictionary<string, IDictionary<string, float>> actorMovementSpeeds;
        private static IDictionary<string, IDictionary<string, float>> actorFallSpeeds;

        private static int mapWidth;
        private static int mapHeight;

        /// <summary>
        /// Gets or sets the width of the map in tiles.
        /// </summary>
        /// <value>
        /// The width of the map in tiles.
        /// </value>
        public static int MapWidth
        {
            get
            {
                return mapWidth;
            }

            set
            {
                mapWidth = value;
                MapHorizontalCenter = mapWidth / 2;
            }
        }
        
        /// <summary>
        /// Gets the width of the map transformed to universe coordinates.
        /// </summary>
        public static int UniverseMapWidth
        {
            get
            {
                return MapWidth * DrawingManager.TileWidth;
            }
        }

        /// <summary>
        /// Gets the horizontal map center tile.
        /// </summary>
        public static int MapHorizontalCenter 
        { 
            get; private set; 
        }

        /// <summary>
        /// Gets or sets the height of the map in tiles.
        /// </summary>
        /// <value>
        /// The height of the map in tiles.
        /// </value>
        public static int MapHeight
        {
            get
            {
                return mapHeight;
            }

            set
            {
                mapHeight = value;
                MapVerticalCenter = (mapHeight / 2) - 1;
            }
        }

        /// <summary>
        /// Gets the height of the map transformed to universe coordinates.
        /// </summary>
        public static int UniverseMapHeight
        {
            get
            {
                return MapHeight * DrawingManager.TileHeight;
            }
        }

        /// <summary>
        /// Gets the vertical map center tile.
        /// </summary>
        public static int MapVerticalCenter
        {
            get; private set;
        }

        /// <summary>
        /// Gets or sets the actor speed.
        /// </summary>
        /// <value>
        /// The actor speed.
        /// </value>
        public static float ActorSpeed
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the base actor speed.
        /// </summary>
        public static float BaseActorSpeed
        {
            get; private set;
        }

        /// <summary>
        /// Gets or sets the movement acceleration.
        /// </summary>
        /// <value>
        /// The movement acceleration.
        /// </value>
        public static float MovementAcceleration
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the base movement acceleration.
        /// </summary>
        public static float BaseMovementAcceleration
        {
            get; private set;
        }

        /// <summary>
        /// Gets or sets the gravity acceleration.
        /// </summary>
        /// <value>
        /// The gravity acceleration.
        /// </value>
        public static float GravityAcceleration
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the base gravity acceleration.
        /// </summary>
        public static float BaseGravityAcceleration
        {
            get; private set;
        }

        /// <summary>
        /// Gets or sets the ground drag factor.
        /// </summary>
        /// <value>
        /// The ground drag factor.
        /// </value>
        public static float GroundDragFactor
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the base ground drag factor.
        /// </summary>
        public static float BaseGroundDragFactor
        {
            get; private set;
        }

        /// <summary>
        /// Gets or sets the air drag factor.
        /// </summary>
        /// <value>
        /// The air drag factor.
        /// </value>
        public static float AirDragFactor
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the base air drag factor.
        /// </summary>
        public static float BaseAirDragFactor
        {
            get; private set;
        }

        /// <summary>
        /// Gets the actor movement speeds.
        /// </summary>
        public static IDictionary<string, IDictionary<string, float>> ActorMovementSpeeds
        {
            get
            {
                if (actorMovementSpeeds == null)
                {
                    actorMovementSpeeds = new Dictionary<string, IDictionary<string, float>>();
                }

                return actorMovementSpeeds;
            }
        }

        /// <summary>
        /// Gets the actor fall speeds.
        /// </summary>
        public static IDictionary<string, IDictionary<string, float>> ActorFallSpeeds
        {
            get
            {
                if (actorFallSpeeds == null)
                {
                    actorFallSpeeds = new Dictionary<string, IDictionary<string, float>>();
                }

                return actorFallSpeeds;
            }
        }

        /// <summary>
        /// Applies horizontal and vertical impulses to a collider and then tests for environmental collisions.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        /// <param name="o">The o.</param>
        /// <param name="environmentalColliders">The environmental colliders.</param>
        public static void Apply(GameTime gameTime, ICollider o, IEnumerable<Rectangle> environmentalColliders)
        {
            // Determine horizontal velocity.
            var impulse = (o.MovementFactor.X * o.MovementSpeed.X) * BaseMovementAcceleration;

            ApplyHorizontalImpulse(o, impulse);

            // Base velocity is a combination of horizontal movement control and acceleration downward due to gravity.
            if (o.Flags[PhysicsFlag.ReactsToGravity])
            {
                impulse = BaseGravityAcceleration;
            }
            else
            {
                impulse = (o.MovementFactor.Y * o.MovementSpeed.Y) * BaseMovementAcceleration;
            }

            ApplyVerticalImpulse(o, impulse);

            // Apply external forces as required.
            var externalForceX = o.ExternalForce.X;
            var externalForceY = o.ExternalForce.Y;
            o.Velocity = new Vector2(o.Velocity.X + externalForceX, o.Velocity.Y + externalForceY);

            // Apply physics.
            var velocity = o.Velocity;
            var position = o.Parent.Transform.Position;

            var velocityY = velocity.Y * 0.5f;
            var velocityX = velocity.X * 0.5f;
            position.Y += velocityY;
            position.X += velocityX;

#if DEBUG
            Engine.Debugger.Add(HorizontalVelocityDebugKey, velocityX.ToString(CultureInfo.InvariantCulture));
#endif

            // Clamp Position to world boundaries and check for small changes. The crap here with bounding boxes is so that world boundaries are not restricted to the collider's position
            // which is qualified to the top-left corner of a particular tile. I admit, this is kind of janky.
            var horizontalPosition = MathHelper.Clamp((float)Math.Round(position.X), 0f - o.BoundingBox.Left, DrawingManager.BaseResolutionWidth - o.BoundingBox.Right);
            var potentialPosition = Vector2.Zero;
            //#if WINDOWS_PHONE
            //            potentialPosition = new Vector2((int)horizontalPosition, (int)position.Y);
            //#else
            potentialPosition = new Vector2((float)Math.Truncate(horizontalPosition), (float)Math.Truncate(position.Y));
            //#endif

            if (o.Flags[PhysicsFlag.CollidesWithEnvironment])
            {
                HandleCollisionsWithEnvironment(o, potentialPosition, environmentalColliders);
            }
            else
            {
                o.Parent.Transform.Position = potentialPosition;
            }
        }

        /// <summary>
        /// Handle collisions with entities.
        /// </summary>
        /// <param name="o">The collider.</param>
        /// <param name="relevantColliders">The relevant colliders to test against.</param>
        public static void HandleCollisionsWithEntities(ICollider o, IEnumerable<ICollider> relevantColliders)
        {
            foreach (var e in relevantColliders)
            {
                var collision = o.QualifiedBoundingBox.GetIntersectionDepth(e.QualifiedBoundingBox);
                if (collision != Vector2.Zero)
                {
                    // Raise a new collision event for the entity that has been collided with. We do this so we don't have to call this method on every entity; rather just something like the player.
                    // Determine the direction of the collision.
                    var collisionPosition = CollisionPosition.Undefined;
                    var absY = Math.Abs(collision.Y);
                    var absX = Math.Abs(collision.X);

                    // Vertical collision.
                    if (absY < absX)
                    {
                        collisionPosition = collision.Y < 0 ? CollisionPosition.Top : CollisionPosition.Bottom;
                    }
                    else
                    {
                        collisionPosition = collision.X < 0 ? CollisionPosition.Left : CollisionPosition.Right;
                    }

                    e.OnCollision(new CollisionEventArgs(o.Parent, Rectangle.Empty, CollisionTypes.Entity, collisionPosition));
                }
                else
                {
                    var x1 = o.QualifiedBoundingBox.X;
                    var x2 = o.QualifiedBoundingBox.X + o.QualifiedBoundingBox.Width;
                    var y1 = o.QualifiedBoundingBox.Y;
                    var y2 = o.QualifiedBoundingBox.Y + o.QualifiedBoundingBox.Height;

                    var x3 = e.QualifiedBoundingBox.X;
                    var x4 = e.QualifiedBoundingBox.X + e.QualifiedBoundingBox.Width;
                    var y3 = e.QualifiedBoundingBox.Y;
                    var y4 = e.QualifiedBoundingBox.Y + e.QualifiedBoundingBox.Height;

                    // Check for a collision.
                    if ((x4 >= x1 && x3 <= x2) && (y4 >= y1 && y3 <= y2))
                    {
                        // Determine the position of the collision.
                        var collisionPosition = CollisionPosition.Top;
                        if (y2 == y3)
                        {
                            // Top collision.
                        }
                        else if (y3 == y2)
                        {
                            // Bottom collision.
                            collisionPosition = CollisionPosition.Bottom;
                        }
                        else if (x2 == x3)
                        {
                            // Right collision.
                            collisionPosition = CollisionPosition.Right;
                        }
                        else
                        {
                            collisionPosition = CollisionPosition.Left;
                        }

                        e.OnCollision(new CollisionEventArgs(o.Parent, Rectangle.Empty, CollisionTypes.Entity, collisionPosition));
                    }
                }
            }
        }

        /// <summary>
        /// Loads the physics data.
        /// </summary>
        /// <param name="physicsData">The physics data.</param>
        public static void LoadPhysicsData(JToken physicsData)
        {
            MapWidth = (int)physicsData[MapWidthKey];
            MapHeight = (int)physicsData[MapHeightKey];

            BaseActorSpeed = (float)physicsData[BaseActorSpeedKey];
            ActorSpeed = BaseActorSpeed;

            BaseMovementAcceleration = (float)physicsData[BaseMovementAccelerationKey];
            MovementAcceleration = BaseMovementAcceleration;

            BaseGravityAcceleration = (float)physicsData[BaseGravityAccelerationKey];
            GravityAcceleration = BaseGravityAcceleration;

            BaseGroundDragFactor = (float)physicsData[BaseGroundDragFactorKey];
            GroundDragFactor = BaseGroundDragFactor;

            BaseAirDragFactor = (float)physicsData[BaseAirDragFactorKey];
            AirDragFactor = BaseAirDragFactor;
        }

        /// <summary>
        /// Applies the vertical impulse.
        /// </summary>
        /// <param name="o">The world object.</param>
        /// <param name="impulse">The impulse.</param>
        private static void ApplyVerticalImpulse(ICollider o, float impulse)
        {
            // If the object doesn't have any falling speed, then it's ok to return here.
            if (o.MovementSpeed.Y.Equals(0f))
            {
                return;
            }

            var velocity = o.Velocity.Y + impulse;
            velocity = MathHelper.Clamp(velocity, -o.MovementSpeed.Y, o.MovementSpeed.Y);

            // Set a new velocity on the worldObject.
            o.Velocity = new Vector2(o.Velocity.X, velocity);
        }

        /// <summary>
        /// Applies the horizontal impulse.
        /// </summary>
        /// <param name="o">The world object.</param>
        /// <param name="impulse">The impulse.</param>
        private static void ApplyHorizontalImpulse(ICollider o, float impulse)
        {
            var velocity = o.Velocity.X + impulse;

            // Prevent the object from moving faster than its top speed on its own volition.            
            velocity = MathHelper.Clamp(velocity, -o.MovementSpeed.X, o.MovementSpeed.X);

            // Apply pseudo-drag horizontally; only do this when the collider has stopped moving (ie: when the impulse is 0).
            if (o.Flags[PhysicsFlag.Grounded] && impulse.Equals(0f))
            {
                velocity *= BaseGroundDragFactor;
            }
            else
            {
                velocity *= BaseAirDragFactor;
            }

            o.Velocity = new Vector2(velocity, o.Velocity.Y);
        }

        /// <summary>
        /// Detects and resolves all collisions between the character and its neighboring tiles. When a collision is detected, the player is pushed away along one
        /// axis to prevent overlapping. There is some special logic for the Y axis to handle platforms which behave differently depending on direction of movement.
        /// </summary>
        /// <param name="o">The world object.</param>
        /// <param name="potentialPosition">The potential position.</param>
        /// <param name="environmentalColliders">The environmental entities.</param>
        private static void HandleCollisionsWithEnvironment(ICollider o, Vector2 potentialPosition, IEnumerable<Rectangle> environmentalColliders)
        {
            var boundingBox = o.QualifiedBoundingBox;
            var isGrounded = false;

            // For each environmental entity that are collidable.
            foreach (var c in environmentalColliders)
            {
                var depth = Rectangle.Intersect(c, boundingBox);
                if (depth != Rectangle.Empty)
                {
                    // Check along the shallow axis first.
                    if (depth.Height < depth.Width)
                    {
                        // Set flags and ungrounded frames.
                        isGrounded = true;
                        o.Flags[PhysicsFlag.Grounded] = true;
                        o.UngroundedFrames = 0;

                        // Resolve the collision along the Y axis.
                        potentialPosition = new Vector2(potentialPosition.X, c.Top - boundingBox.Height);

                        boundingBox = new Rectangle((int)potentialPosition.X, (int)potentialPosition.Y, boundingBox.Width, boundingBox.Height);

                        // Check the previous collision rect. If it is the same as this one, we shouldn't fire a collision event.
                        if (o.PreviousCollisionRect == Rectangle.Empty || !c.Equals(o.PreviousCollisionRect))
                        {
                            // Raise a new collision event for the platform. This represents a ground collision.
                            o.OnCollision(new CollisionEventArgs(null, c, CollisionTypes.Environment));
                            o.PreviousCollisionRect = c;
                        }
                    }
                    else
                    {
                        // Determine direction of the collision.
                        if (boundingBox.Left < c.Right && boundingBox.Right > c.Right)
                        {
                            // Collision with the left edge.
                            potentialPosition = new Vector2(potentialPosition.X + depth.Width, potentialPosition.Y);
                            o.OnCollision(new CollisionEventArgs(null, c, CollisionTypes.Environment, CollisionPosition.Left));
                        }
                        else
                        {
                            // Collision with the right edge.
                            potentialPosition = new Vector2(potentialPosition.X - depth.Width, potentialPosition.Y);
                            o.OnCollision(new CollisionEventArgs(null, c, CollisionTypes.Environment, CollisionPosition.Right));
                        }


                        // Perform further collisions with the new bounds.
                        boundingBox = new Rectangle((int)potentialPosition.X, (int)potentialPosition.Y, boundingBox.Width, boundingBox.Height);
                    }
                }
                else if (!o.PreviousCollisionRect.Equals(Rectangle.Empty))
                {
                    // Determine whether there is a tile underneath the entity.
                    var testRect = new Rectangle(boundingBox.X, boundingBox.Y, boundingBox.Width, boundingBox.Height + 2);
                    depth = Rectangle.Intersect(c, testRect);
                    if (depth != Rectangle.Empty)
                    {
                        isGrounded = true;
                        potentialPosition = new Vector2(potentialPosition.X, o.PreviousCorrectBoundingBox.Y);
                        boundingBox = new Rectangle((int)potentialPosition.X, (int)potentialPosition.Y, boundingBox.Width, boundingBox.Height);
                    }
                }
            }

            // Check whether the player has made a jump. If so, reset his vertical velocity.
            if (!isGrounded && o.Flags[PhysicsFlag.Grounded])
            {
                o.Velocity = new Vector2(o.Velocity.X, 0f);
                o.PreviousCollisionRect = Rectangle.Empty;
                o.Flags[PhysicsFlag.Grounded] = false;
                o.Parent.Transform.Position = new Vector2(potentialPosition.X, o.Parent.Transform.Position.Y);
            }
            else
            {
                o.Parent.Transform.Position = potentialPosition;
            }

            // Save the new bounds bottom.
            o.PreviousCorrectBoundingBox = boundingBox;
            o.UngroundedFrames++;
        }
    }
}
