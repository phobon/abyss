using Abyss.World.Universe;

namespace Abyss.World.Phases.Concrete.Argus
{
    public class LimboWalking : PhaseBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LimboWalking"/> class.
        /// </summary>
        public LimboWalking(int difficulty, int scoreRequirement) 
            : base(
            Phase.ShortSighted,
            "A phase where the player is no longer safe from extra-dimensional monsters in Limbo.", 
            difficulty,
            UniverseConstants.ArgusColor,
            scoreRequirement)
        {
        }
    }
}
