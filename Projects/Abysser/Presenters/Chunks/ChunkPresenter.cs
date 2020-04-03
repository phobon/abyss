using System.Collections.Generic;
using Abysser.Presenters.Assets;

namespace Abysser.Presenters.Chunks
{
    public class ChunkPresenter : NotifyPropertyChangedBase, IChunk
    {
        public ChunkPresenter(string name, string description)
        {
            this.Name = name;
            this.Description = description;
            this.Metadata = new Dictionary<string, string>();
        }

        public string Name
        {
            get; private set;
        }

        public string Description
        {
            get; private set;
        }

        public IDictionary<string, string> Metadata
        {
            get; private set;
        }

        public int Width
        {
            get; private set;

        }
        public int Height
        {
            get; private set;
        }

        public IDictionary<string, IAsset[,]> AssetLayers
        {
            get; private set;
        }

        public IAsset[,] CurrentLayer
        {
            get; private set;
        }

        public void Save()
        {
        }
    }
}
