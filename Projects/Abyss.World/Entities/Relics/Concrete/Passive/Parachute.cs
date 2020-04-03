using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Occasus.Core.Entities;

namespace Abyss.World.Entities.Relics.Concrete.Passive
{
    public class Parachute : Relic
    {
        public Parachute(Vector2 initialPosition)
            : base(
            RelicKeys.Parachute,
            "Player's fall is slowed.",
            initialPosition,
            Rectangle.Empty,
            RelicType.Passive,
            RelicActivationType.None,
            null)
        {
        }

        /// <summary>
        /// Activates the effects of this relic.
        /// </summary>
        /// <param name="entityCache">The entity cache.</param>
        public override void Activate(IEnumerable<IEntity> entityCache)
        {
            base.Activate(entityCache);
        }
    }
}
