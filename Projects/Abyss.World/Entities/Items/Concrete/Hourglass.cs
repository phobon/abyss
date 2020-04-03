using Abyss.World.Entities.Player;
using Microsoft.Xna.Framework;

namespace Abyss.World.Entities.Items.Concrete
{
    public class Hourglass : Item
    {
        private const int AdditionalTime = 15;

        private static readonly Rectangle boundingBox = new Rectangle(16, 10, 36, 44);

        /// <summary>
        /// Initializes a new instance of the <see cref="Hourglass"/> class.
        /// </summary>
        /// <param name="initialPosition">The initial position.</param>
        public Hourglass(Vector2 initialPosition)
            : base(
                "Hourglass",
                "Constrains intra-dimensional fabric, slowing the passage of time.",
                initialPosition,
                boundingBox)
        {
        }

        /// <summary>
        /// Collects this item.
        /// </summary>
        /// <param name="player">The player.</param>
        public override void Collect(IPlayer player)
        {
            // The hourglass gives the player a bit of time back, so add it to the clock.
            //GameManager.AddRemainingGameTime(AdditionalTime);
            base.Collect(player);
        }
    }
}
