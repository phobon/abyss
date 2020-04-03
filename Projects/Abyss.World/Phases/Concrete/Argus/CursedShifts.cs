using Abyss.World.Entities.Player;
using Abyss.World.Universe;
using Occasus.Core.Layers;

namespace Abyss.World.Phases.Concrete.Argus
{
    public class CursedShifts : PhaseBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CursedShifts"/> class.
        /// </summary>
        public CursedShifts(int difficulty, int scoreRequirement) 
            : base(
            Phase.CursedShifts,
            "A phase where the player is hurt every time they shift into Limbo.",
            difficulty,
            UniverseConstants.ArgusColor,
            scoreRequirement)
        {
        }

        /// <summary>
        /// Applies the effects of this phase.
        /// </summary>
        /// <param name="layer">The layer.</param>
        public override void Apply(ILayer layer)
        {
            GameManager.DimensionShifted += GameManagerOnDimensionShifted;
            base.Apply(layer);
        }

        /// <summary>
        /// Removes the effects of this phase.
        /// </summary>
        /// <param name="layer">The layer.</param>
        public override void Remove(ILayer layer)
        {
            GameManager.DimensionShifted -= GameManagerOnDimensionShifted;
            base.Remove(layer);
        }

        private void GameManagerOnDimensionShifted(DimensionShiftedEventArgs dimensionShiftedEventArgs)
        {
            GameManager.Player.TakeDamage(this.Name);
        }
    }
}
