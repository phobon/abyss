using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Abyss.World.Entities.Relics.Concrete
{
    public abstract class StompRelic : Relic
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StompRelic" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        /// <param name="initialPosition">The initial position.</param>
        /// <param name="boundingBox">The bounding box.</param>
        /// <param name="relevantEntityTags">The relevant entity tags.</param>
        /// <param name="activationChance">The activation chance.</param>
        protected StompRelic(
            string name, 
            string description, 
            Vector2 initialPosition, 
            Rectangle boundingBox, 
            IEnumerable<string> relevantEntityTags,
            int activationChance) 
            : base(
                  name, 
                  description, 
                  initialPosition, 
                  boundingBox, 
                  RelicType.Passive, 
                  RelicActivationType.Stomp, 
                  relevantEntityTags, 
                  activationChance, 
                  0)
        {
        }

        /// <summary>
        /// Gets or sets the current platform.
        /// </summary>
        /// <value>
        /// The current platform.
        /// </value>
        public Rectangle CurrentPlatform
        {
            get; set;
        }
    }
}
