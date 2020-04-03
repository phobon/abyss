using System;
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
            this.LightSourceType = lightSourceType;
        }

        /// <summary>
        /// Gets or sets the intensity.
        /// </summary>
        public float Intensity { get; set; }

        /// <summary>
        /// Gets or sets the decay rate of this light.
        /// </summary>
        public float Decay { get; set; }

        /// <summary>
        /// Gets or sets the color.
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// Gets or sets the scale.
        /// </summary>
        public float Scale { get; set; }

        /// <summary>
        /// Gets the type of light source this is.
        /// </summary>
        public LightSourceType LightSourceType { get; private set; }

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

        protected override void InitializeTags()
        {
            // Add a tag to the parent of this light source so that it can be added to the correct cache.
            this.Parent.Tags.Add(Lighting.DeferredLightSource);
            this.Tags.Add(Lighting.DeferredLightSource);
        }
    }
}
