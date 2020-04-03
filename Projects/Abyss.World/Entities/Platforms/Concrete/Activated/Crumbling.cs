
using Occasus.Core;
using Occasus.Core.Components.Logic;
using Occasus.Core.Drawing.Animation;
using Occasus.Core.Physics;
using System.Collections;
using Occasus.Core.Maps;
using Occasus.Core.Maps.Definitions;

namespace Abyss.World.Entities.Platforms.Concrete.Activated
{
    public class Crumbling : ActivatedPlatform
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Crumbling" /> class.
        /// </summary>
        /// <param name="platformDefinition">The platform definition.</param>
        public Crumbling(IPlatformDefinition platformDefinition)
            : base(
                PlatformKeys.Crumbling,
                "A platform that crumbles and disappears when a player lands on it.",
                platformDefinition.Position,
                platformDefinition.Size)
        {
        }

        protected override void ColliderOnCollision(CollisionEventArgs args)
        {
            if (!this.IsValidCollision(args))
            {
                return;
            }
            
            CoroutineManager.Add(this.Id + "_Crumbling", this.CrumbleEffect());
            this.isActivated = true;
        }

        private IEnumerator CrumbleEffect()
        {
            // Shake for a bit to indicate that this guy is going to disappear.
            yield return this.Transform.Shake(1f, TimingHelper.GetFrameCount(1f));

            // Suspend and flag this entity to be removed because it's gone.
            this.Suspend();
            this.Flags[EngineFlag.Collidable] = false;
            this.Flags[EngineFlag.Relevant] = false;
        }
    }
}
