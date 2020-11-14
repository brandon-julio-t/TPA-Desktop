using System.Windows;
using TPA_Desktop.Core.Facades;
using TPA_Desktop.Views.Departments.HumanResource.Employees;
using TPA_Desktop.Views.Departments.HumanResource.Violations;

namespace TPA_Desktop.Views.Departments.HumanResource
{
    public partial class HumanResourceWindow
    {
        private readonly HumanResourceWindowViewModel _viewModel = new HumanResourceWindowViewModel();

        public HumanResourceWindow()
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

    public class HumanResourceWindowViewModel
    {
        public string Name => $"{Authentication.Employee.FirstName} {Authentication.Employee.LastName}";
        public string Role => Authentication.Employee.EmployeePosition.Name;
    }
}