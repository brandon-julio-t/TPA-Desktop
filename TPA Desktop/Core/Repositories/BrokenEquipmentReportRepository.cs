using System;
using System.Collections.Generic;
using TPA_Desktop.Core.Builders;
using TPA_Desktop.Core.Models;
using TPA_Desktop.Core.Models.Abstracts;

namespace TPA_Desktop.Core.Repositories
{
    public class BrokenEquipmentReportRepository : CrudRepository<BrokenEquipmentReport>
    {
        public override BrokenEquipmentReport FindById(Guid id)
        {
            throw new NotImplementedException();
        }

        public override BrokenEquipmentReport[] FindAll()
        {
            using var reader = QueryBuilder.Table(nameof(BrokenEquipmentReport)).Get();
            var entities = new List<BrokenEquipmentReport>();
            while (reader.Read())
                entities.Add(new BrokenEquipmentReport
                {
                    Id = reader.GetGuid(0),
                    EquipmentId = reader.GetGuid(1),
                    ReportedAt = reader.GetDateTime(2),
                    Description = reader.GetString(3),
                    EmployeeId = reader.GetGuid(4)
                });
            return entities.ToArray();
        }

        public override bool Update(BrokenEquipmentReport entity)
        {
            throw new NotImplementedException();
        }

        public override bool Save(BrokenEquipmentReport entity)
        {
            return QueryBuilder
                .Table(nameof(BrokenEquipmentReport))
                .Insert(new Dictionary<string, object>
                {
                    {"ID", entity.Id},
                    {"EmployeeId", entity.EmployeeId},
                    {"ReportedAt", entity.ReportedAt},
                    {"EquipmentId", entity.EquipmentId},
                    {"Description", entity.Description}
                });
        }

        public override bool Delete(BrokenEquipmentReport entity)
        {
            throw new NotImplementedException();
        }
    }
}