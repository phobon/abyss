using Microsoft.Xna.Framework;
using Occasus.Core.Physics;

namespace Abyss.World.Entities.Platforms.Concrete
{
    public abstract class ActivatedPlatform : Platform
    {
        protected bool isActivated = false;

        protected ActivatedPlatform(
            string name, 
            string description, 
            Vector2 initialPosition, 
            Rectangle size) 
            : base(
                name, 
                description, 
                initialPosition, 
                size, 
                PlatformType.Activated)
        {
        }

        protected bool IsValidCollision(CollisionEventArgs args)
        {
            // If this platform isn't landed on, then don't crumble it away.
            if (args.CollisionPosition != CollisionPosition.Top)
            {
                return false;
            }

            // If this platform has already been landed on by the player, then return out of this method.
            if (this.isActivated)
            {
                return false;
            }

            this.isActivated = true;

            return true;
        }
    }
}
