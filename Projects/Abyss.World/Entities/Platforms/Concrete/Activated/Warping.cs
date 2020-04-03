using Occasus.Core.Physics;

namespace Abyss.World.Entities.Platforms.Concrete.Activated
{
    public class Warping : ActivatedPlatform
    {
        public Warping(IPlatformDefinition platformDefinition)
            : base(
                PlatformKeys.Warping,
                "A platform that warps anyone that steps onto it to another part of the world.",
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
