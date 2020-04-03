using Abyss.World.Entities.Player;
using Microsoft.Xna.Framework;
using Occasus.Core;
using Occasus.Core.Components.Logic;
using Occasus.Core.Drawing.Animation;
using Occasus.Core.Physics;
using System.Collections;

namespace Abyss.World.Entities.Props.Concrete
{
    public class SpikeTrap : Prop
    {
        private static readonly Rectangle boundingBox = new Rectangle(0, 13, 16, 3);

        /// <summary>
        /// Initializes a new instance of the <see cref="SpikeTrap"/> class.
        /// </summary>
        /// <param name="initialPosition">The initial position.</param>
        public SpikeTrap(Vector2 initialPosition)
            : base(
            "SpikeTrap",
            "Sharp, painful spikes that hurt.", 
            initialPosition,
            boundingBox,
            Vector2.Zero)
        {
        }

        protected override void ColliderOnCollision(CollisionEventArgs args)
        {
            base.ColliderOnCollision(args);

            var player = args.CollisionEntity as IPlayer;
            if (player != null)
            {
                // If the player is currently grounded, they can safely walk through the spikes. These will only kill the player if they fall on them.
                if (!player.Collider.Flags[PhysicsFlag.Grounded])
                {
                    // Player is killed by damage.
                    player.TakeDamage(this.Name);
                    CoroutineManager.Add(this.Id + "_Collision", this.CollisionEffect());
                }
            }
        }

        private IEnumerator CollisionEffect()
        {
            this.Flags[EngineFlag.Collidable] = false;
            yield return Coroutines.Pause(TimingHelper.GetFrameCount(0.5f));
            this.Flags[EngineFlag.Collidable] = true;
        }
    }
}
