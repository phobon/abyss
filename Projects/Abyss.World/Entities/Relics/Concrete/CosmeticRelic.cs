using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Abyss.World.Entities.Relics.Concrete
{
    public abstract class CosmeticRelic : Relic
    {
        protected CosmeticRelic(
            string name, 
            string description, 
            Vector2 initialPosition, 
            Rectangle boundingBox, 
            IEnumerable<string> relevantEntityTags) 
            : base(
                  name, 
                  description, 
                  initialPosition, 
                  boundingBox, 
                  RelicType.Cosmetic, 
                  RelicActivationType.None, 
                  relevantEntityTags, 
                  100, 
                  0)
        {
        }
    }
}
