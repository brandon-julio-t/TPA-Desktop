using System;
using System.Net.Configuration;
using TPA_Desktop.Core.Models.Abstracts;

namespace TPA_Desktop.Core.Models
{
    public class VirtualAccount : BaseModel
    {
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiredAt { get; set; }
        public DateTime? PaidAt { get; set; }
        public decimal Amount { get; set; }
        
        public override bool Save()
        {
            throw new System.NotImplementedException(); // TODO
        }

        public override bool Delete() => false;
    }
}
