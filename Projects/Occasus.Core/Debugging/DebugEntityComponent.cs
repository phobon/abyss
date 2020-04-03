using Occasus.Core.Components;
using Occasus.Core.Drawing.Lighting;
using Occasus.Core.Entities;

namespace Occasus.Core.Debugging
{
    public abstract class DebugEntityComponent : EntityComponent
    {
        private const string DebugTag = "Debug";

        protected DebugEntityComponent(IEntity parent, string name, string description) 
            : base(parent, name, description)
        {
        }

        protected override void InitializeFlags()
        {
            this.Flags[EngineFlag.DeferredRender] = true;
        }

        protected override void InitializeTags()
        {
            this.Tags.Add(DebugTag);
            this.Tags.Add(Lighting.DeferredRender);
            this.Parent.Tags.Add(Lighting.HasDeferredRenderComponents);
        }
    }
}
