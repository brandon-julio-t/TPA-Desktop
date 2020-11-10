using TPA_Desktop.Interfaces;
using TPA_Desktop.Models;
using TPA_Desktop.Strategies.Views;

namespace TPA_Desktop.Factories
{
    public static class ViewStrategyFactory
    {
        public static IStrategy CreateViewStrategyFrom(Employee employee)
        {
            switch (employee.EmployeePosition.Name)
            {
                case "Customer Service":
                    return new CustomerServiceViewStrategy();
                case "Finance":
                    return new FinanceViewStrategy();
                case "Human Resource":
                    return new HumanResourceViewStrategy();
                case "Manager":
                    return new ManagerViewStrategy();
                case "Security and Maintenance":
                    return new SecurityAndMaintenanceViewStrategy();
                case "Teller":
                    return new TellerViewStrategy();
                default:
                    return null;
            }
        }
    }
}