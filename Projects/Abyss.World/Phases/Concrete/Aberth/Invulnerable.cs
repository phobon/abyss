namespace Abyss.World.Phases.Concrete.Aberth
{
    public class Invulnerable : AberthPhase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Invulnerable"/> class.
        /// </summary>
        public Invulnerable(int difficulty = 0, int score = 0)
            : base(
                Phase.Invulnerable,
                "Rift energy flows through the player, making them completely invulnerable to harm.")
        {
        }
    }
}
