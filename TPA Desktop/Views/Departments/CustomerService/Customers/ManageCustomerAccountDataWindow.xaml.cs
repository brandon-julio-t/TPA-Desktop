using System;
using System.Linq;
using System.Windows;
using TPA_Desktop.Core.DefaultImplementations;
using TPA_Desktop.Core.Models;

namespace TPA_Desktop.Views.Departments.CustomerService.Customers
{
    public partial class ManageCustomerAccountDataWindow
    {
        private readonly ManageCustomerAccountDataWindowViewModel _viewModel =
            new ManageCustomerAccountDataWindowViewModel();

        public ManageCustomerAccountDataWindow()
        {
            InitializeComponent();
            DataContext = _viewModel;
        }

        private void HandleUpdate(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(_viewModel.SelectedCustomerAccount.Account.Save()
                ? "Update success."
                : "An error occurred while doing update.");
        }

        private void HandleClose(object sender, RoutedEventArgs e)
        {
            _viewModel.SelectedCustomerAccount.Account.ClosedAt = DateTime.Now;
            MessageBox.Show(_viewModel.SelectedCustomerAccount.Account.Save()
                ? "Account close success."
                : "An error occurred while closing account.");
            _viewModel.Refresh();
        }
    }

    public class ManageCustomerAccountDataWindowViewModel : DefaultNotifyPropertyChanged
    {
        public ManageCustomerAccountDataWindowViewModel()
        {
            Refresh();
        }

        public CustomerAccountViewModel[] CustomerAccounts { get; set; }
        public CustomerAccountViewModel SelectedCustomerAccount { get; set; }

        public void Refresh()
        {
            CustomerAccounts = Customer.All()
                .Select(customer => new CustomerAccountViewModel(customer))
                .Where(model => model.Account.Id != Guid.Empty)
                .Where(model => model.Account.ClosedAt == (DateTime?) null)
                .ToArray();

            OnPropertyChanged();
        }
    }

    public class CustomerAccountViewModel
    {
        public CustomerAccountViewModel(Customer account)
        {
            Account = new Account(account);
        }

        public Account Account { get; set; }

        public bool IsMale
        {
            get => Account.Owner.Gender == "Male";
            set => Account.Owner.Gender = value ? "Male" : "Female";
        }

        public bool IsFemale
        {
            get => Account.Owner.Gender == "Female";
            set => Account.Owner.Gender = value ? "Female" : "Male";
        }

        public bool IsBlocked
        {
            get => Account.BlockedAt != null;
            set => Account.BlockedAt = value ? DateTime.Now : (DateTime?) null;
        }
    }
}