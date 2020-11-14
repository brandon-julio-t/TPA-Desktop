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
            InitializeComponent();

            _customerAccountStore = customerAccountStore;
            _viewModel.Transaction = new Transaction("Transfer") {Account = customerAccountStore.Account};

            DataContext = _viewModel;
        }

        private void HandleTransfer(object sender, RoutedEventArgs e)
        {
            if (!_viewModel.Validate()) return;

            var success = Database.Transaction(
                () =>
                {
                    var sourceAccount = _customerAccountStore.Account;
                    var destinationAccount = new Account(_viewModel.DestinationAccountNumber);

                    sourceAccount.ReduceBalance(_viewModel.Transaction.Amount);
                    destinationAccount.IncreaseBalance(_viewModel.Transaction.Amount);

                    return sourceAccount.Save() &&
                           destinationAccount.Save() &&
                           _viewModel.Transaction.Save();
                });

            MessageBox.Show(success ? "Transfer success." : "An error occurred while doing the transfer.");
        }
    }

    public class TransferMoneyWindowViewModel
    {
        public Transaction Transaction { get; set; }
        public string DestinationAccountNumber { get; set; }

        public bool Validate() =>
            new Validator("Destination Account Number", DestinationAccountNumber)
                .NotEmpty()
                .Numeric()
                .IsValid
            &&
            new Validator("Amount", Transaction.Amount)
                .NotEmpty()
                .Numeric()
                .MoreThan(0)
                .IsValid;
    }
}