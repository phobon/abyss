using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Occasus.Core.Drawing.Shaders
{
    public interface IShader
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the description.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Gets the effect.
        /// </summary>
        Effect Effect { get; }

        /// <summary>
        /// Gets the usages of this particular shader.
        /// </summary>
        IDictionary<ShaderUsage, bool> Usages { get; }

        /// <summary>
        /// Gets the draw offset for this shader. This is only applicable to fullscreen effects that alter rendered pixel positions in the shader.
        /// eg: Vertical and Horizontal flip.
        /// </summary>
        Vector2 DrawOffset { get; }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Loads the content.
        /// </summary>
        void LoadContent();

        /// <summary>
        /// Applies this Shader.
        /// </summary>
        void Apply();
    }
}
