using System.Windows;
using TPA_Desktop.Core.Facades;
using TPA_Desktop.Core.Interfaces;
using TPA_Desktop.Core.Models;
using TPA_Desktop.Core.Repositories;
using TPA_Desktop.Views.Departments.Customer_Service.CreditCards;

namespace TPA_Desktop.Core.Commands
{
    public class CreateCreditCardRequestCommand : ICommand, ICustomerAccountStore
    {
        private readonly Document _document;
        private readonly CreditCardCompany? _creditCardCompany;
        private readonly Account? _account;

        public CreateCreditCardRequestCommand(ICustomerAccountStore customerAccountStore,
            RequestCreditCardWindowViewModel viewModel)
        {
            _document = viewModel.Document;
            _creditCardCompany = viewModel.SelectedCreditCardCompany;
            _account = customerAccountStore.Account;
        }

        public void Execute()
        {
            var creditCardRepository = new CreditCardRepository();
            var documentRepository = new DocumentRepository();
            var expenseRequestTypeRepository = new ExpenseRequestTypeRepository();
            var expenseRequestRepository = new ExpenseRequestRepository();
            var notificationRepository = new NotificationRepository();
            var employeePositionRepository = new EmployeePositionRepository();

            var card = new CreditCard
            {
                CreditCardCompanyID = _creditCardCompany!.Id,
                AccountNumber = _account!.AccountNumber
            };
            var expenseRequest = new ExpenseRequest
            {
                EntityId = card.Id,
                ExpenseRequestTypeId = expenseRequestTypeRepository.FindByName("Credit Card").Id
            };
            var notification = new Notification
            {
                Title = "New Credit Card Request",
                EmployeePosition = employeePositionRepository.FindByName("Finance")
            };

            var success = Database.Transaction(() =>
                documentRepository.Save(_document) &&
                creditCardRepository.Save(card) &&
                expenseRequestRepository.Save(expenseRequest) &&
                notificationRepository.Save(notification)
            );

            MessageBox.Show(success
                ? "Credit card requested."
                : "An error occurred while requesting credit card.");
        }

        public Customer? Customer { get; set; }
        public Account? Account { get; set; }
    }
}