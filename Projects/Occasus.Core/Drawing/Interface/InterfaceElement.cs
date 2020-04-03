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
        }

        protected override void InitializeTags()
        {
            this.Tags.Add("InterfaceElement");
        }

        protected override void InitializeSprite()
        {
            this.AddComponent(Sprite.Tag, Atlas.GetSprite("Interface", this.Name, this));
        }
    }
}
