using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Occasus.Core.Entities;

namespace Abyss.World.Entities.Relics.Concrete.Persistent
{
    public class SpeedUp : Relic
    {
        private static readonly Rectangle boundingBox = new Rectangle(15, 16, 32, 32);

        /// <summary>
        /// Initializes a new instance of the <see cref="SpeedUp"/> class.
        /// </summary>
        /// <param name="initialPosition">The initial position.</param>
        public SpeedUp(Vector2 initialPosition) 
            : base(
            RelicKeys.SpeedUp, 
            "A relic increases the speed of the player by 1.", 
            initialPosition,
            boundingBox,
            RelicType.Persistent,
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
            GameManager.Player.MovementSpeed++;
        }
    }
}
