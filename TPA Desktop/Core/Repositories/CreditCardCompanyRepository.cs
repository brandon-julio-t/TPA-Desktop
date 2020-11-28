using System;
using System.Collections.Generic;
using TPA_Desktop.Core.Builders;
using TPA_Desktop.Core.Interfaces;
using TPA_Desktop.Core.Models;
using TPA_Desktop.Core.Models.Abstracts;

namespace TPA_Desktop.Core.Repositories
{
    public class CreditCardCompanyRepository : ReadOnlyRepository<CreditCardCompany>
    {
        public override CreditCardCompany FindById(Guid id)
        {
            throw new NotImplementedException();
        }

        public override CreditCardCompany[] FindAll()
        {
            using var reader = QueryBuilder
                .Table(nameof(CreditCardCompany))
                .Get();

            var entities = new List<CreditCardCompany>();
            while (reader.Read())
            {
                entities.Add(new CreditCardCompany
                {
                    Id = reader.GetGuid(0),
                    Email = reader.GetString(1),
                    Name = reader.GetString(2),
                    PhoneNumber = reader.GetString(3),
                    Address = reader.GetString(4)
                });
            }

            return entities.ToArray();
        }
    }
}