using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using TPA_Desktop.Core.Builders;
using TPA_Desktop.Core.Facades;
using TPA_Desktop.Core.Models.Abstracts;

namespace TPA_Desktop.Core.Models
{
    public class Account : BaseModel
    {
        public Account()
        {
        }

        public Account(string accountNumber)
        {
            string firstName;
            string lastName;
            DateTime dateOfBirth;
            string motherMaidenName;

            using (var reader = QueryBuilder
                .Table("Account")
                .Join("Customer", "Account.CustomerID", "=", "Customer.ID")
                .Join("User", "Customer.ID", "=", "[User].ID")
                .Where("AccountNumber", accountNumber)
                .Select(
                    "AccountNumber",
                    "Balance",
                    "Interest",
                    "MaximumWithdrawalAmount",
                    "MaximumTransferAmount",
                    "GuardianAccountNumber",
                    "SupportForeignCurrency",
                    "Name",
                    "BlockedAt",
                    "CreatedAt",
                    "ClosedAt",
                    "AdministrationFee",
                    "MinimumSavingAmount",
                    "UseAutomaticRollOver",
                    "FirstName",
                    "LastName",
                    "DateOfBirth",
                    "MotherMaidenName"
                ).Get())
            {
                if (!reader.Read() || !reader.HasRows)
                {
                    MessageBox.Show("Account doesn't exist.");
                    Id = Guid.Empty;
                    return;
                }

                PopulateProperties(reader);
                firstName = reader.GetString(14);
                lastName = reader.GetString(15);
                dateOfBirth = reader.GetDateTime(16);
                motherMaidenName = reader.GetString(17);
                IsSaved = true;
            }

            Owner = new Customer(firstName, lastName, dateOfBirth, motherMaidenName);
        }

        public Account(Customer customer, string accountNumber)
        {
            using (var reader = QueryBuilder
                .Table("Account")
                .Where("CustomerID", customer.Id.ToString())
                .Where("AccountNumber", accountNumber)
                .Select(
                    "AccountNumber",
                    "Balance",
                    "Interest",
                    "MaximumWithdrawalAmount",
                    "MaximumTransferAmount",
                    "GuardianAccountNumber",
                    "SupportForeignCurrency",
                    "Name",
                    "BlockedAt",
                    "CreatedAt",
                    "ClosedAt",
                    "AdministrationFee",
                    "MinimumSavingAmount",
                    "UseAutomaticRollOver"
                ).Get())
            {
                if (!reader.Read() || !reader.HasRows)
                {
                    MessageBox.Show("Account doesn't exist.");
                    Id = Guid.Empty;
                    return;
                }

                PopulateProperties(reader);
                Owner = customer;
                IsSaved = true;
            }
        }

        public Account(Customer customer)
        {
            using (var reader = QueryBuilder
                .Table("Account")
                .Where("CustomerID", customer.Id.ToString())
                .Select(
                    "AccountNumber",
                    "Balance",
                    "Interest",
                    "MaximumWithdrawalAmount",
                    "MaximumTransferAmount",
                    "GuardianAccountNumber",
                    "SupportForeignCurrency",
                    "Name",
                    "BlockedAt",
                    "CreatedAt",
                    "ClosedAt",
                    "AdministrationFee",
                    "MinimumSavingAmount",
                    "UseAutomaticRollOver"
                ).Get())
            {
                if (!reader.Read() || !reader.HasRows)
                {
                    Id = Guid.Empty;
                    return;
                }

                PopulateProperties(reader);
                Owner = customer;
                IsSaved = true;
            }
        }

        public Guid CustomerId { get; set; }
        public DebitCard? DebitCard { get; set; }
        public Customer Owner { get; set; }
        public DateTime? BlockedAt { get; set; }
        public DateTime? ClosedAt { get; set; }
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
        public string? GuardianAccountNumber { get; set; }
        public string Name { get; set; }
        public string Level { get; set; }

        private void PopulateProperties(IDataRecord reader)
        {
            AccountNumber = reader.GetString(0);
            Balance = reader.GetDecimal(1);
            Interest = reader.GetFloat(2);
            MaximumWithdrawalAmount = reader.GetDecimal(3);
            MaximumTransferAmount = reader.GetDecimal(4);
            GuardianAccountNumber = reader.IsDBNull(5) ? null : reader.GetString(5);
            SupportForeignCurrency = reader.GetBoolean(6);
            Name = reader.GetString(7);
            BlockedAt = reader.IsDBNull(8) ? (DateTime?) null : reader.GetDateTime(8);
            CreatedAt = reader.GetDateTime(9);
            ClosedAt = reader.IsDBNull(10) ? (DateTime?) null : reader.GetDateTime(10);
            AdministrationFee = reader.GetDecimal(11);
            MinimumSavingAmount = reader.GetDecimal(12);
            UseAutomaticRollOver = reader.GetBoolean(13);
        }

        public override bool Save()
        {
            if (!IsSaved)
            {
                AccountNumber = Helpers.RandomDigitString(16);
                CreatedAt = DateTime.Now;
            }

            var data = new Dictionary<string, object>
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
                {nameof(UseAutomaticRollOver), UseAutomaticRollOver},
                {nameof(BlockedAt), BlockedAt},
                {nameof(ClosedAt), ClosedAt}
            };

            var queryBuilderAccount = QueryBuilder.Table("Account");
            return IsSaved
                ? Database.Transaction(
                    () =>
                        queryBuilderAccount.Where("AccountNumber", AccountNumber).Update(data)
                        &&
                        QueryBuilder
                            .Table("Customer")
                            .Where("ID", Owner.Id.ToString())
                            .Update(
                                new Dictionary<string, object>
                                {
                                    {"IsBusinessOwner", Owner.IsBusinessOwner},
                                    {"MotherMaidenName", Owner.MotherMaidenName}
                                }
                            )
                        &&
                        QueryBuilder
                            .Table("User")
                            .Where("ID", Owner.Id.ToString())
                            .Update(
                                new Dictionary<string, object>
                                {
                                    {"FirstName", Owner.FirstName},
                                    {"LastName", Owner.LastName},
                                    {"Gender", Owner.Gender},
                                    {"DateOfBirth", Owner.DateOfBirth},
                                    {"PhoneNumber", Owner.PhoneNumber}
                                }
                            )
                )
                : queryBuilderAccount.Insert(data);
        }

        public void IncreaseBalance(decimal amount)
        {
            Balance += amount;
        }

        public void ReduceBalance(decimal amount)
        {
            Balance -= amount;
        }

        public override bool Delete()
        {
            throw new NotImplementedException();
        }
    }
}