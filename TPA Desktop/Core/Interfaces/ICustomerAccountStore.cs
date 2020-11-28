using TPA_Desktop.Core.Models;

namespace TPA_Desktop.Core.Interfaces
{
    public interface ICustomerAccountStore
    {
        Customer? Customer { get; set; }
        Account? Account { get; set; }
    }
}