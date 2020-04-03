using System.Collections;

namespace Occasus.Core.Components.Logic
{
    public static class Coroutines
    {
        public delegate bool Predicate();

        /// <summary>
        /// Pauses for a specified amount of time.
        /// </summary>
        /// <param name="totalFrames">The total frames to pause.</param>
        /// <returns>Pause co-routine.</returns>
        public static IEnumerator Pause(int totalFrames)
        {
            return PauseInternal(totalFrames).GetEnumerator();
        }

        /// <summary>
        /// Pauses until something happens.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>Pause until co-routine.</returns>
        public static IEnumerator PauseUntil(Predicate predicate)
        {
            while (!predicate())
            {
                yield return null;
            }
        }

        private static IEnumerable PauseInternal(int totalFrames)
        {
            var elapsedFrames = 0;
            while (elapsedFrames <= totalFrames)
            {
                yield return null;
                elapsedFrames++;
            }
        }
    }
}
