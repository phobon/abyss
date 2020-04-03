using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;

namespace Occasus.Core.Audio
{
    public abstract class SoundBase : ISound
    {
        private readonly IList<SoundEffectInstance> soundEffectInstances;

        /// <summary>
        /// Initializes a new instance of the <see cref="SoundBase"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="soundEffect">The sound effect.</param>
        /// <param name="instances">The instances.</param>
        protected SoundBase(string id, SoundEffect soundEffect, int instances = 1)
        {
            this.Id = id;
            this.SoundEffect = soundEffect;
            this.soundEffectInstances = new List<SoundEffectInstance>();
            for (var i = 0; i < instances; i++)
            {
                this.soundEffectInstances.Add(this.SoundEffect.CreateInstance());
            }
        }

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        public string Id
        {
            get; private set;
        }

        /// <summary>
        /// Gets the sound effect.
        /// </summary>
        public SoundEffect SoundEffect
        {
            get; private set;
        }

        /// <summary>
        /// Gets the sound effect instances.
        /// </summary>
        public IEnumerable<SoundEffectInstance> SoundEffectInstances
        {
            get
            {
                return this.soundEffectInstances;
            }
        }

        /// <summary>
        /// Plays this sound.
        /// </summary>
        /// <param name="magnitude">The magnitude.</param>
        /// <param name="volume">The volume.</param>
        public abstract void Play(float magnitude = 160f, float volume = 1f);

        /// <summary>
        /// Stops this sound.
        /// </summary>
        public abstract void Stop();

        /// <summary>
        /// Pauses this sound.
        /// </summary>
        public abstract void Pause();

        /// <summary>
        /// Resumes this sound.
        /// </summary>
        public abstract void Resume();

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.SoundEffect.Dispose();

            foreach (var s in this.SoundEffectInstances)
            {
                s.Dispose();
            }

            this.soundEffectInstances.Clear();
        }
    }
}
