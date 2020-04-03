﻿using Microsoft.Xna.Framework.Graphics;
using Occasus.Core.Drawing;
using Occasus.Core.Drawing.Shaders;

namespace Abyss.World.Drawing.Shaders
{
    public class SepiaEffect : Shader
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SepiaEffect"/> class.
        /// </summary>
        public SepiaEffect()
            : base("Sepia Effect", "Pixel shader that applies a sepia effect to a scene.", new[] { ShaderUsage.IncludeDeferredRenderEntities })
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
            ShaderManager.CurrentShader.Effect.CurrentTechnique = ShaderManager.CurrentShader.Effect.Techniques["SepiaToneTechnique"];
        }
    }
}
