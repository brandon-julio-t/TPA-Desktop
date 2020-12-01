using System;
using System.Collections.Generic;
using TPA_Desktop.Core.Builders;
using TPA_Desktop.Core.Models;
using TPA_Desktop.Core.Models.Abstracts;

namespace TPA_Desktop.Core.Repositories
{
    public class TransactionTypeRepository : ReadOnlyRepository<TransactionType>
    {
        public override TransactionType FindById(Guid id)
        {
            throw new NotImplementedException();
        }

        public override TransactionType[] FindAll()
        {
            using var reader = QueryBuilder
                .Table(nameof(TransactionType))
                .Get();
            var entities = new List<TransactionType>();
            while (reader.Read())
                entities.Add(new TransactionType
                {
                    Id = reader.GetGuid(0),
                    Name = reader.GetString(1)
                });
            return entities.ToArray();
        }
    }
}