using System.Windows;
using EVAL.Babalone.View;
using EVAL.Babalone.ViewModel;

namespace EVAL.Babalone
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region Fields

        private MainWindow _view = null!;

        #endregion

        #region Constructors

        public App()
        {
            Startup += new StartupEventHandler(App_Startup);
        }

        #endregion

        #region Application event handlers

        private void App_Startup(object? sender, StartupEventArgs e)
        {
            _view = new MainWindow();
            _view.Show();
        }

        #endregion
    }
}
