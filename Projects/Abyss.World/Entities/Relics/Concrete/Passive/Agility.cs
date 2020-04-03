using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Occasus.Core.Entities;

namespace Abyss.World.Entities.Relics.Concrete.Passive
{
    public class Agility : Relic
    {
        private static readonly Rectangle boundingBox = new Rectangle(15, 16, 32, 32);
        
        public Agility(Vector2 initialPosition) 
            : base(
            RelicKeys.Agility, 
            "A relic increases the speed of the player by 1.", 
            initialPosition,
            boundingBox,
            RelicType.Passive,
            RelicActivationType.None,
            null)
        {
        }

        /// <summary>
        /// Applies the effects of this phase.
        /// </summary>
        /// <param name="entityCache">The entity cache.</param>
        public override void Activate(IEnumerable<IEntity> entityCache)
        {
            base.Activate(entityCache);
            Monde.GameManager.Player.MovementSpeed++;
        }
    }
}
