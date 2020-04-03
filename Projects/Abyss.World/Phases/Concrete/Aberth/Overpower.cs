namespace Abyss.World.Phases.Concrete.Aberth
{
    public class Overpower : AberthPhase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Overpower"/> class.
        /// </summary>
        public Overpower(int difficulty = 0, int score = 0)
            : base(
                Phase.Overpower,
                "Relics become rich with rift energy, allowing their effects to become more potent.")
        {
        }
    }
}
