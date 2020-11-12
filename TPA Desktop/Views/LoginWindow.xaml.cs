using System;
using System.Windows;
using TPA_Desktop.Facades;
using TPA_Desktop.Factories;
using TPA_Desktop.Interfaces;
using TPA_Desktop.Models;
using TPA_Desktop.Views.QueueingMachine;

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
            if (_viewModel.Email == "Queueing Machine")
            {
                new QueueingMachineWindow().Show();
            }
            else
            {
                var employee = new Employee(_viewModel.Email, PasswordBox.Password);
                if (employee.Id == Guid.Empty) return;
                Authentication.Login(employee);

                var viewStrategy = new ViewStrategyFactory(employee).Create() as IStrategy;
                viewStrategy?.Execute();
            }

            Close();
        }
    }

    public class LoginWindowViewModel
    {
        public string Email { get; set; } = "skolastika.gabriella@binus.edu";
    }
}