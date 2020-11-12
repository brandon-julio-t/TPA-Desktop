using TPA_Desktop.Core.Interfaces;
using TPA_Desktop.Views.CustomerService.Customers.Regular;

namespace TPA_Desktop.Core.Strategies.CustomerService
{
    public class RegisterNewCustomerStrategy : IStrategy
    {
        public void Execute() => new NewCustomerWindow().Show();
    }
}