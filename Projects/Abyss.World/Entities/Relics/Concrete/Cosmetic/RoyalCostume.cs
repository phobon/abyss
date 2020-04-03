using Microsoft.Xna.Framework;

namespace Abyss.World.Entities.Relics.Concrete.Cosmetic
{
    public class RoyalCostume : CosmeticRelic
    {
        public RoyalCostume(Vector2 initialPosition)
            : base(
            RelicKeys.RoyalCostume,
            "A costume to make the player look like royalty.",
            initialPosition,
            Rectangle.Empty,
            null)
        {
        }
    }
}
