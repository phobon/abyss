using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Occasus.Core.Maths;

namespace Abyss.World.Entities.Relics
{
    public static class RelicFactory
    {
        private const string BaseQualifier = "Abyss.World.Entities.Relics.Concrete.";

        //private const string KeyRelicQualifier = BaseQualifier + "Key.";
        //private const string PersistentRelicQualifier = BaseQualifier + "Persistent.";
        //private const string PowerUpRelicQualifier = BaseQualifier + "PowerUp.";
        //private const string TransientQualifier = BaseQualifier + "Transient.";

        private const string PassiveRelicQualifier = BaseQualifier + "Passive.";
        private const string ActiveRelicQualifier = BaseQualifier + "Passive.";
        private const string CosmeticRelicQualifier = BaseQualifier + "Passive.";

        private static readonly IDictionary<string, string> passiveRelics = new Dictionary<string, string>
        {
            { RelicKeys.Conduit, PassiveRelicQualifier + RelicKeys.Conduit },
            { RelicKeys.Agility, PassiveRelicQualifier + RelicKeys.Agility },
            { RelicKeys.Vitality, PassiveRelicQualifier + RelicKeys.Vitality },
            { RelicKeys.Spelunker, PassiveRelicQualifier + RelicKeys.Spelunker },
            { RelicKeys.TreasureSeeker, PassiveRelicQualifier + RelicKeys.TreasureSeeker },
            { RelicKeys.HeartSeeker, PassiveRelicQualifier + RelicKeys.HeartSeeker },
            { RelicKeys.Crusher, PassiveRelicQualifier + RelicKeys.Crusher },
            { RelicKeys.Shield, PassiveRelicQualifier + RelicKeys.Shield },
            { RelicKeys.Magnet, PassiveRelicQualifier + RelicKeys.Magnet },
            { RelicKeys.Parachute, PassiveRelicQualifier + RelicKeys.Parachute },
            { RelicKeys.Vampire, PassiveRelicQualifier + RelicKeys.Vampire },
            { RelicKeys.Drain, PassiveRelicQualifier + RelicKeys.Drain },
        };

        private static readonly IDictionary<string, string> activeRelics = new Dictionary<string, string>
        {
            { RelicKeys.Vortex, ActiveRelicQualifier + RelicKeys.Vortex },
            { RelicKeys.Dash, ActiveRelicQualifier + RelicKeys.Dash },
            { RelicKeys.Phase, ActiveRelicQualifier + RelicKeys.Phase },
            { RelicKeys.Warp, ActiveRelicQualifier + RelicKeys.Warp },
            { RelicKeys.Rumble, ActiveRelicQualifier + RelicKeys.Rumble }
        };
        
        private static readonly IDictionary<string, string> cosmeticRelics = new Dictionary<string, string>
        {
            { RelicKeys.RainbowTrail, CosmeticRelicQualifier + RelicKeys.RainbowTrail },
            { RelicKeys.SparkleTrail, CosmeticRelicQualifier + RelicKeys.SparkleTrail },
            { RelicKeys.FireTrail, CosmeticRelicQualifier + RelicKeys.FireTrail },
            { RelicKeys.ShadowTrail, CosmeticRelicQualifier + RelicKeys.ShadowTrail },
            { RelicKeys.RoyalCostume, CosmeticRelicQualifier + RelicKeys.RoyalCostume },
            { RelicKeys.VagrantCostume, CosmeticRelicQualifier + RelicKeys.VagrantCostume },
            { RelicKeys.MonsterCostume, CosmeticRelicQualifier + RelicKeys.MonsterCostume }
        };

        //private static readonly IDictionary<string, string> keyRelics = new Dictionary<string, string>
        //{
        //    { RelicKeys.ArgusKey, KeyRelicQualifier + RelicKeys.ArgusKey },
        //    { RelicKeys.DioninKey, KeyRelicQualifier + RelicKeys.DioninKey },
        //    { RelicKeys.PhobonKey, KeyRelicQualifier + RelicKeys.PhobonKey },
        //    { RelicKeys.ValusKey, KeyRelicQualifier + RelicKeys.ValusKey }
        //};

        //private static readonly IDictionary<string, string> persistentRelics = new Dictionary<string, string>
        //{
        //    { RelicKeys.Conduit, PersistentRelicQualifier + RelicKeys.Conduit },
        //    { RelicKeys.Shift, PersistentRelicQualifier + RelicKeys.Shift },
        //    { RelicKeys.SpeedUp, PersistentRelicQualifier + RelicKeys.SpeedUp },
        //    { RelicKeys.Vampire, PersistentRelicQualifier + RelicKeys.Vampire },
        //    { RelicKeys.Vitality, PersistentRelicQualifier + RelicKeys.Vitality },
        //    { RelicKeys.Stability, PersistentRelicQualifier + RelicKeys.Stability },
        //    { RelicKeys.Wallet, PersistentRelicQualifier + RelicKeys.Wallet }
        //};

        //private static readonly IDictionary<string, string> powerUpRelics = new Dictionary<string, string>
        //{
        //    { RelicKeys.Cataclysm, PowerUpRelicQualifier + RelicKeys.Cataclysm },
        //    { RelicKeys.Invulnerability, PowerUpRelicQualifier + RelicKeys.Invulnerability },
        //    { RelicKeys.Shield, PowerUpRelicQualifier + RelicKeys.Shield },
        //    { RelicKeys.Abundance, PowerUpRelicQualifier + RelicKeys.Abundance },
        //    { RelicKeys.Merge, PowerUpRelicQualifier + RelicKeys.Merge },
        //};

        //private static readonly IDictionary<string, string> transientRelics = new Dictionary<string, string>
        //{
        //    { RelicKeys.CataclysmBoost, TransientQualifier + RelicKeys.CataclysmBoost },
        //    { RelicKeys.ChainReaction, TransientQualifier + RelicKeys.ChainReaction },
        //    { RelicKeys.Coalesce, TransientQualifier + RelicKeys.Coalesce },
        //    { RelicKeys.Firewalker, TransientQualifier + RelicKeys.Firewalker },
        //    { RelicKeys.InvulnerabilityBoost, TransientQualifier + RelicKeys.InvulnerabilityBoost },
        //    { RelicKeys.Magnet, TransientQualifier + RelicKeys.Magnet },
        //    { RelicKeys.Riftwalker, TransientQualifier + RelicKeys.Riftwalker },
        //    { RelicKeys.ShieldBoost, TransientQualifier + RelicKeys.ShieldBoost },
        //    { RelicKeys.Spelunker, TransientQualifier + RelicKeys.Spelunker },
        //    { RelicKeys.TreasureSeeker, TransientQualifier + RelicKeys.TreasureSeeker },
        //    { RelicKeys.Voidwalker, TransientQualifier + RelicKeys.Voidwalker },
        //    { RelicKeys.Vortex, TransientQualifier + RelicKeys.Vortex },
        //    { RelicKeys.Warp, TransientQualifier + RelicKeys.Warp }
        //};

        //private static readonly IDictionary<string, string> allRelics = new Dictionary<string, string>
        //                                                                      {
        //                                                                          { RelicKeys.ArgusKey, KeyRelicQualifier + RelicKeys.ArgusKey },
        //                                                                          { RelicKeys.DioninKey, KeyRelicQualifier + RelicKeys.DioninKey },
        //                                                                          { RelicKeys.PhobonKey, KeyRelicQualifier + RelicKeys.PhobonKey },
        //                                                                          { RelicKeys.ValusKey, KeyRelicQualifier + RelicKeys.ValusKey },

        //                                                                          { RelicKeys.Conduit, PersistentRelicQualifier + RelicKeys.Conduit },
        //                                                                          { RelicKeys.Shift, PersistentRelicQualifier + RelicKeys.Shift },
        //                                                                          { RelicKeys.SpeedUp, PersistentRelicQualifier + RelicKeys.SpeedUp },
        //                                                                          { RelicKeys.Vampire, PersistentRelicQualifier + RelicKeys.Vampire },
        //                                                                          { RelicKeys.Vitality, PersistentRelicQualifier + RelicKeys.Vitality },
        //                                                                          { RelicKeys.Stability, PersistentRelicQualifier + RelicKeys.Stability },
        //                                                                          { RelicKeys.Wallet, PersistentRelicQualifier + RelicKeys.Wallet },

        //                                                                          { RelicKeys.Cataclysm, PowerUpRelicQualifier + RelicKeys.Cataclysm },
        //                                                                          { RelicKeys.Invulnerability, PowerUpRelicQualifier + RelicKeys.Invulnerability },
        //                                                                          { RelicKeys.Shield, PowerUpRelicQualifier + RelicKeys.Shield },
        //                                                                          { RelicKeys.Abundance, PowerUpRelicQualifier + RelicKeys.Abundance },
        //                                                                          { RelicKeys.Merge, PowerUpRelicQualifier + RelicKeys.Merge },

        //                                                                          { RelicKeys.CataclysmBoost, TransientQualifier + RelicKeys.CataclysmBoost },
        //                                                                          { RelicKeys.ChainReaction, TransientQualifier + RelicKeys.ChainReaction },
        //                                                                          { RelicKeys.Coalesce, TransientQualifier + RelicKeys.Coalesce },
        //                                                                          { RelicKeys.Firewalker, TransientQualifier + RelicKeys.Firewalker },
        //                                                                          { RelicKeys.InvulnerabilityBoost, TransientQualifier + RelicKeys.InvulnerabilityBoost },
        //                                                                          { RelicKeys.Magnet, TransientQualifier + RelicKeys.Magnet },
        //                                                                          { RelicKeys.Riftwalker, TransientQualifier + RelicKeys.Riftwalker },
        //                                                                          { RelicKeys.ShieldBoost, TransientQualifier + RelicKeys.ShieldBoost },
        //                                                                          { RelicKeys.Spelunker, TransientQualifier + RelicKeys.Spelunker },
        //                                                                          { RelicKeys.TreasureSeeker, TransientQualifier + RelicKeys.TreasureSeeker },
        //                                                                          { RelicKeys.Voidwalker, TransientQualifier + RelicKeys.Voidwalker },
        //                                                                          { RelicKeys.Vortex, TransientQualifier + RelicKeys.Vortex },
        //                                                                          { RelicKeys.Warp, TransientQualifier + RelicKeys.Warp }
        //                                                                      };

        private static IDictionary<string, string> allRelics;

        private static IDictionary<string, string> AllRelics
        {
            get
            {
                if (allRelics == null)
                {
                    allRelics = new Dictionary<string, string>();
                    foreach (var relic in passiveRelics)
                    {
                        allRelics.Add(relic.Key, relic.Value);
                    }

                    foreach (var relic in activeRelics)
                    {
                        allRelics.Add(relic.Key, relic.Value);
                    }

                    foreach (var relic in cosmeticRelics)
                    {
                        allRelics.Add(relic.Key, relic.Value);
                    }
                }

                return allRelics;
            }
        }

        public static IRelic GetRelicById(string id, Vector2 initialPosition)
        {
            var qualifiedName = allRelics[id];
            var relic = (IRelic)Activator.CreateInstance(Type.GetType(qualifiedName), initialPosition);
            relic.Initialize();
            return relic;
        }

        public static IRelic GetRandomPassiveRelic(Vector2 initialPosition)
        {
            return GetRandomRelic(initialPosition, passiveRelics);
        }

        public static IRelic GetRandomActiveRelic(Vector2 initialPosition)
        {
            return GetRandomRelic(initialPosition, activeRelics);
        }

        public static IRelic GetRandomCosmeticRelic(Vector2 initialPosition)
        {
            return GetRandomRelic(initialPosition, cosmeticRelics);
        }

        //public static IRelic GetRandomTransientRelic(Vector2 initialPosition)
        //{
        //    return GetRandomRelic(initialPosition, transientRelics);
        //}

        private static IRelic GetRandomRelic(Vector2 initialPosition, IDictionary<string, string> relics)
        {
            var randomRelicId = relics.Values.ElementAt(MathsHelper.Random(relics.Count));
            var randomRelic = (IRelic)Activator.CreateInstance(Type.GetType(randomRelicId), initialPosition);
            randomRelic.Initialize();
            return randomRelic;
        }
    }
}
