using System;
using System.Collections.Generic;
using System.Linq;
using TPA_Desktop.Core.Builders;
using TPA_Desktop.Core.Models;
using TPA_Desktop.Core.Models.Abstracts;

namespace TPA_Desktop.Views.Shared
{
    public class AccountRepository : CrudRepository<Account>
    {
        public override Account FindById(Guid id)
        {
            throw new NotImplementedException();
        }

        public override Account[] FindAll()
        {
            using var reader = QueryBuilder.Table(nameof(Account)).Get();
            var entities = new List<Account>();
            while (reader.Read())
                entities.Add(new Account
                {
                    CustomerId = reader.GetGuid(0),
                    AccountNumber = reader.GetString(1),
                    Balance = reader.GetDecimal(2),
                    Interest = reader.GetFloat(3),
                    MaximumWithdrawalAmount = reader.GetDecimal(4),
                    MaximumTransferAmount = reader.GetDecimal(5),
                    GuardianAccountNumber = reader.IsDBNull(6) ? null : reader.GetString(6),
                    SupportForeignCurrency = reader.GetBoolean(7),
                    Name = reader.GetString(8),
                    BlockedAt = reader.IsDBNull(9) ? null as DateTime? : reader.GetDateTime(9),
                    CreatedAt = reader.GetDateTime(10),
                    ClosedAt = reader.IsDBNull(11) ? null as DateTime? : reader.GetDateTime(11),
                    AdministrationFee = reader.GetDecimal(12),
                    MinimumSavingAmount = reader.GetDecimal(13),
                    UseAutomaticRollOver = reader.GetBoolean(14)
                });
            return entities
                .Where(e => e.BlockedAt == null as DateTime?)
                .Where(e => e.ClosedAt == null as DateTime?)
                .ToArray();
        }

        public override bool Update(Account entity)
        {
            throw new NotImplementedException();
        }

        public override bool Save(Account entity)
        {
            throw new NotImplementedException();
        }

        public override bool Delete(Account entity)
        {
            throw new NotImplementedException();
        }
    }
}