using Abyss.World.Universe;

namespace Abyss.World.Phases.Concrete.Valus
{
    public class Marooned : PhaseBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Marooned"/> class.
        /// </summary>
        public Marooned(int difficulty, int scoreRequirement) 
            : base(
            Phase.Marooned,
            "A phase where entities can no longer shift between dimensions in the Abyss.", 
            difficulty,
            UniverseConstants.ValusColor,
            scoreRequirement)
        {
        }
    }
}
