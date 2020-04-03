using System.Windows;
using System.Windows.Controls;

namespace Atlasser.Views
{
    public class SelectedNodeTemplateSelector : DataTemplateSelector
    {
        public DataTemplate NullTemplate { get; set; }

        public DataTemplate NodeTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item != null)
            {
                return this.NodeTemplate;
            }

            return this.NullTemplate;
        }
    }
}
