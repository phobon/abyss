namespace Abyss.World.Scoring
{
    public class PhaseScore : IPhaseScore
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PhaseScore"/> class.
        /// </summary>
        /// <param name="phaseName">Name of the phase.</param>
        /// <param name="rank">The rank.</param>
        /// <param name="score">The score.</param>
        /// <param name="depthTravelled">The depth travelled.</param>
        /// <param name="riftCollected">The rift collected.</param>
        public PhaseScore(string phaseName, PhaseRank rank, int score, int depthTravelled, int riftCollected)
        {
            this.PhaseName = phaseName;
            this.Rank = rank;
            this.Score = score;
            this.DepthTravelled = depthTravelled;
            this.RiftCollected = riftCollected;
        }

        /// <summary>
        /// Gets the phase name.
        /// </summary>
        public string PhaseName
        {
            get; private set;
        }

        /// <summary>
        /// Gets the rank.
        /// </summary>
        public PhaseRank Rank
        {
            get; private set;
        }

        /// <summary>
        /// Gets the score.
        /// </summary>
        public int Score
        {
            get; private set;
        }

        /// <summary>
        /// Gets the depth travelled.
        /// </summary>
        public int DepthTravelled
        {
            get; private set;
        }

        /// <summary>
        /// Gets the rift collected.
        /// </summary>
        public int RiftCollected
        {
            get; private set;
        }
    }
}
