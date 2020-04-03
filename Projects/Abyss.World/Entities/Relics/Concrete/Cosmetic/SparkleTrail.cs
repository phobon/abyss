using Microsoft.Xna.Framework;

namespace Abyss.World.Entities.Relics.Concrete.Cosmetic
{
    public class SparkleTrail : CosmeticRelic
    {
        public SparkleTrail(Vector2 initialPosition)
            : base(
            RelicKeys.SparkleTrail,
            "A sparkle trail cosmetic effect.",
            initialPosition,
            Rectangle.Empty,
            null)
        {
        }
    }
}
