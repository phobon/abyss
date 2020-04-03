using Microsoft.Xna.Framework.Graphics;
using Occasus.Core.Drawing;
using Occasus.Core.Drawing.Shaders;

namespace Abyss.World.Drawing.Shaders
{
    public class DesaturateEffect : Shader
    {
        private static string Technique = "DesaturateTechnique";
        private static string DesaturationAmountKey = "DesaturationAmount";

        public float DesaturationAmount
        {
            get
            {
                return this.Effect.Parameters[DesaturationAmountKey].GetValueSingle();
            }

            set
            {
                this.Effect.Parameters[DesaturationAmountKey].SetValue(value);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DesaturateEffect"/> class.
        /// </summary>
        public DesaturateEffect()
            : base("Desaturate Effect", "Pixel shader that removes all colour from a particular scene.", Technique, new [] { ShaderUsage.IncludeDeferredRenderEntities })
        {
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        public override void LoadContent()
        {
            this.Effect = DrawingManager.ContentManager.Load<Effect>("Effects/Screen");
            DesaturationAmount = 1.0f;
        }
    }
}
