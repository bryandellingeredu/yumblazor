using YumBlazor.Models;

namespace YumBlazor.Repository
{
    public interface IRepository<T> where T : class, IEntity
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(Guid id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(Guid id);
        Task DeleteRangeAsync(IEnumerable<T> entities);
        Task<IEnumerable<T>> GetAllIncludingAsync(params string[] includes);

    }
}