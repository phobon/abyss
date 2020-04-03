using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Occasus.Core.Drawing;
using Occasus.Core.Drawing.Shaders;

namespace Abyss.World.Drawing.Shaders
{
    public class FlipVerticalEffect : Shader
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FlipVerticalEffect"/> class.
        /// </summary>
        public FlipVerticalEffect()
            : base("Flip Vertical", "Pixel shader that flips a scene vertically.", new[] { ShaderUsage.ApplyBeforeScale, ShaderUsage.IncludeDeferredRenderEntities }, new Vector2(0, -DrawingManager.ScaledWindowHeight + DrawingManager.BaseResolutionHeight))
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
            ShaderManager.CurrentShader.Effect.CurrentTechnique = ShaderManager.CurrentShader.Effect.Techniques["FlipVerticalTechnique"];
        }
    }
}
