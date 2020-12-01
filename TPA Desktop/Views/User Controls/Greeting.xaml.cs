using System.Windows;
using TPA_Desktop.Core.Facades;
using TPA_Desktop.Core.Repositories;
using TPA_Desktop.Views.Shared;

namespace TPA_Desktop.Views.User_Controls
{
    public partial class Greeting
    {
        private readonly GreetingViewModel _viewModel = new GreetingViewModel();

        public Greeting()
        {
            InitializeComponent();
            DataContext = _viewModel;
        }

        private void HandleReportBrokenItem(object sender, RoutedEventArgs e)
        {
            new ReportBrokenItemWindow(
                new EquipmentRepository(),
                new EquipmentConditionRepository()
            ).Show();
        }
    }

    public class GreetingViewModel
    {
        public string Name => $"{Authentication.Employee.FirstName} {Authentication.Employee.LastName}";
        public string Role => Authentication.Employee.EmployeePosition.Name;
    }
}