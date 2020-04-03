using Occasus.Core.Entities;

namespace Abyss.World.Entities.Player.Components
{
    public interface IBarrier : IEntity
    {
        /// <summary>
        /// Gets the charges.
        /// </summary>
        int Charges { get; }

        /// <summary>
        /// Generates the barrier.
        /// </summary>
        void Generate(int charges);

        /// <summary>
        /// Breaks the barrier.
        /// </summary>
        void Break();

        /// <summary>
        /// Resets the barrier.
        /// </summary>
        void Reset();
    }
}
