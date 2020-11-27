using TPA_Desktop.Core.Models.Abstracts;

namespace TPA_Desktop.Core.Models
{
    public class Document : BaseModel
    {
        public DocumentType DocumentType { get; set; }
        public decimal Value { get; set; }
        public string Comment { get; set; }
        public string DocumentId { get; set; }
        
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