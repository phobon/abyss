//using System.Collections.Generic;
//using Abyss.World.Entities.Monsters;
//using Microsoft.Xna.Framework;
//using Occasus.Core.Entities;

//namespace Abyss.World.Entities.Relics.Concrete.Transient
//{
//    public class ChainReaction : StompRelic
//    {
//        /// <summary>
//        /// Initializes a new instance of the <see cref="ChainReaction"/> class.
//        /// </summary>
//        /// <param name="initialPosition">The initial position.</param>
//        public ChainReaction(Vector2 initialPosition)
//            : base(
//            RelicKeys.ChainReaction,
//            "Chance to split all monsters on a platform the player lands on.",
//            initialPosition,
//            Rectangle.Empty,
//            new List<string> { EntityTags.Monster },
//            30)
//        {
//        }

//        /// <summary>
//        /// Activates the effects of this relic.
//        /// </summary>
//        /// <param name="entityCache">The entity cache.</param>
//        public override void Activate(IEnumerable<IEntity> entityCache)
//        {
//            base.Activate(entityCache);

//            // We have a platform that the player has landed on and we have the cache. So we need to determine which monsters are near the platform
//            // then split them all.
//            foreach (var m in entityCache)
//            {
//                if (m.Collider.QualifiedBoundingBox.Intersects(this.CurrentPlatform))
//                {
//                    ((IMonster)m).Die();
//                }
//            }
//        }
//    }
//}
