using System.Collections;
using System.Collections.Generic;
using Abyss.World.Drawing.ParticleEffects;
using Abyss.World.Universe;
using Microsoft.Xna.Framework;
using Occasus.Core.Components.Logic;
using Occasus.Core.Drawing.ParticleEffects;
using Occasus.Core.Entities;
using Occasus.Core.Maths;

namespace Abyss.World.Entities.Items.Concrete.RiftShards
{
    public class SmallRiftShard : RiftShard
    {
        private static readonly Rectangle boundingBox = new Rectangle(5, 5, 5, 5);

        /// <summary>
        /// Initializes a new instance of the <see cref="SmallRiftShard" /> class.
        /// </summary>
        /// <param name="initialPosition">The initial position.</param>
        public SmallRiftShard(Vector2 initialPosition)
            : base(
            "SmallRiftShard",
            "A small shard of pure, crystalized Rift energy.",
            initialPosition,
            boundingBox,
            1)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            // Set these guys as invisible.
            foreach (var s in this.GetSprites())
            {
                s.Opacity = 0f;
            }
        }

        /// <summary>
        /// Performs any animations, state logic or operations required when this engine component begins.
        /// </summary>
        public override void Begin()
        {
            base.Begin();
            CoroutineManager.Add(this.BeginEffect());
        }

        private IEnumerator BeginEffect()
        {
            ((IParticleEffect)this.Components["PowerEffect"]).Emit(180);
            yield return this.SetOpacity(1f, 120, Ease.QuadOut);
        }
    }
}
