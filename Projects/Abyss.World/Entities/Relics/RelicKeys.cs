using System.Collections.Generic;

namespace Abyss.World.Entities.Relics
{
    public static class RelicKeys
    {
        public const string Conduit = "Conduit";
        public const string Agility = "Agility";
        public const string Vitality = "Vitality";

        public const string Spelunker = "Spelunker";
        public const string TreasureSeeker = "Treasure Seeker";
        public const string HeartSeeker = "Heart Seeker";

        public const string Crusher = "Crusher";
        public const string Shield = "Shield";
        public const string Magnet = "Magnet";
        public const string Parachute = "Parachute";
        public const string Vampire = "Vampire";
        public const string Drain = "Drain";

        // Passive relics.
        public static IDictionary<string, IRelic> PassiveRelics = new Dictionary<string, IRelic>
        {
            { Conduit, null },
            { Agility, null },
            { Vitality, null },
            { Spelunker, null },
            { TreasureSeeker, null },
            { HeartSeeker, null },
            { Crusher, null },
            { Shield, null },
            { Magnet, null },
            { Parachute, null },
            { Vampire, null },
            { Drain, null }
        };

        public const string Vortex = "Vortex";
        public const string Dash = "Dash";
        public const string Phase = "Phase";
        public const string Warp = "Warp";
        public const string Rumble = "Rumble";

        // Active relics.
        public static IDictionary<string, IRelic> ActiveRelics = new Dictionary<string, IRelic>
        {
            { Vortex, null },
            { Dash, null },
            { Phase, null },
            { Warp, null },
            { Rumble, null },
            { HeartSeeker, null }
        };

        // Cosmetic relics.

        public const string RainbowTrail = "RainbowTrail";
        public const string SparkleTrail = "SparkleTrail";
        public const string FireTrail = "FireTrail";
        public const string ShadowTrail = "ShadowTrail";

        public const string RoyalCostume = "RoyalCostume";
        public const string VagrantCostume = "VagrantCostume";
        public const string MonsterCostume = "MonsterCostume";

        public static IDictionary<string, IRelic> CosmeticRelics = new Dictionary<string, IRelic>
        {
            { RainbowTrail, null },
            { SparkleTrail, null },
            { FireTrail, null },
            { ShadowTrail, null },
            { RoyalCostume, null },
            { VagrantCostume, null },
            { MonsterCostume, null }
        };

        //// Key relics
        //public const string ArgusKey = "Argus";
        //public const string DioninKey = "Dionin";
        //public const string ValusKey = "Valus";
        //public const string PhobonKey = "Phobon";

        //public static IDictionary<string, IRelic> KeyRelics = new Dictionary<string, IRelic>
        //{
        //    { ArgusKey, null },
        //    { DioninKey, null },
        //    { ValusKey, null },
        //    { PhobonKey, null },
        //};

        //// Persistent relics
        //public const string Conduit = "Conduit";
        //public const string Shift = "Shift";
        //public const string SpeedUp = "SpeedUp";
        //public const string Vampire = "Vampire";
        //public const string Vitality = "Vitality";
        //public const string Stability = "Stability";
        //public const string Wallet = "Wallet";

        //public static IDictionary<string, IRelic> PersistentRelics = new Dictionary<string, IRelic>
        //{
        //    { Conduit, null },
        //    { Shift, null },
        //    { SpeedUp, null },
        //    { Vampire, null },
        //    { Vitality, null },
        //    { Stability, null},
        //    { Wallet, null},
        //};

        //// Powerup relics
        //public const string Cataclysm = "Cataclysm";
        //public const string Invulnerability = "Invulnerability";
        //public const string Shield = "Shield";
        //public const string Abundance = "Abundance";
        //public const string Merge = "Merge";

        //public static IDictionary<string, IRelic> PowerupRelics = new Dictionary<string, IRelic>
        //{
        //    { Cataclysm, null },
        //    { Invulnerability, null },
        //    { Shield, null },
        //    { Abundance, null },
        //    { Merge, null }
        //};

        //// Boost relics
        //public const string CataclysmBoost = "CataclysmBoost";
        //public const string InvulnerabilityBoost = "InvulnerabilityBoost";
        //public const string ShieldBoost = "ShieldBoost";
        //public const string SlowTimeBoost = "SlowTimeBoost";

        //// Passive relics
        //public const string Firewalker = "Firewalker";
        //public const string Riftwalker = "Riftwalker";
        //public const string Voidwalker = "Voidwalker";

        //// Stomp relics
        //public const string ChainReaction = "ChainReaction";
        //public const string Coalesce = "Coalesce";
        //public const string Spelunker = "Spelunker";
        //public const string TreasureSeeker = "TreasureSeeker";

        //// Dimension Shift relics
        //public const string Magnet = "Magnet";
        //public const string Phased = "Phased";
        //public const string Vortex = "Vortex";
        //public const string Warp = "Warp";

        //public static IDictionary<string, IRelic> TransientRelics = new Dictionary<string, IRelic>
        //{
        //    { CataclysmBoost, null },
        //    { InvulnerabilityBoost, null },
        //    { ShieldBoost, null },
        //    { SlowTimeBoost, null },

        //    { Firewalker, null },
        //    { Riftwalker, null },
        //    { Voidwalker, null },

        //    { ChainReaction, null },
        //    { Coalesce, null },
        //    { Spelunker, null },
        //    { TreasureSeeker, null },

        //    { Magnet, null },
        //    { Phased, null },
        //    { Vortex, null },
        //    { Warp, null }
        //};
    }
}
