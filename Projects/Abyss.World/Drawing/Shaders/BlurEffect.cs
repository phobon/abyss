using Microsoft.Xna.Framework.Graphics;
using Occasus.Core.Drawing;
using Occasus.Core.Drawing.Shaders;

namespace Abyss.World.Drawing.Shaders
{
    public class BlurEffect : Shader
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BlurEffect"/> class.
        /// </summary>
        public BlurEffect()
            : base("Blur Effect", "Pixel shader that adds a gaussian blur effect.", new[] { ShaderUsage.IncludeDeferredRenderEntities })
        {
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        public override void LoadContent()
        {
            //this.Effect = DrawingManager.ContentManager.Load<Effect>("Effects/Blur");
        }

        /// <summary>
        /// Applies this Shader.
        /// </summary>
        public override void Apply()
        {
            //ShaderManager.CurrentShader.Effect.CurrentTechnique = ShaderManager.CurrentShader.Effect.Techniques["GaussianBlurTechnique"];
        }
    }
}
