using System.Windows;
using TPA_Desktop.Core.Builders;
using TPA_Desktop.Core.DefaultImplementations;
using TPA_Desktop.Core.Facades;
using TPA_Desktop.Core.Models;

namespace TPA_Desktop.Views.CustomerService.VirtualAccounts
{
    public partial class CreateVirtualAccountWindow
    {
        private readonly CreateVirtualAccountWindowViewModel _viewModel = new CreateVirtualAccountWindowViewModel();

        public CreateVirtualAccountWindow()
        {
            InitializeComponent();
            DataContext = _viewModel;
        }

        private void HandleSubmit(object sender, RoutedEventArgs e)
        {
            if (!_viewModel.Validate()) return;
            
            MessageBox.Show(_viewModel.VirtualAccount.Save()
                ? "Virtual account created."
                : "Failed to create virtual account.");
            
            _viewModel.Reset();
        }
    }

    public class CreateVirtualAccountWindowViewModel : DefaultNotifyPropertyChanged
    {
        public VirtualAccount VirtualAccount { get; set; } = new VirtualAccount();

        public bool Validate() =>
            new Validator("Source Account Number", VirtualAccount.SourceAccountNumber)
                .NotEmpty()
                .Numeric()
                .IsValid
            &&
            new Validator("Destination Account Number", VirtualAccount.DestinationAccountNumber)
                .NotEmpty()
                .Numeric()
                .NoMatch("Source Account Number", VirtualAccount.SourceAccountNumber)
                .IsValid
            &&
            new Validator("Amount", VirtualAccount.Amount)
                .NotEmpty()
                .MoreThan(0)
                .IsValid
            &&
            new Validator(
                    "Source Account Number",
                    QueryBuilder
                        .Table("Account")
                        .Where("AccountNumber", VirtualAccount.SourceAccountNumber)
                        .Get()
                ).Exists()
                .IsValid
            &&
            new Validator(
                    "Destination Account Number",
                    QueryBuilder
                        .Table("Account")
                        .Where("AccountNumber", VirtualAccount.DestinationAccountNumber)
                        .Get()
                ).Exists()
                .IsValid;

        public void Reset()
        {
            VirtualAccount = new VirtualAccount();
            OnPropertyChanged();
        }
    }
}