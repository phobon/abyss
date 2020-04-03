using Abyss.World.Universe;

namespace Abyss.World.Phases.Concrete.Dionin
{
    public class Haywire : PhaseBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Haywire"/> class.
        /// </summary>
        public Haywire(int difficulty, int scoreRequirement) 
            : base(
            Phase.CursedRelics,
            "A phase where relics and powerups are not what they seem.",
            difficulty,
            UniverseConstants.DioninColor,
            scoreRequirement)
        {
        }
    }
}
