using System;
using TPA_Desktop.Core.Models.Abstracts;

namespace TPA_Desktop.Core.Models
{
    public class CreditCard : BaseModel
    {
        public Guid CreditCardCompanyID { get; set; }
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