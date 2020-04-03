using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;

namespace Occasus.Core.Audio
{
    public interface ISound : IDisposable
    {
        /// <summary>
        /// Gets the identifier.
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Gets the sound effect.
        /// </summary>
        SoundEffect SoundEffect { get; }

        /// <summary>
        /// Gets the sound effect instances.
        /// </summary>
        IEnumerable<SoundEffectInstance> SoundEffectInstances { get; }

        /// <summary>
        /// Plays this sound.
        /// </summary>
        /// <param name="magnitude">The magnitude.</param>
        /// <param name="volume">The volume.</param>
        void Play(float magnitude = 160f, float volume = 1f);

        /// <summary>
        /// Stops this sound.
        /// </summary>
        void Stop();

        /// <summary>
        /// Pauses this sound.
        /// </summary>
        void Pause();

        /// <summary>
        /// Resumes this sound.
        /// </summary>
        void Resume();
    }
}
