using System.Windows;
using System.Windows.Threading;
using TPA_Desktop.Core;
using TPA_Desktop.Core.Facades;

namespace TPA_Desktop
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private void App_OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            var debug = Environment.IsDevelopment
                ? $"from {sender}: {e.Exception.Message}\n{e.Exception.StackTrace}"
                : "";
            MessageBox.Show($"Fatal error {debug}".Trim());
            e.Handled = true;
        }
    }
}