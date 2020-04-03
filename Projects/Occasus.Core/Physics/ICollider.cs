using Microsoft.Xna.Framework;
using Occasus.Core.Entities;
using System.Collections.Generic;

namespace Occasus.Core.Physics
{
    public interface ICollider
    {
        /// <summary>
        /// Occurs when a collision with an entity occurs.
        /// </summary>
        event CollisionEventHandler Collision;

        /// <summary>
        /// Gets the parent entity.
        /// </summary>
        IEntity Parent { get; }

        /// <summary>
        /// Gets or sets the velocity.
        /// </summary>
        Vector2 Velocity { get; set; }

        Vector2 Origin { get; }

        /// <summary>
        /// Gets or sets the external force.
        /// </summary>
        Vector2 ExternalForce { get; set; }

        /// <summary>
        /// Gets or sets the movement factor.
        /// </summary>
        Vector2 MovementFactor { get; set; }

        /// <summary>
        /// Gets or sets the movement speed.
        /// </summary>
        Vector2 MovementSpeed { get; set; }

        /// <summary>
        /// Gets the bounding box.
        /// </summary>
        Rectangle BoundingBox { get; }

        /// <summary>
        /// Gets the qualified bounding box.
        /// </summary>
        Rectangle QualifiedBoundingBox { get; }

        /// <summary>
        /// Gets or sets the previous correct bounding box.
        /// </summary>
        /// <value>
        /// The previous correct bounding box.
        /// </value>
        Rectangle PreviousCorrectBoundingBox { get; set; }

        /// <summary>
        /// Gets or sets the previous collision rect.
        /// </summary>
        /// <value>
        /// The previous collision rect.
        /// </value>
        Rectangle PreviousCollisionRect { get; set; }

        /// <summary>
        /// Gets the flags.
        /// </summary>
        IDictionary<PhysicsFlag, bool> Flags { get; }

        /// <summary>
        /// Gets or sets the air frames.
        /// </summary>
        /// <value>
        /// The air frames.
        /// </value>
        int UngroundedFrames { get; set; }

        /// <summary>
        /// Raises the <see cref="Collision" /> event.
        /// </summary>
        /// <param name="e">The <see cref="CollisionEventArgs"/> instance containing the event data.</param>
        void OnCollision(CollisionEventArgs e);

        /// <summary>
        /// Changes the movement speed for a set amount of time.
        /// </summary>
        /// <param name="newMovementSpeed">The new movement speed.</param>
        /// <param name="time">The time.</param>
        void CacheMovementSpeed(Vector2 newMovementSpeed, int time);
    }
}
