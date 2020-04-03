using System.Collections;
using Occasus.Core.Components.Logic;
using Occasus.Core.Physics;

namespace Abyss.World.Entities.Platforms.Concrete.Activated
{
    public class Spikey : ActivatedPlatform
    {
        public Spikey(IPlatformDefinition platformDefinition)
            : base(
                PlatformKeys.Spikey,
                "A platform that shoots deadly spikes into the air when activated.",
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

            CoroutineManager.Add(this.SpikeEffect().GetEnumerator());
            this.isActivated = true;
        }

        private IEnumerable SpikeEffect()
        {
            yield return null;
        }
    }
}
