using Abyss.World.Entities.Player;
using Microsoft.Xna.Framework;

namespace Abyss.World.Entities.Items.Concrete
{
    public class Heart : Item
    {
        private static readonly Rectangle boundingBox = new Rectangle(5, 5, 7, 6);

        /// <summary>
        /// Initializes a new instance of the <see cref="Heart"/> class.
        /// </summary>
        /// <param name="initialPosition">The initial position.</param>
        public Heart(Vector2 initialPosition)
            : base(
                "Heart",
                "Allows the player to survive for longer in the Abyss.",
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
            // If the player has maximum lives, then give some points instead.
            if (GameManager.Player.Lives != 3)
            {
                GameManager.Player.Lives++;
            }
            else
            {
                GameManager.StatisticManager.TotalScore += 100;
            }

            base.Collect(player);
        }
    }
}
