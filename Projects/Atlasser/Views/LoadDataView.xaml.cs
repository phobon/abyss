using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Atlasser.Model.ViewModes;
using Microsoft.Win32;

namespace Atlasser.Views
{
    /// <summary>
    /// Interaction logic for LoadDataSerializeView.xaml
    /// </summary>
    public partial class LoadDataView : UserControl
    {
        private LoadDataPresenter LoadDataPresenter { get; set; }

        public LoadDataView()
        {
            InitializeComponent();

            this.DataContextChanged += (sender, args) =>
            {
                this.LoadDataPresenter = (LoadDataPresenter)args.NewValue;
            };
        }

        private void FilePickerOnClick(object sender, RoutedEventArgs e)
        {
            // Configure open file dialog box.
            var dialog = new OpenFileDialog
            {
                FileName = "Data",
                DefaultExt = ".txt",
                Filter = "Text documents (.txt)|*.txt"
            };

            // Show open file dialog box.
            var result = dialog.ShowDialog();

            // Process open file dialog box results.
            if (result == true)
            {
                this.LoadDataPresenter.DataLocation = dialog.FileName;
            }
        }

        private void AtlasLocationOnClick(object sender, RoutedEventArgs e)
        {
            // Configure open file dialog box.
            var dialog = new OpenFileDialog
            {
                FileName = "spritesheet",
                DefaultExt = ".xml",
                Filter = "XML documents (.xml)|*.xml"
            };

            // Show open file dialog box.
            var result = dialog.ShowDialog();

            // Process open file dialog box results.
            if (result == true)
            {
                this.LoadDataPresenter.AtlasLocation = dialog.FileName;
            }
        }

        private void SpritesheetLocationOnClick(object sender, RoutedEventArgs e)
        {
            // Configure open file dialog box.
            var dialog = new OpenFileDialog
            {
                FileName = "Data",
                DefaultExt = ".txt",
                Filter = "Text documents (.txt)|*.txt"
            };

            // Show open file dialog box.
            var result = dialog.ShowDialog();

            // Process open file dialog box results.
            if (result == true)
            {
                this.LoadDataPresenter.SpriteSheetLocation = dialog.FileName;
            }
        }

        private void AddAtlasKeyOnClick(object sender, RoutedEventArgs e)
        {
            var atlasKey = atlasKeyTextBox.Text;
            if (SpriteSheetPresenter.AtlasKeys.Any(o => o.Equals(atlasKey, StringComparison.InvariantCultureIgnoreCase)))
            {
                return;
            }

            SpriteSheetPresenter.AtlasKeys.Add(atlasKey);
        }

        private void DeleteAtlasKeyOnClick(object sender, RoutedEventArgs e)
        {
            var atlasKey = (string)((Button) sender).DataContext;
            SpriteSheetPresenter.AtlasKeys.Remove(atlasKey);
        }
    }
}
