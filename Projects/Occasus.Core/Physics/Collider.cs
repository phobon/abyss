using System.Collections;
using Microsoft.Xna.Framework;
using Occasus.Core.Components.Logic;
using Occasus.Core.Entities;
using System.Collections.Generic;

namespace Occasus.Core.Physics
{
    public class Collider : ICollider
    {
        private readonly string changeMovementSpeedKey;

        /// <summary>
        /// Initializes a new instance of the <see cref="Collider" /> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="boundingBox">The bounding box.</param>
        /// <param name="origin">The origin.</param>
        public Collider(IEntity parent, Rectangle boundingBox, Vector2 origin)
        {
            this.Parent = parent;
            this.BoundingBox = boundingBox;
            this.Origin = origin;

            this.Flags = new Dictionary<PhysicsFlag, bool>
                             {
                                 { PhysicsFlag.Grounded, false },
                                 { PhysicsFlag.ReactsToGravity, false },
                                 { PhysicsFlag.ReactsToPhysics, false },
                                 { PhysicsFlag.CollidesWithEnvironment, true }
                             };

            this.changeMovementSpeedKey = this.Parent.Id + "_ChangeMovementSpeed";
        }

        /// <summary>
        /// Occurs when a collision with an entity occurs.
        /// </summary>
        public event CollisionEventHandler Collision;

        /// <summary>
        /// Gets the parent entity.
        /// </summary>
        public IEntity Parent
        {
            get; private set;
        }

        /// <summary>
        /// Gets the origin.
        /// </summary>
        public Vector2 Origin
        {
            get; private set;
        }

        /// <summary>
        /// Gets or sets the velocity.
        /// </summary>
        public Vector2 Velocity
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the external force.
        /// </summary>
        public Vector2 ExternalForce { get; set; }

        /// <summary>
        /// Gets or sets the movement factor.
        /// </summary>
        public Vector2 MovementFactor
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the movement speed.
        /// </summary>
        public Vector2 MovementSpeed { get; set; }

        /// <summary>
        /// Gets the bounding box.
        /// </summary>
        public Rectangle BoundingBox
        {
            get; private set;
        }

        /// <summary>
        /// Gets the qualified bounding box.
        /// </summary>
        public Rectangle QualifiedBoundingBox
        {
            get
            {
                return new Rectangle(((int)this.Parent.Transform.Position.X + this.BoundingBox.X) - (int)this.Origin.X, ((int)this.Parent.Transform.Position.Y + this.BoundingBox.Y) - (int)this.Origin.Y, this.BoundingBox.Width, this.BoundingBox.Height);
            }
        }

        /// <summary>
        /// Gets or sets the previous correct bounding box.
        /// </summary>
        /// <value>
        /// The previous correct bounding box.
        /// </value>
        public Rectangle PreviousCorrectBoundingBox
        {
            get; set;
        }

        public Rectangle PreviousCollisionRect
        {
            get; set;
        }

        /// <summary>
        /// Gets the flags.
        /// </summary>
        public IDictionary<PhysicsFlag, bool> Flags
        {
            get; private set;
        }

        /// <summary>
        /// Gets or sets the air frames.
        /// </summary>
        /// <value>
        /// The air frames.
        /// </value>
        public int UngroundedFrames
        {
            get; set;
        }

        /// <summary>
        /// Raises the <see cref="Collision" /> event.
        /// </summary>
        /// <param name="e">The <see cref="CollisionEventArgs" /> instance containing the event data.</param>
        public void OnCollision(CollisionEventArgs e)
        {
            var handler = this.Collision;
            if (handler != null)
            {
                handler(e);
            }
        }

        /// <summary>
        /// Changes the movement speed for a set amount of time.
        /// </summary>
        /// <param name="newMovementSpeed">The new movement speed.</param>
        /// <param name="time">The time.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void CacheMovementSpeed(Vector2 newMovementSpeed, int time)
        {
            CoroutineManager.Add(changeMovementSpeedKey, this.TemporaryMovementSpeedEffect(newMovementSpeed, time));
        }

        private IEnumerator TemporaryMovementSpeedEffect(Vector2 newMovementSpeed, int time)
        {
            var oldMovementSpeed = this.MovementSpeed;
            this.MovementSpeed = newMovementSpeed;
            yield return Coroutines.Pause(time);
            this.MovementSpeed = oldMovementSpeed;
        }
    }
}
