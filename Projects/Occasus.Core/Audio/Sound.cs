using System.Linq;
using Microsoft.Xna.Framework.Audio;

namespace Occasus.Core.Audio
{
    public class Sound : SoundBase
    {
        protected readonly SoundEffectInstance instance;

        /// <summary>
        /// Initializes a new instance of the <see cref="Sound"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="soundEffect">The sound effect.</param>
        public Sound(string id, SoundEffect soundEffect) 
            : base(id, soundEffect)
        {
            this.instance = this.SoundEffectInstances.First();
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

            this.instance.Stop(true);
            this.instance.Pan = AudioManager.CalculatePan(magnitude);
            this.instance.Volume = volume;
            this.instance.Play();
        }

        /// <summary>
        /// Stops this sound.
        /// </summary>
        public override void Stop()
        {
            if (this.instance.State != SoundState.Stopped)
            {
                this.instance.Stop(true);
            }
        }

        /// <summary>
        /// Pauses this sound.
        /// </summary>
        public override void Pause()
        {
            if (this.instance.State == SoundState.Playing)
            {
                this.instance.Pause();
            }
        }

        /// <summary>
        /// Resumes this sound.
        /// </summary>
        public override void Resume()
        {
            if (this.instance.State == SoundState.Paused)
            {
                this.instance.Resume();
            }
        }
    }
}
