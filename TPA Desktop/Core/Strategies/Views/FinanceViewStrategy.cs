using TPA_Desktop.Core.Interfaces;
using TPA_Desktop.Views.Departments.Finance_Team;

namespace TPA_Desktop.Core.Strategies.Views
{
    public class FinanceViewStrategy : IStrategy
    {
        public void Execute()
        {
            new FinanceWindow().Show();
        }
    }
}