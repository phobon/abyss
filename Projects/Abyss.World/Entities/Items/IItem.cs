using System;
using Abyss.World.Entities.Player;
using Occasus.Core.Entities;

namespace Abyss.World.Entities.Items
{
    /// <summary>
    /// Interface for an Item in Abyss. Items are things that a player picks up that aids them on their journey.
    /// </summary>
    public interface IItem : IEntity
    {
        /// <summary>
        /// Occurs when the item is collected.
        /// </summary>
        event EventHandler Collected;

        /// <summary>
        /// Collects this item.
        /// </summary>
        /// <param name="player">The player.</param>
        void Collect(IPlayer player);

        /// <summary>
        /// Makes this item hover when required.
        /// </summary>
        void Hover();

        /// <summary>
        /// Fades the item out of the current dimension.
        /// </summary>
        void FadeOut();

        /// <summary>
        /// Fades the item into the current dimension.
        /// </summary>
        void FadeIn();
    }
}
