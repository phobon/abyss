using System;
using System.Collections.Generic;

namespace Abyss.World.Entities.Platforms
{
    public static class PlatformFactory
    {
        private const string Qualifier = "Abyss.World.Entities.Platforms.Concrete.";
        private const string DynamicQualifier = Qualifier + "Dynamic.";
        private const string ActivatedQualifier = Qualifier + "Activated.";

        private static readonly IDictionary<string, string> platformsQualifiers = new Dictionary<string, string>
        {
            { PlatformKeys.Crushing, DynamicQualifier + PlatformKeys.Crushing },
            { PlatformKeys.Moving, DynamicQualifier + PlatformKeys.Moving },
            { PlatformKeys.Phasing, DynamicQualifier + PlatformKeys.Phasing },
            { PlatformKeys.Crumbling, ActivatedQualifier + PlatformKeys.Crumbling },
            { PlatformKeys.Exploding, ActivatedQualifier + PlatformKeys.Exploding },
            { PlatformKeys.Gate, ActivatedQualifier + PlatformKeys.Gate },
            { PlatformKeys.Icy, ActivatedQualifier + PlatformKeys.Icy },
            { PlatformKeys.Key, ActivatedQualifier + PlatformKeys.Key },
            { PlatformKeys.Spikey, ActivatedQualifier + PlatformKeys.Spikey },
            { PlatformKeys.Warping, ActivatedQualifier + PlatformKeys.Warping }
        };

        public static IPlatform GetPlatform(IPlatformDefinition platformDefinition)
        {
            var qualifiedName = platformsQualifiers[platformDefinition.Name];
            return (IPlatform)Activator.CreateInstance(Type.GetType(qualifiedName), platformDefinition);
        }
    }
}
