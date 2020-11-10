using System;
using System.Windows;
using TPA_Desktop.Facades;
using TPA_Desktop.Factories;
using TPA_Desktop.Models;
using TPA_Desktop.Models.Abstract;

namespace TPA_Desktop.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class LoginWindow
    {
        private readonly LoginViewModel _viewModel = new LoginViewModel();

        public LoginWindow()
        {
            InitializeComponent();
            DataContext = _viewModel;
        }

        private void HandleLogin(object sender, RoutedEventArgs e)
        {
            var employee = new Employee(_viewModel.Email, _viewModel.Password);
            
            if (employee.Id == Guid.Empty) return;
            
            Authentication.Login(employee);
            ViewStrategyFactory.CreateViewStrategyFrom(employee).Execute();
            Close();
        }
    }

    public class LoginViewModel
    {
        public string Email { get; set; } = "clarissa.chuardi@binus.edu";
        public string Password { get; set; } = "clarissa123";
    }
}