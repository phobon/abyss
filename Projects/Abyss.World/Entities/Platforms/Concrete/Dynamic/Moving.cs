using Microsoft.Xna.Framework;
using Occasus.Core.Components.Logic;
using Occasus.Core.Drawing.Animation;
using Occasus.Core.Entities;
using Occasus.Core.Input;
using Occasus.Core.Maths;
using Occasus.Core.Physics;
using System.Collections;
using System.Linq;

namespace Abyss.World.Entities.Platforms.Concrete.Dynamic
{
    public class Moving : DynamicPlatform
    {
        private const float HoverTime = 3f;

        private IEntity cachedCollisionEntity;
        private Vector2 cachedPosition;

        /// <summary>
        /// Initializes a new instance of the <see cref="Moving" /> class.
        /// </summary>
        /// <param name="platformDefinition">The platform definition.</param>
        public Moving(IPlatformDefinition platformDefinition)
            : base(
                PlatformKeys.Moving,
                "A platform that floats in the air and moves with otherworldly motion.",
                platformDefinition.Position,
                platformDefinition.Size,
                platformDefinition.Path)
        {
            this.Collider.MovementSpeed = new Vector2(75f, PhysicsManager.BaseActorSpeed);
        }

        /// <summary>
        /// Performs any animations, state logic or operations required when this engine component begins.
        /// </summary>
        public override void Begin()
        {
            base.Begin();
            
            CoroutineManager.Add(this.Id + "_MoveAlongPath", this.MoveAlongPath());
        }

        /// <summary>
        /// Updates the Engine Component.
        /// </summary>
        /// <param name="gameTime">The game time object.</param>
        /// <param name="inputState">The current input state.</param>
        public override void Update(GameTime gameTime, IInputState inputState)
        {
            base.Update(gameTime, inputState);

            // If something is standing on this platform and not moving, then it is carried by platform.
            if (this.cachedCollisionEntity != null && this.cachedCollisionEntity.Collider.MovementFactor.Equals(0f))
            {
                var offset = this.cachedPosition - this.Transform.Position;
                this.cachedCollisionEntity.Transform.Position = new Vector2(this.cachedCollisionEntity.Transform.Position.X - offset.X, this.cachedCollisionEntity.Transform.Position.Y - offset.Y);
            }

            // Reset the collision entity.
            this.cachedCollisionEntity = null;
        }

        protected override void ColliderOnCollision(CollisionEventArgs args)
        {
            // If this platform isn't landed on, then don't crumble it away.
            if (args.CollisionPosition != CollisionPosition.Top || !args.CollisionEntity.Collider.Flags[PhysicsFlag.Grounded])
            {
                return;
            }

            base.ColliderOnCollision(args);

            this.cachedCollisionEntity = args.CollisionEntity;
            this.cachedPosition = this.Transform.Position;
        }

        private IEnumerator MoveAlongPath()
        {
            var index = 0;
            var pathCount = this.Path.Count();

            while (true)
            {
                var nextPathTarget = this.Path.ElementAt(index);

                // Calculate distance between the two points to determine speed.
                var distance = Vector2.Distance(this.Transform.Position, nextPathTarget);
                var t = distance / this.Collider.MovementSpeed.X;

                // Accelerate towards the end of the path.
                yield return this.Transform.MoveTo(nextPathTarget, TimingHelper.GetFrameCount(t), EaseType.QuadInOut);

                // Pause until the next move.
                yield return Coroutines.Pause(TimingHelper.GetFrameCount(2f));

                index++;
                if (index == pathCount)
                {
                    index = 0;
                }
            }
        }
    }
}
