using Microsoft.Xna.Framework;
using Occasus.Core.Debugging.Components;
using Occasus.Core.Physics;

namespace Occasus.Core.Entities
{
    public abstract class Trigger : Entity, ITrigger
    {
        private readonly Rectangle boundingBox;
        private readonly Vector2 origin;

        /// <summary>
        /// Initializes a new instance of the <see cref="Trigger" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        /// <param name="initialPosition">The initial position.</param>
        /// <param name="boundingBox">The bounding box.</param>
        /// <param name="origin">The origin.</param>
        protected Trigger(string name, string description, Vector2 initialPosition, Rectangle boundingBox, Vector2 origin)
            : base(name: name, description: description)
        {
            this.boundingBox = boundingBox;
            this.origin = origin;

            this.Transform.Position = initialPosition;
        }

        protected override void InitializeCollider()
        {
            this.Collider = new Collider(this, boundingBox, origin);
        }

        protected override void InitializeTags()
        {
            this.Tags.Add("Trigger");
        }

        protected override void InitializeComponents()
        {
#if DEBUG
            this.AddComponent("BoundingBox", new Border(this, Color.Plum));
#endif
        }
    }
}
