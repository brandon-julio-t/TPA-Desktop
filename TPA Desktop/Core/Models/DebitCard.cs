using TPA_Desktop.Core.Models.Abstracts;

namespace TPA_Desktop.Core.Models
{
    public class DebitCard : BaseModel
    {
        public Account Account;

        public DebitCard(Account account)
        {
            Account = account;
        }
        
        public override bool Save()
        {
            throw new System.NotImplementedException();
        }

        public override bool Delete()
        {
            throw new System.NotImplementedException();
        }
    }
}