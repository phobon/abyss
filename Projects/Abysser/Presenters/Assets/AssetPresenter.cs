using Microsoft.Xna.Framework;

namespace Abysser.Presenters.Assets
{
    public abstract class AssetPresenter : NotifyPropertyChangedBase, IAsset
    {
        private Vector2 position;

        protected AssetPresenter(string name, string description, AssetType assetType, Rectangle boundingRectangle)
        {
            this.Name = name;
            this.Description = description;
            this.AssetType = assetType;
        }

        public string Name
        {
            get; private set;
        }

        public string Description
        {
            get; private set;
        }

        public AssetType AssetType
        {
            get; private set;
        }

        public Rectangle BoundingRectangle
        {
            get; private set;
        }

        public Vector2 Position
        {
            get
            {
                return this.position;
            }

            set
            {
                if (this.position == value)
                {
                    return;
                }

                this.position = value;
                this.OnPropertyChanged("Position");
            }
        }
    }
}
