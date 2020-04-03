using System.Collections.Generic;
using Abyss.World.Universe;
using Microsoft.Xna.Framework;

namespace Abyss.World.Entities.Monsters.Concrete
{
    public class Idler : Monster
    {
        private static readonly Rectangle boundingBox = new Rectangle(3, 10, 9, 6);

        /// <summary>
        /// Initializes a new instance of the <see cref="Idler" /> class.
        /// </summary>
        /// <param name="initialPosition">The initial position.</param>
        /// <param name="path">The path.</param>
        public Idler(Vector2 initialPosition, IEnumerable<Vector2> path)
            : base(
            "Idler",
            "Idlers stand around being a nuisance.",
            initialPosition,
            null,
            boundingBox,
            ZoneType.Normal)
        {
            this.Tags.Add(EntityTags.GroundedMonster);
            this.Tags.Add(EntityTags.Idler);
        }
    }
}
