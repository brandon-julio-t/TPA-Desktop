using System;
using System.Collections.Generic;
using TPA_Desktop.Core.Builders;
using TPA_Desktop.Core.Models.Abstracts;

namespace TPA_Desktop.Core.Models
{
    public class QrCode : BaseModel
    {
        public string Url { get; set; }
        public DateTime CreatedAt { get; set; }
        
        public QrCode(BaseModel queue)
        {
            Url = $"https://www.kongbubank.com/customers/satisfaction/submit/{queue.Id}";
        }
        
        public override bool Save()
        {
            return QueryBuilder
                .Table(nameof(QrCode))
                .Insert(
                    new Dictionary<string, object>
                    {
                        {"ID", Id},
                        {"URL", Url},
                        {"CreatedAt", DateTime.Now}
                    }
                );
        }

        public override bool Delete()
        {
            return false;
        }
    }
}