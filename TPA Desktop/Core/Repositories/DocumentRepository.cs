using System;
using System.Collections.Generic;
using TPA_Desktop.Core.Builders;
using TPA_Desktop.Core.Interfaces;
using TPA_Desktop.Core.Models;
using TPA_Desktop.Core.Models.Abstracts;

namespace TPA_Desktop.Core.Repositories
{
    public class DocumentRepository : CrudRepository<Document>
    {
        public override Document FindById(Guid id)
        {
            throw new NotImplementedException();
        }

        public override Document[] FindAll()
        {
            throw new NotImplementedException();
        }

        public override bool Update(Document entity)
        {
            throw new NotImplementedException();
        }

        public override bool Save(Document entity)
        {
            return QueryBuilder
                .Table(nameof(Document))
                .Insert(new Dictionary<string, object>
                {
                    {"ID", entity.Id},
                    {"Value", entity.Value},
                    {"Comment", entity.Comment},
                    {"DocumentId", entity.DocumentId}
                });
        }

        public override bool Delete(Document entity)
        {
            throw new NotImplementedException();
        }
    }
}