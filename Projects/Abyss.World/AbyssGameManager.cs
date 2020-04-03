using Abyss.World.Entities.Player;
using Abyss.World.Entities.Relics;
using Abyss.World.Phases;
using Abyss.World.Scoring;
using Abyss.World.Universe;
using Occasus.Core.Components.Logic;
using Occasus.Core.Drawing;
using Occasus.Core.Drawing.Animation;
using Occasus.Core.Drawing.Shaders;
using System;
using System.Collections;
using System.Linq;
using Occasus.Core;

namespace Abyss.World
{
    public sealed class AbyssGameManager : GameManager<IPlayer>, IAbyssGameManager, IGameManager<IPlayer>
    {
        private const string CyclePhaseKey = "CyclePhase";
        private const string UpdateRemainingGameTimeKey = "UpdateRemainingGameTime";
        private const string RemoveAmbientLightKey = "RemoveAmbientLight";
        private const string ReturnAmbientLightKey = "ReturnAmbientLight";

        private const int BaseFramerate = 60;

        private const float AmbientLightStep = 0.005f;
        
        private int currentDepth;
        private int elapsedFrames;

        //private static int previousPhaseDepth;
        //private static int previousPhaseRift;

        private Dimension currentDimension;
        
        /// <summary>
        /// Occurs when the dimension shifts.
        /// </summary>
        public event DimensionShiftedEventHandler DimensionShifted;

        /// <summary>
        /// Gets or sets the game mode.
        /// </summary>
        public GameMode GameMode { get; set; }

        /// <summary>
        /// Gets or sets the current zone.
        /// </summary>
        public ZoneType CurrentZone { get; set; }

        /// <summary>
        /// Gets or sets the current difficulty.
        /// </summary>
        public int CurrentDifficulty { get; set; }
        
        /// <summary>
        /// Gets or sets the current depth.
        /// </summary>
        public int CurrentDepth
        {
            get
            {
                return currentDepth;
            }

            set
            {
                if (currentDepth == value)
                {
                    return;
                }

                // If it's not an Aberth phase, check how far the player has gone since the last check and update the depth til next fold.
                if (this.HasStarted && !PhaseManager.IsAberthPhase)
                {
                    var depthDifference = value - currentDepth;
                    PhaseManager.CurrentPhases.Last().CurrentProgress += depthDifference;
                }

                currentDepth = value;
            }
        }
        
        /// <summary>
        /// Gets or sets the current dimension.
        /// </summary>
        public Dimension CurrentDimension
        {
            get
            {
                return currentDimension;
            }

            set
            {
                currentDimension = value;
                OnDimensionShifted(new DimensionShiftedEventArgs(currentDimension));
            }
        }

        /// <summary>
        /// Gets the relics that have been collected.
        /// </summary>
        public IRelicCollection RelicCollection { get; set; }

        /// <summary>
        /// Gets or sets the statistic manager.
        /// </summary>
        public IStatisticManager StatisticManager { get; set; }

        /// <summary>
        /// Lightses the out.
        /// </summary>
        public void LightsOut()
        {
            CoroutineManager.Add(RemoveAmbientLightKey, RemoveAmbientLight());
        }

        /// <summary>
        /// Lightses the on.
        /// </summary>
        public void LightsOn()
        {
            CoroutineManager.Add(ReturnAmbientLightKey, ReturnAmbientLight());
        }

        /// <summary>
        /// Initializes the game.
        /// </summary>
        public override void Initialize()
        {
            if (StatisticManager == null)
            {
                StatisticManager = new StatisticManager();
            }
            else
            {
                StatisticManager.Reset();
            }

            ShaderManager.ClearActiveShaders();

            // Reset current dimension.
            CurrentDimension = Dimension.Normal;
            
            // Reset scoring.
            CurrentDifficulty = 1;
            //previousPhaseDepth = 0;
            //previousPhaseRift = 0;
            CurrentDepth = 0;

            DrawingManager.AmbientLightValue = 0.05f;
            DrawingManager.AmbientLightColor = UniverseConstants.AmbientColor;

            // Pulse ambient light.
            //CoroutineManager.Remove("GameManager_LightPulse");
            //CoroutineManager.Add("GameManager_LightPulse", AmbientLightPulse());
        }

        public override void Reset()
        {
            PhaseManager.Reset();
        }

        /// <summary>
        /// Begins the game.
        /// </summary>
        public override bool Begin()
        {
            var begun = base.Begin();
            if (base.Begin())
            {
                CoroutineManager.Add(BeginGameEffect());
            }

            return begun;
        }

        /// <summary>
        /// Ends the game.
        /// </summary>
        public override void End()
        {
            base.End();

            // Remove coroutines if they exist.
            CoroutineManager.Remove(UpdateRemainingGameTimeKey);

            // Remove phases.
            PhaseManager.Reset();
            CoroutineManager.Remove(CyclePhaseKey);
        }

        private IEnumerator UpdateGameTimer()
        {
            while (true)
            {
                // Remove a frame from the frames remaining.
                elapsedFrames++;

                if (elapsedFrames >= BaseFramerate)
                {
                    StatisticManager.TimeSurvived++;
                    elapsedFrames = 0;
                }

                yield return null;
            }
        }

        private IEnumerator RemoveAmbientLight()
        {
            var elapsedFrames = 0;
            var totalFrames = 60;

            var startingValue = 0.05f;

            while (elapsedFrames <= totalFrames)
            {
                DrawingManager.AmbientLightValue = startingValue - ((float)elapsedFrames / (float)totalFrames);
                elapsedFrames++;
                yield return null;
            }

            DrawingManager.AmbientLightValue = -1f;
        }

        private IEnumerator ReturnAmbientLight()
        {
            var elapsedFrames = 0;
            var totalFrames = 60;

            var startingValue = -1f;

            while (elapsedFrames <= totalFrames)
            {
                DrawingManager.AmbientLightValue = startingValue + ((float)elapsedFrames / (float)totalFrames);
                elapsedFrames++;
                yield return null;
            }

            DrawingManager.AmbientLightValue = 0.05f;
        }

        private IEnumerator AmbientLightPulse()
        {
            var step = 0.005f;
            while (true)
            {
                DrawingManager.AmbientLightValue += step;

                if (DrawingManager.AmbientLightValue >= 0.5f)
                {
                    step = -0.0005f;
                }
                else if (DrawingManager.AmbientLightValue <= 0.3f)
                {
                    step = 0.0005f;
                }

                yield return null;
            }
        }

        private IEnumerator BeginGameEffect()
        {
            if (RelicCollection == null)
            {
                RelicCollection = new RelicCollection();
            }
            else
            {
                RelicCollection.Reset();
            }

            if (StatisticManager == null)
            {
                StatisticManager = new StatisticManager();
            }
            else
            {
                StatisticManager.Reset();
            }

            // Fire a game begun event.
            OnGameBegun(EventArgs.Empty);
            
            yield return Coroutines.Pause(TimingHelper.GetFrameCount(2.5f));

            // Kick off the game with a coalsced phase. This should always be the Calm phase and we do want the player to be notified of this.
            PhaseManager.CoalescePlane();

            // Add a coroutine to handle updating remaining game time.
            CoroutineManager.Add(UpdateRemainingGameTimeKey, UpdateGameTimer());
        }

        private void OnDimensionShifted(DimensionShiftedEventArgs args)
        {
            var handler = DimensionShifted;
            if (handler != null)
            {
                handler(args);
            }
        }
    }
}
