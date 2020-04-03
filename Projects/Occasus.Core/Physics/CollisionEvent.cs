using Microsoft.Xna.Framework;
using Occasus.Core.Entities;
using System;

namespace Occasus.Core.Physics
{
    /// <summary>
    /// Custom event handler for a collision between two <see cref="IEntity"/> objects.
    /// </summary>
    /// <param name="e">The <see cref="CollisionEventArgs"/> instance containing the event data.</param>
    public delegate void CollisionEventHandler(CollisionEventArgs e);

    public class CollisionEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CollisionEventArgs" /> class.
        /// </summary>
        /// <param name="collisionEntity">The collision entity.</param>
        /// <param name="rectangle">The rectangle.</param>
        /// <param name="collisionType">Type of the collision.</param>
        public CollisionEventArgs(IEntity collisionEntity, Rectangle rectangle, string collisionType)
        {
            this.CollisionEntity = collisionEntity;
            this.Rectangle = rectangle;
            this.CollisionType = collisionType;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CollisionEventArgs" /> class.
        /// </summary>
        /// <param name="collisionEntity">The collision entity.</param>
        /// <param name="rectangle">The rectangle.</param>
        /// <param name="collisionType">Type of the collision.</param>
        /// <param name="collisionPosition">The collision position.</param>
        public CollisionEventArgs(IEntity collisionEntity, Rectangle rectangle, string collisionType, CollisionPosition collisionPosition)
        {
            this.CollisionEntity = collisionEntity;
            this.Rectangle = rectangle;
            this.CollisionType = collisionType;
            this.CollisionPosition = collisionPosition;
        }

        /// <summary>
        /// Gets the collision entity.
        /// </summary>
        public IEntity CollisionEntity
        {
            get; private set;
        }

        public Rectangle Rectangle
        {
            get; private set;
        }

        /// <summary>
        /// Gets the type of the collision that occurred.
        /// </summary>
        public string CollisionType
        {
            get; private set;
        }

        /// <summary>
        /// Gets the collision position.
        /// </summary>
        public CollisionPosition CollisionPosition
        {
            get; private set;
        }
    }
}
