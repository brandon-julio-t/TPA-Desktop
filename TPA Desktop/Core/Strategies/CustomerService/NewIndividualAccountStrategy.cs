using TPA_Desktop.Core.Interfaces;
using TPA_Desktop.Views.Departments.CustomerService.Accounts;

namespace TPA_Desktop.Core.Strategies.CustomerService
{
    public class NewIndividualAccountStrategy : IStrategy
    {
        public void Execute()
        {
            new NewIndividualAccountWindow().Show();
        }
    }
}