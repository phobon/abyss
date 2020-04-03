using Microsoft.Xna.Framework.Graphics;
using Occasus.Core.Drawing;
using Occasus.Core.Drawing.Shaders;

namespace Abyss.World.Drawing.Shaders
{
    public class MaskEffect : Shader
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MaskEffect"/> class.
        /// </summary>
        public MaskEffect()
            : base("Mask Effect", "Pixel shader that blacks out all textures.", new[] { ShaderUsage.IncludeDeferredRenderEntities })
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
            ShaderManager.CurrentShader.Effect.CurrentTechnique = ShaderManager.CurrentShader.Effect.Techniques["MaskTechnique"];
        }
    }
}
