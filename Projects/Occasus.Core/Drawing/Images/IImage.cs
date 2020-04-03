using System.Collections.Generic;
using Occasus.Core.Components.Graphics;

namespace Occasus.Core.Drawing.Images
{
    public interface IImage : IGraphicsComponent
    {
        /// <summary>
        /// Gets the layers of this sprite.
        /// </summary>
        IDictionary<string, IImageLayer> Layers { get; }
    }
}
