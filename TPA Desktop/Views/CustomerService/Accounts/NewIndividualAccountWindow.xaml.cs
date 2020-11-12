using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using TPA_Desktop.Annotations;
using TPA_Desktop.Facades;
using TPA_Desktop.Facades.Builders;
using TPA_Desktop.Facades.Builders.Directors;
using TPA_Desktop.Models;

namespace TPA_Desktop.Views.CustomerService.Accounts
{
    public partial class NewIndividualAccountWindow : Window
    {
        public readonly NewIndividualAccountWindowViewModel ViewModel = new NewIndividualAccountWindowViewModel();
        public Customer Customer;
        public NewIndividualAccountWindowState State;

        public NewIndividualAccountWindow()
        {
            InitializeComponent();
            DataContext = ViewModel;
            State = new HandleCustomerState(this);
        }

        private void HandleSubmit(object sender, RoutedEventArgs e)
        {
            State.HandleSubmit();
        }
    }

    public sealed class NewIndividualAccountWindowViewModel : INotifyPropertyChanged
    {
        private bool _isCustomerValid;
        private int _selectedAccountTypeIndex;
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string MotherMaidenName { get; set; }
        public string[] AccountTypes { get; set; } = {"Regular", "Student", "Saving", "Deposit"};
        public string SelectedAccountType => AccountTypes[_selectedAccountTypeIndex];
        public string[] AccountLevels { get; set; } = {"Bronze", "Silver", "Gold", "Black"};
        public int SelectedAccountLevelIndex { get; set; }
        public string SelectedAccountLevel => AccountLevels[SelectedAccountLevelIndex];
        public string RegularAccountNumber { get; set; }
        public string GuardianAccountNumber { get; set; }
        public bool UseAutomaticRollOver { get; set; }

        public int SelectedAccountTypeIndex
        {
            get => _selectedAccountTypeIndex;
            set
            {
                _selectedAccountTypeIndex = value;
                OnPropertyChanged(nameof(AccountLevelVisibility));
                OnPropertyChanged(nameof(RegularAccountNumberVisibility));
                OnPropertyChanged(nameof(GuardianAccountNumberVisibility));
                OnPropertyChanged(nameof(AutomaticRollOverVisibility));
            }
        }

        public bool IsCustomerValid
        {
            get => _isCustomerValid;
            set
            {
                _isCustomerValid = value;
                OnPropertyChanged(nameof(AccountFormVisibility));
                OnPropertyChanged(nameof(AccountLevelVisibility));
                OnPropertyChanged(nameof(RegularAccountNumberVisibility));
                OnPropertyChanged(nameof(GuardianAccountNumberVisibility));
                OnPropertyChanged(nameof(AutomaticRollOverVisibility));
            }
        }

        public Visibility AccountFormVisibility => IsCustomerValid ? Visibility.Visible : Visibility.Collapsed;

        public Visibility AccountLevelVisibility =>
            AccountFormVisibility == Visibility.Visible &&
            (_selectedAccountTypeIndex == 0 || _selectedAccountTypeIndex == 1)
                ? Visibility.Visible
                : Visibility.Collapsed;

        public Visibility RegularAccountNumberVisibility =>
            AccountFormVisibility == Visibility.Visible &&
            (_selectedAccountTypeIndex == 2 || _selectedAccountTypeIndex == 3)
                ? Visibility.Visible
                : Visibility.Collapsed;

        public Visibility GuardianAccountNumberVisibility =>
            AccountFormVisibility == Visibility.Visible
            && _selectedAccountTypeIndex == 1
                ? Visibility.Visible
                : Visibility.Collapsed;

        public Visibility AutomaticRollOverVisibility =>
            AccountFormVisibility == Visibility.Visible
            && _selectedAccountTypeIndex == 3
                ? Visibility.Visible
                : Visibility.Collapsed;

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public abstract class NewIndividualAccountWindowState
    {
        protected readonly NewIndividualAccountWindow Window;

        protected NewIndividualAccountWindowState(NewIndividualAccountWindow window) => Window = window;

        public abstract void HandleSubmit();
    }

    internal sealed class HandleCustomerState : NewIndividualAccountWindowState
    {
        public HandleCustomerState(NewIndividualAccountWindow window) : base(window)
        {
        }

        public override void HandleSubmit()
        {
            if (!Validate()) return;

            var customer = new Customer(
                Window.ViewModel.FirstName,
                Window.ViewModel.LastName,
                Window.ViewModel.DateOfBirth,
                Window.ViewModel.MotherMaidenName
            );

            if (customer.Id == Guid.Empty) return;

            MessageBox.Show("Customer found.");
            Window.ViewModel.IsCustomerValid = true;
            Window.Customer = customer;
            Window.State = new HandleAccountState(Window);
        }

        private bool Validate() =>
            new Validator("First Name", Window.ViewModel.FirstName).NotEmpty().IsValid &&
            new Validator("Last Name", Window.ViewModel.LastName).NotEmpty().IsValid &&
            new Validator("Date Of Birth", Window.ViewModel.DateOfBirth).NotEmpty().IsValid &&
            new Validator("Mother Maiden Name", Window.ViewModel.MotherMaidenName).NotEmpty().IsValid;
    }

    internal sealed class HandleAccountState : NewIndividualAccountWindowState
    {
        public HandleAccountState(NewIndividualAccountWindow window) : base(window)
        {
        }

        public override void HandleSubmit()
        {
            if (!Validate()) return;

            var director = new AccountBuilderDirector(Window.ViewModel.SelectedAccountLevel);
            var builder = new AccountBuilder();

            switch (Window.ViewModel.SelectedAccountType)
            {
                case "Regular":
                    director.MakeRegularAccount(builder);
                    break;
                case "Student":
                    director.MakeStudentAccount(builder, Window.ViewModel.GuardianAccountNumber);
                    break;
                case "Saving":
                    director.MakeSavingAccount(builder);
                    break;
                case "Deposit":
                    director.MakeDepositAccount(builder, Window.ViewModel.UseAutomaticRollOver);
                    break;
            }

            var account = builder.GetResult();
            account.Owner = Window.Customer;
            MessageBox.Show(account.Save() ? "Account created." : "An error occurred while creating account.");
        }

        private bool Validate()
        {
            var isValid = new Validator("Account Type", Window.ViewModel.SelectedAccountType)
                .NotEmpty()
                .IsValid;

            if (Window.ViewModel.AccountLevelVisibility == Visibility.Visible)
                isValid &= new Validator("Account Level", Window.ViewModel.SelectedAccountLevel)
                    .NotEmpty()
                    .IsValid;

            if (Window.ViewModel.RegularAccountNumberVisibility == Visibility.Visible)
                isValid &= new Validator("Regular Account Number", Window.ViewModel.RegularAccountNumber)
                               .NotEmpty()
                               .Numeric()
                               .IsValid
                           &&
                           new Validator("Regular Account Number", QueryBuilder
                                   .Table("Account")
                                   .Where("AccountNumber", Window.ViewModel.RegularAccountNumber)
                                   .Where("Name", "Regular")
                                   .Get()
                               ).Exists()
                               .IsValid;

            if (Window.ViewModel.GuardianAccountNumberVisibility == Visibility.Visible)
                isValid &= new Validator("Guardian Account Number", Window.ViewModel.GuardianAccountNumber)
                               .NotEmpty()
                               .Numeric()
                               .IsValid
                           &&
                           new Validator("Guardian Account Number", QueryBuilder
                                   .Table("Account")
                                   .Where("AccountNumber", Window.ViewModel.GuardianAccountNumber)
                                   .Where("Name", "Regular")
                                   .Get()
                               ).Exists()
                               .IsValid;

            return isValid;
        }
    }
}