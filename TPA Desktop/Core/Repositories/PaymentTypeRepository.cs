using System;
using System.Collections.Generic;
using TPA_Desktop.Core.Builders;
using TPA_Desktop.Core.Models;
using TPA_Desktop.Core.Models.Abstracts;

namespace TPA_Desktop.Core.Repositories
{
    public class PaymentTypeRepository : ReadOnlyRepository<PaymentType>
    {
        public override PaymentType FindById(Guid id)
        {
            throw new NotImplementedException();
        }

        public override PaymentType[] FindAll()
        {
            using var reader = QueryBuilder
                .Table(nameof(PaymentType))
                .Get();
            var entities = new List<PaymentType>();
            while (reader.Read())
                entities.Add(new PaymentType
                {
                    Id = reader.GetGuid(0),
                    Name = reader.GetString(1)
                });
            return entities.ToArray();
        }
    }
}