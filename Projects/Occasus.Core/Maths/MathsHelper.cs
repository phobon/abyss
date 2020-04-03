using System;

namespace Occasus.Core.Maths
{
    public static class MathsHelper
    {
        private static Random random;

        /// <summary>
        /// Seeds the random number generator with the specified.
        /// </summary>
        /// <param name="seed">The seed.</param>
        public static void Seed(int seed)
        {
            random = new Random(seed);
        }

        /// <summary>
        /// Gets a random number between 0 and 100.
        /// </summary>
        /// <returns>A random number.</returns>
        public static int Random()
        {
            if (random == null)
            {
                Seed(Guid.NewGuid().GetHashCode());
            }

            return random.Next(100);
        }

        /// <summary>
        /// Gets a random number between 0 and the specified maximum.
        /// </summary>
        /// <param name="maximum">The maximum.</param>
        /// <returns>A random number.</returns>
        public static int Random(int maximum)
        {
            if (random == null)
            {
                Seed(Guid.NewGuid().GetHashCode());
            }

            return random.Next(maximum);
        }

        /// <summary>
        /// Gets a random number between the specified minimum and the specified maximum.
        /// </summary>
        /// <param name="minimum">The minimum.</param>
        /// <param name="maximum">The maximum.</param>
        /// <returns>A random number.</returns>
        public static int Random(int minimum, int maximum)
        {
            if (random == null)
            {
                Seed(Guid.NewGuid().GetHashCode());
            }

            return random.Next(minimum, maximum);
        }

        /// <summary>
        /// Gets a random float between 0 and 1.
        /// </summary>
        /// <returns>A random number.</returns>
        public static float NextFloat()
        {
            if (random == null)
            {
                Seed(Guid.NewGuid().GetHashCode());
            }

            return (float)random.NextDouble();
        }
    }
}
