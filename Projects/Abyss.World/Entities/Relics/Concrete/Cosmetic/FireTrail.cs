using Microsoft.Xna.Framework;

namespace Abyss.World.Entities.Relics.Concrete.Cosmetic
{
    public class FireTrail : CosmeticRelic
    {
        public FireTrail(Vector2 initialPosition)
            : base(
            RelicKeys.FireTrail,
            "A fire trail cosmetic effect.",
            initialPosition,
            Rectangle.Empty,
            null)
        {
        }
    }
}
