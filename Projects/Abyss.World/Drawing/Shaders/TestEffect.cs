using Microsoft.Xna.Framework.Graphics;
using Occasus.Core.Drawing;
using Occasus.Core.Drawing.Shaders;

namespace Abyss.World.Drawing.Shaders
{
    public class TestEffect : Shader
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestEffect"/> class.
        /// </summary>
        public TestEffect()
            : base("Test Effect", "Pixel shader test.", new[] { ShaderUsage.IncludeDeferredRenderEntities })
        {
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        public override void LoadContent()
        {
            //this.Effect = DrawingManager.ContentManager.Load<Effect>("Effects/Test");
        }

        /// <summary>
        /// Applies this Shader.
        /// </summary>
        public override void Apply()
        {
            //ShaderManager.CurrentShader.Effect.CurrentTechnique = ShaderManager.CurrentShader.Effect.Techniques["GrayscaleTest"];
            //ShaderManager.CurrentShader.Effect.Parameters["DesaturationAmount"].SetValue(1.0f);
        }
    }
}
