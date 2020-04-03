using System.Collections.Generic;
using Occasus.Core.Entities;

namespace Occasus.Core.Components
{
    public abstract class EntityComponent : EngineComponent, IEntityComponent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityComponent" /> class.
        /// </summary>
        /// <param name="parent">The entity.</param>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        protected EntityComponent(IEntity parent, string name, string description)
            : base(name, description)
        {
            this.Parent = parent;
            this.Tags = new List<string>();
        }
        
        /// <summary>
        /// Gets the parent entity of this component.
        /// </summary>
        public IEntity Parent
        {
            get; private set;
        }

        public IList<string> Tags { get; private set; }
        
        /// <summary>
        /// Initializes the Engine Component.
        /// </summary>
        public override void Initialize()
        {
            this.InitializeTags();

            base.Initialize();
            this.Resume();
        }

        protected abstract void InitializeTags();
    }
}
