using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Occasus.Core.Drawing;
using Occasus.Core.Drawing.Shaders;

namespace Abyss.World.Drawing.Shaders
{
    public class FlipHorizontalEffect : Shader
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FlipHorizontalEffect"/> class.
        /// </summary>
        public FlipHorizontalEffect()
            : base("Flip Horizontal", "Pixel shader that flips a scene horizontally.", new[] { ShaderUsage.ApplyBeforeScale, ShaderUsage.IncludeDeferredRenderEntities }, new Vector2(-DrawingManager.ScaledWindowWidth + DrawingManager.BaseResolutionWidth, 0))
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
            ShaderManager.CurrentShader.Effect.CurrentTechnique = ShaderManager.CurrentShader.Effect.Techniques["FlipHorizontalTechnique"];
        }
    }
}
