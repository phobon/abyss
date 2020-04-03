using System.Collections.Generic;

namespace Abyss.World.Scoring
{
    public interface IStatisticManager
    {
        /// <summary>
        /// Gets or sets the game over message.
        /// </summary>
        /// <value>
        /// The game over message.
        /// </value>
        string GameOverMessage { get; set; }

        /// <summary>
        /// Gets the statistics.
        /// </summary>
        IDictionary<string, int> Statistics { get; }

        /// <summary>
        /// Gets or sets the time survived by the player.
        /// </summary>
        /// <value>
        /// The time survived.
        /// </value>
        int TimeSurvived { get; set; }

        /// <summary>
        /// Gets or sets the amount of rift collected by the player.
        /// </summary>
        /// <value>
        /// The rift collected.
        /// </value>
        int RiftCollected { get; set; }

        /// <summary>
        /// Gets or sets the number of times dimensions shifted were shifted by the player.
        /// </summary>
        /// <value>
        /// The dimensions shifted.
        /// </value>
        int DimensionsShifted { get; set; }

        /// <summary>
        /// Gets the relics the player has collected.
        /// </summary>
        IList<string> RelicsCollected { get; }

        /// <summary>
        /// Gets the total relics the player has colelcted.
        /// </summary>
        int TotalRelicsCollected { get; }

        /// <summary>
        /// Gets the deaths the player has had.
        /// </summary>
        IList<string> Deaths { get; }

        /// <summary>
        /// Gets the total deaths the player has had.
        /// </summary>
        int TotalDeaths { get; }

        /// <summary>
        /// Gets the phase scores.
        /// </summary>
        IEnumerable<IPhaseScore> PhaseScores { get; }

        /// <summary>
        /// Gets the total score.
        /// </summary>
        int TotalScore { get; set; }

        /// <summary>
        /// Resets the statistics.
        /// </summary>
        void Reset();

        /// <summary>
        /// Adds the score.
        /// </summary>
        /// <param name="score">The score.</param>
        void AddScore(IPhaseScore score);
    }
}
