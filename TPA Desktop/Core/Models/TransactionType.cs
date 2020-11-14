using System;
using System.Collections.Generic;
using System.Windows;
using TPA_Desktop.Core.Builders;

namespace TPA_Desktop.Core.Models
{
    public class TransactionType
    {
        public TransactionType()
        {
        }

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

        public static TransactionType[] All()
        {
            using (var reader = QueryBuilder
                .Table("TransactionType")
                .Select("ID", "Name")
                .Get())
            {
                var paymentTypes = new List<TransactionType>();

                while (reader.Read())
                    paymentTypes.Add(
                        new TransactionType
                        {
                            Id = reader.GetGuid(0),
                            Name = reader.GetString(1)
                        }
                    );

                return paymentTypes.ToArray();
            }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}