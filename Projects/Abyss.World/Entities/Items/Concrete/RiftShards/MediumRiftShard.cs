using Microsoft.Xna.Framework;

namespace Abyss.World.Entities.Items.Concrete.RiftShards
{
    public class MediumRiftShard : RiftShard
    {
        private static readonly Rectangle boundingBox = new Rectangle(4, 4, 7, 7);

        /// <summary>
        /// Initializes a new instance of the <see cref="MediumRiftShard" /> class.
        /// </summary>
        /// <param name="initialPosition">The initial position.</param>
        public MediumRiftShard(Vector2 initialPosition)
            : base(
                "MediumRiftShard",
                "A medium-sized chunk of pure, crystalized Rift energy.",
                initialPosition,
                boundingBox,
                5)
        {
        }
    }
}
