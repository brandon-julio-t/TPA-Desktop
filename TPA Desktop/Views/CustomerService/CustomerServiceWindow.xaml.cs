using System.Windows;
using TPA_Desktop.Facades;
using TPA_Desktop.Views.CustomerService.Accounts;
using TPA_Desktop.Views.CustomerService.Customers.Regular;

namespace TPA_Desktop.Views.CustomerService
{
    public partial class CustomerServiceWindow
    {
        private readonly CustomerServiceWindowViewModel _viewModel = new CustomerServiceWindowViewModel();

        public CustomerServiceWindow()
        {
            InitializeComponent();
            DataContext = _viewModel;
        }

        private void HandleRegisterNewCustomer(object sender, RoutedEventArgs e)
        {
            new NewCustomerWindow().Show();
        }

        private void HandleNewIndividualAccount(object sender, RoutedEventArgs e)
        {
            new NewIndividualAccountWindow().Show();
        }
    }

    public class CustomerServiceWindowViewModel
    {
        public string Name => $"{Authentication.Employee.FirstName} {Authentication.Employee.LastName}";
        public string Role => Authentication.Employee.EmployeePosition.Name;
    }
}