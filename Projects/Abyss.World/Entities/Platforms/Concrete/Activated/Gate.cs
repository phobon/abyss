using Microsoft.Xna.Framework;
using Occasus.Core;
using Occasus.Core.Components.Logic;
using Occasus.Core.Drawing.Animation;
using Occasus.Core.Entities;
using Occasus.Core.Physics;
using Occasus.Core.States;
using System.Collections;
using Occasus.Core.Maps;
using Occasus.Core.Maps.Definitions;

namespace Abyss.World.Entities.Platforms.Concrete.Activated
{
    public class Gate : ActivatedPlatform, IConnectedPlatform
    {
        private const int MaximumUnlockedKeys = 3;
        private const float VerticalWarp = 256f;
        
        private int unlockedKeys;
        
        public Gate(IPlatformDefinition platformDefinition)
            : base(
                PlatformKeys.Gate,
                "A platform that is impenetrable until it is fed a set amount of rift energy.",
                platformDefinition.Position,
                platformDefinition.Size)
        {
            this.Tags.Add(EntityTags.Gate);
            this.VerticalWarpPoint = platformDefinition.Position.Y - VerticalWarp;
        }

        /// <summary>
        /// Gets or sets the connected identifier.
        /// </summary>
        /// <value>
        /// The connected identifier.
        /// </value>
        public string ConnectedId
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the unlocked keys.
        /// </summary>
        /// <value>
        /// The unlocked keys.
        /// </value>
        public int UnlockedKeys
        {
            get
            {
                return this.unlockedKeys;
            }

            set
            {
                if (this.unlockedKeys == value)
                {
                    return;
                }

                this.unlockedKeys = value;

                if (this.unlockedKeys >= MaximumUnlockedKeys)
                {
                    this.CurrentState = PlatformStates.Unlocked;
                    this.Flags[EngineFlag.Collidable] = false;
                }
            }
        }

        /// <summary>
        /// Gets the vertical warp point.
        /// </summary>
        public float VerticalWarpPoint
        {
            get; private set;
        }
        
        protected override void SetupStates()
        {
            var sprite = this.GetSprite();
            this.States.Add(PlatformStates.Locked, State.GenericState(PlatformStates.Locked, sprite));
            this.States.Add(PlatformStates.Unlocked, State.GenericState(PlatformStates.Unlocked, sprite));
            base.SetupStates();
        }

        protected override void ColliderOnCollision(CollisionEventArgs args)
        {
            if (!this.IsValidCollision(args))
            {
                return;
            }

            CoroutineManager.Add(this.Warp());
            this.isActivated = true;
        }

        private IEnumerator Warp()
        {
            Monde.GameManager.Player.Collider.Flags[PhysicsFlag.CollidesWithEnvironment] = false;

            // TODO: Add some sort of animation or something that makes things a little less jarring.
            yield return Monde.GameManager.Player.Transform.MoveTo(new Vector2(Monde.GameManager.Player.Transform.Position.X, this.VerticalWarpPoint), TimingHelper.GetFrameCount(0.2f));

            Monde.GameManager.Player.Collider.Flags[PhysicsFlag.CollidesWithEnvironment] = true;

            this.isActivated = false;
        }
    }
}
