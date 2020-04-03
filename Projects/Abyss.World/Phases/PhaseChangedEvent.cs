using System;
using System.Collections.Generic;

namespace Abyss.World.Phases
{
    /// <summary>
    /// Custom event handler for when the current Phase changes.
    /// </summary>
    /// <param name="e">The <see cref="PhaseChangedEventArgs"/> instance containing the event data.</param>
    public delegate void PhaseChangedEventHandler(PhaseChangedEventArgs e);

    public class PhaseChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PhaseChangedEventArgs" /> class.
        /// </summary>
        /// <param name="newPhase">The new phase.</param>
        /// <param name="previousPhases">The previous phase.</param>
        public PhaseChangedEventArgs(IPhase newPhase, IEnumerable<IPhase> previousPhases = null)
        {
            this.NewPhase = newPhase;
            this.PreviousPhases = previousPhases;
        }

        /// <summary>
        /// Gets the new phases.
        /// </summary>
        public IPhase NewPhase
        {
            get; private set;
        }

        /// <summary>
        /// Gets the old phases if applicable.
        /// </summary>
        public IEnumerable<IPhase> PreviousPhases
        {
            get; private set;
        }
    }
}
