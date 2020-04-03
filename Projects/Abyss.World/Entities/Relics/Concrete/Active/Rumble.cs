using Microsoft.Xna.Framework;

namespace Abyss.World.Entities.Relics.Concrete.Active
{
    public class Rumble : ActiveRelic
    {
        public Rumble(Vector2 initialPosition)
            : base(
            RelicKeys.Rumble,
            "Stomps the current platform, stunning all the monsters on it.",
            initialPosition,
            Rectangle.Empty,
            null)
        {
        }
    }
}
