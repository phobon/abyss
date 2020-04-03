using System.Collections;

namespace Occasus.Core.States
{
    public interface IState
    {
        /// <summary>
        /// Occurs when the state starts.
        /// </summary>
        event StateEventHandler Started;

        /// <summary>
        /// Occurs when the state completes.
        /// </summary>
        event StateEventHandler Completed;

        /// <summary>
        /// Gets the name of this state.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the qualified name of this state.
        /// </summary>
        string QualifiedName { get; }

        /// <summary>
        /// Gets the coroutine.
        /// </summary>
        IEnumerable Coroutine { get; }

        /// <summary>
        /// Gets a value indicating whether this state must complete before the next state is executed.
        /// </summary>
        /// <value>
        ///   <c>True</c> if the state must complete; otherwise, <c>false</c>.
        /// </value>
        bool MustComplete { get; }

        /// <summary>
        /// Gets a value indicating whether this state has completed.
        /// </summary>
        /// <value>
        /// <c>True</c> if this state has completed; otherwise, <c>false</c>.
        /// </value>
        bool IsComplete { get; }

        /// <summary>
        /// Starts this start.
        /// </summary>
        void Start();

        /// <summary>
        /// Completes this state.
        /// </summary>
        void Complete();
    }
}
