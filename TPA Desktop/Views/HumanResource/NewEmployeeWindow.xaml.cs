using System;
using System.Linq;
using System.Windows;
using TPA_Desktop.Facades;
using TPA_Desktop.Models;

namespace TPA_Desktop.Views.HumanResource
{
    public partial class NewEmployeeWindow
    {
        private readonly NewEmployeeViewModel _viewModel = new NewEmployeeViewModel();

        public NewEmployeeWindow()
        {
            InitializeComponent();
            DataContext = _viewModel;
        }

        private void HandleSubmit(object sender, RoutedEventArgs e)
        {
            if (!_viewModel.ValidateFields()) return;

            var employee = new Employee
            {
                FirstName = _viewModel.FirstName,
                LastName = _viewModel.LastName,
                Gender = _viewModel.IsMaleChecked ? "Male" : "Female",
                DateOfBirth = DateTime.Parse(_viewModel.DateOfBirth),
                RegisteredAt = DateTime.Now,
                PhoneNumber = _viewModel.PhoneNumber,
                Salary = int.Parse(_viewModel.Salary),
                Email = $"{_viewModel.FirstName.ToLower()}.{_viewModel.LastName.ToLower()}@binus.edu",
                Password = $"{_viewModel.FirstName.ToLower()}123",
                EmployeePosition = _viewModel.SelectedEmployeePosition
            };
            
            if (employee.Save())
            {
                MessageBox.Show("Employee Added.");
            }
            else
            {
                MessageBox.Show("An error occured while saving employee.");
            }
        }
    }

    public class NewEmployeeViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsMaleChecked { get; set; }
        public bool IsFemaleChecked { get; set; }
        public string DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string Salary { get; set; }
        public EmployeePosition[] EmployeePositions => EmployeePosition.GetAllEmployeePositions();
        public EmployeePosition SelectedEmployeePosition { get; set; }

        public bool ValidateFields() => new Validator("First Name", FirstName)
                                            .NotEmpty()
                                            .IsValid
                                        && new Validator("Last Name", LastName)
                                            .NotEmpty()
                                            .IsValid
                                        && new Validator("Gender", IsMaleChecked, IsFemaleChecked)
                                            .AnyIsSelected()
                                            .IsValid
                                        && new Validator("Date of Birth", DateOfBirth)
                                            .NotEmpty()
                                            .IsValid
                                        && new Validator("Phone Number", PhoneNumber)
                                            .NotEmpty()
                                            .Numeric()
                                            .IsValid
                                        && new Validator("Salary", Salary)
                                            .NotEmpty()
                                            .Numeric()
                                            .IsValid
                                        && new Validator("Employee Position", SelectedEmployeePosition)
                                            .NotEmpty()
                                            .IsValid;
    }
}