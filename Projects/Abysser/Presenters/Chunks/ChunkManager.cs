using System.Collections.ObjectModel;

namespace Abysser.Presenters.Chunks
{
    public class ChunkManager : NotifyPropertyChangedBase
    {
        private IChunk currentChunk;

        public ChunkManager()
        {
            this.Chunks = new ObservableCollection<IChunk>();
        }

        public ObservableCollection<IChunk> Chunks
        {
            get; private set;
        }

        public IChunk CurrentChunk
        {
            get
            {
                return this.currentChunk;
            }

            set
            {
                if (this.currentChunk == value)
                {
                    return;
                }

                this.currentChunk = value;
                this.OnPropertyChanged("CurrentChunk");
            }
        }

        public void Initialize()
        {
            // Load saved chunks.
        }
        
        public void SaveAll()
        {
            foreach (var c in this.Chunks)
            {
                c.Save();
            }
        }

        public void New()
        {
            this.Chunks.Add(new ChunkPresenter("new chunk", "new chunk description"));
        }

        public void Delete(IChunk c)
        {
            this.Chunks.Remove(c);
        }
    }
}
