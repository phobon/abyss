using System.Collections.Generic;
using Abyss.World.Entities.Monsters;
using Microsoft.Xna.Framework;
using Occasus.Core.Entities;

namespace Abyss.World.Entities.Relics.Concrete.Active
{
    public class Vortex : ActiveRelic
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
                if (Vector2.Distance(i.Transform.Position, Monde.GameManager.Player.Transform.Position) < 5)
                {
                    var monster = (IMonster)i;
                    monster.Die();
                }
            }
        }
    }
}
