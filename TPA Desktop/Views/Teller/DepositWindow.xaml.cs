using System;
using System.ComponentModel;
using System.Windows;
using TPA_Desktop.Annotations;
using TPA_Desktop.Core;
using TPA_Desktop.Core.Builders;
using TPA_Desktop.Core.DefaultImplementations;
using TPA_Desktop.Core.Facades;
using TPA_Desktop.Core.Models;

namespace TPA_Desktop.Views.Teller
{
    public partial class DepositWindow
    {
        public readonly DepositWindowViewModel ViewModel = new DepositWindowViewModel();
        public readonly DepositWindowStore Store = new DepositWindowStore();
        public DepositWindowState State { get; set; }

        public DepositWindow()
        {
            InitializeComponent();
            DataContext = ViewModel;
            State = new HandleCustomerState(this);
        }

        private void HandleSubmit(object sender, RoutedEventArgs e) => State.HandleSubmit();
    }

    public sealed class DepositWindowViewModel : DefaultNotifyPropertyChanged
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string MotherMaidenName { get; set; }
        public string AccountNumber { get; set; }
        public string DepositAmount { get; set; }
        public bool IsCustomerExists { get; set; }
        public bool IsAccountExists { get; set; }
        public Visibility AccountNumberVisibility => IsCustomerExists ? Visibility.Visible : Visibility.Collapsed;

        public Visibility DepositAmountVisibility =>
            IsCustomerExists && IsAccountExists ? Visibility.Visible : Visibility.Collapsed;

        public bool Validate()
        {
            var isValid = new Validator("First Name", FirstName).NotEmpty().IsValid &&
                          new Validator("Last Name", LastName).NotEmpty().IsValid &&
                          new Validator("Date Of Birth", DateOfBirth).NotEmpty().IsValid &&
                          new Validator("Mother's Maiden Name", MotherMaidenName).NotEmpty().IsValid;

            if (AccountNumberVisibility == Visibility.Visible)
                isValid &=
                    new Validator("Account Number", AccountNumber)
                        .NotEmpty()
                        .Numeric()
                        .IsValid
                    &&
                    new Validator("Account Number", QueryBuilder
                            .Table("Account")
                            .Where("AccountNumber", AccountNumber)
                            .Get()
                        ).Exists()
                        .IsValid;

            if (DepositAmountVisibility == Visibility.Visible)
                isValid &= new Validator("Deposit Amount", DepositAmount).NotEmpty().Numeric().IsValid;

            return isValid;
        }

        public void Reset()
        {
            FirstName = string.Empty;
            LastName = string.Empty;
            DateOfBirth = DateTime.MinValue;
            MotherMaidenName = string.Empty;
            AccountNumber = string.Empty;
            DepositAmount = string.Empty;
            IsCustomerExists = false;
            IsAccountExists = false;
            OnPropertyChanged();
        }
    }

    public class DepositWindowStore
    {
        public Customer Customer { get; set; }
        public Account Account { get; set; }
    }

    public abstract class DepositWindowState
    {
        protected readonly DepositWindow Window;
        protected DepositWindowState(DepositWindow window) => Window = window;
        public abstract void HandleSubmit();
    }

    public class HandleCustomerState : DepositWindowState
    {
        public HandleCustomerState(DepositWindow window) : base(window)
        {
        }

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

            Window.Store.Customer = customer;
            Window.ViewModel.IsCustomerExists = true;
            Window.ViewModel.OnPropertyChanged();
            Window.State = new HandleAccountState(Window);
        }
    }

    public class HandleAccountState : DepositWindowState
    {
        public HandleAccountState(DepositWindow window) : base(window)
        {
        }

        public override void HandleSubmit()
        {
            if (!Window.ViewModel.Validate()) return;

            var account = new Account(Window.Store.Customer, Window.ViewModel.AccountNumber);

            if (account.Id == Guid.Empty) return;

            Window.Store.Account = account;
            Window.ViewModel.IsAccountExists = true;
            Window.ViewModel.OnPropertyChanged();
            Window.State = new HandleDepositState(Window);
        }
    }

    public class HandleDepositState : DepositWindowState
    {
        public HandleDepositState(DepositWindow window) : base(window)
        {
        }

        public override void HandleSubmit()
        {
            if (!Window.ViewModel.Validate()) return;

            var account = Window.Store.Account;
            account.Balance += Convert.ToDecimal(Window.ViewModel.DepositAmount);

            if (!account.Save())
            {
                MessageBox.Show("Balance update failed.");
                return;
            }

            MessageBox.Show("Balance updated.");

            Window.ViewModel.Reset();
            Window.State = new HandleCustomerState(Window);
        }
    }
}