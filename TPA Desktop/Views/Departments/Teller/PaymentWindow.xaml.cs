﻿using System.Windows;
using TPA_Desktop.Core.Commands;
using TPA_Desktop.Core.Facades;
using TPA_Desktop.Core.Interfaces;
using TPA_Desktop.Core.Models;

namespace TPA_Desktop.Views.Departments.Teller
{
    public partial class PaymentWindow
    {
        private readonly ICustomerAccountStore _customerAccountStore;
        private readonly PaymentWindowViewModel _viewModel = new PaymentWindowViewModel();

        public PaymentWindow(ICustomerAccountStore customerAccountStore)
        {
            InitializeComponent();
            DataContext = _viewModel;
            _customerAccountStore = customerAccountStore;
        }

        private void HandleSubmit(object sender, RoutedEventArgs e)
        {
            if (!_viewModel.Validate()) return;
            if (_customerAccountStore.Account.Balance < _viewModel.Transaction.Amount)
            {
                MessageBox.Show("Insufficient customer account balance.");
                return;
            }

            new PaymentCommand(_customerAccountStore, _viewModel).Execute();
        }
    }

    public class PaymentWindowViewModel
    {
        public PaymentType[] PaymentTypes { get; set; } = PaymentType.All();
        public Transaction Transaction { get; set; } = new Transaction("Payment");
        public bool IsCredit { get; set; }

        public bool Validate()
        {
            return new Validator("Payment Type", Transaction.PaymentType).NotEmpty().IsValid &&
                   new Validator("Amount", Transaction.Amount).NotEmpty().MoreThan(0).IsValid;
        }
    }
}