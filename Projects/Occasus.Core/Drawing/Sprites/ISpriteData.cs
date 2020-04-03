using Microsoft.Xna.Framework;

using Occasus.Core.Drawing.Animation;
using System.Collections.Generic;
using Occasus.Core.Drawing.Images;

namespace Occasus.Core.Drawing.Sprites
{
    public interface ISpriteData
    {
        /// <summary>
        /// Gets the origin.
        /// </summary>
        Vector2 Origin { get; }

        /// <summary>
        /// Gets the size of the frame.
        /// </summary>
        Vector2 FrameSize { get; }

        /// <summary>
        /// Gets a value indicating whether textures should be tiled or not.
        /// </summary>
        bool TileTexture { get; }

        bool CropTexture { get; }

        /// <summary>
        /// Gets the layers.
        /// </summary>
        IEnumerable<IImageLayer> Layers { get; }

        /// <summary>
        /// Gets the animation states.
        /// </summary>
        IEnumerable<IAnimation> AnimationStates { get; }
    }
}
