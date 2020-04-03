using Microsoft.Xna.Framework;

using Occasus.Core.Assets;
using Occasus.Core.Components;
using Occasus.Core.Drawing.Sprites;
using Occasus.Core.Entities;
using Occasus.Core.Physics;

using Occasus.Core.States;

namespace Abyss.World.Entities.Props
{
    public abstract class Prop : Entity, IProp
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Prop" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        /// <param name="initialPosition">The initial position.</param>
        /// <param name="boundingBox">The bounding box.</param>
        /// <param name="origin">The origin.</param>
        protected Prop(
            string name,
            string description,
            Vector2 initialPosition,
            Rectangle boundingBox,
            Vector2 origin)
            : base(name, description)
        {
            this.Transform.Position = initialPosition;

            // Add tags.
            this.Tags.Add(EntityTags.Prop);

            // Add a sprite to this component.
            var sprite = Atlas.GetSprite(AtlasTags.Gameplay, this.Name, this, origin, new Vector2(boundingBox.Width, boundingBox.Height));
            this.Components.Add(Sprite.Tag, sprite);

            // Has collision.
            this.Collider = new Collider(this, boundingBox, origin)
                                {
                                    MovementSpeed = new Vector2(PhysicsManager.BaseActorSpeed, PhysicsManager.BaseActorSpeed)
                                };
        }

        protected override void SetupStates()
        {
            this.States.Add(PropStates.Idle, State.GenericState(PropStates.Idle, this.GetSprite()));
            base.SetupStates();
        }
    }
}
