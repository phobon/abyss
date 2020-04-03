using Abyss.World.Entities.Relics;
using Abyss.World.Universe;
using Microsoft.Xna.Framework;
using Occasus.Core.Maths;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Abyss.World.Entities.Items
{
    public static class ItemFactory
    {
        private const string BaseQualifier = "Abyss.World.Entities.Items.Concrete.";
        private const string RiftShardQualifier = "Abyss.World.Entities.Items.Concrete.RiftShards.";

        private static readonly List<string> Items = new List<string>
                                                   {
                                                       //BaseQualifier + "Hourglass",
                                                       BaseQualifier + "Heart"
                                                   };

        public static IItem GetItemById(string id, Vector2 position)
        {
            var qualifiedName = BaseQualifier + id;
            return (IItem)Activator.CreateInstance(Type.GetType(qualifiedName), position);
        }

        public static IItem GetRandomItem(Vector2 position)
        {
            var randomItemId = Items.ElementAt(MathsHelper.Random(Items.Count));
            var randomItem = (IItem)Activator.CreateInstance(Type.GetType(randomItemId), position);
            return randomItem;
        }

        public static IItem GetRandomTreasureItem(Vector2 position)
        {
            // If the player is lucky, they get a random relic instead of just a regular piece of treasure.
            var r = MathsHelper.Random();
            if (r > 85)
            {
                return RelicFactory.GetRandomPassiveRelic(position);
            }

            // Determine whether we get some other cool item.
            r = MathsHelper.Random();
            if (r < 50)
            {
                var shard = GetRiftShard("SmallRiftShard", position);
                shard.Initialize();
                return shard;
            }
            
            if (r < 75)
            {
                var shard = GetRiftShard("LargeRiftShard", position);
                shard.Initialize();
                return shard;
            }

            // If we reach this point, the player gets a heart for their troubles.
            var item = GetRandomItem(position);
            item.Initialize();
            return item;
        }

        public static IItem GetRandomShopItem(Vector2 position)
        {
            var r = MathsHelper.Random();
            if (r < 65)
            {
                return RelicFactory.GetRandomActiveRelic(position);
            }

            var item = GetRandomItem(position);
            item.Initialize();
            return item;
        }

        public static IItem GetRiftShard(string id, Vector2 position)
        {
            var qualifiedName = RiftShardQualifier + id;
            return (IItem)Activator.CreateInstance(Type.GetType(qualifiedName), position);
        }
    }
}
