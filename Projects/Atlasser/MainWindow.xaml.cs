using System.Windows.Input;
using System.Windows;

namespace Atlasser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Model.Atlasser Atlasser { get; private set; }

        public MainWindow()
        {
            InitializeComponent();

            this.Atlasser = new Model.Atlasser();
            this.DataContext = this.Atlasser;
        }

        private void MaximizeWindowButtonClick(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Maximized;
        }

        private void MinimizeWindowButtonClick(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void RestoreWindowButtonClick(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Normal;
        }

        private void WindowMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Handle dragging the window.
            DragMove();

            // Handle a double click on the title bar.
            // TODO: This should be on MouseUp (ie: Click) but the event gets swallowed :(
            if (e.ClickCount == 2)
            {
                WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
            }
        }

        private void CloseWindowButtonClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
