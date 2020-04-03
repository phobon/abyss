using System;
using System.Collections;
using System.Collections.Generic;
using Occasus.Core.Components.Logic;
using Occasus.Core.Drawing.Sprites;

namespace Occasus.Core.States
{
    public class State : IState
    {
        private readonly StateEventArgs stateArgs;

        /// <summary>
        /// Initializes a new instance of the <see cref="State" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="coroutine">The coroutine.</param>
        /// <param name="mustComplete">if set to <c>true</c> [must complete].</param>
        public State(string name, IEnumerable coroutine, bool mustComplete)
        {
            this.Name = name;
            this.QualifiedName = name + "_" + Guid.NewGuid();
            this.Coroutine = coroutine;
            this.MustComplete = mustComplete;

            this.stateArgs = new StateEventArgs(name);
        }

        /// <summary>
        /// Occurs when the state starts.
        /// </summary>
        public event StateEventHandler Started;

        /// <summary>
        /// Occurs when the state completes.
        /// </summary>
        public event StateEventHandler Completed;

        /// <summary>
        /// Generates a generic state with the supplied name and sprite.
        /// </summary>
        /// <param name="stateName">Name of the state.</param>
        /// <param name="sprite">The sprite.</param>
        /// <returns>A generic state.</returns>
        public static IState GenericState(string stateName, ISprite sprite)
        {
            return new State(stateName, GenericStateCoroutine(sprite, stateName), false);
        }

        /// <summary>
        /// Generates a generic state with the supplied name and sprite.
        /// </summary>
        /// <param name="stateName">Name of the state.</param>
        /// <param name="sprites">The sprites.</param>
        /// <returns>A generic state.</returns>
        public static IState GenericState(string stateName, IEnumerable<ISprite> sprites)
        {
            return new State(stateName, GenericStateCoroutine(sprites, stateName), false);
        }

        /// <summary>
        /// Gets the name of this state.
        /// </summary>
        public string Name
        {
            get; private set;
        }

        /// <summary>
        /// Gets the qualified name of this state.
        /// </summary>
        public string QualifiedName
        {
            get; private set;
        }

        /// <summary>
        /// Gets the coroutine for this state.
        /// </summary>
        public IEnumerable Coroutine
        {
            get; private set;
        }

        /// <summary>
        /// Gets a value indicating whether this state must complete before the next state is executed.
        /// </summary>
        /// <value>
        ///   <c>True</c> if the state must complete; otherwise, <c>false</c>.
        /// </value>
        public bool MustComplete
        {
            get; private set;
        }

        /// <summary>
        /// Gets a value indicating whether this state has completed.
        /// </summary>
        /// <value>
        /// <c>True</c> if this state has completed; otherwise, <c>false</c>.
        /// </value>
        public bool IsComplete
        {
            get; private set;
        }

        /// <summary>
        /// Starts this state.
        /// </summary>
        public void Start()
        {
            CoroutineManager.Add(this.QualifiedName, this.StateStarted().GetEnumerator());
            this.IsComplete = false;
            this.OnStarted();
        }

        /// <summary>
        /// Completes this state.
        /// </summary>
        public void Complete()
        {
            CoroutineManager.Remove(this.QualifiedName);
            this.IsComplete = true;
            this.OnCompleted();
        }

        private static IEnumerable GenericStateCoroutine(ISprite sprite, string animationKey)
        {
            sprite.GoToAnimation(animationKey);
            yield return null;
        }

        private static IEnumerable GenericStateCoroutine(IEnumerable<ISprite> sprites, string animationKey)
        {
            foreach (var s in sprites)
            {
                s.GoToAnimation(animationKey);
            }

            yield return null;
        }

        private IEnumerable StateStarted()
        {
            yield return this.Coroutine.GetEnumerator();
            this.Complete();
        }

        private void OnStarted()
        {
            var handler = this.Started;
            if (handler != null)
            {
                handler(this.stateArgs);
            }
        }

        private void OnCompleted()
        {
            var handler = this.Completed;
            if (handler != null)
            {
                handler(this.stateArgs);
            }
        }
    }
}
