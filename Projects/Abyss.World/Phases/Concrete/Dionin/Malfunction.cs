using Abyss.World.Universe;

namespace Abyss.World.Phases.Concrete.Dionin
{
    public class Malfunction : PhaseBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Malfunction"/> class.
        /// </summary>
        public Malfunction(int difficulty, int scoreRequirement) 
            : base(
            Phase.Malfunction,
            "A phase where relics no longer function in the Abyss.", 
            difficulty,
            UniverseConstants.DioninColor,
            scoreRequirement)
        {
        }
    }
}
