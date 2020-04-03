using Abysser.Presenters.Assets;
using Abysser.Presenters.Chunks;

namespace Abysser.Presenters
{
    public class AbysserPresenter : NotifyPropertyChangedBase
    {
        public AssetManager AssetManager
        {
            get; private set;
        }

        public ChunkManager ChunkManager
        {
            get; private set;
        }

        public void Initialize()
        {
            // Create a new AssetManager and ChunkManager and load everything up.
            this.AssetManager = new AssetManager();
            this.AssetManager.Initialize();

            this.ChunkManager = new ChunkManager();
            this.ChunkManager.Initialize();
        }
    }
}
