using System;
using System.Collections.Generic;
using System.Linq;

namespace Abyss.World.Phases
{
    public static class PhaseManager
    {
        private const int MaximumActivePhases = 5;

        private static List<IPhase> currentPhases;
        private static Dictionary<string, bool> currentPhaseState;

        private static IPhaseFactory factory;

        /// <summary>
        /// Occurs when a new interdimensional phase is encountered.
        /// </summary>
        public static event PhaseChangedEventHandler PlaneFolded;

        /// <summary>
        /// Occurs when an interdimensional phase dissipates.
        /// </summary>
        public static event PhaseChangedEventHandler PlaneCoalesced;

        public static bool IsAberthPhase
        {
            get; private set;
        }

        public static int AberthPhaseLength
        {
            get; set;
        }

        /// <summary>
        /// Gets the phases that are currently active.
        /// </summary>
        public static IEnumerable<IPhase> CurrentPhases
        {
            get
            {
                return currentPhases;
            }
        }

        /// <summary>
        /// Gets the current phase state for the game.
        /// </summary>
        public static IDictionary<string, bool> CurrentPhaseState
        {
            get
            {
                return currentPhaseState;
            }
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public static void Initialize()
        {
            factory = new PhaseFactory();
            currentPhases = new List<IPhase>();
            currentPhaseState = new Dictionary<string, bool>();

            Reset();
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        public static void LoadContent()
        {
            factory.LoadContent();
        }

        /// <summary>
        /// Resets this instance.
        /// </summary>
        public static void Reset()
        {
            currentPhases.Clear();
            AberthPhaseLength = 10;
            IsAberthPhase = true;
        }

        /// <summary>
        /// Determines whether a phase with the specified name is active or not.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>A value indicating whether the specified phase is active or not.</returns>
        public static bool IsPhaseActive(string name)
        {
            return CurrentPhaseState.ContainsKey(name) && CurrentPhaseState[name];
        }

        /// <summary>
        /// Coalesces dangerous phases into a beneficial Aberth phase.
        /// </summary>
        public static void CoalescePlane()
        {
            var score = 0;
            foreach (var p in currentPhases)
            {
                // TODO: Add the score to the statistic manager and discover the weight of the current phases.
                //StatisticManager.AddScore(phase.GetScore(CurrentDepth - previousPhaseDepth, Player.Rift - previousPhaseRift));

                //previousPhaseDepth = CurrentDepth;
                //previousPhaseRift = Player.Rift;

                // TODO: Add a 'score' to phases.
                // score += p.Score;

                // Reset the flag.
                CurrentPhaseState[p.Name] = false;
            }

            // Create a new Aberth phase from the phase score and attach an event to handle when it's finished.
            // The way I want this to work is that all of the scores (maybe not scores, but weights) of the phases that the player has survived
            // all come in here and depending on how well a player did, they get a chance of a highly beneficial Aberth phase appearing.
            var aberthPhase = factory.GenerateById(Phase.Calm);
            if (CurrentPhaseState.ContainsKey(aberthPhase.Name))
            {
                CurrentPhaseState[aberthPhase.Name] = true;
            }
            else
            {
                CurrentPhaseState.Add(aberthPhase.Name, true);
            }

            aberthPhase.PhaseCompleted += AberthPhaseOnPhaseCompleted;

            OnPlaneCoalesced(new PhaseChangedEventArgs(aberthPhase, currentPhases.ToList()));

            // Clear the collection in preparation of the new Aberth phase.
            currentPhases.Clear();
            currentPhases.Add(aberthPhase);
            IsAberthPhase = true;
        }

        /// <summary>
        /// Folds a new phase onto the current dimensional plane. This is the opposite operation to coalescing the dimensional plane.
        /// </summary>
        public static void FoldPlane(IPhase aberthPhase = null)
        {
            // Get a random difficulty phase from the phase factory and add it to the collection.
            var newPhase = factory.GenerateRandomByDifficulty(currentPhases.Count + 1);
            currentPhases.Add(newPhase);
            if (CurrentPhaseState.ContainsKey(newPhase.Name))
            {
                CurrentPhaseState[newPhase.Name] = true;
            }
            else
            {
                CurrentPhaseState.Add(newPhase.Name, true);
            }

            OnPlaneFolded(aberthPhase != null
                ? new PhaseChangedEventArgs(newPhase, new[] { aberthPhase })
                : new PhaseChangedEventArgs(newPhase));

            IsAberthPhase = false;

            // Add an event to handle when this phase completes.
            newPhase.PhaseCompleted += NewPhaseOnPhaseCompleted;
        }

        private static void AberthPhaseOnPhaseCompleted(object sender, EventArgs eventArgs)
        {
            // Detach events and all of that maintenance stuff, then fold the plane.
            var aberthPhase = (IPhase)sender;
            aberthPhase.PhaseCompleted -= AberthPhaseOnPhaseCompleted;
            CurrentPhaseState[aberthPhase.Name] = false;
            currentPhases.Clear();

            FoldPlane(aberthPhase);
        }

        private static void NewPhaseOnPhaseCompleted(object sender, EventArgs eventArgs)
        {
            var phase = (IPhase)sender;
            phase.PhaseCompleted -= NewPhaseOnPhaseCompleted;

            if (CurrentPhases.Count() >= MaximumActivePhases)
            {
                CoalescePlane();
            }
            else
            {
                FoldPlane();
            }
        }

        private static void OnPlaneFolded(PhaseChangedEventArgs args)
        {
            var handler = PlaneFolded;
            if (handler != null)
            {
                handler(args);
            }
        }

        private static void OnPlaneCoalesced(PhaseChangedEventArgs args)
        {
            var handler = PlaneCoalesced;
            if (handler != null)
            {
                handler(args);
            }
        }
    }
}
