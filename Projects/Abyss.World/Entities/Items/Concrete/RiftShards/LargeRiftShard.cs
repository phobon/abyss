using Microsoft.Xna.Framework;

namespace Abyss.World.Entities.Items.Concrete.RiftShards
{
    public class LargeRiftShard : RiftShard
    {
        private static readonly Rectangle boundingBox = new Rectangle(4, 4, 7, 9);

        /// <summary>
        /// Initializes a new instance of the <see cref="LargeRiftShard" /> class.
        /// </summary>
        /// <param name="initialPosition">The initial position.</param>
        public LargeRiftShard(Vector2 initialPosition)
            : base(
            "LargeRiftShard",
            "A magnificent cluster of pure, crystalized Rift energy.",
            initialPosition,
            boundingBox,
            10)
        {
        }
    }
}
