using System.Linq;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Occasus.Core.Drawing;

namespace Occasus.Core.Audio
{
    public static class AudioManager
    {
        private const float PanThreshold = 0.5f;
        private const float PanMagnitudeFactor = 320f;

        private static readonly IDictionary<string, SoundEffect> soundEffectCache = new Dictionary<string, SoundEffect>();
        private static readonly IDictionary<string, ISound> allSounds = new Dictionary<string, ISound>(StringComparer.InvariantCultureIgnoreCase);
        private static IDictionary<SoundType, IDictionary<string, ISound>> sounds;

        private static float masterVolume;

        /// <summary>
        /// Gets or sets the master volume.
        /// </summary>
        /// <value>
        /// The master volume.
        /// </value>
        public static float MasterVolume
        {
            get
            {
                return masterVolume;
            }

            set
            {
                if (masterVolume.Equals(value))
                {
                    return;
                }

                masterVolume = MathHelper.Clamp(value, 0, 1);
                NormalisedMasterVolume = (int)Math.Round(masterVolume * 10f);
            }
        }

        /// <summary>
        /// Gets the normalised master volume.
        /// </summary>
        public static int NormalisedMasterVolume
        {
            get; private set;
        }

        /// <summary>
        /// Gets the sound effect instances.
        /// </summary>
        public static IDictionary<SoundType, IDictionary<string, ISound>> Sounds
        {
            get
            {
                if (sounds == null)
                {
                    sounds = new Dictionary<SoundType, IDictionary<string, ISound>>
                    {
                        { SoundType.Basic, new Dictionary<string, ISound>() },
                        { SoundType.Instanced, new Dictionary<string, ISound>() },
                        { SoundType.Looping, new Dictionary<string, ISound>() }
                    };
                }

                return sounds;
            }
        }

        /// <summary>
        /// Plays the specified sound effect.
        /// </summary>
        /// <param name="effectId">The effect identifier.</param>
        /// <param name="soundType">Type of the sound.</param>
        /// <param name="magnitude">The magnitude.</param>
        /// <param name="volume">The volume.</param>
        public static void Play(string effectId, SoundType soundType, float magnitude = 160, float volume = 1f)
        {
            var soundsByType = Sounds[soundType];
            if (!soundsByType.ContainsKey(effectId))
            {
                // Create the sound.
                AddSound(effectId, soundType);
            }

            var sound = soundsByType[effectId];
            sound.Play(magnitude, volume);
        }

        /// <summary>
        /// Plays the specified effect identifier.
        /// </summary>
        /// <param name="effectId">The effect identifier.</param>
        /// <param name="magnitude">The magnitude.</param>
        /// <param name="volume">The volume.</param>
        public static void Play(string effectId, float magnitude = 160, float volume = 1f)
        {
            if (!allSounds.ContainsKey(effectId))
            {
                throw new InvalidOperationException(string.Format("Sound does not exist in AudioManager: {0}", effectId));
            }

            var sound = allSounds[effectId];
            sound.Play(magnitude, volume);
        }

        /// <summary>
        /// Stops the specified sound effect.
        /// </summary>
        /// <param name="effectId">The effect identifier.</param>
        public static void Stop(string effectId)
        {
            foreach (var s in Sounds.Values)
            {
                if (s.ContainsKey(effectId))
                {
                    s[effectId].Stop();
                }
            }
        }

        /// <summary>
        /// Stops all sound effects.
        /// </summary>
        public static void StopAll()
        {
            foreach (var s in Sounds.Values)
            {
                foreach (var v in s.Values)
                {
                    v.Stop();
                }
            }
        }

        /// <summary>
        /// Pauses all playing sound effects.
        /// </summary>
        public static void PauseAll()
        {
            foreach (var s in Sounds.Values)
            {
                foreach (var v in s.Values)
                {
                    v.Pause();
                }
            }
        }

        /// <summary>
        /// Resumes all paused sound effects.
        /// </summary>
        public static void ResumeAll()
        {
            foreach (var s in Sounds.Values)
            {
                foreach (var v in s.Values)
                {
                    v.Resume();
                }
            }
        }

        public static void LoadSoundData(string uri)
        {
            var stream = TitleContainer.OpenStream(uri);
            var doc = XDocument.Load(stream);

            var atlasNode = doc.Descendants("SoundAtlas").Single();

            // Load sounds
            foreach (var s in atlasNode.Descendants("Sounds").Descendants("Sound"))
            {
                LoadSound(s);
            }

            // Load songs
        }

        /// <summary>
        /// Loads sound data given a specified URI.
        /// </summary>
        /// <param name="uri">The URI.</param>
        public static void Load(string uri)
        {
            if (!Directory.Exists(uri))
            {
                throw new InvalidOperationException("Audio directory does not exist.");
            }

            var prefix = uri + @"\";
            foreach (var file in Directory.EnumerateFiles(uri, "*.wav", SearchOption.AllDirectories))
            {
                var name = file.Remove(0, prefix.Length);
                name = name.Remove(name.IndexOf(".wav", StringComparison.InvariantCultureIgnoreCase));

                var stream = new FileStream(file, FileMode.Open);
                var sound = SoundEffect.FromStream(stream);
                stream.Close();

                // Add to the cache and create basic sounds for each of these sounds.
                // TODO: Maybe flyweight this.
                soundEffectCache.Add(name, sound);
                Sounds[SoundType.Basic].Add(name, sound.ToSound(name));
            }
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        public static void Dispose()
        {
            soundEffectCache.Clear();

            foreach (var s in Sounds.Values)
            {
                foreach (var v in s.Values)
                {
                    v.Dispose();
                }
            }

            Sounds.Clear();
        }

        /// <summary>
        /// Calculates the pan.
        /// </summary>
        /// <param name="magnitude">The magnitude.</param>
        /// <returns>A value representing the pan.</returns>
        public static float CalculatePan(float magnitude)
        {
            return MathHelper.Lerp(-PanThreshold, PanThreshold, magnitude / PanMagnitudeFactor);
        }

        /// <summary>
        /// Calculates the pan magnitude.
        /// </summary>
        /// <param name="pan">The pan.</param>
        /// <returns>A value representing the pan magnitude.</returns>
        public static float CalculatePanMagnitude(float pan)
        {
            return MathHelper.Lerp(0, PanMagnitudeFactor, pan + PanThreshold);
        }

        private static void LoadSound(XElement e)
        {
            var id = e.Attribute("id").Value;
            var location = e.Attribute("location").Value;
            var type = (SoundType)(Enum.Parse(typeof(SoundType), e.Attribute("type").Value));

            ISound sound = null;
            switch (type)
            {
                case SoundType.Basic:
                    sound = DrawingManager.ContentManager.Load<SoundEffect>(location).ToSound(id);
                    break;
                case SoundType.Instanced:
                    // Determine number of instances.
                    var instances = int.Parse(e.Attribute("instances").Value);
                    sound = DrawingManager.ContentManager.Load<SoundEffect>(location).ToInstancedSound(id, instances);
                    break;
                case SoundType.Looping:
                    sound = DrawingManager.ContentManager.Load<SoundEffect>(location).ToLoopingSound(id);
                    break;
            }

            if (sound == null)
            {
                throw new InvalidOperationException(string.Format("Sound could not be created: {0}", id));
            }
            
            allSounds.Add(id, sound);
            Sounds[type].Add(id, sound);
        }

        private static void AddSound(string id, SoundType soundType, int instances = 2)
        {
            if (Sounds[soundType].ContainsKey(id))
            {
                throw new InvalidOperationException("Cannot add a duplicate sound of the same type.");
            }

            switch (soundType)
            {
                case SoundType.Basic:
                    break;
                case SoundType.Instanced:
                    Sounds[soundType].Add(id, soundEffectCache[id].ToInstancedSound(id, instances));
                    break;
                case SoundType.Looping:
                    Sounds[soundType].Add(id, soundEffectCache[id].ToLoopingSound(id));
                    break;
            }
        }
    }
}
