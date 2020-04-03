using System.Linq;

using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Occasus.Core.Layers;

namespace Occasus.Core.Drawing.Animation
{
    public class Animation : IAnimation
    {
        private readonly IList<Rectangle> frameRectangles;
        private readonly IList<int> frameIndexes;

        private int currentFrameIndex;

        private int loopLagFrames;
        private int holdEndFrames;

        /// <summary>
        /// Initializes a new instance of the <see cref="Animation" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="spriteMapLocation">The sprite map location.</param>
        /// <param name="frameRectangle">The frame rect.</param>
        /// <param name="frameIndexes">The frame indexes.</param>
        /// <param name="frameDelay">The frame delay.</param>
        /// <param name="playInFull">if set to <c>true</c> [play in full].</param>
        /// <param name="isLooping">if set to <c>true</c> [is looping].</param>
        public Animation(
            string name, 
            Point spriteMapLocation, 
            Rectangle frameRectangle,
            IList<int> frameIndexes, 
            int frameDelay, 
            bool playInFull, 
            bool isLooping)
        {
            this.Name = name;

            this.SpriteMapLocation = spriteMapLocation;
            this.FrameRectangle = frameRectangle;
            this.frameIndexes = frameIndexes;
            this.TotalFrames = this.frameIndexes.Count;
            this.DelayFrames = frameDelay;

            this.Flags = new Dictionary<AnimationFlag, bool>
                             {
                                 { AnimationFlag.Static, this.TotalFrames == 1 },
                                 { AnimationFlag.Looping, isLooping },
                                 { AnimationFlag.PlayInFull, playInFull },
                                 { AnimationFlag.HoldEnd, false }
                             };

            this.frameRectangles = new List<Rectangle>();
            for (var i = 0; i < this.frameIndexes.Count; i++)
            {
                var layerFrame = this.FrameRectangle;
                layerFrame.X += this.frameIndexes[i] * layerFrame.Width;
                this.frameRectangles.Add(layerFrame);
            }

            this.CurrentFrame = this.frameRectangles[0];
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name
        {
            get; private set;
        }

        /// <summary>
        /// Gets the location of this animation.
        /// </summary>
        public Point SpriteMapLocation
        {
            get; private set;
        }

        /// <summary>
        /// Gets the frame indexes for this animation.
        /// </summary>
        /// <remarks>
        /// These can be a linear range, a single index or a custom range.
        /// </remarks>
        public IEnumerable<int> FrameIndexes
        {
            get
            {
                return this.frameIndexes;
            }
        }

        public Rectangle CurrentFrame
        {
            get; private set;
        }

        /// <summary>
        /// Gets the current frame.
        /// </summary>
        public IEnumerable<Rectangle> FrameRectangles
        {
            get
            {
                return this.frameRectangles;
            }
        }

        /// <summary>
        /// Gets or sets the index of the current frame.
        /// </summary>
        /// <value>
        /// The index of the current frame.
        /// </value>
        public int CurrentFrameIndex
        {
            get
            {
                return this.currentFrameIndex;
            }

            set
            {
                if (this.currentFrameIndex == value)
                {
                    return;
                }

                this.currentFrameIndex = value;

                // Map to the correct layer frame.
                this.CurrentFrame = this.frameRectangles[value];
            }
        }

        /// <summary>
        /// Gets or sets the frame rectangle.
        /// </summary>
        /// <value>
        /// The frame rectangle.
        /// </value>
        public Rectangle FrameRectangle
        {
            get; set;
        }

        /// <summary>
        /// Gets the total number of frames in this animation.
        /// </summary>
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
        /// Gets or sets a value indicating how much time there should be between animation loops.
        /// </summary>
        /// <value>
        /// The number of frames to wait until looping the animation.
        /// </value>
        public int LoopLagFrames
        {
            get
            {
                return this.loopLagFrames;
            }

            set
            {
                if (this.loopLagFrames.Equals(value))
                {
                    return;
                }

                this.loopLagFrames = value;

                // To implement loop lag, the looping flag must be set.
                if (this.loopLagFrames > 0f)
                {
                    this.Flags[AnimationFlag.Looping] = true;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating how many frames to hold the end frame of the animation before it ends.
        /// </summary>
        /// <value>
        /// The number of frames to hold at the end of animation.
        /// </value>
        public int HoldEndFrames
        {
            get
            {
                return this.holdEndFrames;
            }

            set
            {
                if (this.holdEndFrames.Equals(value))
                {
                    return;
                }

                this.holdEndFrames = value;

                // To implement hold end, the hold end flag must be set.
                if (this.holdEndFrames > 0f)
                {
                    this.Flags[AnimationFlag.HoldEnd] = true;
                }
            }
        }

        /// <summary>
        /// Gets the flags.
        /// </summary>
        public IDictionary<AnimationFlag, bool> Flags
        {
            get; private set;
        }
    }
}
