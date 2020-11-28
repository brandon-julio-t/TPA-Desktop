using System;
using TPA_Desktop.Core.Models.Abstracts;

namespace TPA_Desktop.Core.Models
{
    public class Charge : BaseModel
    {
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public DateTime DueAt { get; set; }
        public DateTime? PaidAt { get; set; }
        public string AccountNumber { get; set; }
        
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