using Occasus.Core.Entities;

namespace Occasus.Core.Drawing.Animation
{
    public static class TimingHelper
    {
        /// <summary>
        /// Gets the duration, given the number of frames.
        /// </summary>
        /// <param name="frameCount">The frame count.</param>
        /// <returns>A duration based on the current DeltaTime and number of frames.</returns>
        public static float GetDuration(int frameCount)
        {
            return Engine<IGameManager<IEntity>>.DeltaTime * frameCount;
        }

        /// <summary>
        /// Gets the frame count, given a duration.
        /// </summary>
        /// <param name="duration">The duration.</param>
        /// <returns>A frame count given the duration.</returns>
        public static int GetFrameCount(float duration)
        {
            return (int)(duration / Engine<IGameManager<IEntity>>.DeltaTime);
        }
    }
}
