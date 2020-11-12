using TPA_Desktop.Core.Interfaces;
using TPA_Desktop.Views.HumanResource;

namespace TPA_Desktop.Core.Strategies.Views
{
    public class HumanResourceViewStrategy : IStrategy
    {
        public void Execute() => new HumanResourceWindow().Show();
    }
}