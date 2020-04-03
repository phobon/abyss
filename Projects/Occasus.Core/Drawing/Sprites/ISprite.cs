using Occasus.Core.Drawing.Animation;
using System.Collections.Generic;
using Occasus.Core.Drawing.Images;
using Occasus.Core.Maths;

namespace Occasus.Core.Drawing.Sprites
{
    public interface ISprite : IImage
    {
        /// <summary>
        /// Gets the animation states.
        /// </summary>
        IDictionary<string, IAnimation> Animations { get; }

        /// <summary>
        /// Gets the current animation.
        /// </summary>
        /// <value>
        /// The current animation.
        /// </value>
        IAnimation CurrentAnimation { get; }

        /// <summary>
        /// Transitions to a new animation.
        /// </summary>
        /// <param name="animationName">Name of the animation.</param>
        void GoToAnimation(string animationName);

        /// <summary>
        /// Blinks this sprite for a set amount of frames for a set duration.
        /// </summary>
        /// <param name="blinkFrames">The frame count.</param>
        /// <param name="durationFrames">The duration.</param>
        void Blink(int blinkFrames, int durationFrames);

        /// <summary>
        /// Gets the length of an animation in frames. Takes into account number of frames and delay between each frame.
        /// </summary>
        /// <param name="animationName">Name of the animation.</param>
        /// <returns>Length of the animation in frames.</returns>
        int GetAnimationFrameLength(string animationName);

        void Squash(float factor, int durationFrames, Easer easingFunction = null);

        void Stretch(float factor, int durationFrames, Easer easingFunction = null);
    }
}
