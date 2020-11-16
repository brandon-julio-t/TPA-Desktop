using System;
using System.Windows;
using TPA_Desktop.Core.Builders;

namespace TPA_Desktop.Core.Models
{
    public class TransactionType
    {
        public TransactionType(string name)
        {
            using (var reader = QueryBuilder
                .Table("TransactionType")
                .Select("ID", "Name")
                .Where("Name", name)
                .Get())
            {
                if (!reader.Read() || !reader.HasRows)
                {
                    MessageBox.Show("Transaction Type doesn't exist");
                    Id = Guid.Empty;
                    return;
                }

                Id = reader.GetGuid(0);
                Name = reader.GetString(1);
            }
        }

        public Guid Id { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}