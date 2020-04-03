using Occasus.Core.Maths;
using System.Collections;

namespace Occasus.Core.Drawing.Text
{
    public static class TextExtensions
    {
        public static IEnumerator SetOpacity(this ITextElement t, float target, int duration, Easer ease)
        {
            var start = t.Opacity;
            var range = target - start;

            var elapsedFrames = 0;
            while (elapsedFrames <= duration)
            {
                t.Opacity = start + (range * ease((float)elapsedFrames / (float)duration));
                yield return null;

                elapsedFrames++;
            }

            t.Opacity = target;
        }
    }
}
