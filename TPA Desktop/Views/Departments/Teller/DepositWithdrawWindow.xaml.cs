using System;
using System.Windows;
using TPA_Desktop.Core.Default_Implementations;
using TPA_Desktop.Core.Facades;
using TPA_Desktop.Core.Interfaces;
using TPA_Desktop.Core.Models;

namespace TPA_Desktop.Views.Departments.Teller
{
    public partial class DepositWithdrawWindow
    {
        private readonly string _activityName;
        private readonly ICustomerAccountStore _customerAccountStore;
        private readonly DepositWindowViewModel _viewModel = new DepositWindowViewModel();

        public DepositWithdrawWindow(ICustomerAccountStore customerAccountStore, string activityName)
        {
            if (!new Validator("Activity Name", activityName).In("Deposit", "Withdraw").IsValid)
                throw new InvalidOperationException("Activity Name must be either 'Deposit' or 'Withdraw'.");

            InitializeComponent();
            Title = activityName;
            _viewModel.Transaction = new Transaction(activityName);
            DataContext = _viewModel;

            _activityName = activityName;
            _customerAccountStore = customerAccountStore;
        }

        private void HandleSubmit(object sender, RoutedEventArgs e)
        {
            if (!_viewModel.Validate()) return;

            var transactionSuccess = Database.Transaction(
                () =>
                {
                    var account = _customerAccountStore.Account;
                    account.Balance += Convert.ToDecimal(_viewModel.Transaction.Amount) *
                                       (_activityName == "Deposit" ? 1 : -1);

                    _viewModel.Transaction.Account = account;

                    return account.Save() && _viewModel.Transaction.Save();
                }
            );

            MessageBox.Show(transactionSuccess
                ? $"{_activityName} success."
                : $"An error occurred while doing {_activityName}");

            if (transactionSuccess) Close();
        }
    }

    public sealed class DepositWindowViewModel : DefaultNotifyPropertyChanged
    {
        public Transaction Transaction { get; set; }

        public bool Validate()
        {
            return new Validator("Deposit Amount", Transaction.Amount)
                .NotEmpty()
                .Numeric()
                .MoreThan(0)
                .IsValid;
        }
    }
}