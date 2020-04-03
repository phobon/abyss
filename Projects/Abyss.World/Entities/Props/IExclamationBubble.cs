using Occasus.Core.Entities;

namespace Abyss.World.Entities.Props
{
    public interface IExclamationBubble : IEntity
    {
        /// <summary>
        /// Gets the parent.
        /// </summary>
        IEntity Parent { get; }

        /// <summary>
        /// Makes the bubble appear.
        /// </summary>
        void Appear();

        /// <summary>
        /// Makes the bubble disappear.
        /// </summary>
        void Disappear();

        /// <summary>
        /// Resets the exclamation bubble.
        /// </summary>
        void Reset();
    }
}
