using System;
using System.Collections.Generic;
using System.Windows;
using TPA_Desktop.Core.Builders;

namespace TPA_Desktop.Core.Models
{
    public class PaymentType
    {
        public PaymentType()
        {
        }

        public PaymentType(Guid id)
        {
            using (var reader = QueryBuilder
                .Table("PaymentType")
                .Where("ID", id.ToString())
                .Select("ID", "Name")
                .Get())
            {
                if (!reader.Read() || !reader.HasRows)
                {
                    MessageBox.Show("Payment type doesn't exists");
                    Id = Guid.Empty;
                    return;
                }

                Id = reader.GetGuid(0);
                Name = reader.GetString(1);
            }
        }

        public Guid Id { get; set; }
        public string Name { get; set; }

        public static PaymentType[] All()
        {
            using (var reader = QueryBuilder
                .Table("PaymentType")
                .Select("ID", "Name")
                .Get())
            {
                var paymentTypes = new List<PaymentType>();

                while (reader.Read())
                    paymentTypes.Add(
                        new PaymentType
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