using System.Windows;
using System.Windows.Threading;

namespace TPA_Desktop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private void App_OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show($"Fatal error from {sender}: {e.Exception.Message}");
            e.Handled = true;
        }
    }
}