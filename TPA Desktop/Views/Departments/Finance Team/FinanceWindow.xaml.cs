using System.Windows;
using TPA_Desktop.Core.Repositories;
using TPA_Desktop.Core.Services;
using TPA_Desktop.Views.Shared;

namespace TPA_Desktop.Views.Departments.Finance_Team
{
    public partial class FinanceWindow
    {
        public FinanceWindow()
        {
            InitializeComponent();
        }

        private void HandleViewTransactions(object sender, RoutedEventArgs e)
        {
            new TransactionsWindow().Show();
        }

        private void HandleViewNotifications(object sender, RoutedEventArgs e)
        {
            new NotificationsWindow(
                new NotificationRepository(),
                new NotificationService(
                    new NotificationRepository()
                )
            ).Show();
        }

        private void HandleManageCreditCardRequests(object sender, RoutedEventArgs e)
        {
            new ManageCreditCardRequestsWindow(
                new ExpenseRequestRepository(),
                new RequestStatusRepository(),
                new ExpenseRequestTypeRepository(),
                new CreditCardCompanyRepository(), 
                new CreditCardRepository(),
                new AccountRepository(),
                new CustomerRepository()
            ).Show();
        }
    }
}