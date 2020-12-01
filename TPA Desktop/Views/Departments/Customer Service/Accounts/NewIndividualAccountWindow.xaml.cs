using System;
using System.Windows;
using TPA_Desktop.Core.Builders;
using TPA_Desktop.Core.Builders.Directors;
using TPA_Desktop.Core.Commands;
using TPA_Desktop.Core.Default_Implementations;
using TPA_Desktop.Core.Facades;
using TPA_Desktop.Core.Models;
using TPA_Desktop.Core.Repositories;
using TPA_Desktop.Views.Shared;

namespace TPA_Desktop.Views.Departments.Customer_Service.Accounts
{
    public partial class NewIndividualAccountWindow
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

    public sealed class NewIndividualAccountWindowViewModel : DefaultNotifyPropertyChanged
    {
        private bool _isCustomerValid;
        private int _selectedAccountTypeIndex;
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? MotherMaidenName { get; set; }
        public string[] AccountTypes { get; set; } = {"Regular", "Student", "Saving", "Deposit", "Business"};
        public string SelectedAccountType => AccountTypes[_selectedAccountTypeIndex];
        public string[] AccountLevels { get; set; } = {"Bronze", "Silver", "Gold", "Black"};
        public string[] BusinessCards { get; set; } = {"Business", "Petty", "Deposit", "Reward"};
        public string SelectedBusinessCard { get; set; }
        public int SelectedAccountLevelIndex { get; set; }
        public string SelectedAccountLevel => AccountLevels[SelectedAccountLevelIndex];
        public string? RegularAccountNumber { get; set; }
        public string? GuardianAccountNumber { get; set; }
        public bool UseAutomaticRollOver { get; set; }

        public int SelectedAccountTypeIndex
        {
            get => _selectedAccountTypeIndex;
            set
            {
                _selectedAccountTypeIndex = value;
                OnPropertyChanged();
            }
        }

        public bool IsCustomerValid
        {
            get => _isCustomerValid;
            set
            {
                _isCustomerValid = value;
                OnPropertyChanged();
            }
        }

        public Visibility AccountFormVisibility => IsCustomerValid ? Visibility.Visible : Visibility.Collapsed;

        public Visibility AccountLevelVisibility =>
            AccountFormVisibility == Visibility.Visible &&
            (SelectedAccountType == "Regular" || SelectedAccountType == "Student")
                ? Visibility.Visible
                : Visibility.Collapsed;

        public Visibility RegularAccountNumberVisibility =>
            AccountFormVisibility == Visibility.Visible &&
            (SelectedAccountType == "Saving" || SelectedAccountType == "Deposit" || SelectedAccountType == "Business")
                ? Visibility.Visible
                : Visibility.Collapsed;

        public Visibility GuardianAccountNumberVisibility =>
            AccountFormVisibility == Visibility.Visible && SelectedAccountType == "Student"
                ? Visibility.Visible
                : Visibility.Collapsed;

        public Visibility AutomaticRollOverVisibility =>
            AccountFormVisibility == Visibility.Visible && SelectedAccountType == "Deposit"
                ? Visibility.Visible
                : Visibility.Collapsed;

        public Visibility BusinessCardVisibility =>
            SelectedAccountType == "Business" ? Visibility.Visible : Visibility.Collapsed;
    }

    public abstract class NewIndividualAccountWindowState
    {
        protected readonly NewIndividualAccountWindow Window;

        protected NewIndividualAccountWindowState(NewIndividualAccountWindow window)
        {
            Window = window;
        }

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

        private bool Validate()
        {
            return new Validator("First Name", Window.ViewModel.FirstName).NotEmpty().IsValid &&
                   new Validator("Last Name", Window.ViewModel.LastName).NotEmpty().IsValid &&
                   new Validator("Date Of Birth", Window.ViewModel.DateOfBirth).NotEmpty().IsValid &&
                   new Validator("Mother Maiden Name", Window.ViewModel.MotherMaidenName).NotEmpty().IsValid;
        }
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
                case "Business":
                    director.MakeBusinessAccount(builder, Window.ViewModel.RegularAccountNumber);
                    break;
            }

            var account = builder.GetResult();
            account.Owner = Window.Customer;
            account.DebitCard = new DebitCard(account);

            if (Window.ViewModel.SelectedAccountType == "Regular")
            {
                var age = DateTime.Today.Year - Window.Customer.DateOfBirth.Year;
                if (age < 15)
                {
                    MessageBox.Show("Age must be at least 15.");
                    return;
                }
            }

            if (Window.ViewModel.SelectedAccountType == "Business")
            {
                new CreateBusinessAccountCommand(
                    Window.Customer,
                    account,
                    Window.ViewModel.SelectedBusinessCard,
                    Window.ViewModel.RegularAccountNumber ?? throw new InvalidOperationException(),
                    new AccountRepository(),
                    new BusinessCardRepository()
                ).Execute();
                return;
            }

            var success =
                Database.Transaction(() =>
                {
                    if (Window.ViewModel.SelectedAccountType == "Student" ||
                        Window.ViewModel.SelectedAccountType == "Deposit")
                        return account.Save();
                    return account.Save() && new DebitCardRepository().Save(account.DebitCard);
                });

            MessageBox.Show(success
                ? $"Account created.\nAccount Number: {account.AccountNumber}"
                : "An error occurred while creating account.");
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