using Microsoft.Xna.Framework.Graphics;
using Occasus.Core.Drawing;
using Occasus.Core.Drawing.Shaders;

namespace Abyss.World.Drawing.Shaders
{
    public class FlipVerticallyEffect : Shader
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FlipVerticallyEffect"/> class.
        /// </summary>
        public FlipVerticallyEffect()
            : base("Flip Vertically Effect", "Pixel shader that flips the scene vertically.")
        {
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        public override void LoadContent()
        {
            this.Effect = DrawingManager.ContentManager.Load<Effect>("Effects/Screen");
        }

        /// <summary>
        /// Applies this Shader.
        /// </summary>
        public override void Apply()
        {
            ShaderManager.CurrentShader.Effect.CurrentTechnique = ShaderManager.CurrentShader.Effect.Techniques["FlipVerticallyTechnique"];
        }
    }
}
