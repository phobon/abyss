using Microsoft.Xna.Framework.Audio;
using System.Linq;

namespace Occasus.Core.Audio
{
    public class InstancedSound : SoundBase
    {
        private int lastPlayedIndex;

        /// <summary>
        /// Initializes a new instance of the <see cref="InstancedSound" /> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="soundEffect">The sound effect.</param>
        /// <param name="instances">The instances.</param>
        public InstancedSound(string id, SoundEffect soundEffect, int instances = 2) 
            : base(id, soundEffect, instances)
        {
        }

        /// <summary>
        /// Plays this sound.
        /// </summary>
        /// <param name="magnitude">The magnitude.</param>
        /// <param name="volume">The volume.</param>
        public override void Play(float magnitude = 160, float volume = 1)
        {
            if (AudioManager.MasterVolume.Equals(0f))
            {
                return;
            }

            volume *= AudioManager.MasterVolume;

            var instanceToPlay = this.SoundEffectInstances.ElementAt(this.lastPlayedIndex);
            this.lastPlayedIndex = (this.lastPlayedIndex + 1) % this.SoundEffectInstances.Count();

            instanceToPlay.Stop(true);
            instanceToPlay.Pan = AudioManager.CalculatePan(magnitude);
            instanceToPlay.Volume = volume;
            instanceToPlay.Play();
        }

        /// <summary>
        /// Stops this sound.
        /// </summary>
        public override void Stop()
        {
            foreach (var s in this.SoundEffectInstances)
            {
                if (s.State != SoundState.Stopped)
                {
                    s.Stop();
                }
            }
        }

        /// <summary>
        /// Pauses this sound.
        /// </summary>
        public override void Pause()
        {
            foreach (var s in this.SoundEffectInstances)
            {
                if (s.State == SoundState.Playing)
                {
                    s.Pause();
                }
            }
        }

        /// <summary>
        /// Resumes this sound.
        /// </summary>
        public override void Resume()
        {
            foreach (var s in this.SoundEffectInstances)
            {
                if (s.State == SoundState.Paused)
                {
                    s.Resume();
                }
            }
        }
    }
}
