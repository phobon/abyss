using System;
using System.Collections.Generic;
using Abyss.World.Entities.Items;
using Occasus.Core.Entities;

namespace Abyss.World.Entities.Relics
{
    public interface IRelic : IItem
    {
        /// <summary>
        /// Occurs when the relic is activated.
        /// </summary>
        event EventHandler Activated;

        /// <summary>
        /// Occurs when the relic is deactivated.
        /// </summary>
        event EventHandler Deactivated;

        /// <summary>
        /// Gets a value indicating whether this relic relates to the whole entity cache.
        /// </summary>
        /// <value>
        /// <c>true</c> if [uses whole entity cache]; otherwise, <c>false</c>.
        /// </value>
        bool UsesWholeEntityCache { get; }

        /// <summary>
        /// Gets the relevant entity tags.
        /// </summary>
        IEnumerable<string> RelevantEntityTags { get; }
            
        /// <summary>
        /// Gets the type of the relic.
        /// </summary>
        RelicType RelicType { get; }

        /// <summary>
        /// Gets the type of activation that this relic requires.
        /// </summary>
        RelicActivationType RelicActivationType { get; }

        /// <summary>
        /// Gets the life span of the relic in seconds.
        /// </summary>
        int LifeSpan { get; }

        /// <summary>
        /// Gets the percentage chance that this relic will activate.
        /// </summary>
        int ActivationChance { get; }

        /// <summary>
        /// Activates the effects of this relic.
        /// </summary>
        /// <param name="entityCache">The entity cache.</param>
        void Activate(IEnumerable<IEntity> entityCache);

        /// <summary>
        /// Deactivates the effects of this relic.
        /// </summary>
        /// <param name="entityCache">The entity cache.</param>
        void Deactivate(IEnumerable<IEntity> entityCache);
    }
}
