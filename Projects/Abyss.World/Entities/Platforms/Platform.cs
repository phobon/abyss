using Abyss.World.Entities.Monsters;

using Microsoft.Xna.Framework;

using Occasus.Core.Assets;
using Occasus.Core.Components;
using Occasus.Core.Drawing.Sprites;
using Occasus.Core.Entities;
using Occasus.Core.Physics;
using Occasus.Core.States;

namespace Abyss.World.Entities.Platforms
{
    /// <summary>
    /// Class that represents a platform with collision.
    /// </summary>
    public abstract class Platform : Entity, IPlatform
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Platform" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        /// <param name="initialPosition">The initial position.</param>
        /// <param name="size">The size.</param>
        /// <param name="platformType">Type of the platform.</param>
        protected Platform(
            string name,
            string description,
            Vector2 initialPosition,
            Rectangle size,
            PlatformType platformType)
            : base(name, description)
        {
            this.Transform.Position = initialPosition;
            this.PlatformType = platformType;

            this.Tags.Add(EntityTags.Platform);
            this.Tags.Add(this.PlatformType.ToString());

            // Add a sprite to this component.
            var sprite = Atlas.GetSprite(AtlasTags.Gameplay, this.Name, this);
            this.Components.Add(Sprite.Tag, sprite);

            // Has collision.
            this.Collider = new Collider(this, size, Vector2.Zero)
                            {
                                MovementSpeed = new Vector2(PhysicsManager.BaseActorSpeed, PhysicsManager.BaseActorSpeed)
                            };
        }

        /// <summary>
        /// Gets the type of the platform.
        /// </summary>
        public PlatformType PlatformType
        {
            get;
            private set;
        }

        protected override void SetupStates()
        {
            this.States.Add(PlatformStates.Idle, State.GenericState(PlatformStates.Idle, this.GetSprite()));
            base.SetupStates();
        }
    }
}
