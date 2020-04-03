using Microsoft.Xna.Framework.Audio;

namespace Occasus.Core.Audio
{
    public static class SoundEffectExtensions
    {
        public static ISound ToSound(this SoundEffect s, string id)
        {
            return new Sound(id, s);
        }

        public static ISound ToInstancedSound(this SoundEffect s, string id, int instances)
        {
            return new InstancedSound(id, s, instances);
        }

        public static ISound ToLoopingSound(this SoundEffect s, string id)
        {
            return new LoopingSound(id, s);
        }
    }
}
