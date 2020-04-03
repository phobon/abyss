using System;

namespace Occasus.Core.States
{
    /// <summary>
    /// Event handler for a state event.
    /// </summary>
    /// <param name="e">The <see cref="StateEventArgs"/> instance containing the event data.</param>
    public delegate void StateEventHandler(StateEventArgs e);

    /// <summary>
    /// Event args for a StateChanged event.
    /// </summary>
    public class StateEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StateEventArgs" /> class.
        /// </summary>
        /// <param name="stateName">The state name.</param>
        public StateEventArgs(string stateName)
        {
            this.StateName = stateName;
        }

        /// <summary>
        /// Gets the state name.
        /// </summary>
        public string StateName
        {
            get; private set;
        }
    }
}
