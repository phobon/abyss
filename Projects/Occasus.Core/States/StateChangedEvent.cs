using System;

namespace Occasus.Core.States
{
    /// <summary>
    /// Event handler for a StateChanged event.
    /// </summary>
    /// <param name="e">The <see cref="StateChangedEventEventArgs"/> instance containing the event data.</param>
    public delegate void StateChangedEventHandler(StateChangedEventEventArgs e);

    /// <summary>
    /// Event args for a StateChanged event.
    /// </summary>
    public class StateChangedEventEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StateChangedEventEventArgs" /> class.
        /// </summary>
        /// <param name="oldState">The old state.</param>
        /// <param name="newState">The new state.</param>
        public StateChangedEventEventArgs(string oldState, string newState)
        {
            this.OldState = oldState;
            this.NewState = newState;
        }

        /// <summary>
        /// Gets the new state.
        /// </summary>
        public string NewState
        {
            get; private set;
        }

        /// <summary>
        /// Gets the old state.
        /// </summary>
        public string OldState
        {
            get; private set;
        }
    }
}
