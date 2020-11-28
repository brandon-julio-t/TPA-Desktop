using TPA_Desktop.Core.Interfaces;
using TPA_Desktop.Views.Departments.Customer_Service.CreditCards;
using TPA_Desktop.Views.Shared;

namespace TPA_Desktop.Core.Strategies.CustomerService
{
    public class RequestCreditCardStrategy : IStrategy
    {
        public void Execute()
        {
            var window = new RequestCreditCardWindow();
            new VerifyCustomerWindow(window, () => window.Show()).Show();
        }
    }
}