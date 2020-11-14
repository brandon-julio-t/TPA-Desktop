using TPA_Desktop.Core.Interfaces;
using TPA_Desktop.Views.Departments.CustomerService.Customers;

namespace TPA_Desktop.Core.Strategies.CustomerService
{
    public class ManageCustomerAccountDataStrategy : IStrategy
    {
        public void Execute()
        {
            new ManageCustomerAccountDataWindow().Show();
        }
    }
}