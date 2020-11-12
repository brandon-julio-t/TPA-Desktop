using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using TPA_Desktop.Facades;
using TPA_Desktop.Facades.Builders;
using TPA_Desktop.Models.Abstract;

namespace TPA_Desktop.Models
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

                PopulateUserProperties(reader);
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