using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Occasus.Core.Drawing;
using Occasus.Core.Drawing.Shaders;

namespace Abyss.World.Drawing.Shaders
{
    public class RippleEffect : Shader
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RippleEffect"/> class.
        /// </summary>
        public RippleEffect()
            : base("Ripple Effect", "Pixel shader that ripples the view on the screen.")
        {
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        public override void LoadContent()
        {
            this.Effect = DrawingManager.ContentManager.Load<Effect>("Effects/Ripple");
        }

        /// <summary>
        /// Applies this Shader.
        /// </summary>
        public override void Apply()
        {
            ShaderManager.CurrentShader.Effect.CurrentTechnique = ShaderManager.CurrentShader.Effect.Techniques["RippleTechnique"];
            ShaderManager.CurrentShader.Effect.Parameters["wave"].SetValue((float)(Math.PI / 0.75f));
            ShaderManager.CurrentShader.Effect.Parameters["distortion"].SetValue(1f);
            ShaderManager.CurrentShader.Effect.Parameters["centerCoord"].SetValue(new Vector2(0.5f));
        }
    }
}
