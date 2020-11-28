using System;
using System.Collections.Generic;
using System.Linq;
using TPA_Desktop.Core.Builders;
using TPA_Desktop.Core.Models;
using TPA_Desktop.Core.Models.Abstracts;

namespace TPA_Desktop.Core.Repositories
{
    public class ExpenseRequestTypeRepository : ReadOnlyRepository<ExpenseRequestType>
    {
        public ExpenseRequestType FindByName(string name)
        {
            using var reader = QueryBuilder
                .Table(nameof(ExpenseRequestType))
                .Where("Name", name)
                .Get();

            if (!reader.Read()) throw new Exception($"Expense request type {name} doesn't exist.");

            return new ExpenseRequestType {Id = reader.GetGuid(0), Name = reader.GetString(1)};
        }

        public override ExpenseRequestType FindById(Guid id)
        {
            throw new NotImplementedException();
        }

        public override ExpenseRequestType[] FindAll()
        {
            using var reader = QueryBuilder.Table(nameof(ExpenseRequestType)).Get();
            var entities = new List<ExpenseRequestType>();
            while (reader.Read())
                entities.Add(new ExpenseRequestType
                {
                    Id = reader.GetGuid(0),
                    Name = reader.GetString(1)
                });
            return entities.ToArray();
        }
    }
}