using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace Atlasser.Adorners
{
    public class NodeAdorner : Adorner
    {
        public NodeAdorner(UIElement adornedElement, SolidColorBrush renderBrush) 
            : base(adornedElement)
        {
            this.NodeRects = new ObservableCollection<Rect>();
            this.RenderBrush = renderBrush;
            this.TransparentRenderBrush = new SolidColorBrush(renderBrush.Color)
            {
                Opacity = 0.3
            };

            this.IsHitTestVisible = false;
        }

        public ObservableCollection<Rect> NodeRects
        {
            get; private set;
        }

        public SolidColorBrush RenderBrush
        {
            get; private set;
        }

        public SolidColorBrush TransparentRenderBrush
        {
            get; private set;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            // Some arbitrary drawing implements.
            var renderPen = new Pen(this.RenderBrush, 1)
            {
                DashStyle = DashStyles.Dot
            };

            // Draw a rectangle for each node rectangle.
            foreach (var n in this.NodeRects)
            {
                drawingContext.DrawRectangle(this.TransparentRenderBrush, renderPen, n);
            }
        }
    }
}
