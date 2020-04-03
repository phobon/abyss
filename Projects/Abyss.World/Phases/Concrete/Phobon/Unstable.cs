using Abyss.World.Universe;
using Occasus.Core.Drawing;
using Occasus.Core.Layers;
using Occasus.Core.Maths;

namespace Abyss.World.Phases.Concrete.Phobon
{
    public class Unstable : PhaseBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Unstable"/> class.
        /// </summary>
        public Unstable(int difficulty, int scoreRequirement) 
            : base(
            Phase.Unstable,
            "A phase where the world becomes unstable and treacherous to traverse.", 
            difficulty,
            UniverseConstants.PhobonColor,
            scoreRequirement)
        {
        }

        /// <summary>
        /// Applies the effects of this phase.
        /// </summary>
        /// <param name="layer">The layer.</param>
        public override void Apply(ILayer layer)
        {
            // Shake the camera!
            DrawingManager.Camera.Shake(10f, 20f, Ease.NullEase);

            base.Apply(layer);
        }
    }
}
