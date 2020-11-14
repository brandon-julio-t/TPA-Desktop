using System.Windows;
using TPA_Desktop.Core.Facades;
using TPA_Desktop.Core.Interfaces;
using TPA_Desktop.Core.Models;

namespace TPA_Desktop.Views.Departments.Teller
{
    public partial class TransferMoneyWindow
    {
        private readonly ICustomerAccountStore _customerAccountStore;
        private readonly TransferMoneyWindowViewModel _viewModel = new TransferMoneyWindowViewModel();

        public TransferMoneyWindow(ICustomerAccountStore customerAccountStore)
        {
            _customerAccountStore = customerAccountStore;
            InitializeComponent();
            DataContext = _viewModel;
        }

        private void HandleTransfer(object sender, RoutedEventArgs e)
        {
            var success = Database.Transaction(
                () =>
                {
                    var sourceAccount = _customerAccountStore.Account;
                    var destinationAccount = new Account(_viewModel.DestinationAccountNumber);

                    sourceAccount.Balance -= _viewModel.Amount;
                    destinationAccount.Balance += _viewModel.Amount;

                    return sourceAccount.Save() && destinationAccount.Save();
                });

            MessageBox.Show(success ? "Transfer success." : "An error occurred while doing the transfer.");
        }
    }

    public class TransferMoneyWindowViewModel
    {
        public string DestinationAccountNumber { get; set; }
        public int Amount { get; set; }
    }
}