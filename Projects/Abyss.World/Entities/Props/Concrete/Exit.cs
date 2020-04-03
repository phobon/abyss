using Abyss.World.Entities.Player;
using Microsoft.Xna.Framework;
using Occasus.Core;

namespace Abyss.World.Entities.Props.Concrete
{
    public class Exit : ActiveProp
    {
        private static readonly Rectangle boundingBox = new Rectangle(7, 0, 50, 64);

        /// <summary>
        /// Initializes a new instance of the <see cref="Exit"/> class.
        /// </summary>
        /// <param name="initialPosition">The initial position.</param>
        public Exit(Vector2 initialPosition)
            : base(
            "Exit", 
            "Ends the game - this is mainly a debug thing.", 
            initialPosition,
            boundingBox,
            Vector2.Zero)
        {
        }

        /// <summary>
        /// Activates the specified player.
        /// </summary>
        /// <param name="player">The player.</param>
        public override void Activate(IPlayer player)
        {
            base.Activate(player);

            Monde.GameManager.StatisticManager.GameOverMessage = "You reached the exit... or did you?";

            // End the game.
            Monde.ActivateScene("GameOver");
        }
    }
}
