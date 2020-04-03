using System.Windows;
using Abysser.Presenters;

namespace Abysser
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static AbysserPresenter Abysser
        {
            get; private set;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Abysser = new AbysserPresenter();
        }
    }
}
