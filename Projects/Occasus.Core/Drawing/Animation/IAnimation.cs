using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Occasus.Core.Drawing.Animation
{
    public interface IAnimation
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the location of this animation.
        /// </summary>
        Point SpriteMapLocation { get; }

        /// <summary>
        /// Gets the frame indexes for this animation.
        /// </summary>
        /// <remarks>
        /// These can be a linear range, a single index or a custom range.
        /// </remarks>
        IEnumerable<int> FrameIndexes { get; }

        /// <summary>
        /// Gets the current frame.
        /// </summary>
        Rectangle CurrentFrame { get; }

        /// <summary>
        /// Gets or sets the index of the current frame.
        /// </summary>
        /// <value>
        /// The index of the current frame.
        /// </value>
        int CurrentFrameIndex { get; set; }

        /// <summary>
        /// Gets the frame rectangle.
        /// </summary>
        Rectangle FrameRectangle { get; set;  }

        /// <summary>
        /// Gets the total number of frames in this animation.
        /// </summary>
        int TotalFrames { get; }

        /// <summary>
        /// Gets the delay between frames for this animation.
        /// </summary>
        int DelayFrames { get; }

        /// <summary>
        /// Gets or sets a value indicating how much time there should be between animation loops.
        /// </summary>
        /// <value>
        /// The number of frames to wait until looping the animation.
        /// </value>
        int LoopLagFrames { get; set; }

        /// <summary>
        /// Gets or sets a value indicating how many frames to hold the end frame of the animation before it ends.
        /// </summary>
        /// <value>
        /// The number of frames to hold at the end of animation.
        /// </value>
        int HoldEndFrames { get; set; }

        /// <summary>
        /// Gets the flags.
        /// </summary>
        IDictionary<AnimationFlag, bool> Flags { get; }
    }
}
