using System.Collections.Generic;

using Microsoft.Xna.Framework.Graphics;

namespace Occasus.Core.Drawing.Shaders
{
    public static class ShaderManager
    {
        /// <summary>
        /// Gets or sets the supported shaders.
        /// </summary>
        /// <value>
        /// The supported shaders.
        /// </value>
        public static IDictionary<string, IShader> SupportedShaders
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the current shader.
        /// </summary>
        /// <value>
        /// The current shader.
        /// </value>
        public static IShader CurrentShader
        {
            get; set;
        }
    }
}
