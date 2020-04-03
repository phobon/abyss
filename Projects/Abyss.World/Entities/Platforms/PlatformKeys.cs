using System.Collections.Generic;

namespace Abyss.World.Entities.Platforms
{
    public static class PlatformKeys
    {
        // Activated platforms.
        public const string Crumbling = "Crumbling";
        public const string Spikey = "Spikey";
        public const string Icy = "Icy";
        public const string Warping = "Warping";
        public const string Key = "Key";
        public const string Exploding = "Exploding";
        public const string Gate = "Gate";

        public static IDictionary<string, IPlatform> ActivatedPlatforms = new Dictionary<string, IPlatform>
        {
            { Crumbling, null },
            { Spikey, null },
            { Icy, null },
            { Warping, null },
            { Key, null },
            { Exploding, null },
            { Gate, null }
        };

        // Dynamic platforms.
        public const string Moving = "Moving";
        public const string Phasing = "Phasing";
        public const string Crushing = "Crushing";

        public static IDictionary<string, IPlatform> DynamicPlatforms = new Dictionary<string, IPlatform>
        {
            { Moving, null },
            { Phasing, null },
            { Crushing, null }
        };
    }
}
