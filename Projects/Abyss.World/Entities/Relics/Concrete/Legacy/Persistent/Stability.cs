//using Microsoft.Xna.Framework;
//using System.Collections.Generic;
//using Abyss.World.Phases;
//using Occasus.Core.Entities;

//namespace Abyss.World.Entities.Relics.Concrete.Persistent
//{
//    public class Stability : Relic
//    {
//        private static readonly Rectangle boundingBox = new Rectangle(15, 16, 32, 32);

//        /// <summary>
//        /// Initializes a new instance of the <see cref="Stability"/> class.
//        /// </summary>
//        /// <param name="initialPosition">The initial position.</param>
//        public Stability(Vector2 initialPosition) 
//            : base(
//            RelicKeys.Stability, 
//            "A relic that increases the amount of time that an Aberth phase lasts for.", 
//            initialPosition,
//            boundingBox,
//            RelicType.Persistent,
//            RelicActivationType.None,
//            null)
//        {
//        }

//        /// <summary>
//        /// Activates the effects of this relic.
//        /// </summary>
//        /// <param name="entityCache">The entity cache.</param>
//        public override void Activate(IEnumerable<IEntity> entityCache)
//        {
//            base.Activate(entityCache);
//            PhaseManager.AberthPhaseLength += 5;
//        }
//    }
//}
