using Microsoft.Xna.Framework;

namespace Abyss.World.Entities.Relics.Concrete.Active
{
    public class Dash : ActiveRelic
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Vortex"/> class.
        /// </summary>
        /// <param name="initialPosition">The initial position.</param>
        public Dash(Vector2 initialPosition)
            : base(
            RelicKeys.Vortex,
            "Player dashes forwards.",
            initialPosition,
            Rectangle.Empty,
            null)
        {
        }
    }
}
