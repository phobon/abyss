using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace Atlasser.Adorners
{
    public class SpriteTargetAdorner : Adorner
    {
        public SpriteTargetAdorner(UIElement adornedElement, Rect spriteRect, SolidColorBrush renderBrush) 
            : base(adornedElement)
        {
            this.SpriteRect = spriteRect;
            this.RenderBrush = renderBrush;
            this.IsHitTestVisible = false;
        }

        public Rect SpriteRect { get; set; }

        public SolidColorBrush RenderBrush { get; private set; }

        protected override void OnRender(DrawingContext drawingContext)
        {
            // Some arbitrary drawing implements.
            var renderPen = new Pen(this.RenderBrush, 1);

            // Draw a rectangle.
            drawingContext.DrawRectangle(null, renderPen, this.SpriteRect);
        }
    }
}
