using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using TPA_Desktop.Core.Interfaces;
using TPA_Desktop.Core.Models;
using TPA_Desktop.Core.Models.Abstracts;
using TPA_Desktop.Core.Repositories;

namespace TPA_Desktop.Views.Departments.Customer_Service.Accounts
{
    public partial class CheckTransactionsWindow : Window
    {
        public CheckTransactionsWindow(
            ICustomerAccountStore customerAccountStore,
            ReadOnlyRepository<Transaction> transactionRepository,
            ReadOnlyRepository<PaymentType> paymentTypeRepository,
            ReadOnlyRepository<TransactionType> transactionTypeRepository)
        {
            InitializeComponent();
            DataContext = new CheckTransactionsWindowViewModel(
                customerAccountStore,
                transactionRepository,
                paymentTypeRepository,
                transactionTypeRepository
            );
        }
    }

    public class CheckTransactionsWindowViewModel
    {
        public CheckTransactionsWindowViewModel(
            ICustomerAccountStore customerAccountStore,
            ReadOnlyRepository<Transaction> transactionRepository,
            ReadOnlyRepository<PaymentType> paymentTypeRepository,
            ReadOnlyRepository<TransactionType> transactionTypeRepository)
        {
            var transactions = (transactionRepository as TransactionRepository)
                ?.FindByAccountNumber(customerAccountStore.Account?.AccountNumber!);
            var paymentTypes = paymentTypeRepository.FindAll();
            var transactionTypes = transactionTypeRepository.FindAll();

            var items =
                from transaction in transactions
                where transaction.Date.Month == DateTime.Today.Month
                join paymentType in paymentTypes on transaction.PaymentTypeId equals paymentType.Id into temp
                from item in temp.DefaultIfEmpty(null)
                join transactionType in transactionTypes on transaction.TransactionTypeId equals transactionType.Id
                select new TransactionDetail
                {
                    Date = transaction.Date,
                    PaymentType = item?.Name ?? "-",
                    TransactionType = transactionType.Name,
                    Amount = transaction.Amount
                };

            TransactionDetails = new ObservableCollection<TransactionDetail>(items);
        }

        public Collection<TransactionDetail> TransactionDetails { get; set; }
    }

    public class TransactionDetail
    {
        public DateTime Date { get; set; }
        public string? PaymentType { get; set; }
        public string TransactionType { get; set; } = "";
        public decimal Amount { get; set; }
    }
}