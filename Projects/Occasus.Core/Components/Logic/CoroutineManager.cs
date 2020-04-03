using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Occasus.Core.Components.Logic
{
    public static class CoroutineManager
    {
        private static readonly List<string> routinesToRemove = new List<string>();
        private static readonly Dictionary<string, IEnumerator> coroutines = new Dictionary<string, IEnumerator>();

        /// <summary>
        /// Gets the coroutines.
        /// </summary>
        public static IDictionary<string, IEnumerator> Coroutines
        {
            get
            {
                return coroutines;
            }
        }

        /// <summary>
        /// Adds the specified routine without a key. These coroutines cannot be stopped or removed.
        /// </summary>
        /// <param name="routine">The routine.</param>
        public static void Add(IEnumerator routine)
        {
            coroutines.Add(Guid.NewGuid().ToString(), routine);
        }

        /// <summary>
        /// Adds the specified coroutine with the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="routine">The routine.</param>
        public static void Add(string key, IEnumerator routine)
        {
            // Remove this first, so we don't have to worry about overlapping coroutines.
            Remove(key);

            coroutines.Add(key, routine);
        }

        /// <summary>
        /// Removes the specified coroutine immediately.
        /// </summary>
        /// <param name="key">The key.</param>
        public static void Remove(string key)
        {
            if (coroutines.ContainsKey(key))
            {
                coroutines.Remove(key);
            }
        }

        /// <summary>
        /// Clears all coroutines from the collection.
        /// </summary>
        public static void Clear()
        {
            coroutines.Clear();
        }

        /// <summary>
        /// Determines whether the specified key contains key.
        /// </summary>
        /// <param name="key">The key.</param>
        public static bool ContainsKey(string key)
        {
            return coroutines.ContainsKey(key);
        }

        /// <summary>
        /// Updates this instance.
        /// </summary>
        public static void Update()
        {
            // Advance current coroutines and mark completed coroutines for removal.
            foreach (var instance in coroutines.ToList())
            {
                var routine = instance.Value;
                var r = routine.Current as IEnumerator;
                if (r != null)
                {
                    if (MoveNext(r))
                    {
                        continue;
                    }
                }

                if (!routine.MoveNext())
                {
                    routinesToRemove.Add(instance.Key);
                }
            }

            // Remove coroutines that are no longer relevant.
            foreach (var routine in routinesToRemove)
            {
                coroutines.Remove(routine);
            }

            routinesToRemove.Clear();
        }

        private static bool MoveNext(IEnumerator routine)
        {
            var r = routine.Current as IEnumerator;
            if (r != null)
            {
                if (MoveNext(r))
                {
                    return true;
                }
            }

            return routine.MoveNext();
        }
    }
}
