using System;
using System.Collections.Generic;
using TPA_Desktop.Core.Builders;
using TPA_Desktop.Core.Models;
using TPA_Desktop.Core.Models.Abstracts;

namespace TPA_Desktop.Core.Repositories
{
    public class EquipmentRepository : CrudRepository<Equipment>
    {
        public override Equipment FindById(Guid id)
        {
            using var reader = QueryBuilder.Table(nameof(Equipment)).Where("ID", id.ToString()).Get();
            if (!reader.Read()) throw new Exception($"Equipment with id {id} doesn't exist.");
            return new Equipment
            {
                Id = reader.GetGuid(0),
                EquipmentConditionId = reader.GetGuid(1),
                Floor = reader.GetInt32(2),
                DeletedAt = reader.IsDBNull(3) ? null as DateTime? : reader.GetDateTime(3),
                Name = reader.GetString(4)
            };
        }

        public override Equipment[] FindAll()
        {
            using var reader = QueryBuilder.Table(nameof(Equipment)).Get();
            var entities = new List<Equipment>();
            while (reader.Read())
                entities.Add(new Equipment
                {
                    Id = reader.GetGuid(0),
                    EquipmentConditionId = reader.GetGuid(1),
                    Floor = reader.GetInt32(2),
                    DeletedAt = reader.IsDBNull(3) ? null as DateTime? : reader.GetDateTime(3),
                    Name = reader.GetString(4)
                });
            return entities.ToArray();
        }

        public override bool Update(Equipment entity)
        {
            throw new NotImplementedException();
        }

        public override bool Save(Equipment entity)
        {
            throw new NotImplementedException();
        }

        public override bool Delete(Equipment entity)
        {
            throw new NotImplementedException();
        }
    }
}