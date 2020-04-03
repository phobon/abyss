using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Occasus.Core.Drawing;
using Occasus.Core.Drawing.Shaders;

namespace Abyss.World.Drawing.Shaders
{
    public class RippleEffect : Shader
    {
        private static string WaveKey = "wave";
        private static string DistortionKey = "distortion";
        private static string CenterCoordKey = "centerCoord";

        public float Wave
        {
            get
            {
                return this.Effect.Parameters[WaveKey].GetValueSingle();
            }

            set
            {
                this.Effect.Parameters[WaveKey].SetValue(value);
            }
        }

        public float Distortion
        {
            get
            {
                return this.Effect.Parameters[DistortionKey].GetValueSingle();
            }

            set
            {
                this.Effect.Parameters[DistortionKey].SetValue(value);
            }
        }

        public Vector2 CenterCoordinate
        {
            get
            {
                return this.Effect.Parameters[CenterCoordKey].GetValueVector2();
            }

            set
            {
                this.Effect.Parameters[CenterCoordKey].SetValue(value);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RippleEffect"/> class.
        /// </summary>
        public RippleEffect()
            : base("Ripple Effect", "Pixel shader that ripples the view on the screen.", "RippleTechnique")
        {
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        public override void LoadContent()
        {
            this.Effect = DrawingManager.ContentManager.Load<Effect>("Effects/Ripple");
            this.Wave = (float) (Math.PI/0.75f);
            this.Distortion = 1f;
            this.CenterCoordinate = new Vector2(0.5f);
        }
    }
}
