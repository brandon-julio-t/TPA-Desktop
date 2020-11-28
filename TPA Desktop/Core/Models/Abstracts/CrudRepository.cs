using TPA_Desktop.Core.Interfaces;

namespace TPA_Desktop.Core.Models.Abstracts
{
    public abstract class CrudRepository<T> : ReadOnlyRepository<T>
    {
        public abstract bool Update(T entity);
        public abstract bool Save(T entity);
        public abstract bool Delete(T entity);
    }
}