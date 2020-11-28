using System;
using System.Collections.Generic;
using TPA_Desktop.Core.Builders;
using TPA_Desktop.Core.Models;
using TPA_Desktop.Core.Models.Abstracts;

namespace TPA_Desktop.Core.Repositories
{
    public class NotificationRepository : CrudRepository<Notification>
    {
        public Notification[] FindByEmployeePosition(EmployeePosition position)
        {
            using var reader = QueryBuilder
                .Table(nameof(Notification))
                .Where("EmployeePositionID", position.Id.ToString())
                .Where("ReadAt", null)
                .Get();

            var entities = new List<Notification>();
            while (reader.Read())
                entities.Add(new Notification
                {
                    Id = reader.GetGuid(0),
                    Title = reader.GetString(1),
                    CreatedAt = reader.GetDateTime(2),
                    ReadAt = reader.IsDBNull(3) ? null as DateTime? : reader.GetDateTime(3)
                });
            return entities.ToArray();
        }

        public override Notification FindById(Guid id)
        {
            throw new NotImplementedException();
        }

        public override Notification[] FindAll()
        {
            throw new NotImplementedException();
        }

        public override bool Update(Notification entity)
        {
            return QueryBuilder
                .Table(nameof(Notification))
                .Where("ID", entity.Id.ToString())
                .Update(new Dictionary<string, object?>
                {
                    {"ReadAt", entity.ReadAt}
                });
        }

        public override bool Save(Notification entity)
        {
            return QueryBuilder
                .Table(nameof(Notification))
                .Insert(new Dictionary<string, object>
                {
                    {"ID", entity.Id},
                    {"Title", entity.Title}
                });
        }

        public override bool Delete(Notification entity)
        {
            throw new NotImplementedException();
        }
    }
}