using System;
using System.Collections.Generic;
using TPA_Desktop.Core.Builders;
using TPA_Desktop.Core.Models;
using TPA_Desktop.Core.Models.Abstracts;

namespace TPA_Desktop.Core.Repositories
{
    public class RequestStatusRepository : ReadOnlyRepository<RequestStatus>
    {
        public RequestStatus FindByName(string name)
        {
            using var reader = QueryBuilder
                .Table(nameof(RequestStatus))
                .Where("Name", name)
                .Get();
            
            if (!reader.Read()) throw new Exception($"Request type {name} doesn't exist.");
            
            return new RequestStatus{Id = reader.GetGuid(0), Name = reader.GetString(1)};
        }
        
        public override RequestStatus FindById(Guid id)
        {
            throw new NotImplementedException();
        }

        public override RequestStatus[] FindAll()
        {
            using var reader = QueryBuilder.Table(nameof(RequestStatus)).Get();
            var entities = new List<RequestStatus>();
            while (reader.Read())
                entities.Add(new RequestStatus
                {
                    Id = reader.GetGuid(0),
                    Name = reader.GetString(1)
                });
            return entities.ToArray();
        }
    }
}