using System;
using TPA_Desktop.Models;
using TPA_Desktop.Views.CustomerService.Accounts;

namespace TPA_Desktop.Facades.Builders.Directors
{
    public class AccountBuilderDirector
    {
        private readonly string _level;

        private decimal MaximumWithdrawalAmountByLevel
        {
            get
            {
                switch (_level)
                {
                    case "Bronze": return 1_000_000;
                    case "Silver": return 2_500_000;
                    case "Gold": return 5_000_000;
                    case "Black": return 10_000_000;
                    default: return -1;
                }
            }
        }

        private decimal MaximumTransferAmountByLevel
        {
            get
            {
                switch (_level)
                {
                    case "Bronze": return 15_000_000;
                    case "Silver": return 50_000_000;
                    case "Gold": return 75_000_000;
                    case "Black": return 100_000_000;
                    default: return -1;
                }
            }
        }

        private double InterestByLevel
        {
            get
            {
                switch (_level)
                {
                    case "Bronze": return 1.5;
                    case "Silver": return 2;
                    case "Gold": return 2.5;
                    case "Black": return 3;
                    default: return -1;
                }
            }
        }

        public AccountBuilderDirector(string level) => _level = level;

        public void MakeRegularAccount(AccountBuilder builder)
        {
            builder.Reset();
            builder.SetAdministrationFee(5_000);
            builder.SetBalance(50_000);
            builder.SetInterest(0.25 + InterestByLevel);
            builder.SetMaximumTransferAmount(MaximumTransferAmountByLevel);
            builder.SetMaximumWithdrawalAmount(MaximumWithdrawalAmountByLevel);
            builder.SetMinimumSavingAmount(50_000);
            builder.SetName("Regular");
            builder.SetSupportForeignCurrency(false);
            builder.SetUseAutomaticRollOver(false);
        }

        public void MakeStudentAccount(AccountBuilder builder, long guardianAccountNumber)
        {
            builder.Reset();
            builder.SetAdministrationFee(5_000);
            builder.SetBalance(5_000);
            builder.SetGuardianAccountNumber(guardianAccountNumber);
            builder.SetInterest(0.15 + InterestByLevel);
            builder.SetMaximumTransferAmount(MaximumTransferAmountByLevel);
            builder.SetMaximumWithdrawalAmount(MaximumWithdrawalAmountByLevel);
            builder.SetMinimumSavingAmount(1_000);
            builder.SetName("Student");
            builder.SetSupportForeignCurrency(false);
            builder.SetUseAutomaticRollOver(false);
        }

        public void MakeSavingAccount(AccountBuilder builder)
        {
            builder.Reset();
            builder.SetAdministrationFee(0);
            builder.SetBalance(0);
            builder.SetInterest(0.1 + InterestByLevel);
            builder.SetMaximumTransferAmount(MaximumTransferAmountByLevel);
            builder.SetMaximumWithdrawalAmount(MaximumWithdrawalAmountByLevel);
            builder.SetMinimumSavingAmount(0);
            builder.SetName("Saving");
            builder.SetSupportForeignCurrency(false);
            builder.SetUseAutomaticRollOver(false);
        }

        public void MakeDepositAccount(AccountBuilder builder, bool useAutomaticRollOver)
        {
            builder.Reset();
            builder.SetAdministrationFee(5_000);
            builder.SetBalance(1_000_000);
            builder.SetInterest(3.5);
            builder.SetMaximumTransferAmount(MaximumTransferAmountByLevel);
            builder.SetMaximumWithdrawalAmount(MaximumWithdrawalAmountByLevel);
            builder.SetMinimumSavingAmount(0);
            builder.SetName("Deposit");
            builder.SetSupportForeignCurrency(true);
            builder.SetUseAutomaticRollOver(useAutomaticRollOver);
        }

        public void MakeBusinessAccount(AccountBuilder builder)
        {
            throw new NotImplementedException();
        }
    }
}