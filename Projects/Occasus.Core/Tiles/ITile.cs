using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Occasus.Core.Entities;

namespace Occasus.Core.Tiles
{
    public interface ITile : IEntity
    {
        /// <summary>
        /// Gets the total frames.
        /// </summary>
        IEnumerable<int> FrameIndexes { get; }

        /// <summary>
        /// Gets the index of the frame.
        /// </summary>
        int CurrentFrame { get; }

        /// <summary>
        /// Gets the delay between frames for this animation.
        /// </summary>
        int DelayFrames { get; }

        /// <summary>
        /// Gets the sprite texture.
        /// </summary>
        string SpriteTexture { get; }

        /// <summary>
        /// Gets the type of tile to render.
        /// </summary>
        int TileType { get; }

        /// <summary>
        /// Gets the source rectangle.
        /// </summary>
        Rectangle SourceRectangle { get; }

        /// <summary>
        /// Gets or sets the opacity.
        /// </summary>
        /// <value>
        /// The opacity.
        /// </value>
        float Opacity { get; set; }
    }
}
