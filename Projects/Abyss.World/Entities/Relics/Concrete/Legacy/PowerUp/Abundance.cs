//using Microsoft.Xna.Framework;
//using System.Collections.Generic;
//using Occasus.Core.Entities;

//namespace Abyss.World.Entities.Relics.Concrete.PowerUp
//{
//    public class Abundance : Relic
//    {
//        private static readonly Rectangle boundingBox = new Rectangle(15, 16, 32, 32);

//        /// <summary>
//        /// Initializes a new instance of the <see cref="Abundance"/> class.
//        /// </summary>
//        /// <param name="initialPosition">The initial position.</param>
//        public Abundance(Vector2 initialPosition) 
//            : base(
//            RelicKeys.Cataclysm, 
//            "A relic that instantly shifts the player into an Abundance phase.", 
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
