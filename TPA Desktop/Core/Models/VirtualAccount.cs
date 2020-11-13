using System;
using System.Collections.Generic;
using System.Net.Configuration;
using TPA_Desktop.Core.Builders;
using TPA_Desktop.Core.Facades;
using TPA_Desktop.Core.Models.Abstracts;

namespace TPA_Desktop.Core.Models
{
    public class VirtualAccount : BaseModel
    {
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiredAt { get; set; }
        public DateTime? PaidAt { get; set; }
        public decimal Amount { get; set; }
        public string DestinationAccountNumber { get; set; }
        public string SourceAccountNumber { get; set; }
        public string VirtualAccountNumber { get; set; }

        public VirtualAccount() => VirtualAccountNumber = Helpers.RandomDigitString(16);

        public override bool Save() =>
            IsSaved
                ? QueryBuilder.Table(nameof(VirtualAccount)).Update(new Dictionary<string, object> {{"PaidAt", PaidAt}})
                : QueryBuilder.Table(nameof(VirtualAccount)).Insert(
                    new Dictionary<string, object>
                    {
                        {nameof(Id), Id},
                        {"CreatedAt", DateTime.Now},
                        {"ExpiredAt", DateTime.Now.Add(TimeSpan.FromDays(3))},
                        {nameof(Amount), Amount},
                        {nameof(DestinationAccountNumber), DestinationAccountNumber},
                        {nameof(SourceAccountNumber), SourceAccountNumber},
                        {nameof(VirtualAccountNumber), VirtualAccountNumber}
                    }
                );

        public override bool Delete() => false;
    }
}