using System.Collections.Generic;
using Abysser.Presenters.Assets;

namespace Abysser.Presenters.Chunks
{
    public interface IChunk
    {
        string Name { get; }

        string Description { get; }

        IDictionary<string, string> Metadata { get; }

        int Width { get; }

        int Height { get; }

        IDictionary<string, IAsset[,]> AssetLayers { get; }

        IAsset[,] CurrentLayer { get; }

        void Save();
    }
}
