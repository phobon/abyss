using System.Linq;

using Microsoft.Xna.Framework;

using Occasus.Core.Drawing.Animation;
using System.Collections.Generic;
using Occasus.Core.Drawing.Images;

namespace Occasus.Core.Drawing.Sprites
{
    public class SpriteData : ISpriteData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SpriteData" /> class.
        /// </summary>
        /// <param name="origin">The origin.</param>
        /// <param name="layers">The layers.</param>
        /// <param name="animationStates">The animation states.</param>
        /// <param name="tileTexture">if set to <c>true</c> [tile texture].</param>
        /// <param name="cropTexture">if set to <c>true</c> [crop texture].</param>
        public SpriteData(
            Vector2 origin,
            IEnumerable<IImageLayer> layers,
            IEnumerable<IAnimation> animationStates,
            bool tileTexture,
            bool cropTexture)
        {
            this.Origin = origin;
            var sourceRect = layers.First().SourceFrame;
            this.FrameSize = new Vector2(sourceRect.Width, sourceRect.Height);
            this.TileTexture = tileTexture;
            this.Layers = layers;
            this.AnimationStates = animationStates;
            this.CropTexture = cropTexture;
        }

        /// <summary>
        /// Gets the origin.
        /// </summary>
        public Vector2 Origin
        {
            get; private set;
        }

        /// <summary>
        /// Gets the size of the frame.
        /// </summary>
        public Vector2 FrameSize
        {
            get; private set;
        }

        /// <summary>
        /// Gets a value indicating whether textures should be tiled or not.
        /// </summary>
        public bool TileTexture
        {
            get; private set;
        }

        public bool CropTexture
        {
            get; private set;
        }

        /// <summary>
        /// Gets the layers.
        /// </summary>
        public IEnumerable<IImageLayer> Layers
        {
            get; private set;
        }

        /// <summary>
        /// Gets the animation states.
        /// </summary>
        public IEnumerable<IAnimation> AnimationStates
        {
            get; private set;
        }
    }
}
