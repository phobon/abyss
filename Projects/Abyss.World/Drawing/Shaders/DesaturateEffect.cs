using Microsoft.Xna.Framework.Graphics;
using Occasus.Core.Drawing;
using Occasus.Core.Drawing.Shaders;

namespace Abyss.World.Drawing.Shaders
{
    public class DesaturateEffect : Shader
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DesaturateEffect"/> class.
        /// </summary>
        public DesaturateEffect()
            : base("Desaturate Effect", "Pixel shader that removes all colour from a particular scene.", new [] { ShaderUsage.IncludeDeferredRenderEntities })
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
            ShaderManager.CurrentShader.Effect.CurrentTechnique = ShaderManager.CurrentShader.Effect.Techniques["DesaturateTechnique"];
            ShaderManager.CurrentShader.Effect.Parameters["DesaturationAmount"].SetValue(1.0f);
        }
    }
}
