using System;
using System.Collections.Generic;
using System.Linq;
using TPA_Desktop.Facades.Builders;
using TPA_Desktop.Models;
using TPA_Desktop.Models.Abstract;

namespace TPA_Desktop.Facades.Models
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
        public string AccountNumber { get; set; }
        public string GuardianAccountNumber { get; set; }
        public string Name { get; set; }

        public override bool Save()
        {
            var rand = new Random();
            AccountNumber = string.Join("", Enumerable.Range(0, 16).Select(_ => rand.Next(10)));

            CreatedAt = DateTime.Now;

            return QueryBuilder
                .Table("Account")
                .Insert(
                    new Dictionary<string, object>
                    {
                        {"CustomerID", Owner.Id},
                        {nameof(AccountNumber), AccountNumber},
                        {nameof(AdministrationFee), AdministrationFee},
                        {nameof(Balance), Balance},
                        {nameof(CreatedAt), CreatedAt},
                        {nameof(GuardianAccountNumber), GuardianAccountNumber},
                        {nameof(Interest), Interest},
                        {nameof(MaximumTransferAmount), MaximumTransferAmount},
                        {nameof(MaximumWithdrawalAmount), MaximumWithdrawalAmount},
                        {nameof(MinimumSavingAmount), MinimumSavingAmount},
                        {nameof(Name), Name},
                        {nameof(SupportForeignCurrency), SupportForeignCurrency},
                        {nameof(UseAutomaticRollOver), UseAutomaticRollOver}
                    }
                );
        }

        public override bool Delete()
        {
            throw new NotImplementedException();
        }
    }
}