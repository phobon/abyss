using Microsoft.Xna.Framework;
using Occasus.Core.Physics;

namespace Occasus.Core.Entities
{
    public abstract class Trigger : Entity, ITrigger
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Trigger" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        /// <param name="initialPosition">The initial position.</param>
        /// <param name="boundingBox">The bounding box.</param>
        /// <param name="origin">The origin.</param>
        protected Trigger(string name, string description, Vector2 initialPosition, Rectangle boundingBox, Vector2 origin)
            : base(name, description)
        {
            this.Tags.Add("Trigger");

            this.Transform.Position = initialPosition;

            // Triggers typically have collision.
            this.Collider = new Collider(this, boundingBox, origin);
        }
    }
}
