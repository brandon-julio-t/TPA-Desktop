using System.Windows;
using TPA_Desktop.Core.Facades;
using TPA_Desktop.Core.Interfaces;
using TPA_Desktop.Core.Models;
using TPA_Desktop.Core.Models.Abstracts;

namespace TPA_Desktop.Core.Commands
{
    public class CreateBusinessAccountCommand : ICommand
    {
        private readonly Account _account;
        private readonly CrudRepository<Account> _accountRepository;
        private readonly CrudRepository<BusinessCard> _businessCardRepository;
        private readonly BusinessCard _card;

        public CreateBusinessAccountCommand(Customer customer, Account account,
            string businessCardType,
            string regularAccountNumber,
            CrudRepository<Account> accountRepository,
            CrudRepository<BusinessCard> businessCardRepository)
        {
            _account = account;
            _accountRepository = accountRepository;
            _businessCardRepository = businessCardRepository;

            _account.CustomerId = customer.Id;

            _card = new BusinessCard
            {
                Name = businessCardType,
                AccountNumber = regularAccountNumber
            };
        }

        public void Execute()
        {
            switch (_card.Name)
            {
                case "Business":
                    _card.MaximumTransactionAmount = 100_000_000;
                    _card.CanWithdraw = true;
                    _card.SupportsForeignCurrency = false;
                    break;
                case "Petty":
                    _card.MaximumTransactionAmount = 10_000_000;
                    _card.CanWithdraw = true;
                    _card.SupportsForeignCurrency = false;
                    break;
                case "Deposit":
                    _card.CanWithdraw = false;
                    _card.SupportsForeignCurrency = true;
                    break;
                case "Reward":
                    _card.MaximumTransactionAmount = 100_000_000;
                    _card.CanWithdraw = false;
                    _card.SupportsForeignCurrency = false;
                    break;
            }

            var success = Database.Transaction(
                () =>
                    _accountRepository.Save(_account) &&
                    _businessCardRepository.Save(_card)
            );

            MessageBox.Show(success
                ? "Business account created"
                : "An error occurred while creating business account.");
        }
    }
}