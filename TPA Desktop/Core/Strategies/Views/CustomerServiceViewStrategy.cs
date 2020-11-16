using TPA_Desktop.Core.Interfaces;
using TPA_Desktop.Views.Departments.Customer_Service;

namespace TPA_Desktop.Core.Strategies.Views
{
    public class CustomerServiceViewStrategy : IStrategy
    {
        public void Execute()
        {
            new CustomerServiceWindow().Show();
        }
    }
}