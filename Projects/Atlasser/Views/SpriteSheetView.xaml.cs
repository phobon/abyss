using System;
using System.ComponentModel;
using System.Linq;
using Atlasser.Adorners;
using Atlasser.Model.Nodes;
using Atlasser.Model.SpriteSheets;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using Atlasser.Model.ViewModes;

namespace Atlasser.Views
{
    /// <summary>
    /// Interaction logic for SpriteSheetView.xaml
    /// </summary>
    public partial class SpriteSheetView : UserControl
    {
        private SpriteTargetAdorner spriteTargetAdorner;
        private SpriteTargetAdorner selectedSpriteTargetAdorner;
        private NodeAdorner nodeAdorner;

        private SpriteSheet spriteSheet;

        private SpriteSheetPresenter SpriteSheetPresenter { get; set; }

        private SpriteSheet SpriteSheet
        {
            get
            {
                return this.spriteSheet;
            }

            set
            {
                this.spriteSheet = value;
                if (this.spriteSheet != null)
                {
                    this.LoadNodeAdorner();
                }
            }
        }

        public SpriteSheetView()
        {
            InitializeComponent();
            this.DataContextChanged += (sender, args) =>
            {
                if (this.SpriteSheetPresenter != null)
                {
                    this.SpriteSheetPresenter.PropertyChanged -= this.SpriteSheetPresenterOnPropertyChanged;
                }

                this.SpriteSheetPresenter = (SpriteSheetPresenter) this.DataContext;
                
                if (this.SpriteSheetPresenter != null)
                {
                    this.SpriteSheet = this.SpriteSheetPresenter.CurrentSpriteSheet;
                    this.SpriteSheetPresenter.PropertyChanged += this.SpriteSheetPresenterOnPropertyChanged;
                }
            };
        }

        private void SpriteSheetPresenterOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs.PropertyName.Equals("CurrentSpriteSheet", StringComparison.InvariantCultureIgnoreCase))
            {
                this.SpriteSheet = this.SpriteSheetPresenter.CurrentSpriteSheet;
            }
        }

        private void SpriteSheetOnMouseMove(object sender, MouseEventArgs e)
        {
            var image = (Image) sender;
            var position = e.GetPosition(image);

            var roundedX = (int)Math.Round(position.X);
            var roundedY = (int)Math.Round(position.Y);

            var x = roundedX - (roundedX % this.SpriteSheet.SpriteSize.X);
            var y = roundedY - (roundedY % this.SpriteSheet.SpriteSize.Y);
            var spriteRect = new Rect(x, y, this.SpriteSheet.SpriteSize.X, this.SpriteSheet.SpriteSize.Y);

            if (this.spriteTargetAdorner != null)
            {
                if (this.spriteTargetAdorner.SpriteRect == spriteRect)
                {
                    return;
                }
            }

            var adornerLayer = AdornerLayer.GetAdornerLayer(image);
            if (this.spriteTargetAdorner == null)
            {
                this.spriteTargetAdorner = new SpriteTargetAdorner(image, spriteRect, (SolidColorBrush)this.Resources["AccentBlueBrush"]);
            }
            else
            {
                adornerLayer.Remove(this.spriteTargetAdorner);
                this.spriteTargetAdorner.SpriteRect = spriteRect;
            }
            
            adornerLayer.Add(this.spriteTargetAdorner);
        }

        private void ImageOnMouseDown(object sender, MouseButtonEventArgs e)
        {
            var image = (Image)sender;
            var adornerLayer = AdornerLayer.GetAdornerLayer(image);
            if (this.selectedSpriteTargetAdorner == null)
            {
                this.selectedSpriteTargetAdorner = new SpriteTargetAdorner(image, this.spriteTargetAdorner.SpriteRect, (SolidColorBrush)this.Resources["AccentEmeraldBrush"]);
            }
            else
            {
                adornerLayer.Remove(this.selectedSpriteTargetAdorner);
                this.selectedSpriteTargetAdorner.SpriteRect = this.spriteTargetAdorner.SpriteRect;
            }

            adornerLayer.Add(this.selectedSpriteTargetAdorner);

            this.SpriteSheet.SelectedPoint = new Point(this.selectedSpriteTargetAdorner.SpriteRect.X / this.SpriteSheet.SpriteSize.X, this.selectedSpriteTargetAdorner.SpriteRect.Y / this.SpriteSheet.SpriteSize.Y);

            e.Handled = true;
        }

        private void ImageOnMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
            {
                this.SpriteSheet.Scale++;
            }
            else
            {
                this.SpriteSheet.Scale--;
            }
        }


        private void ImageOnLoaded(object sender, RoutedEventArgs e)
        {
            var image = (Image)sender;
            var adornerLayer = AdornerLayer.GetAdornerLayer(image);
            this.nodeAdorner = new NodeAdorner(image, (SolidColorBrush)this.Resources["AccentRedBrush"]);
            adornerLayer.Add(this.nodeAdorner);

            this.LoadNodeAdorner();

            // Set the keyboard focus to this control.
            Keyboard.Focus(this);
        }

        private void LoadNodeAdorner()
        {
            if (this.nodeAdorner == null)
            {
                return;
            }

            this.nodeAdorner.NodeRects.Clear();

            foreach (var n in this.SpriteSheet.NodeMap)
            {
                this.nodeAdorner.NodeRects.Add(new Rect(n.Key.X * this.SpriteSheet.SpriteSize.X, n.Key.Y * this.SpriteSheet.SpriteSize.Y, this.SpriteSheet.SpriteSize.X, this.SpriteSheet.SpriteSize.Y));
            }

            var adornerLayer = AdornerLayer.GetAdornerLayer(i);
            if (adornerLayer != null)
            {
                adornerLayer.Update();
            }
        }

        private void DeleteNodeOnClick(object sender, RoutedEventArgs e)
        {
            this.DeleteNode();
        }

        private void AddNodeOnClick(object sender, RoutedEventArgs e)
        {
            var button = (Button) sender;
            var content = (string) button.Content;
            
            this.AddNode(content);
        }

        private void SpriteSheetView_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (this.SpriteSheet.SelectedNode != null)
            {
                if (e.Key == Key.Delete)
                {
                    this.DeleteNode();
                }

                return;
            }

            var type = string.Empty;
            switch (e.Key)
            {
                case Key.D:
                    type = "doodad";
                    break;
                case Key.S:
                    type = "sprite";
                    break;
                case Key.P:
                    type = "particle";
                    break;
            }

            if (!string.IsNullOrEmpty(type))
            {
                this.AddNode(type);
            }
        }

        private void DeleteNode()
        {
            var qualifiedLocation = new Point(this.SpriteSheet.SelectedPoint.X * this.SpriteSheet.SpriteSize.X, this.SpriteSheet.SelectedPoint.Y * this.SpriteSheet.SpriteSize.Y);
            this.nodeAdorner.NodeRects.Remove(new Rect(qualifiedLocation.X, qualifiedLocation.Y, this.SpriteSheet.SpriteSize.X, this.SpriteSheet.SpriteSize.Y));
            AdornerLayer.GetAdornerLayer(i).Update();

            this.SpriteSheet.RemoveNode(this.SpriteSheet.SelectedPoint, this.SpriteSheet.SelectedNode);
        }

        private void AddNode(string type)
        {
            switch (type)
            {
                case "doodad":
                    this.SpriteSheet.AddNode(new Doodad("doodad", "Doodads", this.SpriteSheet.SpriteSize)
                    {
                        SpriteLocation = this.SpriteSheet.SelectedPoint
                    });
                    break;
                case "sprite":
                    this.SpriteSheet.AddNode(new Sprite("sprite", this.SpriteSheet.Id, SpriteSheetPresenter.AtlasKeys.First(), this.SpriteSheet.SpriteSize, (int)this.SpriteSheet.SelectedPoint.X, (int)this.SpriteSheet.SelectedPoint.Y));
                    break;
                case "particle":
                    this.SpriteSheet.AddNode(new Particle("particle", this.SpriteSheet.Id, SpriteSheetPresenter.AtlasKeys.First(), this.SpriteSheet.SpriteSize, (int)this.SpriteSheet.SelectedPoint.X, (int)this.SpriteSheet.SelectedPoint.Y));
                    break;
            }

            var qualifiedLocation = new Point(this.SpriteSheet.SelectedPoint.X * this.SpriteSheet.SpriteSize.X, this.SpriteSheet.SelectedPoint.Y * this.SpriteSheet.SpriteSize.Y);
            this.nodeAdorner.NodeRects.Add(new Rect(qualifiedLocation.X, qualifiedLocation.Y, this.SpriteSheet.SpriteSize.X, this.SpriteSheet.SpriteSize.Y));
            AdornerLayer.GetAdornerLayer(i).Update();
        }
    }
}
