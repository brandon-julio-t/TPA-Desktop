using System;
using System.Collections.Generic;
using TPA_Desktop.Core.Builders;
using TPA_Desktop.Core.Interfaces;
using TPA_Desktop.Core.Models;

namespace TPA_Desktop.Core.Repositories
{
    public class DebitCardRepository : ICrudRepository<DebitCard>
    {
        public DebitCard FindById(Guid id)
        {
            throw new NotImplementedException();
        }

        public DebitCard[] FindAll()
        {
            throw new NotImplementedException();
        }

        public bool Update(DebitCard entity)
        {
            throw new NotImplementedException();
        }

        public bool Save(DebitCard entity)
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

        public bool Delete(DebitCard entity)
        {
            throw new NotImplementedException();
        }
    }
}