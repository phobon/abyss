using System;
using System.Windows;

namespace Abysser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            App.Abysser.Initialize();
            this.main.Content = App.Abysser;
        }
    }
}
