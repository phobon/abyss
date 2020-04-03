using Abyss.World.Entities.Items;
using Abyss.World.Entities.Monsters;
using Abyss.World.Entities.Platforms;
using Abyss.World.Entities.Props;
using System.Collections.Generic;
using Occasus.Core.Entities;

namespace Abyss.World.Maps
{
    public interface IMapEntities
    {
        /// <summary>
        /// Gets the platforms.
        /// </summary>
        IList<IPlatform> Platforms { get; }

        /// <summary>
        /// Gets the hazards.
        /// </summary>
        IList<IEntity> Hazards { get; }

        /// <summary>
        /// Gets the props.
        /// </summary>
        IList<IProp> Props { get; }

        /// <summary>
        /// Gets the items.
        /// </summary>
        IList<IItem> Items { get; }

        /// <summary>
        /// Gets the monsters.
        /// </summary>
        IList<IMonster> Monsters { get; }

        /// <summary>
        /// Gets the triggers.
        /// </summary>
        IList<ITrigger> Triggers { get; }
    }
}
