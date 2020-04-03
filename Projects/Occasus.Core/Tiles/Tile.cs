using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Occasus.Core.Assets;
using Occasus.Core.Drawing;
using Occasus.Core.Entities;

namespace Occasus.Core.Tiles
{
    /// <summary>
    /// A lightweight class that contains Tile data; used for drawing purposes.
    /// </summary>
    public class Tile : Entity, ITile
    {
        public const int EmptyTile = -1;
        
        private int frameIndex;
        private int delayFramesRemaining;

        private readonly bool isAnimated;

        private readonly Rectangle sourceRectangle;

        /// <summary>
        /// Initializes a new instance of the <see cref="Tile" /> class.
        /// </summary>
        /// <param name="spriteTexture">The sprite texture.</param>
        /// <param name="tileType">Type of the tile.</param>
        /// <param name="sourceRectangle">The source rectangle.</param>
        public Tile(string spriteTexture, int tileType, Rectangle sourceRectangle)
            : base("Tile", "Lightweight Tile entity.")
        {
            this.SpriteTexture = spriteTexture;
            this.TileType = tileType;

            this.sourceRectangle = sourceRectangle;
            this.SourceRectangle = this.sourceRectangle;

            this.FrameIndexes = new List<int> { 0 };
            this.TotalFrames = 1;
            this.Opacity = 1f;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Tile" /> class.
        /// </summary>
        /// <param name="spriteTexture">The sprite texture.</param>
        /// <param name="frameIndexes">The total frames.</param>
        /// <param name="delayFrames">The delay frames.</param>
        /// <param name="tileType">Type of the tile.</param>
        /// <param name="sourceRectangle">The source rectangle.</param>
        public Tile(string spriteTexture, IEnumerable<int> frameIndexes, int delayFrames, int tileType, Rectangle sourceRectangle)
            : base("Tile", "Lightweight Tile entity.")
        {
            this.SpriteTexture = spriteTexture;
            this.TileType = tileType;
            this.FrameIndexes = frameIndexes;
            this.TotalFrames = this.FrameIndexes.Count();
            this.DelayFrames = delayFrames;

            this.sourceRectangle = sourceRectangle;
            this.SourceRectangle = this.sourceRectangle;

            this.isAnimated = this.TotalFrames > 1;
            this.Opacity = 1f;
        }

        /// <summary>
        /// Gets the total frames.
        /// </summary>
        public IEnumerable<int> FrameIndexes
        {
            get; private set;
        }

        /// <summary>
        /// Gets the index of the frame.
        /// </summary>
        public int CurrentFrame
        {
            get
            {
                return this.frameIndex;
            }

            private set
            {
                if (this.frameIndex == value)
                {
                    return;
                }

                this.frameIndex = value;

                // Reset the number of delay frames remaining.
                this.delayFramesRemaining = this.DelayFrames;
            }
        }

        public int TotalFrames
        {
            get; private set;
        }

        /// <summary>
        /// Gets the delay between frames for this animation.
        /// </summary>
        public int DelayFrames
        {
            get; private set;
        }

        /// <summary>
        /// Gets the sprite texture.
        /// </summary>
        public string SpriteTexture
        {
            get; private set;
        }

        /// <summary>
        /// Gets the type of tile to render.
        /// </summary>
        public int TileType
        {
            get; private set;
        }

        /// <summary>
        /// Gets or sets the source rectangle.
        /// </summary>
        /// <value>
        /// The source rectangle.
        /// </value>
        public Rectangle SourceRectangle
        {
            get; private set;
        }

        /// <summary>
        /// Gets or sets the opacity.
        /// </summary>
        /// <value>
        /// The opacity.
        /// </value>
        public float Opacity
        {
            get; set;
        }

        /// <summary>
        /// Draws the Engine Component.
        /// </summary>
        /// <param name="gameTime">The game time object.</param>
        /// <param name="spriteBatch">The sprite batch.</param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
            
            // If this is a static tile (ie: not animated), just draw it.
            if (this.isAnimated)
            {
                // Determine whether to delay frames or continue.
                if (this.delayFramesRemaining == 0)
                {
                    // Clamp frame index as necessary.
                    var nextFrame = this.CurrentFrame + 1;
                    if (nextFrame >= this.TotalFrames)
                    {
                        nextFrame = 0;
                    }

                    // Get the new frame index and set a new source rectangle if required.
                    this.CurrentFrame = nextFrame;
                    this.SourceRectangle = this.GetFrame();
                }
                else if (this.delayFramesRemaining > 0)
                {
                    this.delayFramesRemaining--;
                }
            } 

            // Draw the tile.
            spriteBatch.Draw(TextureManager.Textures[this.SpriteTexture], this.Transform.Position, this.SourceRectangle, Color.White * this.Opacity, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
        }

        private Rectangle GetFrame()
        {
            return new Rectangle(this.sourceRectangle.X + (this.FrameIndexes.ElementAt(this.CurrentFrame) * DrawingManager.TileWidth), this.sourceRectangle.Y, this.sourceRectangle.Width, this.sourceRectangle.Height);
        }
    }
}
