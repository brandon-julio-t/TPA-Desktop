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
        public NewIndividualAccountWindowState State;
        public Customer Customer;

        public NewIndividualAccountWindow()
        {
            InitializeComponent();
            DataContext = ViewModel;
            State = new HandleCustomerState(this);
        }

        private void HandleSubmit(object sender, RoutedEventArgs e) => State.HandleSubmit();
    }

    public sealed class NewIndividualAccountWindowViewModel : INotifyPropertyChanged
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string MotherMaidenName { get; set; }
        private int _selectedAccountTypeIndex;
        public string[] AccountTypes { get; set; } = {"Regular", "Student", "Saving", "Deposit"};
        public string SelectedAccountType => AccountTypes[_selectedAccountTypeIndex];
        public string[] AccountLevels { get; set; } = {"Bronze", "Silver", "Gold", "Black"};
        public int SelectedAccountLevelIndex { get; set; }
        public string SelectedAccountLevel => AccountLevels[SelectedAccountLevelIndex];

        public int SelectedAccountTypeIndex
        {
            get => _selectedAccountTypeIndex;
            set
            {
                _selectedAccountTypeIndex = value;
                OnPropertyChanged(nameof(AccountLevelVisibility));
            }
        }

        private bool _isCustomerValid;

        public bool IsCustomerValid
        {
            get => _isCustomerValid;
            set
            {
                _isCustomerValid = value;
                OnPropertyChanged(nameof(AccountFormVisibility));
                OnPropertyChanged(nameof(AccountLevelVisibility));
            }
        }

        public Visibility AccountFormVisibility => IsCustomerValid ? Visibility.Visible : Visibility.Hidden;

        public Visibility AccountLevelVisibility =>
            AccountFormVisibility == Visibility.Visible &&
            (_selectedAccountTypeIndex == 0 || _selectedAccountTypeIndex == 1)
                ? Visibility.Visible
                : Visibility.Hidden;

        public bool Validate() => new Validator("First Name", FirstName).NotEmpty().IsValid &&
                                  new Validator("Last Name", LastName).NotEmpty().IsValid &&
                                  new Validator("Date Of Birth", DateOfBirth).NotEmpty().IsValid &&
                                  new Validator("Mother Maiden Name", MotherMaidenName).NotEmpty().IsValid;

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

        protected NewIndividualAccountWindowState(NewIndividualAccountWindow window)
        {
            Window = window;
        }

        public abstract void HandleSubmit();
    }

    internal class HandleCustomerState : NewIndividualAccountWindowState
    {
        public override void HandleSubmit()
        {
            if (!Window.ViewModel.Validate()) return;

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

        public HandleCustomerState(NewIndividualAccountWindow window) : base(window)
        {
        }
    }

    internal class HandleAccountState : NewIndividualAccountWindowState
    {
        public override void HandleSubmit()
        {
            var director = new AccountBuilderDirector(Window.ViewModel.SelectedAccountLevel);
            var builder = new AccountBuilder();
            
            switch (Window.ViewModel.SelectedAccountType)
            {
                case "Regular":
                    director.MakeRegularAccount(builder);
                    break;
                case "Student":
                    director.MakeStudentAccount(builder, 0); // TODO
                    break;
                case "Saving":
                    director.MakeSavingAccount(builder);
                    break;
                case "Deposit":
                    director.MakeDepositAccount(builder, false); // TODO
                    break;
            }

            var account = builder.GetResult();
            MessageBox.Show(account.Save() ? "Account created." : "An error occurred while creating account.");
        }

        public HandleAccountState(NewIndividualAccountWindow window) : base(window)
        {
        }
    }
}