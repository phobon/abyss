using Abyss.World.Entities.Items;
using Abyss.World.Entities.Monsters;
using Abyss.World.Entities.Platforms;
using Abyss.World.Entities.Props;
using System.Collections.Generic;
using Occasus.Core.Entities;

namespace Abyss.World.Maps
{
    public class MapEntities : IMapEntities
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MapEntities"/> class.
        /// </summary>
        public MapEntities()
        {
            this.Platforms = new List<IPlatform>();
            this.Props = new List<IProp>();
            this.Hazards = new List<IEntity>();
            this.Items = new List<IItem>();
            this.Monsters = new List<IMonster>();
            this.Triggers = new List<ITrigger>();
        }

        /// <summary>
        /// Gets the platforms.
        /// </summary>
        public IList<IPlatform> Platforms
        {
            get; private set;
        }

        /// <summary>
        /// Gets the props.
        /// </summary>
        public IList<IProp> Props
        {
            get; private set;
        }

        /// <summary>
        /// Gets the hazards.
        /// </summary>
        public IList<IEntity> Hazards
        {
            get; private set;
        }

        /// <summary>
        /// Gets the items.
        /// </summary>
        public IList<IItem> Items
        {
            get; private set;
        }

        /// <summary>
        /// Gets the monsters.
        /// </summary>
        public IList<IMonster> Monsters
        {
            get; private set;
        }

        /// <summary>
        /// Gets the triggers.
        /// </summary>
        public IList<ITrigger> Triggers
        {
            get; private set;
        }
    }
}
