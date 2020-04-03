using System.Collections.Generic;

namespace Occasus.Core.States
{
    public interface IStateMachine
    {
        /// <summary>
        /// Occurs when the state changes.
        /// </summary>
        event StateChangedEventHandler StateChanged;

        /// <summary>
        /// Gets or sets the currently active state.
        /// </summary>
        /// <value>
        /// The state of the current.
        /// </value>
        string CurrentState { get; set; }

        /// <summary>
        /// Gets 
        /// </summary>
        string FallbackState { get; set; }

        /// <summary>
        /// Gets the states in this state machine.
        /// </summary>
        IDictionary<string, IState> States { get; }
    }
}
