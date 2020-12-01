using System;
using System.Collections.Generic;
using TPA_Desktop.Core.Builders;
using TPA_Desktop.Core.Models;
using TPA_Desktop.Core.Models.Abstracts;

namespace TPA_Desktop.Core.Repositories
{
    public class TransactionRepository : CrudRepository<Transaction>
    {
        public Transaction[] FindByAccountNumber(string accountNumber)
        {
            using var reader = QueryBuilder
                .Table(nameof(Transaction))
                .Where("AccountNumber", accountNumber)
                .Where("month(Date)", DateTime.Today.Month.ToString())
                .Get();
            var entities = new List<Transaction>();
            while (reader.Read())
                entities.Add(new Transaction
                {
                    Id = reader.GetGuid(0),
                    TransactionTypeId = reader.GetGuid(1),
                    PaymentTypeId = reader.IsDBNull(2) ? null as Guid? : reader.GetGuid(2),
                    CustomerId = reader.GetGuid(3),
                    Date = reader.GetDateTime(4),
                    Amount = reader.GetDecimal(5),
                    AccountNumber = reader.GetString(6),
                });
            return entities.ToArray();
        }

        public override Transaction FindById(Guid id)
        {
            throw new NotImplementedException();
        }

        public override Transaction[] FindAll()
        {
            throw new NotImplementedException();
        }

        public override bool Update(Transaction entity)
        {
            throw new NotImplementedException();
        }

        public override bool Save(Transaction entity)
        {
            throw new NotImplementedException();
        }

        public override bool Delete(Transaction entity)
        {
            throw new NotImplementedException();
        }
    }
}