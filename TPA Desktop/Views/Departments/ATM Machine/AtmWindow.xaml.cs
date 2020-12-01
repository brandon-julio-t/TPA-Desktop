using System;
using System.Windows;
using TPA_Desktop.Core.Interfaces;
using TPA_Desktop.Core.Models;
using TPA_Desktop.Views.Departments.Teller;
using TPA_Desktop.Views.Shared;

namespace TPA_Desktop.Views.Departments.ATM_Machine
{
    public partial class AtmWindow : ICustomerAccountStore
    {
        private readonly IMediator _mediator;

        public AtmWindow()
        {
            InitializeComponent();
            _mediator = new AtmWindowMediator(this);
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

    public class AtmWindowMediator : IMediator
    {
        private readonly AtmWindow _atmWindow;

        public AtmWindowMediator(AtmWindow atmWindow)
        {
            _atmWindow = atmWindow;
        }

        public void Notify(object sender, string @event)
        {
            new VerifyCustomerWindow(
                _atmWindow,
                () =>
                {
                    Window window;

                    switch (@event)
                    {
                        case "Transfer":
                            window = new TransferMoneyWindow(_atmWindow);
                            break;
                        case "Deposit":
                            window = new DepositWithdrawWindow(_atmWindow, "Deposit");
                            break;
                        case "Payment":
                            window = new PaymentWindow(_atmWindow);
                            break;
                        case "Withdraw":
                            window = new DepositWithdrawWindow(_atmWindow, "Withdraw");
                            break;
                        case "Transfer Virtual Account":
                            window = new TransferVirtualAccountWindow(_atmWindow);
                            break;
                        default: throw new InvalidOperationException($"Teller cannot handle {@event}.");
                    }

                    window.Show();
                }
            ).Show();
        }
    }
}