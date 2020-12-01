using System;
using System.Collections.Generic;
using TPA_Desktop.Core.Builders;
using TPA_Desktop.Core.Models;
using TPA_Desktop.Core.Models.Abstracts;

namespace TPA_Desktop.Core.Repositories
{
    public class BusinessCardRepository : CrudRepository<BusinessCard>
    {
        public override BusinessCard FindById(Guid id)
        {
            throw new NotImplementedException();
        }

        public override BusinessCard[] FindAll()
        {
            throw new NotImplementedException();
        }

        public override bool Update(BusinessCard entity)
        {
            throw new NotImplementedException();
        }

        public override bool Save(BusinessCard entity)
        {
            return QueryBuilder
                .Table(nameof(BusinessCard))
                .Insert(new Dictionary<string, object>
                {
                    {"ID", entity.Id},
                    {"Name", entity.Name},
                    {"CanWithdraw", entity.CanWithdraw},
                    {"MaximumTransactionAmount", entity.MaximumTransactionAmount},
                    {"SupportsForeignCurrency", entity.SupportsForeignCurrency}
                });
        }

        public override bool Delete(BusinessCard entity)
        {
            throw new NotImplementedException();
        }
    }
}