using System;
using System.Windows;
using TPA_Desktop.Core.Facades;
using TPA_Desktop.Core.Factories;
using TPA_Desktop.Core.Models;
using TPA_Desktop.Views.Departments.Queueing_Machine;

namespace TPA_Desktop.Views
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class LoginWindow
    {
        private readonly LoginWindowViewModel _viewModel = new LoginWindowViewModel();

        public LoginWindow()
        {
            InitializeComponent();
            DataContext = _viewModel;
        }

        private void HandleLogin(object sender, RoutedEventArgs e)
        {
            var employee = new Employee(_viewModel.Email, PasswordBox.Password);
            if (employee.Id == Guid.Empty) return;

            Authentication.Login(employee);

            new ViewStrategyFactory(employee).Create()?.Execute();
            Close();
        }

        private void HandleLoginAsQueueingMachine(object sender, RoutedEventArgs e)
        {
            new QueueingMachineWindow().Show();
            Close();
        }
    }

    public class LoginWindowViewModel
    {
        public string Email { get; set; }
    }
}