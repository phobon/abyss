using Microsoft.Xna.Framework;

namespace Abyss.World.Entities.Relics.Concrete.Active
{
    public class Phase : ActiveRelic
    {
        public Phase(Vector2 initialPosition)
            : base(
            RelicKeys.Dash,
            "Player becomes invulnerable for a short amount of time.",
            initialPosition,
            Rectangle.Empty,
            null)
        {
        }
    }
}
