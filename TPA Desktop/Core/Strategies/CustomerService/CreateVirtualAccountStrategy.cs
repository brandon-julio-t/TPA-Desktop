using TPA_Desktop.Core.Interfaces;
using TPA_Desktop.Views.CustomerService.VirtualAccounts;

namespace TPA_Desktop.Core.Strategies.CustomerService
{
    public class CreateVirtualAccountStrategy :IStrategy
    {
        public void Execute() => new CreateVirtualAccountWindow().Show();
    }
}