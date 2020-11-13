﻿using System.Windows;
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
                ? $"\nSource: {sender}\n{e.Exception.StackTrace}"
                : "";
            MessageBox.Show($"Fatal error: {e.Exception.Message} {debug}".Trim());
            e.Handled = true;
        }
    }
}