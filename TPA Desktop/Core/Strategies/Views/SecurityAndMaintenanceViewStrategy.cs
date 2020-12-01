using TPA_Desktop.Core.Interfaces;
using TPA_Desktop.Views.Departments.Security_and_Maintenance;

namespace TPA_Desktop.Core.Strategies.Views
{
    public class SecurityAndMaintenanceViewStrategy : IStrategy
    {
        public void Execute()
        {
            new SecurityAndMaintenanceWindow().Show();
        }
    }
}