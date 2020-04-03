using Abyss.World.Entities.Items;
using Abyss.World.Entities.Items.Concrete.RiftShards;
using Microsoft.Xna.Framework;
using Occasus.Core;
using Occasus.Core.Drawing.ParticleEffects;
using Occasus.Core.Entities;
using System;
using System.Collections.Generic;

namespace Abyss.World.Entities.Relics.Concrete.Passive
{
    public class Conduit : Relic
    {
        private static readonly Rectangle boundingBox = new Rectangle(15, 16, 32, 32);

        /// <summary>
        /// Initializes a new instance of the <see cref="Conduit"/> class.
        /// </summary>
        /// <param name="initialPosition">The initial position.</param>
        public Conduit(Vector2 initialPosition) 
            : base(
            RelicKeys.Conduit, 
            "A relic that draws rift energy from other dimensions, making rift crystals far more potent.",
            initialPosition,
            boundingBox,
            RelicType.Passive,
            RelicActivationType.None,
            new List<string> { EntityTags.All, EntityTags.RiftCrystal })
        {
        }

        /// <summary>
        /// Applies the effects of this phase.
        /// </summary>
        /// <param name="entityCache">The entity cache.</param>
        public override void Activate(IEnumerable<IEntity> entityCache)
        {
            foreach (var e in entityCache)
            {
                var item = (IItem)e;
                if (e.Flags[EngineFlag.Active])
                {
                    ((IParticleEffect)e.Components["PowerEffect"]).Emit();
                }

                item.Collected += ItemOnCollected;
            }

            base.Activate(entityCache);
        }

        private void ItemOnCollected(object sender, EventArgs eventArgs)
        {
            var riftShard = (RiftShard) sender;
            GameManager.Player.Rift += riftShard.RiftAmount;
        }
    }
}
