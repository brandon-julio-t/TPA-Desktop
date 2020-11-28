using System;
using System.Collections.Generic;
using TPA_Desktop.Core.Builders;
using TPA_Desktop.Core.Models;
using TPA_Desktop.Core.Models.Abstracts;

namespace TPA_Desktop.Core.Repositories
{
    public class QueueRepository : CrudRepository<Queue>
    {
        public Queue? FindByNumber(long number, string tableName)
        {
            using var reader = QueryBuilder
                .Table(tableName)
                .Join(nameof(Queue), $"{tableName}.ID", "=", "Queue.ID")
                .Where("Number", number.ToString())
                .Select(
                    "Queue.ID",
                    "Number",
                    "QueuedAt",
                    "ServiceStartAt",
                    "ServedAt",
                    "QRCodeID"
                ).Get();

            if (!reader.Read()) return null;

            return new Queue
            {
                Id = reader.GetGuid(0),
                Number = reader.GetInt64(1),
                QueuedAt = reader.GetDateTime(2),
                ServiceStartAt = reader.IsDBNull(3) ? null as DateTime? : reader.GetDateTime(3),
                ServedAt = reader.IsDBNull(4) ? null as DateTime? : reader.GetDateTime(4),
                QrCodeId = reader.IsDBNull(5) ? null as Guid? : reader.GetGuid(5)
            };
        }

        public override Queue FindById(Guid id)
        {
            throw new NotImplementedException();
        }

        public override Queue[] FindAll()
        {
            throw new NotImplementedException();
        }

        public override bool Update(Queue entity)
        {
            return QueryBuilder
                .Table(nameof(Queue))
                .Where("ID", entity.Id.ToString())
                .Update(new Dictionary<string, object?>
                {
                    {"ServiceStartAt", entity.ServiceStartAt},
                    {"ServedAt", entity.ServedAt},
                    {"QRCodeID", entity.QrCodeId}
                });
        }

        public override bool Save(Queue entity)
        {
            throw new NotImplementedException();
        }

        public override bool Delete(Queue entity)
        {
            throw new NotImplementedException();
        }
    }
}