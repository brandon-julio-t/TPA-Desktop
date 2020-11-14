using System;
using System.Windows;
using TPA_Desktop.Core.DefaultImplementations;
using TPA_Desktop.Core.Facades;
using TPA_Desktop.Core.Interfaces;

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
            DataContext = _viewModel;

            _activityName = activityName;
            _customerAccountStore = customerAccountStore;
        }

        private void HandleSubmit(object sender, RoutedEventArgs e)
        {
            if (!_viewModel.Validate()) return;

            var account = _customerAccountStore.Account;
            account.Balance += Convert.ToDecimal(_viewModel.DepositAmount) * (_activityName == "Deposit" ? 1 : -1);

            if (!account.Save())
            {
                MessageBox.Show("Balance update failed.");
                return;
            }

            MessageBox.Show("Balance updated.");
            Close();
        }
    }

    public sealed class DepositWindowViewModel : DefaultNotifyPropertyChanged
    {
        public string DepositAmount { get; set; }

        public bool Validate()
        {
            return new Validator("Deposit Amount", DepositAmount).NotEmpty().Numeric().IsValid;
        }
    }
}