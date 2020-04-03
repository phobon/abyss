using Microsoft.Xna.Framework;

namespace Abyss.World.Entities.Relics.Concrete.Cosmetic
{
    public class ShadowTrail : CosmeticRelic
    {
        public ShadowTrail(Vector2 initialPosition)
            : base(
            RelicKeys.ShadowTrail,
            "A shadow trail cosmetic effect.",
            initialPosition,
            Rectangle.Empty,
            null)
        {
        }
    }
}
