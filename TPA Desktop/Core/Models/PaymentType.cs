using System;
using System.Collections.Generic;
using TPA_Desktop.Core.Builders;

namespace TPA_Desktop.Core.Models
{
    public class PaymentType
    {
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