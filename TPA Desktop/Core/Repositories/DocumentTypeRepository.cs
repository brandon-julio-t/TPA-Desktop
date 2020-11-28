using System;
using System.Collections.Generic;
using System.Windows;
using TPA_Desktop.Core.Builders;
using TPA_Desktop.Core.Interfaces;
using TPA_Desktop.Core.Models;
using TPA_Desktop.Core.Models.Abstracts;
using Environment = TPA_Desktop.Core.Facades.Environment;

namespace TPA_Desktop.Core.Repositories
{
    public class DocumentTypeRepository : ReadOnlyRepository<DocumentType>
    {
        public override DocumentType FindById(Guid id)
        {
            using var reader = QueryBuilder
                .Table(nameof(DocumentType))
                .Get();

            if (!reader.Read() || !reader.HasRows)
                throw new Exception(Environment.IsDevelopment
                    ? $"Document type with id {id} doesn't exist."
                    : "Document type doesn't exist. Please contact DBA.");

            return new DocumentType
            {
                Id = reader.GetGuid(0),
                Name = reader.GetString(1)
            };
        }

        public override DocumentType[] FindAll()
        {
            using var reader = QueryBuilder
                .Table(nameof(DocumentType))
                .Get();

            var entities = new List<DocumentType>();
            while (reader.Read())
            {
                var entity = new DocumentType
                {
                    Id = reader.GetGuid(0),
                    Name = reader.GetString(1)
                };
                entities.Add(entity);
            }

            return entities.ToArray();
        }
    }
}