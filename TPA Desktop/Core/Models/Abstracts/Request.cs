using System;
using TPA_Desktop.Core.Repositories;

namespace TPA_Desktop.Core.Models.Abstracts
{
    public abstract class Request : BaseModel
    {
        private readonly RequestStatusRepository _requestStatusRepository = new RequestStatusRepository();
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Guid RequestStatusId { get; set; }

        protected Request()
        {
            RequestStatusId = _requestStatusRepository.FindByName("Pending").Id;
        }

        public void Approve()
        {
            RequestStatusId = _requestStatusRepository.FindByName("Approved").Id;
            UpdatedAt = DateTime.Now;
        }

        public void Reject()
        {
            RequestStatusId = _requestStatusRepository.FindByName("Rejected").Id;
            UpdatedAt = DateTime.Now;
        }

        public override bool Save()
        {
            throw new System.NotImplementedException();
        }

        public override bool Delete()
        {
            throw new System.NotImplementedException();
        }
    }
}