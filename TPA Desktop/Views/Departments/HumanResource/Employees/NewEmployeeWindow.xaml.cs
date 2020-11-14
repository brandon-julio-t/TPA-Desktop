using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using TPA_Desktop.Annotations;
using TPA_Desktop.Core.Facades;
using TPA_Desktop.Core.Models;
using TPA_Desktop.Models;

namespace TPA_Desktop.Views.Departments.HumanResource.Employees
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

            MessageBox.Show(employee.Save() ? "Employee Added." : "An error occured while saving employee.");
            _viewModel.ResetFields();
        }
    }

    public class NewEmployeeViewModel : INotifyPropertyChanged
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

        public event PropertyChangedEventHandler PropertyChanged;

        public bool ValidateFields()
        {
            return new Validator("First Name", FirstName).NotEmpty().IsValid
                   && new Validator("Last Name", LastName).NotEmpty().IsValid
                   && new Validator("Gender", IsMaleChecked, IsFemaleChecked).AnyIsSelected().IsValid
                   && new Validator("Date of Birth", DateOfBirth).NotEmpty().IsValid
                   && new Validator("Phone Number", PhoneNumber).NotEmpty().Numeric().IsValid
                   && new Validator("Salary", Salary).NotEmpty().Numeric().IsValid
                   && new Validator("Employee Position", SelectedEmployeePosition).NotEmpty().IsValid;
        }

        public void ResetFields()
        {
            FirstName = string.Empty;
            LastName = string.Empty;
            IsMaleChecked = false;
            IsFemaleChecked = false;
            DateOfBirth = string.Empty;
            PhoneNumber = string.Empty;
            Salary = string.Empty;
            SelectedEmployeePosition = null;

            OnPropertyChanged(nameof(FirstName));
            OnPropertyChanged(nameof(LastName));
            OnPropertyChanged(nameof(IsMaleChecked));
            OnPropertyChanged(nameof(IsFemaleChecked));
            OnPropertyChanged(nameof(DateOfBirth));
            OnPropertyChanged(nameof(PhoneNumber));
            OnPropertyChanged(nameof(Salary));
            OnPropertyChanged(nameof(SelectedEmployeePosition));
        }

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}