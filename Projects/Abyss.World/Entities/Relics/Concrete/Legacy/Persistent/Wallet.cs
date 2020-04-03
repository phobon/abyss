using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Occasus.Core.Entities;

namespace Abyss.World.Entities.Relics.Concrete.Persistent
{
    public class Wallet : Relic
    {
        private const int WalletIncrease = 100;

        private static readonly Rectangle boundingBox = new Rectangle(15, 16, 32, 32);

        /// <summary>
        /// Initializes a new instance of the <see cref="Wallet"/> class.
        /// </summary>
        /// <param name="initialPosition">The initial position.</param>
        public Wallet(Vector2 initialPosition) 
            : base(
            RelicKeys.Vitality, 
            "A relic increases the amount of rift energy a player can carry.", 
            initialPosition,
            boundingBox,
            RelicType.Persistent,
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
            GameManager.Player.MaximumRift += WalletIncrease;
        }
    }
}
