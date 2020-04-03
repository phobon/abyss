using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Occasus.Core.Entities;

namespace Abyss.World.Entities.Relics.Concrete.Persistent
{
    public class Vampire : Relic
    {
        private static readonly Rectangle boundingBox = new Rectangle(15, 16, 32, 32);

        /// <summary>
        /// Initializes a new instance of the <see cref="Vampire"/> class.
        /// </summary>
        /// <param name="initialPosition">The initial position.</param>
        public Vampire(Vector2 initialPosition) 
            : base(
            RelicKeys.Vampire,
            "Player leeches rift energy from monsters that they split.", 
            initialPosition,
            boundingBox,
            RelicType.Persistent,
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
            GameManager.Player.MonsterStomped += PlayerOnMonsterSplit;
        }

        /// <summary>
        /// Deactivates the effects of this relic.
        /// </summary>
        /// <param name="entityCache">The entity cache.</param>
        public override void Deactivate(IEnumerable<IEntity> entityCache)
        {
            base.Deactivate(entityCache);
            GameManager.Player.MonsterStomped -= PlayerOnMonsterSplit;
        }

        private void PlayerOnMonsterSplit(object sender, EventArgs eventArgs)
        {
            GameManager.Player.Rift += 10;
        }
    }
}
