using Abyss.World.Entities.Player;
using Abyss.World.Entities.Relics;
using Abyss.World.Phases;
using Abyss.World.Scoring;
using Abyss.World.Universe;
using Microsoft.Xna.Framework;
using Occasus.Core.Components.Logic;
using Occasus.Core.Drawing;
using Occasus.Core.Drawing.Animation;
using Occasus.Core.Drawing.Shaders;
using Occasus.Core.Input;
using Occasus.Core.Maths;
using Occasus.Core.Physics;
using System;
using System.Collections;
using System.Linq;

namespace Abyss.World
{
    public static class GameManager
    {
        private const string CyclePhaseKey = "CyclePhase";
        private const string UpdateRemainingGameTimeKey = "UpdateRemainingGameTime";
        private const string RemoveAmbientLightKey = "RemoveAmbientLight";
        private const string ReturnAmbientLightKey = "ReturnAmbientLight";

        private const int BaseFramerate = 60;

        private const float AmbientLightStep = 0.005f;
        
        private static int currentDepth;

        private static int seed;
        private static int elapsedFrames;
        private static bool gameBegun;

        //private static int previousPhaseDepth;
        //private static int previousPhaseRift;

        private static Dimension currentDimension;

        /// <summary>
        /// Occurs when the game has been begun.
        /// </summary>
        public static event EventHandler GameBegun;
        
        /// <summary>
        /// Occurs when the dimension shifts.
        /// </summary>
        public static event DimensionShiftedEventHandler DimensionShifted;

        /// <summary>
        /// Gets or sets the seed.
        /// </summary>
        /// <value>
        /// The seed.
        /// </value>
        public static int Seed
        {
            get
            {
                return seed;
            }

            set
            {
                seed = value;
                MathsHelper.Seed(value);
            }
        }

        /// <summary>
        /// Gets or sets the game mode.
        /// </summary>
        /// <value>
        /// The game mode.
        /// </value>
        public static GameMode GameMode
        {
            get; set; 
        }

        /// <summary>
        /// Gets or sets the current zone.
        /// </summary>
        /// <value>
        /// The current zone.
        /// </value>
        public static ZoneType CurrentZone
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the current difficulty.
        /// </summary>
        /// <value>
        /// The current difficulty.
        /// </value>
        public static int CurrentDifficulty
        {
            get; set;
        }
        
        /// <summary>
        /// Gets or sets the current depth.
        /// </summary>
        /// <value>
        /// The current depth.
        /// </value>
        public static int CurrentDepth
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
                if (gameBegun && !PhaseManager.IsAberthPhase)
                {
                    var depthDifference = value - currentDepth;
                    PhaseManager.CurrentPhases.Last().CurrentProgress += depthDifference;
                }

                currentDepth = value;
            }
        }

        /// <summary>
        /// Gets the game's current view port.
        /// </summary>
        public static Rectangle GameViewPort
        {
            get; private set;
        }

        /// <summary>
        /// Gets the game's current view port transformed to universe coordinates.
        /// </summary>
        public static Rectangle UniverseGameViewPort
        {
            get; private set;
        }

        /// <summary>
        /// Gets or sets the player.
        /// </summary>
        /// <value>
        /// The player.
        /// </value>
        public static IPlayer Player
        {
            get; set;
        }
        
        /// <summary>
        /// Gets or sets the current dimension.
        /// </summary>
        /// <value>
        /// The current dimension.
        /// </value>
        public static Dimension CurrentDimension
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
        public static IRelicCollection RelicCollection
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the statistic manager.
        /// </summary>
        /// <value>
        /// The statistic manager.
        /// </value>
        public static IStatisticManager StatisticManager
        {
            get; set;
        }

        /// <summary>
        /// Lightses the out.
        /// </summary>
        public static void LightsOut()
        {
            CoroutineManager.Add(RemoveAmbientLightKey, RemoveAmbientLight());
        }

        /// <summary>
        /// Lightses the on.
        /// </summary>
        public static void LightsOn()
        {
            CoroutineManager.Add(ReturnAmbientLightKey, ReturnAmbientLight());
        }

        /// <summary>
        /// Initializes the game.
        /// </summary>
        public static void Initialize()
        {
            if (StatisticManager == null)
            {
                StatisticManager = new StatisticManager();
            }
            else
            {
                StatisticManager.Reset();
            }

            ShaderManager.CurrentShader = null;

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

        public static void Reset()
        {
            PhaseManager.Reset();
        }

        /// <summary>
        /// Begins the game.
        /// </summary>
        public static void Begin()
        {
            if (!gameBegun)
            {
                gameBegun = true;
                CoroutineManager.Add(BeginGameEffect());
            }
        }

        /// <summary>
        /// Ends the game.
        /// </summary>
        public static void End()
        {
            // Remove coroutines if they exist.
            gameBegun = false;
            CoroutineManager.Remove(UpdateRemainingGameTimeKey);

            // Remove phases.
            PhaseManager.Reset();
            CoroutineManager.Remove(CyclePhaseKey);
        }

        /// <summary>
        /// Updates the specified game time.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        /// <param name="inputState">State of the input.</param>
        public static void Update(GameTime gameTime, IInputState inputState)
        {
            // Update the CurrentGridDepth with the Player's position.
            var currentGridDepth = Player.Transform.GridPosition.Y;

            // Determine the current drawable area of the game.
            var highVerticalBoundary = currentGridDepth - PhysicsManager.MapVerticalCenter;
            if (highVerticalBoundary < 0)
            {
                highVerticalBoundary = 0;
            }

            GameViewPort = new Rectangle(0, highVerticalBoundary, PhysicsManager.MapWidth, PhysicsManager.MapHeight);
            UniverseGameViewPort = new Rectangle(0, GameViewPort.Y * DrawingManager.TileHeight, PhysicsManager.UniverseMapWidth, PhysicsManager.UniverseMapHeight);
        }

        private static IEnumerator UpdateGameTimer()
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

        private static IEnumerator RemoveAmbientLight()
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

        private static IEnumerator ReturnAmbientLight()
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

        private static IEnumerator AmbientLightPulse()
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

        private static IEnumerator BeginGameEffect()
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

        private static void OnGameBegun(EventArgs args)
        {
            var handler = GameBegun;
            if (handler != null)
            {
                handler(null, args);
            }
        }

        private static void OnDimensionShifted(DimensionShiftedEventArgs args)
        {
            var handler = DimensionShifted;
            if (handler != null)
            {
                handler(args);
            }
        }
    }
}
