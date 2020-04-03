using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Occasus.Core.Physics;

namespace Abyss.World.Entities.Platforms.Concrete
{
    public abstract class DynamicPlatform : Platform, IDynamicPlatform
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicPlatform" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        /// <param name="initialPosition">The initial position.</param>
        /// <param name="size">The size.</param>
        /// <param name="path">The path.</param>
        protected DynamicPlatform(
            string name, 
            string description, 
            Vector2 initialPosition, 
            Rectangle size, 
            IEnumerable<Vector2> path = null)
            : base(
                  name, 
                  description, 
                  initialPosition, 
                  size, 
                  PlatformType.Dynamic)
        {
            this.Path = path;
        }

        /// <summary>
        /// Gets the path of this platform.
        /// </summary>
        public IEnumerable<Vector2> Path
        {
            get; private set;
        }

        protected override void ColliderOnCollision(CollisionEventArgs args)
        {
        }
    }
}
