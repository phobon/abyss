using System.Windows;
using System.Windows.Controls;
using Atlasser.Model.Drawing;
using Atlasser.Model.Nodes;

namespace Atlasser.Views.Nodes
{
    /// <summary>
    /// Interaction logic for SpriteNodeView.xaml
    /// </summary>
    public partial class SpriteNodeView : UserControl
    {
        private AnimatedNode Node
        {
            get { return (AnimatedNode) this.DataContext; }
        }

        public SpriteNodeView()
        {
            InitializeComponent();
        }

        private void AddLayerOnClick(object sender, RoutedEventArgs e)
        {
            this.Node.Layers.Add(new Layer(this.Node.Texture, this.Node.Size));
        }

        private void AddAnimationOnClick(object sender, RoutedEventArgs e)
        {
            this.Node.Animations.Add(new Animation
            {
                Name = "new",
                Speed = 30,
                Frames = "0-2"
            });
        }

        private void DeleteAnimationOnClick(object sender, RoutedEventArgs e)
        {
            var animation = (Animation) ((Button) sender).DataContext;
            this.Node.Animations.Remove(animation);
        }

        private void DeleteLayerOnClick(object sender, RoutedEventArgs e)
        {
            var layer = (Layer)((Button)sender).DataContext;
            this.Node.Layers.Remove(layer);
        }
    }
}
