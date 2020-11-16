using TPA_Desktop.Core.Interfaces;
using TPA_Desktop.Views.Departments.Customer_Service.VirtualAccounts;

namespace TPA_Desktop.Core.Strategies.CustomerService
{
    public class CreateVirtualAccountStrategy : IStrategy
    {
        public void Execute()
        {
            new CreateVirtualAccountWindow().Show();
        }
    }
}