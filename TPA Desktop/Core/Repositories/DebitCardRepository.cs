using System;
using System.Collections.Generic;
using TPA_Desktop.Core.Builders;
using TPA_Desktop.Core.Interfaces;
using TPA_Desktop.Core.Models;
using TPA_Desktop.Core.Models.Abstracts;

namespace TPA_Desktop.Core.Repositories
{
    public class DebitCardRepository : CrudRepository<DebitCard>
    {
        public override DebitCard FindById(Guid id)
        {
            throw new NotImplementedException();
        }

        // ReSharper disable once UnusedMember.Global
        public override DebitCard[] FindAll()
        {
            throw new NotImplementedException();
        }

        // ReSharper disable once UnusedMember.Global
        public override bool Update(DebitCard entity)
        {
            throw new NotImplementedException();
        }

        public override bool Save(DebitCard entity)
        {
            return QueryBuilder
                .Table(nameof(DebitCard))
                .Insert(
                    new Dictionary<string, object>
                    {
                        {"ID", entity.Id},
                        {"AccountNumber", entity.Account.AccountNumber}
                    }
                );
        }

        public override bool Delete(DebitCard entity)
        {
            throw new NotImplementedException();
        }
    }
}