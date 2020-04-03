using System;

namespace Atlasser.Model.ViewModes
{
    public interface IViewMode
    {
        event EventHandler Activated;

        string Name { get; }
        
        bool IsActive { get; set; }

        bool IsAccessible { get; set; }
    }
}
