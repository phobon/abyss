using Microsoft.Xna.Framework;

using Occasus.Core.Assets;
using Occasus.Core.Drawing.Sprites;
using Occasus.Core.Entities;

namespace Occasus.Core.Drawing.Interface
{
    public class InterfaceElement : Entity, IInterfaceElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InterfaceElement" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        /// <param name="initialPosition">The initial position.</param>
        public InterfaceElement(
            string name, 
            string description,
            Vector2 initialPosition)
            : base(name, description)
        {
            this.Transform.Position = initialPosition;

            // Add tags.
            this.Tags.Add("InterfaceElement");

            this.Components.Add(Sprite.Tag, Atlas.GetSprite("Interface", this.Name, this));
        }
    }
}
