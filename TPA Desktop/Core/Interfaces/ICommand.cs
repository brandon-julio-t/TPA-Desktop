using TPA_Desktop.Core.Models;

namespace TPA_Desktop.Core.Interfaces
{
    public interface ICommand<out T>
    {
        DebitCard? Execute();
    }
}