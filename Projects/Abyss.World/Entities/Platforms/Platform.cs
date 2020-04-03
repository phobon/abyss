using Microsoft.Xna.Framework;

using Occasus.Core.Assets;
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
        private readonly Rectangle boundingBox;

        /// <summary>
        /// Initializes a new instance of the <see cref="Platform" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        /// <param name="initialPosition">The initial position.</param>
        /// <param name="boundingBox">The bounding box.</param>
        /// <param name="platformType">Type of the platform.</param>
        protected Platform(
            string name,
            string description,
            Vector2 initialPosition,
            Rectangle boundingBox,
            PlatformType platformType)
            : base(name, description)
        {
            this.boundingBox = boundingBox;
            this.Transform.Position = initialPosition;
            this.PlatformType = platformType;
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

        protected override void InitializeTags()
        {
            this.Tags.Add(EntityTags.Platform);
            this.Tags.Add(this.PlatformType.ToString());
        }

        protected override void InitializeSprite()
        {
            this.AddComponent(Sprite.Tag, Atlas.GetSprite(AtlasTags.Gameplay, this.Name, this));
        }

        protected override void InitializeCollider()
        {
            this.Collider = new Collider(this, this.boundingBox, Vector2.Zero)
            {
                MovementSpeed = new Vector2(PhysicsManager.BaseActorSpeed, PhysicsManager.BaseActorSpeed)
            };
        }
    }
}
