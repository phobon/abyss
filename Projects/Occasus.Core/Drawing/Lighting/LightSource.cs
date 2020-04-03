using Microsoft.Xna.Framework;
using Occasus.Core.Components;
using Occasus.Core.Entities;

namespace Occasus.Core.Drawing.Lighting
{
    public abstract class LightSource : EntityComponent, ILightSource
    {
        public static string Tag = "LightSource";

        /// <summary>
        /// Initializes a new instance of the <see cref="LightSource" /> class.
        /// </summary>
        /// <param name="parent">The entity.</param>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        /// <param name="lightSourceType">Type of the light source.</param>
        protected LightSource(IEntity parent, string name, string description, LightSourceType lightSourceType)
            : base(parent, name, description)
        {
            // Add a tag to the parent of this light source so that it can be added to the correct cache.
            parent.Tags.Add(Lighting.DeferredLightSource);

            this.LightSourceType = lightSourceType;
        }

        /// <summary>
        /// Gets or sets the intensity.
        /// </summary>
        /// <value>
        /// The intensity.
        /// </value>
        public float Intensity
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the decay rate of this light.
        /// </summary>
        /// <value>
        /// The decay.
        /// </value>
        public float Decay
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the color.
        /// </summary>
        /// <value>
        /// The color.
        /// </value>
        public Color Color
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the scale.
        /// </summary>
        /// <value>
        /// The scale.
        /// </value>
        public float Scale
        {
            get; set;
        }

        /// <summary>
        /// Gets the type of light source this is.
        /// </summary>
        public LightSourceType LightSourceType
        {
            get; private set;
        }

        /// <summary>
        /// Resumes updating and drawing of the Engine Component.
        /// </summary>
        public override void Resume()
        {
            this.Flags[EngineFlag.Active] = true;
        }

        /// <summary>
        /// Suspends the Engine Component from updating or drawing.
        /// </summary>
        public override void Suspend()
        {
            this.Flags[EngineFlag.Active] = false;
        }
    }
}
