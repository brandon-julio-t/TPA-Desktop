using TPA_Desktop.Core.Default_Implementations;
using TPA_Desktop.Core.Interfaces;
using TPA_Desktop.Core.Repositories;
using TPA_Desktop.Views.Departments.Customer_Service.Accounts;
using TPA_Desktop.Views.Shared;

namespace TPA_Desktop.Core.Strategies.CustomerService
{
    public class CheckTransactionsStrategy : IStrategy
    {
        public void Execute()
        {
            var store = new DefaultCustomerAccountStore();
            new VerifyCustomerWindow(store, () => new CheckTransactionsWindow(
                    store,
                    new TransactionRepository(),
                    new PaymentTypeRepository(),
                    new TransactionTypeRepository()
                ).Show()
            ).Show();
        }
    }
}