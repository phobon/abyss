using System.Windows;
using System.Windows.Controls;
using Atlasser.Model.ViewModes;

namespace Atlasser.Views
{
    /// <summary>
    /// Interaction logic for SerializeDataView.xaml
    /// </summary>
    public partial class SerializeDataView : UserControl
    {
        private SerializeDataPresenter SerializeDataPresenter { get; set; }

        public SerializeDataView()
        {
            InitializeComponent();
            this.DataContextChanged += (sender, args) =>
            {
                this.SerializeDataPresenter = (SerializeDataPresenter) args.NewValue;
            };
        }

        private void SerializeOnClick(object sender, RoutedEventArgs e)
        {
            this.SerializeDataPresenter.Serialize();
        }
    }
}
