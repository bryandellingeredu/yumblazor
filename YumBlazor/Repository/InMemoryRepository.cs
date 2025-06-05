
namespace YumBlazor.Repository
{
    public class InMemoryRepository<T> : IRepository<T> where T : class, IEntity
    {
        private readonly List<T> _items = new();
        public Task AddAsync(T entity)
        {
            _items.Add(entity);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Guid id)
        {
            var entity = _items.FirstOrDefault(e => e.Id == id);
            if (entity != null)
            {
                _items.Remove(entity);
            }
            return Task.CompletedTask;
        }

        public Task DeleteRangeAsync(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                _items.Remove(entity);
            }
            return Task.CompletedTask;
        }

        public Task<IEnumerable<T>> GetAllAsync()
        {
            return Task.FromResult(_items.AsEnumerable());
        }

        public Task<IEnumerable<T>> GetAllIncludingAsync(params string[] includes)
        {
            return Task.FromResult(_items.AsEnumerable());
        }

        public Task<T?> GetByIdAsync(Guid id)
        {
            var entity = _items.FirstOrDefault(e => e.Id == id);
            return Task.FromResult(entity);
        }

        public Task UpdateAsync(T entity)
        {
            var index = _items.FindIndex(e => e.Id == entity.Id);
            if (index >= 0)
            {
                _items[index] = entity;
            }
            return Task.CompletedTask;
        }
    }
}
