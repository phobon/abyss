//using Microsoft.Xna.Framework;
//using System.Collections.Generic;
//using Occasus.Core.Entities;

//namespace Abyss.World.Entities.Relics.Concrete.PowerUp
//{
//    public class Merge : Relic
//    {
//        private static readonly Rectangle boundingBox = new Rectangle(15, 16, 32, 32);

//        /// <summary>
//        /// Initializes a new instance of the <see cref="Cataclysm"/> class.
//        /// </summary>
//        /// <param name="initialPosition">The initial position.</param>
//        public Merge(Vector2 initialPosition) 
//            : base(
//            RelicKeys.Cataclysm, 
//            "A relic that instantly merges dimensional planes and returns to a calm phase.", 
//            initialPosition,
//            boundingBox,
//            RelicType.PowerUp,
//            RelicActivationType.Instant,
//            new List<string>(),
//            100,
//            0)
//        {
//        }

//        /// <summary>
//        /// Activates the effects of this relic.
//        /// </summary>
//        /// <param name="entityCache">The entity cache.</param>
//        public override void Activate(IEnumerable<IEntity> entityCache)
//        {
//            base.Activate(entityCache);
//        }

//        /// <summary>
//        /// Deactivates the effects of this relic.
//        /// </summary>
//        /// <param name="entityCache">The entity cache.</param>
//        public override void Deactivate(IEnumerable<IEntity> entityCache)
//        {
//            base.Deactivate(entityCache);
//        }
//    }
//}
