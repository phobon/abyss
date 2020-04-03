using Microsoft.Xna.Framework;

using Occasus.Core.Components;

namespace Occasus.Core.Drawing.Lighting
{
    public interface ILightSource : IEntityComponent
    {
        /// <summary>
        /// Gets or sets the intensity.
        /// </summary>
        /// <value>
        /// The intensity.
        /// </value>
        float Intensity { get; set; }

        /// <summary>
        /// Gets or sets the color.
        /// </summary>
        /// <value>
        /// The color.
        /// </value>
        Color Color { get; set; }

        /// <summary>
        /// Gets or sets the scale.
        /// </summary>
        /// <value>
        /// The scale.
        /// </value>
        float Scale { get; set; }

        /// <summary>
        /// Gets the type of light source this is.
        /// </summary>
        LightSourceType LightSourceType { get; }
    }
}
