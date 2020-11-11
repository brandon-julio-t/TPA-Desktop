using System.Windows;
using TPA_Desktop.Interfaces;
using TPA_Desktop.Models;
using TPA_Desktop.Strategies.Views;

namespace TPA_Desktop.Factories
{
    public class ViewStrategyFactory : IFactory
    {
        private readonly Employee _employee;

        public ViewStrategyFactory(Employee employee) => _employee = employee;

        public object Create()
        {
            switch (_employee.EmployeePosition.Name)
            {
                case "Customer Service": return new CustomerServiceViewStrategy();
                case "Finance": return new FinanceViewStrategy();
                case "Human Resource": return new HumanResourceViewStrategy();
                case "Manager": return new ManagerViewStrategy();
                case "Security and Maintenance": return new SecurityAndMaintenanceViewStrategy();
                case "Teller": return new TellerViewStrategy();
                default:
                    MessageBox.Show($"View doesn't exist for employee position: {_employee.EmployeePosition.Name}");
                    return null;
            }
        }
    }
}