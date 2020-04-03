//using Microsoft.Xna.Framework;
//using System.Collections.Generic;
//using Occasus.Core.Entities;

//namespace Abyss.World.Entities.Relics.Concrete.PowerUp
//{
//    public class Cataclysm : Relic
//    {
//        private const int BaseLifeSpan = 15;

//        private static readonly Rectangle boundingBox = new Rectangle(15, 16, 32, 32);

//        /// <summary>
//        /// Initializes a new instance of the <see cref="Cataclysm"/> class.
//        /// </summary>
//        /// <param name="initialPosition">The initial position.</param>
//        public Cataclysm(Vector2 initialPosition) 
//            : base(
//            RelicKeys.Cataclysm, 
//            "A relic that transforms all living matter into pure rift energy.", 
//            initialPosition,
//            boundingBox,
//            RelicType.PowerUp,
//            RelicActivationType.Instant,
//            new List<string> { EntityTags.All, EntityTags.Monster },
//            100,
//            BaseLifeSpan)
//        {
//        }

//        /// <summary>
//        /// Activates the effects of this relic.
//        /// </summary>
//        /// <param name="entityCache">The entity cache.</param>
//        public override void Activate(IEnumerable<IEntity> entityCache)
//        {
//            if (GameManager.RelicCollection.TransientRelics[RelicKeys.CataclysmBoost] != null)
//            {
//                this.LifeSpan += BaseLifeSpan;
//            }

//            base.Activate(entityCache);

//            // Add some sort of glowy effect to all monsters to indicate they're under the effects of cataclysm.
//        }

//        /// <summary>
//        /// Deactivates the effects of this relic.
//        /// </summary>
//        /// <param name="entityCache">The entity cache.</param>
//        public override void Deactivate(IEnumerable<IEntity> entityCache)
//        {
//            base.Deactivate(entityCache);

//            // Remove the glowy effect.
//        }
//    }
//}
