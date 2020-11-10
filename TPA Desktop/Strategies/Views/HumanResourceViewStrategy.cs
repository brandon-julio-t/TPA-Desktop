using TPA_Desktop.Interfaces;
using TPA_Desktop.Views;
using TPA_Desktop.Views.HumanResource;

namespace TPA_Desktop.Strategies.Views
{
    public class HumanResourceViewStrategy : IStrategy
    {
        public void Execute()
        {
            new MainWindow().Show();
        }
    }
}