namespace Abyss.World.Phases.Concrete.Aberth
{
    public class Calm : AberthPhase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Calm"/> class.
        /// </summary>
        public Calm(int difficulty = 0, int score = 0)
            : base(
                Phase.Calm,
                "A phase where everything is calm.")
        {
        }
    }
}
