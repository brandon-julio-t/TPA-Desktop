using System;
using System.Windows;
using TPA_Desktop.Core.Builders;
using TPA_Desktop.Core.DefaultImplementations;
using TPA_Desktop.Core.Facades;
using TPA_Desktop.Core.Interfaces;
using TPA_Desktop.Core.Models;

namespace TPA_Desktop.Views
{
    public partial class VerifyCustomerWindow
    {
        public readonly Action CustomerValidCallback;
        public readonly ICustomerAccountStore Store;
        public readonly VerifyCustomerWindowViewModel ViewModel = new VerifyCustomerWindowViewModel();
        public VerifyCustomerWindowState State;

        public VerifyCustomerWindow(ICustomerAccountStore store, Action customerValidCallback)
        {
            InitializeComponent();
            DataContext = ViewModel;
            Store = store;
            CustomerValidCallback = customerValidCallback;
            State = new VerifyCustomerState(this);
        }

        private void HandleSubmit(object sender, RoutedEventArgs e)
        {
            State.HandleSubmit();
        }
    }

    public class VerifyCustomerWindowViewModel : DefaultNotifyPropertyChanged
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string MotherMaidenName { get; set; }
        public string AccountNumber { get; set; }
        public bool IsCustomerExists { get; set; }
        public Visibility AccountNumberVisibility => IsCustomerExists ? Visibility.Visible : Visibility.Collapsed;

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

            return isValid;
        }
    }

    public abstract class VerifyCustomerWindowState
    {
        protected readonly VerifyCustomerWindow Window;

        protected VerifyCustomerWindowState(VerifyCustomerWindow window)
        {
            Window = window;
        }

        public abstract void HandleSubmit();
    }

    public class VerifyCustomerState : VerifyCustomerWindowState
    {
        public VerifyCustomerState(VerifyCustomerWindow window) : base(window)
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
            Window.State = new VerifyAccountState(Window);
        }
    }

    public class VerifyAccountState : VerifyCustomerWindowState
    {
        public VerifyAccountState(VerifyCustomerWindow window) : base(window)
        {
        }

        public override void HandleSubmit()
        {
            if (!Window.ViewModel.Validate()) return;

            var account = new Account(Window.Store.Customer, Window.ViewModel.AccountNumber);
            if (account.Id == Guid.Empty) return;
            if (account.ClosedAt != null)
            {
                MessageBox.Show("Account is closed.");
                return;
            }

            if (account.BlockedAt != null)
            {
                MessageBox.Show("Account is blocked.");
                return;
            }

            Window.Store.Account = account;
            Window.CustomerValidCallback.Invoke();
            Window.Close();
        }
    }
}