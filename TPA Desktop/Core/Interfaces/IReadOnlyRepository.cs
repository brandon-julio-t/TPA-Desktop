using System;

namespace TPA_Desktop.Core.Interfaces
{
    public interface IReadOnlyRepository<out T>
    {
        T FindById(Guid id);
        T[] FindAll();
    }
}