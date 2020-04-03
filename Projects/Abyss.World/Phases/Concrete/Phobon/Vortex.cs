using Abyss.World.Universe;
using Occasus.Core.Drawing;
using Occasus.Core.Layers;
using Occasus.Core.Maths;

namespace Abyss.World.Phases.Concrete.Phobon
{
    public class Vortex : PhaseBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Vortex"/> class.
        /// </summary>
        public Vortex(int difficulty, int scoreRequirement) 
            : base(
            Phase.Unstable,
            "A phase where extra-dimensional monsters warp to different places periodically.", 
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
