using Occasus.Core.Maps;
using Occasus.Core.Maps.Definitions;
using Occasus.Core.Physics;

namespace Abyss.World.Entities.Platforms.Concrete.Activated
{
    public class Icy : ActivatedPlatform
    {
        private const float IcyDragFactor = 0.8f;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="Icy" /> class.
        /// </summary>
        /// <param name="platformDefinition">The platform definition.</param>
        public Icy(IPlatformDefinition platformDefinition)
            : base(
                PlatformKeys.Icy,
                "A platform that is almost impossible to gain traction on.",
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

            // Change the ground drag factor so the entities slide over it.
            PhysicsManager.GroundDragFactor = IcyDragFactor;
        }
    }
}
