using System;
using System.Windows;
using TPA_Desktop.Core.Facades;
using TPA_Desktop.Core.Interfaces;
using TPA_Desktop.Core.Models;
using TPA_Desktop.Core.Repositories;
using TPA_Desktop.Core.Services;
using TPA_Desktop.Views.Departments.Teller;

namespace TPA_Desktop.Core.Commands
{
    public class PaymentCommand : ICommand
    {
        private readonly ICustomerAccountStore _customerAccountStore;
        private readonly PaymentWindowViewModel _viewModel;

        public PaymentCommand(ICustomerAccountStore customerAccountStore, PaymentWindowViewModel viewModel)
        {
            _customerAccountStore = customerAccountStore;
            _viewModel = viewModel;
        }

        public void Execute()
        {
            var transactionSuccess = Database.Transaction(
                () =>
                {
                    if (!_viewModel.IsCredit)
                    {
                        _customerAccountStore.Account!.Balance -= _viewModel.Transaction.Amount;
                        _viewModel.Transaction.Account = _customerAccountStore.Account!;
                        return _customerAccountStore.Account!.Save() && _viewModel.Transaction.Save();
                    }

                    var creditCardRepository = new CreditCardRepository();
                    var creditCard = creditCardRepository.FindByAccountNumber(_customerAccountStore.Account!.AccountNumber);
                    var creditCardService = new CreditCardService(creditCard);
                    var chargeRepository = new ChargeRepository();
                    var charge = new Charge
                    {
                        Amount = _viewModel.Transaction.Amount * 1.02m,
                        Description = $"{_viewModel.Transaction.PaymentType} Payment.",
                        DueAt = DateTime.Now.AddMonths(1),
                        AccountNumber = _customerAccountStore.Account.AccountNumber,
                    };

                    var isActivated = creditCardService.CheckIsActivated();
                    if (!isActivated) throw new Exception("Credit card is not activated.");

                    return chargeRepository.Save(charge); // TODO test charge cc
                }
            );

            MessageBox.Show(transactionSuccess ? "Payment success." : "An error occurred while doing payment.");
        }
    }
}