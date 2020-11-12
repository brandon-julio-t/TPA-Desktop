using TPA_Desktop.Facades.Models;
using TPA_Desktop.Models;

namespace TPA_Desktop.Facades.Builders
{
    public class AccountBuilder
    {
        private Account _account;

        public void SetAdministrationFee(decimal value)
        {
            _account.AdministrationFee = value;
        }

        public void SetBalance(decimal value)
        {
            _account.Balance = value;
        }

        public void SetGuardianAccountNumber(string value)
        {
            _account.GuardianAccountNumber = value;
        }

        public void SetInterest(double value)
        {
            _account.Interest = value;
        }

        public void SetMaximumTransferAmount(decimal value)
        {
            _account.MaximumTransferAmount = value;
        }

        public void SetMaximumWithdrawalAmount(decimal value)
        {
            _account.MaximumWithdrawalAmount = value;
        }

        public void SetMinimumSavingAmount(decimal value)
        {
            _account.MinimumSavingAmount = value;
        }

        public void SetName(string value)
        {
            _account.Name = value;
        }

        public void SetSupportForeignCurrency(bool value)
        {
            _account.SupportForeignCurrency = value;
        }

        public void SetUseAutomaticRollOver(bool value)
        {
            _account.UseAutomaticRollOver = value;
        }

        public void Reset()
        {
            _account = new Account();
        }

        public Account GetResult()
        {
            return _account;
        }
    }
}