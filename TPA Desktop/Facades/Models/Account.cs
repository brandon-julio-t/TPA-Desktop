using System;
using TPA_Desktop.Models.Abstract;

namespace TPA_Desktop.Models
{
    public class Account : BaseModel
    {
        public Customer Owner { get; set; }
        public DateTime BlockedAt { get; set; }
        public DateTime ClosedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool SupportForeignCurrency { get; set; }
        public bool UseAutomaticRollOver { get; set; }
        public decimal AdministrationFee { get; set; }
        public decimal Balance { get; set; }
        public decimal MaximumTransferAmount { get; set; }
        public decimal MaximumWithdrawalAmount { get; set; }
        public decimal MinimumSavingAmount { get; set; }
        public double Interest { get; set; }
        public long AccountNumber { get; set; }
        public long GuardianAccountNumber { get; set; }
        public string Name { get; set; }

        public override bool Save()
        {
            throw new NotImplementedException();
        }

        public override bool Delete()
        {
            throw new NotImplementedException();
        }
    }
}