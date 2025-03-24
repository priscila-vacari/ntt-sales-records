using System.Linq.Expressions;

namespace Sales.Infra.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetAllAsyncIncludes(params Expression<Func<T, object>>[] includes);
        Task<T?> GetByIdAsync(int id);
        Task<T?> GetByIdAsyncIncludes(int id, params Expression<Func<T, object>>[] includes);
        Task<T?> GetByKeysAsync(params object[] keys);
        Task<IEnumerable<T>> GetWhereAsync(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> GetWhereAsyncIncludes(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(params object[] keys);
        Task DeleteRangeAsync(List<T> entities);
    }
}
