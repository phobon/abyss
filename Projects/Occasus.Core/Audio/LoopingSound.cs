using Microsoft.Xna.Framework.Audio;

namespace Occasus.Core.Audio
{
    public class LoopingSound : Sound
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LoopingSound" /> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="soundEffect">The sound effect.</param>
        public LoopingSound(string id, SoundEffect soundEffect) 
            : base(id, soundEffect)
        {
            this.instance.IsLooped = true;
        }
    }
}
