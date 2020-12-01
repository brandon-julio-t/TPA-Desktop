using System;
using System.Collections.Generic;
using TPA_Desktop.Core.Builders;
using TPA_Desktop.Core.Models;
using TPA_Desktop.Core.Models.Abstracts;

namespace TPA_Desktop.Core.Repositories
{
    public class EquipmentConditionRepository : ReadOnlyRepository<EquipmentCondition>
    {
        public override EquipmentCondition FindById(Guid id)
        {
            throw new NotImplementedException();
        }

        public override EquipmentCondition[] FindAll()
        {
            using var reader = QueryBuilder.Table(nameof(EquipmentCondition)).Get();
            var entities = new List<EquipmentCondition>();
            while (reader.Read())
                entities.Add(new EquipmentCondition
                {
                    Id = reader.GetGuid(0),
                    Name = reader.GetString(1)
                });
            return entities.ToArray();
        }
    }
}