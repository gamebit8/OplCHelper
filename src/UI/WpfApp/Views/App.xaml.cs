using System.Windows;

namespace WpfApp
{
    public partial class App : Application
    {
        readonly MainWindow _mainWindow;
        public App(MainWindow mainWindow)
        {
            this._mainWindow = mainWindow;
        }

        protected override void OnStartup(StartupEventArgs e)
        {         
            this._mainWindow.Show();
            base.OnStartup(e);
        }
    }
}
