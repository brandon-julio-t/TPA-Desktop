using TPA_Desktop.Core.Interfaces;
using TPA_Desktop.Views.Departments.Manager;

namespace TPA_Desktop.Core.Strategies.Views
{
    public class ManagerViewStrategy : IStrategy
    {
        public void Execute() => new ManagerWindow().Show();
    }
}