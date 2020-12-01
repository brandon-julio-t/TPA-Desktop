using System;
using TPA_Desktop.Core.Models.Abstracts;

namespace TPA_Desktop.Core.Models
{
    public class BrokenEquipmentReport : BaseModel
    {
        public DateTime ReportedAt { get; set; }
        public string Description { get; set; } = "";
        public Guid EquipmentId { get; set; }
        public Guid EmployeeId { get; set; }

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