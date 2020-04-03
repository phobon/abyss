using Microsoft.Xna.Framework;

namespace Abyss.World.Entities.Relics.Concrete.Cosmetic
{
    public class MonsterCostume : CosmeticRelic
    {
        public MonsterCostume(Vector2 initialPosition)
            : base(
            RelicKeys.MonsterCostume,
            "A costume to make the player look like monster.",
            initialPosition,
            Rectangle.Empty,
            null)
        {
        }
    }
}
