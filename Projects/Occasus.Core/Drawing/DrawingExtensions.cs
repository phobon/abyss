using System.Collections;
using Occasus.Core.Drawing.Sprites;
using Occasus.Core.Maths;

namespace Occasus.Core.Drawing
{
    public static class DrawingExtensions
    {
        public static IEnumerator FadeTo(this ISprite s, float target, int framesRemaining, Easer ease)
        {
            var start = s.Opacity;
            var range = target - start;

            // Determine the number of frames this tween should take based on duration and delta time.
            var elapsedFrames = 0;

            while (elapsedFrames <= framesRemaining)
            {
                s.Opacity = start + (range * ease((float)elapsedFrames / (float)framesRemaining));
                yield return null;

                elapsedFrames++;
            }

            s.Opacity = target;
        }
    }
}
