using System;

namespace TPA_Desktop.Core.Models.Abstracts
{
    public abstract class ReadOnlyRepository<T>
    {
        public abstract T FindById(Guid id);
        public abstract T[] FindAll();
    }
}