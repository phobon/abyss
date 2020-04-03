//using Microsoft.Xna.Framework;
//using Occasus.Core.Entities;
//using System.Collections.Generic;

//namespace Abyss.World.Entities.Relics.Concrete.Persistent
//{
//    public class Shift : Relic
//    {
//        private static readonly Rectangle boundingBox = new Rectangle(15, 16, 32, 32);

//        /// <summary>
//        /// Initializes a new instance of the <see cref="Shift"/> class.
//        /// </summary>
//        /// <param name="initialPosition">The initial position.</param>
//        public Shift(Vector2 initialPosition) 
//            : base(
//            RelicKeys.Shift, 
//            "A relic that increases the amount of time that the Player spends in Limbo.",
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
//            GameManager.Player.LimboTime += 60;
//        }
//    }
//}
