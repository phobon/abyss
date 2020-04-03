using System;
using Abyss.World.Entities.Player;

using Occasus.Core.Entities;

namespace Abyss.World.Entities.Props
{
    public interface IActiveProp : IEntity
    {
        /// <summary>
        /// Occurs when prop is activated.
        /// </summary>
        event EventHandler Activated;

        /// <summary>
        /// Gets a value indicating whether the prop can activate or not.
        /// </summary>
        /// <value><c>True</c> if the prop can activate; otherwise, <c>false</c>.
        /// </value>
        bool CanActivate { get; }

        /// <summary>
        /// Activates the prop.
        /// </summary>
        /// <param name="player">The player.</param>
        void Activate(IPlayer player);
    }
}
