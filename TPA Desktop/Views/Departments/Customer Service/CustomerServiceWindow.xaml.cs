﻿using System;
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

        private void HandleRequestCreditCard(object sender, RoutedEventArgs e)
        {
            _mediator.Notify(sender, "Request Credit Card");
        }
    }

    public class CustomerServiceWindowMediator : IMediator
    {
        public void Notify(object sender, string @event)
        {
            IStrategy strategy = @event switch
            {
                "Register New Customer" => new RegisterNewCustomerStrategy(),
                "New Individual Account" => new NewIndividualAccountStrategy(),
                "Account" => new NewIndividualAccountStrategy(),
                "Create Virtual Account" => new CreateVirtualAccountStrategy(),
                "Generate Virtual Accounts From Excel" => new GenerateVirtualAccountsFromExcelStrategy(),
                "Manage Customer Account Data" => new ManageCustomerAccountDataStrategy(),
                "Request Credit Card" => new RequestCreditCardStrategy(),
                _ => throw new InvalidOperationException($"{@event} is not supported.")
            };

            strategy.Execute();
        }
    }
}