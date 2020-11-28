using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using TPA_Desktop.Core.Models;
using TPA_Desktop.Core.Models.Abstracts;

namespace TPA_Desktop.Views.Departments.Finance_Team
{
    public partial class ManageCreditCardRequestsWindow
    {
        private readonly CrudRepository<ExpenseRequest> _expenseRequestRepository;
        private readonly ManageCreditCardRequestsWindowViewModel _viewModel;

        public ManageCreditCardRequestsWindow(
            CrudRepository<ExpenseRequest> expenseRequestRepository,
            ReadOnlyRepository<RequestStatus> requestStatusRepository,
            ReadOnlyRepository<ExpenseRequestType> expenseRequestTypeRepository,
            ReadOnlyRepository<CreditCardCompany> creditCardCompanyRepository,
            ReadOnlyRepository<CreditCard> creditCardRepository,
            ReadOnlyRepository<Account> accountRepository,
            ReadOnlyRepository<Customer> customerRepository)
        {
            InitializeComponent();
            _expenseRequestRepository = expenseRequestRepository;
            DataContext = _viewModel = new ManageCreditCardRequestsWindowViewModel(
                expenseRequestRepository,
                expenseRequestTypeRepository,
                requestStatusRepository,
                creditCardCompanyRepository,
                creditCardRepository,
                accountRepository,
                customerRepository
            );
        }

        private void HandleApprove(object sender, RoutedEventArgs e)
        {
            var expenseRequest = _expenseRequestRepository.FindById(_viewModel.SelectedExpenseRequestDetail!.Id);
            expenseRequest.Approve();
            var success = _expenseRequestRepository.Update(expenseRequest);
            MessageBox.Show(success ? "Request approved." : "An error occurred while approving request.");
            _viewModel.ExpenseRequestDetails.Remove(_viewModel.SelectedExpenseRequestDetail);
        }

        private void HandleReject(object sender, RoutedEventArgs e)
        {
            var expenseRequest = _expenseRequestRepository.FindById(_viewModel.SelectedExpenseRequestDetail!.Id);
            expenseRequest.Reject();
            var success = _expenseRequestRepository.Update(expenseRequest);
            MessageBox.Show(success ? "Request rejected." : "An error occurred while rejecting request.");
            _viewModel.ExpenseRequestDetails.Remove(_viewModel.SelectedExpenseRequestDetail);
        }
    }

    public class ManageCreditCardRequestsWindowViewModel
    {
        public ObservableCollection<ExpenseRequestDetail> ExpenseRequestDetails { get; set; }
        public ExpenseRequestDetail? SelectedExpenseRequestDetail { get; set; }

        public ManageCreditCardRequestsWindowViewModel(
            CrudRepository<ExpenseRequest> expenseRequestRepository,
            ReadOnlyRepository<ExpenseRequestType> expenseRequestTypeRepository,
            ReadOnlyRepository<RequestStatus> requestStatusRepository,
            ReadOnlyRepository<CreditCardCompany> creditCardCompanyRepository,
            ReadOnlyRepository<CreditCard> creditCardRepository,
            ReadOnlyRepository<Account> accountRepository,
            ReadOnlyRepository<Customer> customerRepository)
        {
            var expenseRequests = expenseRequestRepository.FindAll();
            var expenseRequestTypes = expenseRequestTypeRepository.FindAll();
            var requestStatuses = requestStatusRepository.FindAll();
            var creditCards = creditCardRepository.FindAll();
            var creditCardCompanies = creditCardCompanyRepository.FindAll();
            var accounts = accountRepository.FindAll();
            var customers = customerRepository.FindAll();

            var requests = from expenseRequest in expenseRequests
                join expenseRequestType in expenseRequestTypes
                    on expenseRequest.ExpenseRequestTypeId equals expenseRequestType.Id
                join requestStatus in requestStatuses on expenseRequest.RequestStatusId equals requestStatus.Id
                join creditCard in creditCards on expenseRequest.EntityId equals creditCard.Id
                join creditCardCompany in creditCardCompanies
                    on creditCard.CreditCardCompanyID equals creditCardCompany.Id
                join account in accounts on creditCard.AccountNumber equals account.AccountNumber
                join customer in customers on account.CustomerId equals customer.Id
                select new ExpenseRequestDetail
                {
                    Id = expenseRequest.Id,
                    RequestType = expenseRequestType.Name,
                    RequestStatus = requestStatus.Name,
                    AccountNumber = account.AccountNumber,
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    CreditCardCompany = creditCardCompany.Name,
                    CreatedAt = expenseRequest.CreatedAt
                };

            ExpenseRequestDetails = new ObservableCollection<ExpenseRequestDetail>(requests);
        }
    }

    public class ExpenseRequestDetail
    {
        public Guid Id { get; set; }
        public string RequestType { get; set; }
        public string RequestStatus { get; set; }
        public string AccountNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CreditCardCompany { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}