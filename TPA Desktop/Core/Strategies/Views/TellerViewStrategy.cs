using TPA_Desktop.Core.Interfaces;
using TPA_Desktop.Views.Departments.ATM_Machine;

namespace TPA_Desktop.Core.Strategies.Views
{
    public class TellerViewStrategy : IStrategy
    {
        public void Execute()
        {
            new AtmWindow().Show();
        }
    }
}