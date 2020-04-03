using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Abyss.World.Entities.Relics.Concrete
{
    public abstract class ActiveRelic : Relic
    {
        protected ActiveRelic(
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
                  RelicType.Active, 
                  RelicActivationType.Explicit, 
                  relevantEntityTags, 
                  100, 
                  0)
        {
        }
    }
}
