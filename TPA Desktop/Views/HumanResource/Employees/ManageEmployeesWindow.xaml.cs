using System.Linq;
using System.Windows;
using System.Windows.Controls;
using TPA_Desktop.Models;

namespace TPA_Desktop.Views.HumanResource.Employees
{
    public partial class ManageEmployeesWindow
    {
        private readonly ManageEmployeesViewModel _viewModel = new ManageEmployeesViewModel();

        public ManageEmployeesWindow()
        {
            InitializeComponent();
            DataContext = _viewModel;
        }

        private void HandleUpdate(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(_viewModel.SelectedEmployee.Save()
                ? "Employee updated."
                : "An error occurred when saving employee.");
        }

        private void HandleGenderChange(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton rb)
                _viewModel.SelectedEmployee.Gender = rb.Content.ToString();
        }

        private void HandleDateOfBirthChange(object sender, SelectionChangedEventArgs e)
        {
            if (sender is DatePicker dp && dp.SelectedDate.HasValue)
                _viewModel.SelectedEmployee.DateOfBirth = dp.SelectedDate.Value;
        }

        private void HandleAddViolation(object sender, RoutedEventArgs e)
        {
            new AddViolationWindow(_viewModel.SelectedEmployee).Show();
        }
    }

    public class ManageEmployeesViewModel
    {
        public ManageEmployeesViewModel()
        {
            EmployeeViewModels = Employee
                .All()
                .Select(employee => new EmployeeViewModel(employee))
                .ToArray();
        }

        public EmployeeViewModel[] EmployeeViewModels { get; set; }
        public EmployeeViewModel SelectedEmployeeViewModel => EmployeeViewModels[SelectedEmployeeIndex];
        public Employee SelectedEmployee => SelectedEmployeeViewModel.Employee;
        public int SelectedEmployeeIndex { get; set; }
    }

    public class EmployeeViewModel
    {
        public EmployeeViewModel(Employee employee)
        {
            Employee = employee;
        }

        public Employee Employee { get; set; }

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