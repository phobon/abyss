using Abyss.World.Entities.Player;
using Abyss.World.Phases;
using Microsoft.Xna.Framework;

namespace Abyss.World.Entities.Items.Concrete
{
    public class Folder : Item
    {
        private static readonly Rectangle boundingBox = new Rectangle(5, 5, 7, 6);

        /// <summary>
        /// Initializes a new instance of the <see cref="Folder"/> class.
        /// </summary>
        /// <param name="initialPosition">The initial position.</param>
        public Folder(Vector2 initialPosition)
            : base(
                "Folder",
                "Allows the player to force dimensions to fold ahead of time.",
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
            base.Collect(player);

            // Force a fold of dimensional planes.
            PhaseManager.FoldPlane();
        }
    }
}
