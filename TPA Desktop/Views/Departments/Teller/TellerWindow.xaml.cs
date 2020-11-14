using System;
using System.Windows;
using TPA_Desktop.Core.Interfaces;
using TPA_Desktop.Core.Models;

namespace TPA_Desktop.Views.Departments.Teller
{
    public partial class TellerWindow : ICustomerAccountStore
    {
        private readonly IMediator _mediator;

        public TellerWindow()
        {
            InitializeComponent();
            _mediator = new TellerWindowMediator(this);
        }

        public Customer Customer { get; set; }
        public Account Account { get; set; }

        private void HandleTransferMoney(object sender, RoutedEventArgs e)
        {
            _mediator.Notify(sender, "Transfer");
        }

        private void HandleDepositMoney(object sender, RoutedEventArgs e)
        {
            _mediator.Notify(sender, "Deposit");
        }

        private void HandlePayment(object sender, RoutedEventArgs e)
        {
            _mediator.Notify(sender, "Payment");
        }

        private void HandleWithdrawMoney(object sender, RoutedEventArgs e)
        {
            _mediator.Notify(sender, "Withdraw");
        }

        private void HandleTransferToVirtualAccount(object sender, RoutedEventArgs e)
        {
            _mediator.Notify(sender, "Transfer Virtual Account");
        }
    }

    public class TellerWindowMediator : IMediator
    {
        private readonly TellerWindow _tellerWindow;

        public TellerWindowMediator(TellerWindow tellerWindow)
        {
            _tellerWindow = tellerWindow;
        }

        public void Notify(object sender, string @event)
        {
            new VerifyCustomerWindow(
                _tellerWindow,
                () =>
                {
                    Window window;

                    switch (@event)
                    {
                        case "Transfer":
                            window = new TransferMoneyWindow(_tellerWindow);
                            break;
                        case "Deposit":
                            window = new DepositWithdrawWindow(_tellerWindow, "Deposit");
                            break;
                        case "Payment": throw new NotImplementedException();
                        case "Withdraw":
                            window = new DepositWithdrawWindow(_tellerWindow, "Withdraw");
                            break;
                        case "Transfer Virtual Account": throw new NotImplementedException();
                        default: throw new InvalidOperationException();
                    }

                    window.Show();
                }
            ).Show();
        }
    }
}