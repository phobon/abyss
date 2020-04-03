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
        public Idler(Vector2 initialPosition)
            : base(
            "Idler",
            "Idlers stand around being a nuisance.",
            initialPosition,
            null,
            boundingBox,
            ZoneType.Normal)
        {
        }

        protected override void InitializeTags()
        {
            base.InitializeTags();
            this.Tags.Add(EntityTags.GroundedMonster);
            this.Tags.Add(EntityTags.Idler);
        }
    }
}
