using TPA_Desktop.Core.Models.Abstracts;

namespace TPA_Desktop.Core.Models
{
    public class EquipmentCondition : BaseModel
    {
        public string Name { get; set; } = "";

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