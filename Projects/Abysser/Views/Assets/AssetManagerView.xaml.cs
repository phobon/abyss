using System;
using System.Windows;
using System.Windows.Controls;
using Abysser.Presenters.Assets;

namespace Abysser.Views.Assets
{
    /// <summary>
    /// Interaction logic for AssetManagerView.xaml
    /// </summary>
    public partial class AssetManagerView
    {
        private AssetManager assetManager;

        public AssetManagerView()
        {
            InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            this.assetManager = (AssetManager)this.DataContext;
        }

        private void TexturesButtonClick(object sender, RoutedEventArgs e)
        {
            if (this.assetManager == null)
            {
                this.assetManager = (AssetManager)this.DataContext;
            }

            this.assetManager.CurrentAssetType = AssetType.Texture;
        }

        private void TriggersButtonClick(object sender, RoutedEventArgs e)
        {
            if (this.assetManager == null)
            {
                this.assetManager = (AssetManager)this.DataContext;
            }

            this.assetManager.CurrentAssetType = AssetType.Trigger;
        }
    }
}
