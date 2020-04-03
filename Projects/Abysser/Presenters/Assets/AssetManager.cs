using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using Newtonsoft.Json;

namespace Abysser.Presenters.Assets
{
    public class AssetManager : NotifyPropertyChangedBase
    {
        private static readonly string textureDataFileName = Path.Combine(Environment.CurrentDirectory, "Textures", "textureData.txt");
        private static readonly string textureFileName = Path.Combine(Environment.CurrentDirectory, "Textures", "zones.png");
        
        private AssetType currentAssetType;
        private IAsset currentAsset;
        
        public AssetType CurrentAssetType
        {
            get { return this.currentAssetType; }

            set
            {
                if (this.currentAssetType == value)
                {
                    return;
                }

                this.currentAssetType = value;
                this.OnPropertyChanged("CurrentAssetType");
            }
        }

        public IAsset CurrentAsset
        {
            get
            {
                return this.currentAsset;
            }

            set
            {
                if (this.currentAsset == value)
                {
                    return;
                }

                this.currentAsset = value;
                this.OnPropertyChanged("CurrentAsset");
            }
        }

        public IEnumerable<ITile> Textures
        {
            get; private set;
        }

        public IEnumerable<ITile> Triggers
        {
            get; private set;
        }

        public void Initialize()
        {
            // Load texture data.
            var textures = new List<ITile>();
            var texturePresenters = JsonConvert.DeserializeObject<List<TilePresenter>>(File.ReadAllText(textureDataFileName));
            foreach (var t in texturePresenters)
            {
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(textureFileName, UriKind.Relative);
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();

                t.Texture = new CroppedBitmap(bitmap, new Int32Rect(t.AtlasLocation.X * AssetConstants.TextureSize, t.AtlasLocation.Y * AssetConstants.TextureSize, AssetConstants.TextureSize, AssetConstants.TextureSize));
                textures.Add(t);
            }

            this.Textures = textures;

            // TODO: Load trigger data.
        }
    }
}
