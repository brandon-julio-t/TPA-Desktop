﻿using System.Windows;
using TPA_Desktop.Core.Interfaces;
using TPA_Desktop.Core.Strategies.Views;
using TPA_Desktop.Models;

namespace TPA_Desktop.Core.Factories
{
    public class ViewStrategyFactory
    {
        private readonly Employee _employee;

        public ViewStrategyFactory(Employee employee) => _employee = employee;

        public IStrategy Create()
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