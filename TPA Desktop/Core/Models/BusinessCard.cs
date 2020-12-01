using TPA_Desktop.Core.Models.Abstracts;

namespace TPA_Desktop.Core.Models
{
    public class BusinessCard : BaseModel
    {
        public string AccountNumber { get; set; } = "";
        public string Name { get; set; } = "";
        public bool CanWithdraw { get; set; } = false;
        public decimal MaximumTransactionAmount { get; set; } = 100_000_000;
        public bool SupportsForeignCurrency { get; set; } = false;

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