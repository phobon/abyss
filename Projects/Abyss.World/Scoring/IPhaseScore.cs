namespace Abyss.World.Scoring
{
    public interface IPhaseScore
    {
        /// <summary>
        /// Gets the phase name.
        /// </summary>
        string PhaseName { get; }

        /// <summary>
        /// Gets the rank.
        /// </summary>
        PhaseRank Rank { get; }

        /// <summary>
        /// Gets the score.
        /// </summary>
        int Score { get; }

        /// <summary>
        /// Gets the depth travelled.
        /// </summary>
        int DepthTravelled { get; }

        /// <summary>
        /// Gets the rift collected.
        /// </summary>
        int RiftCollected { get; }
    }
}
