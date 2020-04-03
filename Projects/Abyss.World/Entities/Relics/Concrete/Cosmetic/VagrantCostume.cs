using Microsoft.Xna.Framework;

namespace Abyss.World.Entities.Relics.Concrete.Cosmetic
{
    public class VagrantCostume : CosmeticRelic
    {
        public VagrantCostume(Vector2 initialPosition)
            : base(
            RelicKeys.VagrantCostume,
            "A costume to make the player look like vagrant.",
            initialPosition,
            Rectangle.Empty,
            null)
        {
        }
    }
}
