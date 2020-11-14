using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using TPA_Desktop.Core.Builders;
using TPA_Desktop.Core.Facades;
using TPA_Desktop.Core.Models.Abstracts;

namespace TPA_Desktop.Core.Models
{
    public class VirtualAccount : BaseModel
    {
        public VirtualAccount()
        {
            VirtualAccountNumber = Helpers.RandomDigitString(16);
        }

        public VirtualAccount(string accountNumber)
        {
            using (var reader = QueryBuilder
                .Table("VirtualAccount")
                .Where("VirtualAccountNumber", accountNumber)
                .Select(
                    "ID",
                    "SourceAccountNumber",
                    "DestinationAccountNumber",
                    "VirtualAccountNumber",
                    "Amount",
                    "CreatedAt",
                    "ExpiredAt",
                    "PaidAt"
                ).Get())
            {
                if (!reader.Read() || !reader.HasRows)
                {
                    MessageBox.Show("Virtual account doesn't exist.");
                    return;
                }

                PopulateProperties(reader);
                IsSaved = true;
            }
        }

        public DateTime CreatedAt { get; set; }
        public DateTime ExpiredAt { get; set; }
        public DateTime? PaidAt { get; set; }
        public decimal Amount { get; set; }
        public string DestinationAccountNumber { get; set; }
        public string SourceAccountNumber { get; set; }
        public string VirtualAccountNumber { get; set; }

        private void PopulateProperties(IDataRecord reader)
        {
            Id = reader.GetGuid(0);
            SourceAccountNumber = reader.GetString(1);
            DestinationAccountNumber = reader.GetString(2);
            VirtualAccountNumber = reader.GetString(3);
            Amount = reader.GetDecimal(4);
            CreatedAt = reader.GetDateTime(5);
            ExpiredAt = reader.GetDateTime(6);
            PaidAt = reader.IsDBNull(7) ? (DateTime?) null : reader.GetDateTime(7);
        }

        public override bool Save()
        {
            return IsSaved
                ? QueryBuilder
                    .Table(nameof(VirtualAccount))
                    .Where("ID", Id.ToString())
                    .Update(new Dictionary<string, object> {{"PaidAt", PaidAt}})
                : QueryBuilder
                    .Table(nameof(VirtualAccount))
                    .Insert(
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
        }

        public override bool Delete()
        {
            return false;
        }
    }
}