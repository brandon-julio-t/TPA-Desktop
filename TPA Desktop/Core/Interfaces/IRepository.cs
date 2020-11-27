using System;
using TPA_Desktop.Core.Models;

namespace TPA_Desktop.Core.Interfaces
{
    public interface IRepository<T>
    {
        T FindById(Guid id);
        T[] FindAll();
        bool Save(T entity);
        bool Delete(T entity);
    }
}