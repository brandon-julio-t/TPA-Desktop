using System;
using TPA_Desktop.Core.Models.Abstracts;

namespace TPA_Desktop.Core.Models
{
    public class Equipment : BaseModel
    {
        public string Name { get; set; } = "";
        public int Floor { get; set; }
        public DateTime? DeletedAt { get; set; }
        public Guid EquipmentConditionId { get; set; }

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