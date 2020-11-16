using System;
using System.Windows;
using TPA_Desktop.Core.Interfaces;
using TPA_Desktop.Core.Strategies.CustomerService;

namespace TPA_Desktop.Views.Departments.Customer_Service
{
    public partial class CustomerServiceWindow
    {
        private readonly IMediator _mediator = new CustomerServiceWindowMediator();

        public CustomerServiceWindow()
        {
            InitializeComponent();
        }

        private void HandleRegisterNewCustomer(object sender, RoutedEventArgs e)
        {
            _mediator.Notify(sender, "Register New Customer");
        }

        private void HandleNewIndividualAccount(object sender, RoutedEventArgs e)
        {
            _mediator.Notify(sender, "New Individual Account");
        }

        private void CreateVirtualAccount(object sender, RoutedEventArgs e)
        {
            _mediator.Notify(sender, "Create Virtual Account");
        }

        private void GenerateVirtualAccountsFromExcel(object sender, RoutedEventArgs e)
        {
            _mediator.Notify(sender, "Generate Virtual Accounts From Excel");
        }

        private void HandleUpdateCustomerData(object sender, RoutedEventArgs e)
        {
            _mediator.Notify(sender, "Manage Customer Account Data");
        }
    }

    public class CustomerServiceWindowMediator : IMediator
    {
        public void Notify(object sender, string @event)
        {
            IStrategy strategy;

            switch (@event)
            {
                case "Register New Customer":
                    strategy = new RegisterNewCustomerStrategy();
                    break;
                case "New Individual Account":
                    strategy = new NewIndividualAccountStrategy();
                    break;
                case "Create Virtual Account":
                    strategy = new CreateVirtualAccountStrategy();
                    break;
                case "Generate Virtual Accounts From Excel":
                    strategy = new GenerateVirtualAccountsFromExcelStrategy();
                    break;
                case "Manage Customer Account Data":
                    strategy = new ManageCustomerAccountDataStrategy();
                    break;
                default: throw new InvalidOperationException();
            }

            strategy.Execute();
        }
    }
}