﻿using System.Linq;
using System.Windows;
using System.Windows.Controls;
using TPA_Desktop.Models;

namespace TPA_Desktop.Views.HumanResource.Employees
{
    public partial class ManageEmployeesWindow
    {
        private readonly ManageEmployeesWindowViewModel _windowViewModel = new ManageEmployeesWindowViewModel();

        public ManageEmployeesWindow()
        {
            InitializeComponent();
            DataContext = _windowViewModel;
        }

        private void HandleUpdate(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(_windowViewModel.SelectedEmployee.Save()
                ? "Employee updated."
                : "An error occurred when saving employee.");
        }

        private void HandleGenderChange(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton rb)
                _windowViewModel.SelectedEmployee.Gender = rb.Content.ToString();
        }

        private void HandleDateOfBirthChange(object sender, SelectionChangedEventArgs e)
        {
            if (sender is DatePicker dp && dp.SelectedDate.HasValue)
                _windowViewModel.SelectedEmployee.DateOfBirth = dp.SelectedDate.Value;
        }

        private void HandleAddViolation(object sender, RoutedEventArgs e)
        {
            new AddViolationWindow(_windowViewModel.SelectedEmployee).Show();
        }
    }

    public class ManageEmployeesWindowViewModel
    {
        public EmployeeViewModel[] EmployeeViewModels { get; set; }
        public EmployeeViewModel SelectedEmployeeViewModel => EmployeeViewModels[SelectedEmployeeIndex];
        public Employee SelectedEmployee => SelectedEmployeeViewModel.Employee;
        public int SelectedEmployeeIndex { get; set; }

        public ManageEmployeesWindowViewModel()
        {
            EmployeeViewModels = Employee
                .All()
                .Select(employee => new EmployeeViewModel(employee))
                .ToArray();
        }
    }

    public class EmployeeViewModel
    {
        public Employee Employee { get; set; }
        public EmployeeViewModel(Employee employee) => Employee = employee;

        public bool IsMale
        {
            get => Employee.Gender == "Male";
            set => Employee.Gender = value ? "Male" : "Female";
        }

        public bool IsFemale
        {
            get => !IsMale;
            set => IsMale = !value;
        }
    }
}