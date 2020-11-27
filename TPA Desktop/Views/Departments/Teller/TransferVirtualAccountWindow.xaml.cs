using System;
using System.Windows;
using TPA_Desktop.Core.Facades;
using TPA_Desktop.Core.Interfaces;
using TPA_Desktop.Core.Models;

namespace TPA_Desktop.Views.Departments.Teller
{
    public partial class TransferVirtualAccountWindow
    {
        private readonly ICustomerAccountStore _customerAccountStore;
        private readonly TransferVirtualAccountWindowViewModel _viewModel = new TransferVirtualAccountWindowViewModel();

        public TransferVirtualAccountWindow(ICustomerAccountStore customerAccountStore)
        {
            InitializeComponent();
            _customerAccountStore = customerAccountStore;
            DataContext = _viewModel;
        }

        private void HandleTransfer(object sender, RoutedEventArgs e)
        {
            if (!_viewModel.Validate()) return;

            var virtualAccount = new VirtualAccount(_viewModel.VirtualAccountNumber);
            if (!virtualAccount.Validate(_customerAccountStore.Account)) return;

            var transactionSuccess = Database.Transaction(
                () =>
                {
                    _customerAccountStore.Account.Balance -= virtualAccount.Amount;

                    var destinationAccount = new Account(virtualAccount.DestinationAccountNumber);
                    destinationAccount.Balance += virtualAccount.Amount;

                    virtualAccount.PaidAt = DateTime.Now;

                    return _customerAccountStore.Account.Save() &&
                           destinationAccount.Save() &&
                           virtualAccount.Save() &&
                           new Transaction("Transfer Virtual Account")
                           {
                               Account = _customerAccountStore.Account,
                               Amount = virtualAccount.Amount
                           }.Save();
                }
            );

            MessageBox.Show(transactionSuccess
                ? "Virtual account transaction success."
                : "An error occurred while doing transaction.");
        }
    }

    public class TransferVirtualAccountWindowViewModel
    {
        public string? VirtualAccountNumber { get; set; }

        public bool Validate()
        {
            return new Validator("Virtual Account Number", VirtualAccountNumber)
                .NotEmpty()
                .Numeric()
                .IsValid;
        }
    }
}