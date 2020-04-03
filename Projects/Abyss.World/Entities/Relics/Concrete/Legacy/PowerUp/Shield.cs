using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Occasus.Core.Entities;

namespace Abyss.World.Entities.Relics.Concrete.PowerUp
{
    public class Shield : Relic
    {
        private static readonly Rectangle boundingBox = new Rectangle(15, 16, 32, 32);

        /// <summary>
        /// Initializes a new instance of the <see cref="Shield"/> class.
        /// </summary>
        /// <param name="initialPosition">The initial position.</param>
        public Shield(Vector2 initialPosition) 
            : base(
            RelicKeys.Shield, 
            "A relic that bestows a semi-permanent shield of pure rift energy that protects the wielder from a single source of harm.",
            initialPosition,
            boundingBox,
            RelicType.PowerUp,
            RelicActivationType.Instant,
            null)
        {
        }

        /// <summary>
        /// Activates the effects of this relic.
        /// </summary>
        /// <param name="entityCache">The entity cache.</param>
        public override void Activate(IEnumerable<IEntity> entityCache)
        {
            var shieldCharges = 1;
            if (GameManager.RelicCollection.TransientRelics[RelicKeys.ShieldBoost] != null)
            {
                shieldCharges++;
            }

            // Generate the player's shield.
            GameManager.Player.Barrier.Generate(shieldCharges);

            base.Activate(entityCache);
        }
    }
}
