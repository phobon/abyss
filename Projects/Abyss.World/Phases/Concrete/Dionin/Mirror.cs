using Abyss.World.Universe;
using Occasus.Core.Drawing.Shaders;

using Occasus.Core.Layers;

namespace Abyss.World.Phases.Concrete.Dionin
{
    public class Mirror : PhaseBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Mirror"/> class.
        /// </summary>
        public Mirror(int difficulty, int scoreRequirement) 
            : base(
            Phase.Mirror,
            "A phase that the world becomes mirrored, reversing directions.", 
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

            // Apply the HorizontalFlip effect.
            ShaderManager.CurrentShader = ShaderManager.SupportedShaders["FlipHorizontal"];
            ShaderManager.CurrentShader.Apply();
        }
    }
}
