using System;
using System.Collections.Generic;
using System.Data;
using TPA_Desktop.Core.Builders;
using TPA_Desktop.Core.Models.Abstracts;
using TPA_Desktop.Properties;

namespace TPA_Desktop.Core.Models
{
    public class Transaction : BaseModel
    {
        public Transaction()
        {
        }

        public Transaction(string transactionTypeName)
        {
            TransactionType = new TransactionType(transactionTypeName);
        }

        public Customer Customer { get; set; }
        public Account Account { get; set; }
        public DateTime Date { get; set; }
        [CanBeNull] public PaymentType PaymentType { get; set; }
        public TransactionType TransactionType { get; set; }
        public decimal Amount { get; set; }

        public static IEnumerable<Transaction> All()
        {
            using (var reader = QueryBuilder
                .Table("Transaction")
                .Select(
                    "[Transaction].ID",
                    "[Transaction].AccountNumber",
                    "[Transaction].CustomerID",
                    "[Transaction].PaymentTypeID",
                    "[Transaction].TransactionTypeID",
                    "[Transaction].Date",
                    "[Transaction].Amount"
                )
                .Get())
            {
                var transactions = new List<Transaction>();
                while (reader.Read())
                {
                    var transaction = new Transaction();
                    transaction.PopulateProperties(reader);

                    transactions.Add(transaction);
                }

                return transactions.ToArray();
            }
        }

        private void PopulateProperties(IDataRecord reader)
        {
            Id = reader.GetGuid(0);
            Account = new Account(reader.GetString(1));
            Customer = new Customer(reader.GetGuid(2));
            PaymentType = reader.IsDBNull(3) ? null : new PaymentType(reader.GetGuid(3));
            TransactionType = new TransactionType(reader.GetGuid(4));
            Date = reader.GetDateTime(5);
            Amount = reader.GetDecimal(6);
        }

        public override bool Save()
        {
            return QueryBuilder
                .Table("Transaction")
                .Insert(
                    new Dictionary<string, object>
                    {
                        {"ID", Id},
                        {"Date", DateTime.Now},
                        {"PaymentTypeID", PaymentType?.Id},
                        {"TransactionTypeID", TransactionType.Id},
                        {"Amount", Amount},
                        {"CustomerID", Account.Owner.Id},
                        {"AccountNumber", Account.AccountNumber}
                    }
                );
        }

        public override bool Delete()
        {
            return false;
        }
    }
}