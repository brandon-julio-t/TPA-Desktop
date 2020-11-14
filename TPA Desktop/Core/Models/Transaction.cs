using System;
using System.Collections.Generic;
using TPA_Desktop.Core.Builders;
using TPA_Desktop.Core.Models.Abstracts;
using TPA_Desktop.Properties;

namespace TPA_Desktop.Core.Models
{
    public class Transaction : BaseModel
    {
        public Transaction(string transactionTypeName)
        {
            TransactionType = new TransactionType(transactionTypeName);
        }

        public Account Account { get; set; }
        public DateTime Date { get; set; }
        [CanBeNull] public PaymentType PaymentType { get; set; }
        public TransactionType TransactionType { get; set; }
        public decimal Amount { get; set; }

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