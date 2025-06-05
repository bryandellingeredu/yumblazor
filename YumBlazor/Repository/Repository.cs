using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using YumBlazor.Data;
using YumBlazor.Models;

namespace YumBlazor.Repository
{
  

    public class Repository <T>: IRepository<T> where T : class, IEntity
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

        public Repository(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<IEnumerable<T>> GetAllAsync() {
            using var _context = _contextFactory.CreateDbContext();
            return await _context.Set<T>().ToListAsync();
        } 
        public async Task<T?> GetByIdAsync(Guid id)
        {
            using var _context = _contextFactory.CreateDbContext();
            return await _context.Set<T>().FindAsync(id);
        }
        public async Task DeleteAsync(Guid id)
        {
            using var _context = _contextFactory.CreateDbContext();
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _context.Set<T>().Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
        public async Task AddAsync(T entity)
        {
            using var _context = _contextFactory.CreateDbContext();
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(T entity)
        {
            using var _context = _contextFactory.CreateDbContext();
            _context.Set<T>().Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteRangeAsync(IEnumerable<T> entities)
        {
            using var _context = _contextFactory.CreateDbContext();
            _context.Set<T>().RemoveRange(entities);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAllIncludingAsync(params string[] includes)
        {
            using var _context = _contextFactory.CreateDbContext();
            IQueryable<T> query = _context.Set<T>();

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.ToListAsync();
        }
    }
   
    
}
