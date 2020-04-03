using System;
using Microsoft.Xna.Framework;
using Occasus.Core.Entities;

namespace Abyss.World.Entities.Triggers
{
    public static class TriggerFactory
    {
        private const string Qualifier = "Abyss.World.Entities.Triggers.Concrete.";

        public static ITrigger GetTrigger(string id, Vector2 initialPosition, Rectangle boundingRectangle)
        {
            var qualifiedName = Qualifier + id;
            var t = (ITrigger)Activator.CreateInstance(Type.GetType(qualifiedName), initialPosition, boundingRectangle);
            t.Initialize();
            return t;
        }
    }
}
