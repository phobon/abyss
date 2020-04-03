using Abyss.World.Universe;

namespace Abyss.World.Phases.Concrete.Valus
{
    public class Volatile : PhaseBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Volatile"/> class.
        /// </summary>
        public Volatile(int difficulty, int scoreRequirement) 
            : base(
            Phase.Marooned,
            "A phase where extra-dimensional monsters become too volatile to touch, damaging the player if they try to stomp them.", 
            difficulty,
            UniverseConstants.ValusColor,
            scoreRequirement)
        {
        }
    }
}
