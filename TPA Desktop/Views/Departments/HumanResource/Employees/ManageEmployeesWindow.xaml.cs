using System.Linq;
using System.Windows;
using TPA_Desktop.Core.Models;
using TPA_Desktop.Views.Departments.HumanResource.Violations;

namespace TPA_Desktop.Views.Departments.HumanResource.Employees
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

        private void HandleAddViolation(object sender, RoutedEventArgs e)
        {
            new AddViolationWindow(_windowViewModel.SelectedEmployee).Show();
        }
    }

    public class ManageEmployeesWindowViewModel
    {
        public ManageEmployeesWindowViewModel()
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