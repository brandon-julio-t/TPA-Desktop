using TPA_Desktop.Core.Interfaces;
using TPA_Desktop.Core.Models;

namespace TPA_Desktop.Core.Default_Implementations
{
    public class DefaultCustomerAccountStore : ICustomerAccountStore
    {
        public Customer? Customer { get; set; }
        public Account? Account { get; set; }
    }
}