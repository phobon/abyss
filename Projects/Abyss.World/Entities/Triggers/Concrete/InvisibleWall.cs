using Microsoft.Xna.Framework;
using Occasus.Core.Entities;

namespace Abyss.World.Entities.Triggers.Concrete
{
    public class InvisibleWall : Trigger
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvisibleWall" /> class.
        /// </summary>
        /// <param name="initialPosition">The initial position.</param>
        /// <param name="boundingBox">The bounding box.</param>
        public InvisibleWall(Vector2 initialPosition, Rectangle boundingBox)
            : base("Invisible Wall", "Used to block monster AI.", initialPosition, boundingBox, Vector2.Zero)
        {
            this.Tags.Add("InvisibleWall");
        }
    }
}
