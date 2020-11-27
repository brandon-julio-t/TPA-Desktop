using TPA_Desktop.Core.Interfaces;
using TPA_Desktop.Views.Departments.Customer_Service.CreditCards;

namespace TPA_Desktop.Core.Strategies.CustomerService
{
    public class RequestCreditCardStrategy : IStrategy
    {
        public void Execute()
        {
            new RequestCreditCardWindow().Show();
        }
    }
}