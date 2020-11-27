namespace TPA_Desktop.Core.Interfaces
{
    public interface ICrudRepository<T> : IReadOnlyRepository<T>
    {
        bool Update(T entity);
        bool Save(T entity);
        bool Delete(T entity);
    }
}