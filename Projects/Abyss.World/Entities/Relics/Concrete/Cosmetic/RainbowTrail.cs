using Microsoft.Xna.Framework;

namespace Abyss.World.Entities.Relics.Concrete.Cosmetic
{
    public class RainbowTrail : CosmeticRelic
    {
        public RainbowTrail(Vector2 initialPosition)
            : base(
            RelicKeys.RainbowTrail,
            "A rainbow trail cosmetic effect.",
            initialPosition,
            Rectangle.Empty,
            null)
        {
        }
    }
}
