using System;
using System.Windows;
using TPA_Desktop.Core.Interfaces;

namespace TPA_Desktop.Views.Teller
{
    public partial class TellerWindow
    {
        private readonly IMediator _mediator = new TellerWindowMediator();
        public TellerWindow() => InitializeComponent();
        private void HandleTransferMoney(object sender, RoutedEventArgs e) => _mediator.Notify(sender, "Transfer");
        private void HandleDepositMoney(object sender, RoutedEventArgs e) => _mediator.Notify(sender, "Deposit");
        private void HandlePayment(object sender, RoutedEventArgs e) => _mediator.Notify(sender, "Payment");
        private void HandleWithdrawMoney(object sender, RoutedEventArgs e) => _mediator.Notify(sender, "Withdraw");

        private void HandleTransferToVirtualAccount(object sender, RoutedEventArgs e) =>
            _mediator.Notify(sender, "Transfer Virtual Account");
    }

    public class TellerWindowMediator : IMediator
    {
        public void Notify(object sender, string @event)
        {
            Window window;

            switch (@event)
            {
                case "Transfer":
                    window = new DepositWindow();
                    break;
                case "Deposit":
                    window = new DepositWindow();
                    break;
                case "Payment":
                    window = new DepositWindow();
                    break;
                case "Withdraw":
                    window = new DepositWindow();
                    break;
                case "Transfer Virtual Account":
                    window = new DepositWindow();
                    break;
                default: throw new InvalidOperationException();
            }

            window.Show();
        }
    }
}