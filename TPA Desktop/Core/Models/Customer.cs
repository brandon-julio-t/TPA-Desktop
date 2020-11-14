using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using TPA_Desktop.Core.Builders;
using TPA_Desktop.Core.Facades;
using TPA_Desktop.Core.Models.Abstracts;

namespace TPA_Desktop.Core.Models
{
    public class Customer : User
    {
        public Customer()
        {
        }

        public Customer(string firstName, string lastName, DateTime dateOfBirth, string motherMaidenName)
        {
            using (var reader = QueryBuilder
                .Table("Customer")
                .Join("User", "[User].ID", "=", "Customer.ID")
                .Where("FirstName", firstName)
                .Where("LastName", lastName)
                .Where("DateOfBirth", dateOfBirth.ToString(CultureInfo.InvariantCulture))
                .Where("MotherMaidenName", motherMaidenName)
                .Select(
                    "Customer.ID",
                    "FirstName",
                    "LastName",
                    "Gender",
                    "DateOfBirth",
                    "RegisteredAt",
                    "DeletedAt",
                    "PhoneNumber",
                    "IsBusinessOwner",
                    "MotherMaidenName"
                ).Get())
            {
                if (!reader.Read() || !reader.HasRows)
                {
                    MessageBox.Show("Customer doesn't exist.");
                    Id = Guid.Empty;
                    return;
                }

                PopulateProperties(reader);
                IsBusinessOwner = reader.GetBoolean(8);
                MotherMaidenName = reader.GetString(9);
            }
        }

        public bool IsBusinessOwner { get; set; }
        public string MotherMaidenName { get; set; }

        public new bool Validate()
        {
            return base.Validate()
                   && new Validator("Is Business Owner", IsBusinessOwner as object).NotEmpty().IsValid
                   && new Validator("Mother's Maiden Name", MotherMaidenName).NotEmpty().IsValid;
        }

        public static IEnumerable<Customer> All()
        {
            using (var reader = QueryBuilder
                .Table("Customer")
                .Join("User", "[User].ID", "=", "Customer.ID")
                .Select(
                    "Customer.ID",
                    "FirstName",
                    "LastName",
                    "Gender",
                    "DateOfBirth",
                    "RegisteredAt",
                    "DeletedAt",
                    "PhoneNumber",
                    "IsBusinessOwner",
                    "MotherMaidenName"
                ).Get())
            {
                var customers = new List<Customer>();
                while (reader.Read())
                {
                    var customer = new Customer();
                    customer.PopulateProperties(reader);
                    customer.IsBusinessOwner = reader.GetBoolean(8);
                    customer.MotherMaidenName = reader.GetString(9);
                    customers.Add(customer);
                }

                return customers.ToArray();
            }
        }

        public override bool Save()
        {
            var userData = new Dictionary<string, object>
            {
                {"ID", Id},
                {"FirstName", FirstName},
                {"LastName", LastName},
                {"Gender", Gender},
                {"DateOfBirth", DateOfBirth},
                {"RegisteredAt", DateTime.Now},
                {"PhoneNumber", PhoneNumber}
            };

            var customerData = new Dictionary<string, object>
            {
                {"ID", Id},
                {"IsBusinessOwner", IsBusinessOwner},
                {"MotherMaidenName", MotherMaidenName}
            };

            var queryBuilderUser = QueryBuilder.Table("User");
            var queryBuilderCustomer = QueryBuilder.Table("Customer");

            return Database.Transaction(() =>
                queryBuilderUser.Insert(userData) && queryBuilderCustomer.Insert(customerData));
        }

        public override bool Delete()
        {
            throw new NotImplementedException();
        }
    }
}