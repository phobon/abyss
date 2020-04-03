using Abyss.World.Universe;
using Occasus.Core.Drawing.Shaders;

using Occasus.Core.Layers;

namespace Abyss.World.Phases.Concrete.Dionin
{
    public class Inversion : PhaseBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Inversion"/> class.
        /// </summary>
        public Inversion(int difficulty, int scoreRequirement) 
            : base(
            Phase.Inversion,
            "A phase where the world inverts; causing entities in the Abyss to fall upwards.", 
            difficulty,
            UniverseConstants.DioninColor,
            scoreRequirement)
        {
        }

        /// <summary>
        /// Applies the effects of this phase.
        /// </summary>
        /// <param name="layer">The layer.</param>
        public override void Apply(ILayer layer)
        {
            base.Apply(layer);
            ShaderManager.SupportedShaders["FlipVertical"].Activate();
        }

        public override void Remove(ILayer layer)
        {
            base.Remove(layer);
            ShaderManager.SupportedShaders["FlipVertical"].Deactivate();
        }
    }
}
