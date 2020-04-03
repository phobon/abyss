using Abyss.World.Entities.Player;
using Microsoft.Xna.Framework;
using Occasus.Core;
using Occasus.Core.Assets;
using Occasus.Core.Components;
using Occasus.Core.Components.Logic;
using Occasus.Core.Entities;
using Occasus.Core.Physics;
using Occasus.Core.States;
using System.Collections;
using Occasus.Core.Drawing.Sprites;

namespace Abyss.World.Entities.Props.Concrete
{
    public class RiftSheet : Entity, IProp
    {
        private const string CanStealFlag = "CanSteal";

        private readonly Rectangle boundingBox;

        /// <summary>
        /// Initializes a new instance of the <see cref="RiftSheet"/> class.
        /// </summary>
        /// <param name="initialPosition">The initial position.</param>
        /// <param name="boundingBox">The bounding box.</param>
        public RiftSheet(Vector2 initialPosition, Rectangle boundingBox)
            : base("RiftSheet", "A sheet of pure energy that steals all rift that is near to it.")
        {
            this.boundingBox = boundingBox;
            this.Transform.Position = initialPosition;
        }

        protected override void SetupStates()
        {
            var sprite = this.GetSprite();
            this.States.Add(PropStates.Idle, State.GenericState(PropStates.Idle, sprite));
            base.SetupStates();
        }

        protected override void InitializeTags()
        {
            this.Tags.Add(EntityTags.Prop);
            this.Tags.Add("RiftSheet");
        }

        protected override void InitializeFlags()
        {
            this.Flags.Add(CanStealFlag, true);
            this.Flags[EngineFlag.ForceIncludeInCache] = true;
        }

        protected override void InitializeSprite()
        {
            this.AddComponent(Sprite.Tag, Atlas.GetSprite(AtlasTags.Gameplay, this.Name, this, Vector2.Zero, new Vector2(boundingBox.Width, boundingBox.Height)));
        }

        protected override void InitializeCollider()
        {
            this.Collider = new Collider(this, boundingBox, Vector2.Zero)
            {
                MovementSpeed = new Vector2(PhysicsManager.BaseActorSpeed, PhysicsManager.BaseActorSpeed)
            };
        }

        protected override void ColliderOnCollision(CollisionEventArgs args)
        {
            base.ColliderOnCollision(args);

            if (this.Flags[CanStealFlag])
            {
                var player = args.CollisionEntity as IPlayer;
                if (player != null)
                {
                    // Player's rift is stolen.
                    player.Rift -= 5;
                    this.Flags[CanStealFlag] = false;
                    CoroutineManager.Add(this.StealCooldown());
                }
            }
        }

        private IEnumerator StealCooldown()
        {
            yield return Coroutines.Pause(20);
            this.Flags[CanStealFlag] = true;
        }
    }
}
