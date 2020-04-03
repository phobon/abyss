using System.Collections.Generic;
using Abyss.World.Entities.Items;
using Microsoft.Xna.Framework;
using Occasus.Core.Entities;

namespace Abyss.World.Entities.Relics.Concrete.Passive
{
    public class Magnet : Relic
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Magnet"/> class.
        /// </summary>
        /// <param name="initialPosition">The initial position.</param>
        public Magnet(Vector2 initialPosition)
            : base(
            RelicKeys.Magnet,
            "Player absorbs all items surrounding them.",
            initialPosition,
            Rectangle.Empty,
            RelicType.Passive,
            RelicActivationType.None,
            new List<string> { EntityTags.Item })
        {
        }

        /// <summary>
        /// Activates the effects of this relic.
        /// </summary>
        /// <param name="entityCache">The entity cache.</param>
        public override void Activate(IEnumerable<IEntity> entityCache)
        {
            base.Activate(entityCache);

            foreach (var i in entityCache)
            {
                if (Vector2.Distance(i.Transform.Position, GameManager.Player.Transform.Position) < 5)
                {
                    var item = (IItem)i;
                    item.Collect(GameManager.Player);
                }
            }
        }
    }
}
