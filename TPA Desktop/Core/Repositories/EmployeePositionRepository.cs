using System;
using TPA_Desktop.Core.Builders;
using TPA_Desktop.Core.Models;
using TPA_Desktop.Core.Models.Abstracts;

namespace TPA_Desktop.Core.Repositories
{
    public class EmployeePositionRepository : ReadOnlyRepository<EmployeePosition>
    {
        public EmployeePosition FindByName(string name)
        {
            using var reader = QueryBuilder
                .Table(nameof(EmployeePosition))
                .Where("Name", name)
                .Get();

            if (!reader.Read()) throw new Exception($"Employee position {name} doesn't exist.");

            return new EmployeePosition {Id = reader.GetGuid(0), Name = reader.GetString(1)};
        }

        public override EmployeePosition FindById(Guid id)
        {
            throw new NotImplementedException();
        }

        public override EmployeePosition[] FindAll()
        {
            throw new NotImplementedException();
        }
    }
}