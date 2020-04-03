using Microsoft.Xna.Framework;
using Occasus.Core.Components.Logic;
using Occasus.Core.Drawing.Animation;
using Occasus.Core.Entities;
using Occasus.Core.Maths;
using Occasus.Core.Physics;
using System.Collections;
using System.Linq;

namespace Abyss.World.Entities.Platforms.Concrete.Dynamic
{
    public class Crushing : DynamicPlatform
    {
        public Crushing(IPlatformDefinition platformDefinition)
            : base(
                PlatformKeys.Crushing,
                "A sentient platform that attempts to crush anything around it.",
                platformDefinition.Position,
                platformDefinition.Size,
                platformDefinition.Path)
        {
            this.Collider.MovementSpeed = new Vector2(300f, PhysicsManager.BaseActorSpeed);
        }

        /// <summary>
        /// Performs any animations, state logic or operations required when this engine component begins.
        /// </summary>
        public override void Begin()
        {
            base.Begin();
            CoroutineManager.Add(this.Id + "_Crush", this.Crush());
        }

        private IEnumerator Crush()
        {
            var index = 0;
            var pathCount = this.Path.Count();

            var sprite = this.GetSprite();
            while (true)
            {
                var nextPathTarget = this.Path.ElementAt(index);

                sprite.GoToAnimation("Activating");
                yield return Coroutines.Pause(36);

                // Shake for a bit to indicate that this guy is going to move.
                yield return this.Transform.Shake(1f, TimingHelper.GetFrameCount(1f));

                sprite.GoToAnimation("Activated");

                // Calculate distance between the two points to determine speed.
                var distance = Vector2.Distance(this.Transform.Position, nextPathTarget);
                var t = distance / this.Collider.MovementSpeed.X;

                // Accelerate towards the end of the path.
                yield return this.Transform.MoveTo(nextPathTarget, TimingHelper.GetFrameCount(t), EaseType.CubeIn);

                // Add an impact effect.
                yield return this.Transform.Shake(1f, TimingHelper.GetFrameCount(0.2f));

                sprite.GoToAnimation("Idle");

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
