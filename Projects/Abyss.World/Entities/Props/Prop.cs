using Microsoft.Xna.Framework;

using Occasus.Core.Assets;
using Occasus.Core.Debugging.Components;
using Occasus.Core.Drawing.Sprites;
using Occasus.Core.Entities;
using Occasus.Core.Physics;

using Occasus.Core.States;

namespace Abyss.World.Entities.Props
{
    public abstract class Prop : Entity, IProp
    {
        private readonly Rectangle boundingBox;
        private readonly Vector2 origin;

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
            this.boundingBox = boundingBox;
            this.origin = origin;
            this.Transform.Position = initialPosition;
        }

        protected override void SetupStates()
        {
            this.States.Add(PropStates.Idle, State.GenericState(PropStates.Idle, this.GetSprite()));
            base.SetupStates();
        }

        protected override void InitializeTags()
        {
            this.Tags.Add(EntityTags.Prop);
        }

        protected override void InitializeSprite()
        {
            var sprite = Atlas.GetSprite(AtlasTags.Gameplay, this.Name, this, this.origin, new Vector2(this.boundingBox.Width, this.boundingBox.Height));
            this.AddComponent(Sprite.Tag, sprite);
        }

        protected override void InitializeCollider()
        {
            this.Collider = new Collider(this, this.boundingBox, this.origin)
            {
                MovementSpeed = new Vector2(PhysicsManager.BaseActorSpeed, PhysicsManager.BaseActorSpeed)
            };
        }

        protected override void InitializeComponents()
        {
#if DEBUG
            this.AddComponent("BoundingBox", new Border(this, Color.Teal));
#endif
        }
    }
}
