using System;
using System.Collections.Generic;
using System.Linq;

namespace Occasus.Core.States
{
    public abstract class StateMachine : EngineComponent, IStateMachine
    {
        private string currentState;
        private string fallbackState;
        private string queuedState;

        /// <summary>
        /// Initializes a new instance of the <see cref="StateMachine"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        protected StateMachine(string name, string description)
            : base(name, description)
        {
            this.States = new Dictionary<string, IState>();
        }

        /// <summary>
        /// Occurs when the state changes.
        /// </summary>
        public event StateChangedEventHandler StateChanged;

        /// <summary>
        /// Gets or sets the currently active state.
        /// </summary>
        /// <value>
        /// The state of the current.
        /// </value>
        public string CurrentState
        {
            get
            {
                return this.currentState;
            }

            set
            {
                if (!string.IsNullOrEmpty(this.currentState) && this.currentState.Equals(value, StringComparison.InvariantCultureIgnoreCase))
                {
                    return;
                }

                // Check whether the current state must complete before executing the next. This way we can queue the next state.
                var oldStateName = this.currentState;
                if (!string.IsNullOrEmpty(oldStateName) && this.States.Count > 0 && this.States.ContainsKey(oldStateName))
                {
                    var oldState = this.States[oldStateName];
                    if (oldState.MustComplete)
                    {
                        if (!oldState.IsComplete)
                        {
                            // Queue the next state instead of changing this state.
                            this.QueuedState = value;
                            return;
                        }

                        // Unsubscribe from the completed event.
                        oldState.Completed -= this.StateOnCompleted;
                    }
                    else
                    {
                        oldState.Complete();
                    }
                }

                this.currentState = value;

                // If there are no states, it means this state machine hasn't been initialized. This will happen at a later date.
                if (this.States.Count == 0)
                {
                    return;
                }
                
                // If the new state must be completed, hook up an event to ensure that this is handled.
                var newState = this.States[this.currentState];
                if (newState.MustComplete)
                {
                    newState.Completed += this.StateOnCompleted;
                }

                newState.Start();

                // Fire off a state changed event.
                this.OnStateChanged(new StateChangedEventEventArgs(oldStateName, this.currentState));
            }
        }

        /// <summary>
        /// Gets or sets the queued state.
        /// </summary>
        /// <value>
        /// The queued state.
        /// </value>
        public string QueuedState
        {
            get
            {
                return this.queuedState;
            }

            set
            {
                // If the queued state is the same as the value, fall back.
                if (!string.IsNullOrEmpty(this.queuedState) && this.queuedState.Equals(value, StringComparison.InvariantCultureIgnoreCase))
                {
                    return;
                }

                // If there is a queued state, fall back.
                if (!string.IsNullOrEmpty(this.queuedState))
                {
                    return;
                }

                this.queuedState = value;
            }
        }

        /// <summary>
        /// Gets or sets the fallback state if there is nothing queued
        /// </summary>
        public string FallbackState
        {
            get
            {
                if (string.IsNullOrEmpty(this.fallbackState) && this.States.Count > 0)
                {
                    this.fallbackState = this.States.Keys.First();
                }

                return this.fallbackState;
            }

            set
            {
                if (this.fallbackState.Equals(value, StringComparison.InvariantCultureIgnoreCase))
                {
                    return;
                }

                this.fallbackState = value;
            }
        }

        /// <summary>
        /// Gets the states in this state machine.
        /// </summary>
        public IDictionary<string, IState> States
        {
            get; private set;
        }

        /// <summary>
        /// Performs any initialization logic required by this engine component.
        /// </summary>
        public override void Initialize()
        {
            this.SetupStates();
            base.Initialize();
        }

        /// <summary>
        /// Sets up all unique states for this machine.
        /// </summary>
        protected virtual void SetupStates()
        {
            // If we have a state name from before states are set up, go directly to this.
            if (!string.IsNullOrEmpty(this.CurrentState) && this.States.ContainsKey(this.CurrentState))
            {
                this.States[this.CurrentState].Start();
            }
            else
            {
                if (this.States.Count > 0)
                {
                    this.CurrentState = this.States.First().Key;
                }
            }
        }

        private void OnStateChanged(StateChangedEventEventArgs args)
        {
            var handler = this.StateChanged;
            if (handler != null)
            {
                handler(args);
            }
        }

        private void StateOnCompleted(StateEventArgs stateEventArgs)
        {
            // If there is a queued state, execute that; if not, go to a fallback state.
            this.CurrentState = string.IsNullOrEmpty(this.QueuedState) ? this.FallbackState : this.QueuedState;
            this.QueuedState = string.Empty;
        }
    }
}
