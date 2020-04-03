using Occasus.Core.Maps;
using Occasus.Core.Maps.Definitions;
using Occasus.Core.Physics;

namespace Abyss.World.Entities.Platforms.Concrete.Activated
{
    public class Exploding : ActivatedPlatform
    {
        public Exploding(IPlatformDefinition platformDefinition)
            : base(
                PlatformKeys.Exploding,
                "A platform that explodes when the plyer touches it.",
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
            
            this.isActivated = true;
        }
    }
}
