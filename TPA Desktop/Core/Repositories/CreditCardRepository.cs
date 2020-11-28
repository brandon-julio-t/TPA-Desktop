using System;
using System.Collections.Generic;
using TPA_Desktop.Core.Builders;
using TPA_Desktop.Core.Models;
using TPA_Desktop.Core.Models.Abstracts;

namespace TPA_Desktop.Core.Repositories
{
    public class CreditCardRepository : CrudRepository<CreditCard>
    {
        public CreditCard FindByAccountNumber(string accountNumber)
        {
            using var reader = QueryBuilder
                .Table(nameof(CreditCard))
                .Where("AccountNumber", accountNumber)
                .Get();

            if (!reader.Read()) throw new Exception($"Credit card with account number {accountNumber} doesn't exist.");

            return new CreditCard
            {
                Id = reader.GetGuid(0),
                CreditCardCompanyID = reader.GetGuid(1),
                AccountNumber = accountNumber
            };
        }

        public override CreditCard FindById(Guid id)
        {
            throw new NotImplementedException();
        }

        public override CreditCard[] FindAll()
        {
            using var reader = QueryBuilder
                .Table(nameof(CreditCard))
                .Get();
            var entities = new List<CreditCard>();
            while (reader.Read())
                entities.Add(new CreditCard
                {
                    Id = reader.GetGuid(0),
                    CreditCardCompanyID = reader.GetGuid(1),
                    AccountNumber = reader.GetString(2)
                });
            return entities.ToArray();
        }

        public override bool Update(CreditCard entity)
        {
            throw new NotImplementedException();
        }

        public override bool Save(CreditCard entity)
        {
            return QueryBuilder
                .Table(nameof(CreditCard))
                .Insert(new Dictionary<string, object>
                {
                    {"ID", entity.Id},
                    {"CreditCardCompanyID", entity.CreditCardCompanyID},
                    {"AccountNumber", entity.AccountNumber}
                });
        }

        public override bool Delete(CreditCard entity)
        {
            throw new NotImplementedException();
        }
    }
}