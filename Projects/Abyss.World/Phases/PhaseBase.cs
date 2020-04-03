using Abyss.World.Scoring;
using Microsoft.Xna.Framework;
using Occasus.Core.Drawing;
using Occasus.Core.Drawing.Shaders;
using Occasus.Core.Layers;
using System;
using Occasus.Core;

namespace Abyss.World.Phases
{
    public abstract class PhaseBase : IPhase
    {
#if DEBUG
        private const string PhaseDebugKey = "Phase(s)";
#endif

        private int currentScore;

        /// <summary>
        /// Initializes a new instance of the <see cref="PhaseBase" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        /// <param name="difficulty">The difficulty.</param>
        /// <param name="lightingColor">Color of the lighting.</param>
        /// <param name="scoreRequirement">The score requirement.</param>
        protected PhaseBase(
            string name, 
            string description, 
            int difficulty,
            Color lightingColor,
            int scoreRequirement = 0)
        {
            this.Name = name;
            this.Description = description;

            this.LightingColour = lightingColor;

            this.Difficulty = difficulty;

            // Score requirements.
            this.RequiredProgress = scoreRequirement;
            this.CurrentProgress = 0;

            // Initialize the percent complete. This should be set by phases that need to.
            this.PercentComplete = 1f;

            this.IsActive = true;
        }

        /// <summary>
        /// Occurs when the phase completes.
        /// </summary>
        public event EventHandler PhaseCompleted;

        /// <summary>
        /// Gets the name of this phase.
        /// </summary>
        public string Name
        {
            get; private set;
        }

        /// <summary>
        /// Gets the description of this phase.
        /// </summary>
        public string Description
        {
            get; private set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is active; otherwise, <c>false</c>.
        /// </value>
        public bool IsActive
        {
            get; set;
        }

        /// <summary>
        /// Gets the lighting colour.
        /// </summary>
        public Color LightingColour
        {
            get; private set;
        }

        /// <summary>
        /// Gets the percent complete.
        /// </summary>
        public float PercentComplete 
        { 
            get; protected set;
        }

        /// <summary>
        /// Gets the required score to complete this phase.
        /// </summary>
        public int RequiredProgress
        {
            get; private set;
        }

        /// <summary>
        /// Gets or sets the remaining score required to complete this phase.
        /// </summary>
        /// <value>
        /// The remaining score.
        /// </value>
        public virtual int CurrentProgress
        {
            get
            {
                return this.currentScore;
            }

            set
            {
                if (this.currentScore == value)
                {
                    return;
                }

                this.currentScore = value;
                this.PercentComplete = 1 - ((float)this.currentScore / (float)this.RequiredProgress);

                // If the remaining score is less than or equal to zero, the phase is completed.
                if (this.currentScore >= this.RequiredProgress)
                {
                    this.OnPhaseCompleted();
                }
            }
        }

        /// <summary>
        /// Gets the difficulty.
        /// </summary>
        public int Difficulty
        {
            get; private set;
        }

        /// <summary>
        /// Applies the effects of this phase.
        /// </summary>
        /// <param name="layer">The layer.</param>
        public virtual void Apply(ILayer layer)
        {
            DrawingManager.AmbientLightColor = this.LightingColour;

#if DEBUG
            Engine.Debugger.Add(PhaseDebugKey, this.Name);
#endif
            // Remove the CurrentShader.
            ShaderManager.CurrentShader = null;
        }

        /// <summary>
        /// Removes the effects of this phase.
        /// </summary>
        /// <param name="layer">The layer.</param>
        public virtual void Remove(ILayer layer)
        {
#if DEBUG
            Engine.Debugger.Remove(PhaseDebugKey);
#endif
        }

        /// <summary>
        /// Gets the score.
        /// </summary>
        /// <param name="depth">The depth.</param>
        /// <param name="rift">The rift.</param>
        /// <returns>
        /// Score for this phase.
        /// </returns>
        public virtual IPhaseScore GetScore(int depth, int rift)
        {
            return new PhaseScore(this.Name, PhaseRank.SS, 0, depth, rift);

            // Execution score is a combination of depth travelled and rift collected. This is then multiplied by the difficulty of the phase
            // to determine a final score. 
            //var maximumDepth = (this.Duration * PhaseConstants.MaximumDepthPerSecond) * PhaseConstants.DepthTravelledCoefficient;
            //var depthScore = (int)((depth / maximumDepth) * 100);
            //depthScore = Math.Min(depthScore, 100);

            //var riftScore = 100;
            //if (this.MaximumPotentialRift > 0)
            //{
            //    riftScore = (rift / this.MaximumPotentialRift) * 100;
            //    riftScore = Math.Min(riftScore, 100);
            //}

            //var totalScore = depthScore + riftScore;
            //var rank = PhaseRank.F;
            //if (totalScore == 0)
            //{
            //}
            //else if (totalScore < 40)
            //{
            //    rank = PhaseRank.C;
            //}
            //else if (totalScore < 80)
            //{
            //    rank = PhaseRank.B;
            //}
            //else if (totalScore < 120)
            //{
            //    rank = PhaseRank.A;
            //}
            //else if (totalScore < 160)
            //{
            //    rank = PhaseRank.S;
            //}
            //else
            //{
            //    rank = PhaseRank.SS;
            //}

            //totalScore *= this.Difficulty;

            //return new PhaseScore(this.Name, rank, totalScore, depth, rift);
        }

        protected void OnPhaseCompleted()
        {
            var handler = this.PhaseCompleted;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }
    }
}
