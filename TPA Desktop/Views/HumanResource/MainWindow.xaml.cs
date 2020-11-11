﻿using System.Windows;
using TPA_Desktop.Facades;
using TPA_Desktop.Models;
using TPA_Desktop.Views.HumanResource.Employees;

namespace TPA_Desktop.Views.HumanResource
{
    public partial class MainWindow : Window
    {
        private readonly  MainWindowViewModel _viewModel = new MainWindowViewModel();
        
        public MainWindow()
        {
            InitializeComponent();
            DataContext = _viewModel;
        }

        private void HandleNewEmployee(object sender, RoutedEventArgs e)
        {
            new NewEmployeeWindow().Show();
        }

        private void HandleManageEmployees(object sender, RoutedEventArgs e)
        {
            new ManageEmployeesWindow().Show();
        }

        private void ViewAllViolations(object sender, RoutedEventArgs e)
        {
            new AllViolationsWindow().Show();
        }
    }

    public class MainWindowViewModel
    {
        public string Name => $"{Authentication.Employee.FirstName} {Authentication.Employee.LastName}";
        public string Role => Authentication.Employee.EmployeePosition.Name;
    }
}