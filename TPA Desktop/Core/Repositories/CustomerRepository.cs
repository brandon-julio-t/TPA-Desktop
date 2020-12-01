using System;
using System.Collections.Generic;
using TPA_Desktop.Core.Builders;
using TPA_Desktop.Core.Models;
using TPA_Desktop.Core.Models.Abstracts;

namespace TPA_Desktop.Core.Repositories
{
    public class CustomerRepository : CrudRepository<Customer>
    {
        public override Customer FindById(Guid id)
        {
            throw new NotImplementedException();
        }

        public override Customer[] FindAll()
        {
            using var reader = QueryBuilder
                .Table(nameof(Customer))
                .Join(nameof(User), "Customer.ID", "=", "[User].ID")
                .Select(
                    "Customer.ID",
                    "FirstName",
                    "LastName",
                    "Gender",
                    "DateOfBirth",
                    "RegisteredAt",
                    "DeletedAt",
                    "PhoneNumber",
                    "IsBusinessOwner",
                    "MotherMaidenName"
                ).Get();
            var entities = new List<Customer>();
            while (reader.Read())
                entities.Add(new Customer
                {
                    Id = reader.GetGuid(0),
                    FirstName = reader.GetString(1),
                    LastName = reader.GetString(2),
                    Gender = reader.GetString(3),
                    DateOfBirth = reader.GetDateTime(4),
                    RegisteredAt = reader.GetDateTime(5),
                    DeletedAt = reader.IsDBNull(6) ? null as DateTime? : reader.GetDateTime(6),
                    PhoneNumber = reader.GetString(7),
                    IsBusinessOwner = reader.GetBoolean(8),
                    MotherMaidenName = reader.GetString(9)
                });
            return entities.ToArray();
        }

        public override bool Update(Customer entity)
        {
            throw new NotImplementedException();
        }

        public override bool Save(Customer entity)
        {
            throw new NotImplementedException();
        }

        public override bool Delete(Customer entity)
        {
            throw new NotImplementedException();
        }
    }
}