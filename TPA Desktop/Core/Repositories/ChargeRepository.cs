using System;
using System.Collections.Generic;
using TPA_Desktop.Core.Builders;
using TPA_Desktop.Core.Models;
using TPA_Desktop.Core.Models.Abstracts;

namespace TPA_Desktop.Core.Repositories
{
    public class ChargeRepository : CrudRepository<Charge>
    {
        public override Charge FindById(Guid id)
        {
            throw new NotImplementedException();
        }

        public override Charge[] FindAll()
        {
            throw new NotImplementedException();
        }

        public override bool Update(Charge entity)
        {
            throw new NotImplementedException();
        }

        public override bool Save(Charge entity)
        {
            return QueryBuilder
                .Table(nameof(Charge))
                .Insert(new Dictionary<string, object>
                {
                    {"ID", entity.Id},
                    {"Description", entity.Description},
                    {"DueAt", entity.DueAt},
                    {"Amount", entity.Amount},
                    {"AccountNumber", entity.AccountNumber}
                });
        }

        public override bool Delete(Charge entity)
        {
            throw new NotImplementedException();
        }
    }
}