using System.Collections.Generic;
using Abyss.World.Entities.Monsters;
using Microsoft.Xna.Framework;
using Occasus.Core.Entities;

namespace Abyss.World.Entities.Relics.Concrete.Transient
{
    public class Vortex : Relic
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Vortex"/> class.
        /// </summary>
        /// <param name="initialPosition">The initial position.</param>
        public Vortex(Vector2 initialPosition)
            : base(
            RelicKeys.Vortex,
            "Splits all monsters surrounding the player.",
            initialPosition,
            Rectangle.Empty,
            RelicType.Transient,
            RelicActivationType.DimensionShift,
            new List<string> { EntityTags.Monster })
        {
        }

        /// <summary>
        /// Activates the effects of this relic.
        /// </summary>
        /// <param name="entityCache">The entity cache.</param>
        public override void Activate(IEnumerable<IEntity> entityCache)
        {
            base.Activate(entityCache);

            foreach (var i in entityCache)
            {
                if (Vector2.Distance(i.Transform.Position, GameManager.Player.Transform.Position) < 5)
                {
                    var monster = (IMonster)i;
                    monster.Die();
                }
            }
        }
    }
}
