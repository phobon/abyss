using Abyss.World.Universe;
using Microsoft.Xna.Framework;
using Occasus.Core.Drawing.ParticleEffects;
using Occasus.Core.Entities;
using Occasus.Core.Maths;
using System;
using System.Collections.Generic;

namespace Abyss.World.Drawing.ParticleEffects
{
    public static class ParticleEffectFactory
    {
        private const string Qualifier = "Abyss.World.Drawing.ParticleEffects.Concrete.";

        private static readonly List<string> particleEffects = new List<string>
                                                                    {
                                                                        Qualifier + "Fire",
                                                                        Qualifier + "Sparkle"
                                                                    };

        private static readonly List<string> interfaceParticleEFfects = new List<string>
                                                                            {
                                                                                Qualifier + "Aura"
                                                                            };

        private static readonly Dictionary<ZoneType, List<string>> fullscreenParticleEffectsByZone = new Dictionary<ZoneType, List<string>>
                                                                                                   {
                                                                                                       { ZoneType.Normal, new List<string> { Qualifier + "Speck" } },
                                                                                                       { ZoneType.Ice, new List<string> { Qualifier + "Snow" } },
                                                                                                       { ZoneType.Electric, new List<string> { } },
                                                                                                       { ZoneType.Fire, new List<string> { } },
                                                                                                       { ZoneType.Shadow, new List<string> { } },
                                                                                                       { ZoneType.Void, new List<string> { } },
                                                                                                   };

        private static readonly Dictionary<string, List<Color>> fullscreenParticleEffectColors = new Dictionary<string, List<Color>>(StringComparer.OrdinalIgnoreCase)
                                                                                                     {
                                                                                                         { Qualifier + "Speck", new List<Color> { Color.Salmon, Color.Green, Color.GreenYellow, Color.Yellow, Color.Teal } },
                                                                                                         { Qualifier + "Snow", new List<Color> { Color.White, Color.LightSkyBlue, Color.Tomato } },
                                                                                                     };

        public static IParticleEffect GetParticleEffect(IEntity parent, string id)
        {
            var qualifiedName = Qualifier + id;
            var particleEffect = (ParticleEffect)Activator.CreateInstance(Type.GetType(qualifiedName), parent);
            particleEffect.Initialize();
            return particleEffect;
        }

        public static IParticleEffect GetParticleEffect(IEntity parent, string id, Vector2 offset)
        {
            var qualifiedName = Qualifier + id;
            var particleEffect = (ParticleEffect)Activator.CreateInstance(Type.GetType(qualifiedName), parent, offset);
            particleEffect.Initialize();
            return particleEffect;
        }

        public static IParticleEffect GetParticleEffect(IEntity parent, string id, Vector2 offset, Color color)
        {
            var qualifiedName = Qualifier + id;
            var particleEffect = (ParticleEffect)Activator.CreateInstance(Type.GetType(qualifiedName), parent, offset, color);
            particleEffect.Initialize();
            return particleEffect;
        }

        //public static SpriteParticleEffect GetParticleEffect(string id, Vector2 initialPosition, ISpriteDetails spriteDetails)
        //{
        //    var qualifiedName = Qualifier + id;
        //    var particleEffect = (SpriteParticleEffect)Activator.CreateInstance(Type.GetType(qualifiedName), initialPosition, spriteDetails);
        //    particleEffect.Initialize();
        //    return particleEffect;
        //}

        public static IInterfaceParticleEffect GetInterfaceParticleEffect(string id, ParticleDensity particleDensity, Color color)
        {
            var qualifiedName = Qualifier + id;
            var particleEffect = (IInterfaceParticleEffect)Activator.CreateInstance(Type.GetType(qualifiedName), particleDensity, color);
            return particleEffect;
        }

        public static IFullScreenParticleEffect GetFullScreenParticleEffect(string id, ParticleDensity particleDensity, Color color)
        {
            var qualifiedName = Qualifier + id;
            var particleEffect = (IFullScreenParticleEffect)Activator.CreateInstance(Type.GetType(qualifiedName), particleDensity, color);
            particleEffect.Initialize();
            return particleEffect;
        }

        public static IFullScreenParticleEffect GetFullScreenParticleEffect(ZoneType zoneType, ParticleDensity particleDensity)
        {
            var effects = fullscreenParticleEffectsByZone[zoneType];
            if (effects.Count == 0)
            {
                return null;
            }

            var qualifiedName = effects[MathsHelper.Random(effects.Count)];

            // Get a random color.
            var colors = fullscreenParticleEffectColors[qualifiedName];
            var color = colors[MathsHelper.Random(colors.Count)];

            var particleEffect = (IFullScreenParticleEffect)Activator.CreateInstance(Type.GetType(qualifiedName), particleDensity, color);
            particleEffect.Initialize();
            return particleEffect;
        }
    }
}
