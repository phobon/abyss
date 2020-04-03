using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Occasus.Core.Entities;

namespace Abyss.World.Entities.Relics.Concrete.Passive
{
    public class Vitality : Relic
    {
        private static readonly Rectangle boundingBox = new Rectangle(15, 16, 32, 32);

        /// <summary>
        /// Initializes a new instance of the <see cref="Vitality"/> class.
        /// </summary>
        /// <param name="initialPosition">The initial position.</param>
        public Vitality(Vector2 initialPosition) 
            : base(
            RelicKeys.Vitality, 
            "A relic increases the life of the player by 1.", 
            initialPosition,
            boundingBox,
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
            Monde.GameManager.Player.MaximumLives++;
        }
    }
}
