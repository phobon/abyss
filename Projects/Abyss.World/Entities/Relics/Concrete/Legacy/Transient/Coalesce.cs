//using System.Collections.Generic;
//using Microsoft.Xna.Framework;
//using Occasus.Core.Entities;

//namespace Abyss.World.Entities.Relics.Concrete.Transient
//{
//    public class Coalesce : StompRelic
//    {
//        /// <summary>
//        /// Initializes a new instance of the <see cref="Coalesce"/> class.
//        /// </summary>
//        /// <param name="initialPosition">The initial position.</param>
//        public Coalesce(Vector2 initialPosition)
//            : base(
//            RelicKeys.Coalesce,
//            "Chance to drain energy from a Rift phase, reducing the time it lasts.",
//            initialPosition,
//            Rectangle.Empty,
//            null,
//            50)
//        {
//        }

//        /// <summary>
//        /// Activates the effects of this relic.
//        /// </summary>
//        /// <param name="entityCache">The entity cache.</param>
//        public override void Activate(IEnumerable<IEntity> entityCache)
//        {
//            base.Activate(entityCache);

//            // Add more progress to the current phase.
//            //GameManager.CurrentPhase.CurrentProgress += 10;

//            // TODO: Some effect is required here.
//        }
//    }
//}
