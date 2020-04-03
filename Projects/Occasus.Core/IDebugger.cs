using System.Collections.Generic;

namespace Occasus.Core
{
    public interface IDebugger : IEngineComponent
    {
        int Framerate { get; }

        int EntityCount { get; set; }

        IDictionary<string, string> DebugInstances { get; }

        void Add(string key, string value);

        void Remove(string key);
    }
}
