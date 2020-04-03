using Abyss.World.Scoring;
using Abyss.World.Universe;
using Occasus.Core.Components.Logic;
using Occasus.Core.Drawing.Animation;
using Occasus.Core.Layers;
using Occasus.Core.Maths;
using System.Collections;

namespace Abyss.World.Phases.Concrete.Aberth
{
    public abstract class AberthPhase : PhaseBase
    {
        private const string CyclePhaseKey = "CyclePhaseKey";

        /// <summary>
        /// Initializes a new instance of the <see cref="AberthPhase"/> class.
        /// </summary>
        /// <param name="phase">The phase.</param>
        /// <param name="description">The description.</param>
        protected AberthPhase(string phase, string description)
            : base(
            phase,
            description,
            0,
            UniverseConstants.AmbientColor,
            0)
        {
            this.PercentComplete = 0f;
        }

        /// <summary>
        /// Gets or sets the remaining score required to complete this phase.
        /// </summary>
        /// <value>
        /// The remaining score.
        /// </value>
        public override int CurrentProgress
        {
            get; set;
        }

        /// <summary>
        /// Applies the effects of this phase.
        /// </summary>
        /// <param name="targetLayer">The target layer.</param>
        public override void Apply(ILayer targetLayer)
        {
            base.Apply(targetLayer);

            // Add the coroutine that handles 
            CoroutineManager.Add(CyclePhaseKey, this.CyclePhase());
        }

        /// <summary>
        /// Removes the effects of this phase.
        /// </summary>
        /// <param name="layer">The layer.</param>
        public override void Remove(ILayer layer)
        {
            base.Remove(layer);

            CoroutineManager.Remove(CyclePhaseKey);
        }

        /// <summary>
        /// Gets the score.
        /// </summary>
        /// <param name="depth">The depth.</param>
        /// <param name="rift">The rift.</param>
        /// <returns>
        /// Score for this phase.
        /// </returns>
        public override IPhaseScore GetScore(int depth, int rift)
        {
            return new PhaseScore(this.Name, PhaseRank.SS, 0, depth, rift);
        }

        private IEnumerator CyclePhase()
        {
            // Determine length of phase.
            var phaseLength = MathsHelper.Random(PhaseManager.AberthPhaseLength, PhaseManager.AberthPhaseLength + 5);

            var totalFrames = TimingHelper.GetFrameCount(phaseLength);
            var currentFrame = 0;

            // Loop through this so that we get a correct indication of how long we have to go in this phase.
            while (currentFrame < totalFrames)
            {
                this.PercentComplete = (float)currentFrame / (float)totalFrames;
                currentFrame++;
                yield return null;
            }

            // Phase is completed.
            this.OnPhaseCompleted();
        }
    }
}
