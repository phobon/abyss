using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Occasus.Core.Entities;

namespace Abyss.World.Entities.Relics.Concrete.Passive
{
    public class Drain : Relic
    {
        private static readonly Rectangle boundingBox = new Rectangle(15, 16, 32, 32);
        
        public Drain(Vector2 initialPosition) 
            : base(
            RelicKeys.Drain,
            "Player leeches rift energy from monsters that they split.", 
            initialPosition,
            boundingBox,
            RelicType.Passive,
            RelicActivationType.None,
            null)
        {
        }

        /// <summary>
        /// Activates the effects of this relic.
        /// </summary>
        /// <param name="entityCache">The entity cache.</param>
        public override void Activate(IEnumerable<IEntity> entityCache)
        {
            base.Activate(entityCache);
            Monde.GameManager.Player.MonsterStomped += PlayerOnMonsterSplit;
        }

        /// <summary>
        /// Deactivates the effects of this relic.
        /// </summary>
        /// <param name="entityCache">The entity cache.</param>
        public override void Deactivate(IEnumerable<IEntity> entityCache)
        {
            base.Deactivate(entityCache);
            Monde.GameManager.Player.MonsterStomped -= PlayerOnMonsterSplit;
        }

        private void PlayerOnMonsterSplit(object sender, EventArgs eventArgs)
        {
            Monde.GameManager.Player.Rift += 10;
        }
    }
}
