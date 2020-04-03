using System;
using System.Collections.Generic;
using System.Linq;

using Occasus.Core.Maths;
using Microsoft.Xna.Framework;

namespace Abyss.World.Entities.Monsters
{
    public static class MonsterFactory
    {
        private const string Qualifier = "Abyss.World.Entities.Monsters.Concrete.";

        private static readonly List<string> Monsters = new List<string>
                                                   {
                                                       Qualifier + "Crawler",
                                                       Qualifier + "Faller",
                                                       Qualifier + "Floater",
                                                       Qualifier + "Idler",
                                                       Qualifier + "Jumper",
                                                       Qualifier + "Shooter",
                                                       Qualifier + "Stalker",
                                                       Qualifier + "Walker",
                                                       Qualifier + "Flyer"
                                                   };

        public static IMonster GetMonster(string id, Vector2 position, IEnumerable<Vector2> path)
        {
            var qualifiedName = Qualifier + id;
            
            if (path.Any())
            {
                return (IMonster)Activator.CreateInstance(Type.GetType(qualifiedName), position, path);
            }

            return (IMonster)Activator.CreateInstance(Type.GetType(qualifiedName), position);
        }
    }
}
