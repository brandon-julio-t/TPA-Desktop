using TPA_Desktop.Core.Interfaces;
using TPA_Desktop.Views.Departments.Teller;

namespace TPA_Desktop.Core.Strategies.Views
{
    public class TellerViewStrategy : IStrategy
    {
        public void Execute()
        {
            new TellerWindow().Show();
        }
    }
}