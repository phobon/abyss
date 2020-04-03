using System;
using System.Collections.Generic;
using System.Linq;

namespace Abyss.World.Scoring
{
    public class StatisticManager : IStatisticManager
    {
        public const string TimeSurvivedKey = "Time Survived";
        public const string RiftCollectedKey = "Rift Collected";
        public const string DimensionsShiftedKey = "Dimensions Shifted";

        private readonly IList<IPhaseScore> phaseScores;
        private int totalScore;

        /// <summary>
        /// Initializes a new instance of the <see cref="StatisticManager"/> class.
        /// </summary>
        public StatisticManager()
        {
            this.Statistics = new Dictionary<string, int>
                                {
                                    { TimeSurvivedKey, 0 },
                                    { RiftCollectedKey, 0 },
                                    { DimensionsShiftedKey, 0 }
                                };
            this.Deaths = new List<string>();
            this.RelicsCollected = new List<string>();
            this.phaseScores = new List<IPhaseScore>();
        }

        /// <summary>
        /// Gets or sets the game over message.
        /// </summary>
        /// <value>
        /// The game over message.
        /// </value>
        public string GameOverMessage
        {
            get; set;
        }

        /// <summary>
        /// Gets the statistics.
        /// </summary>
        public IDictionary<string, int> Statistics
        {
            get; private set;
        }

        /// <summary>
        /// Gets or sets the time survived by the player.
        /// </summary>
        /// <value>
        /// The time survived.
        /// </value>
        public int TimeSurvived
        {
            get
            {
                return this.Statistics[TimeSurvivedKey];
            }

            set
            {
                this.Statistics[TimeSurvivedKey] = value;
            }
        }

        /// <summary>
        /// Gets or sets the amount of rift collected by the player.
        /// </summary>
        /// <value>
        /// The rift collected.
        /// </value>
        public int RiftCollected
        {
            get
            {
                return this.Statistics[RiftCollectedKey];
            }

            set
            {
                this.Statistics[RiftCollectedKey] = value;
            }
        }

        /// <summary>
        /// Gets or sets the number of times dimensions shifted were shifted by the player.
        /// </summary>
        /// <value>
        /// The dimensions shifted.
        /// </value>
        public int DimensionsShifted
        {
            get
            {
                return this.Statistics[DimensionsShiftedKey];
            }

            set
            {
                this.Statistics[DimensionsShiftedKey] = value;
            }
        }

        /// <summary>
        /// Gets the relics the player has collected.
        /// </summary>
        public IList<string> RelicsCollected
        {
            get; private set;
        }

        /// <summary>
        /// Gets the total relics the player has colelcted.
        /// </summary>
        public int TotalRelicsCollected
        {
            get
            {
                return this.RelicsCollected.Count;
            }
        }

        /// <summary>
        /// Gets the deaths the player has had.
        /// </summary>
        public IList<string> Deaths
        {
            get; private set;
        }

        /// <summary>
        /// Gets the total deaths the player has had.
        /// </summary>
        public int TotalDeaths
        {
            get
            {
                return this.Deaths.Count;
            }
        }

        /// <summary>
        /// Gets the phase scores.
        /// </summary>
        public IEnumerable<IPhaseScore> PhaseScores
        {
            get
            {
                return this.phaseScores;
            }
        }
        
        /// <summary>
        /// Gets or sets the total score.
        /// </summary>
        public int TotalScore
        {
            get
            {
                return this.totalScore;
            }

            set
            {
                if (this.totalScore == value)
                {
                    return;
                }

                if (value < 0)
                {
                    throw new InvalidOperationException();
                }

                //if (GameManager.CurrentPhase != null)
                //{
                //    // Determine score differential and add it to the current phase.
                //    var scoreDifference = value - this.totalScore;
                //    GameManager.CurrentPhase.CurrentProgress += scoreDifference;
                //}

                this.totalScore = value;
            }
        }

        /// <summary>
        /// Adds the score.
        /// </summary>
        /// <param name="score">The score.</param>
        public void AddScore(IPhaseScore score)
        {
            this.phaseScores.Add(score);
            this.TotalScore += score.Score;
        }

        /// <summary>
        /// Adds the score.
        /// </summary>
        /// <param name="score">The score.</param>
        public void AddScore(int score)
        {
            this.TotalScore += score;
        }

        /// <summary>
        /// Resets the statistics.
        /// </summary>
        public void Reset()
        {
            this.GameOverMessage = "You died...";

            this.Deaths.Clear();
            this.RelicsCollected.Clear();

            var keys = this.Statistics.Keys.ToList();
            foreach (var k in keys)
            {
                this.Statistics[k] = 0;
            }

            this.TotalScore = 0;
            this.phaseScores.Clear();
        }
    }
}
