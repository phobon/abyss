using System.Collections;

namespace Occasus.Core.Drawing.Images
{
    public static class ImageLayerExtensions
    {
        public static IEnumerator Pulse(this IImageLayer i, int pulseDuration)
        {
            var elapsedFrames = 0;

            while (true)
            {
                // Fade out.
                while (elapsedFrames <= pulseDuration)
                {
                    i.Opacity = 1f - ((float)elapsedFrames / (float)pulseDuration);
                    elapsedFrames++;
                    yield return null;
                }

                // Fade in.
                elapsedFrames = 0;
                while (elapsedFrames <= pulseDuration)
                {
                    i.Opacity = (float)elapsedFrames / (float)pulseDuration;
                    elapsedFrames++;
                    yield return null;
                }

                elapsedFrames = 0;
            }
        }
    }
}
