using System;
using System.Collections.Generic;
using System.Linq;
using TPA_Desktop.Core.Builders;
using TPA_Desktop.Core.Facades;
using TPA_Desktop.Core.Models;
using TPA_Desktop.Core.Models.Abstracts;

namespace TPA_Desktop.Core.Repositories
{
    public class ExpenseRequestRepository : CrudRepository<ExpenseRequest>
    {
        public override ExpenseRequest FindById(Guid id)
        {
            using var reader = QueryBuilder
                .Table(nameof(ExpenseRequest))
                .Join(nameof(Request), "ExpenseRequest.ID", "=", "Request.ID")
                .Select(
                    "ExpenseRequest.ID",
                    "CreatedAt",
                    "EntityID",
                    "UpdatedAt",
                    "RequestStatusID",
                    "ExpenseRequestTypeID"
                ).Get();
            if (!reader.Read() || !reader.HasRows) throw new Exception($"Expense request with id {id} doesn't exist.");
            return new ExpenseRequest
            {
                Id = reader.GetGuid(0),
                CreatedAt = reader.GetDateTime(1),
                EntityId = reader.GetGuid(2),
                UpdatedAt = reader.IsDBNull(3) ? null as DateTime? : reader.GetDateTime(3),
                RequestStatusId = reader.GetGuid(4),
                ExpenseRequestTypeId = reader.GetGuid(5)
            };
        }

        public override ExpenseRequest[] FindAll()
        {
            using var reader = QueryBuilder
                .Table(nameof(ExpenseRequest))
                .Join(nameof(Request), "ExpenseRequest.ID", "=", "Request.ID")
                .Select(
                    "ExpenseRequest.ID",
                    "ExpenseRequestTypeID",
                    "EntityID",
                    "CreatedAt",
                    "UpdatedAt",
                    "RequestStatusID"
                ).Get();
            var entities = new List<ExpenseRequest>();
            while (reader.Read())
                entities.Add(new ExpenseRequest
                {
                    Id = reader.GetGuid(0),
                    ExpenseRequestTypeId = reader.GetGuid(1),
                    EntityId = reader.GetGuid(2),
                    CreatedAt = reader.GetDateTime(3),
                    UpdatedAt = reader.IsDBNull(4) ? null as DateTime? : reader.GetDateTime(4),
                    RequestStatusId = reader.GetGuid(5),
                });
            return entities.Where(entity =>
            {
                var requestStatusRepository = new RequestStatusRepository();
                var pendingId = requestStatusRepository.FindByName("Pending").Id;
                return entity.RequestStatusId == pendingId;
            }).ToArray();
        }

        public override bool Update(ExpenseRequest entity)
        {
            return QueryBuilder
                .Table(nameof(Request))
                .Where("ID", entity.Id.ToString())
                .Update(new Dictionary<string, object?>
                {
                    {"UpdatedAt", entity.UpdatedAt}, 
                    {"RequestStatusID", entity.RequestStatusId}
                });
        }

        public override bool Save(ExpenseRequest entity)
        {
            return
                QueryBuilder
                    .Table(nameof(Request))
                    .Insert(new Dictionary<string, object>
                    {
                        {"ID", entity.Id},
                        {"RequestStatusID", entity.RequestStatusId}
                    })
                &&
                QueryBuilder
                    .Table(nameof(ExpenseRequest))
                    .Insert(new Dictionary<string, object>
                    {
                        {"ID", entity.Id},
                        {"EntityID", entity.EntityId},
                        {"ExpenseRequestTypeID", entity.ExpenseRequestTypeId}
                    });
        }

        public override bool Delete(ExpenseRequest entity)
        {
            throw new NotImplementedException();
        }
    }
}