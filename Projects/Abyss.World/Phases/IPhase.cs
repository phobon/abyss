using System;
using Abyss.World.Scoring;
using Microsoft.Xna.Framework;
using Occasus.Core.Layers;

namespace Abyss.World.Phases
{
    public interface IPhase
    {
        /// <summary>
        /// Occurs when the phase completes.
        /// </summary>
        event EventHandler PhaseCompleted;

        /// <summary>
        /// Gets the name of this phase.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the description of this phase.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is active; otherwise, <c>false</c>.
        /// </value>
        bool IsActive { get; set; }

        /// <summary>
        /// Gets the lighting colour.
        /// </summary>
        Color LightingColour { get; }

        /// <summary>
        /// Gets the percent complete.
        /// </summary>
        float PercentComplete { get; }

        /// <summary>
        /// Gets the required progress to complete this phase.
        /// </summary>
        int RequiredProgress { get; }

        /// <summary>
        /// Gets or sets the remaining progress required to complete this phase.
        /// </summary>
        /// <value>
        /// The remaining score.
        /// </value>
        int CurrentProgress { get; set; }

        /// <summary>
        /// Gets the difficulty.
        /// </summary>
        int Difficulty { get; }

        /// <summary>
        /// Applies the effects of this phase.
        /// </summary>
        /// <param name="targetLayer">The target layer.</param>
        void Apply(ILayer targetLayer);

        /// <summary>
        /// Removes the effects of this phase.
        /// </summary>
        /// <param name="targetLayer">The target layer.</param>
        void Remove(ILayer targetLayer);

        /// <summary>
        /// Gets the score.
        /// </summary>
        /// <param name="depth">The depth.</param>
        /// <param name="rift">The rift.</param>
        /// <returns>Score for this phase.</returns>
        IPhaseScore GetScore(int depth, int rift);
    }
}
