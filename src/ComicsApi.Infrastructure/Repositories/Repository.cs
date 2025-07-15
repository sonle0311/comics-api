using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ComicsApi.Domain.Interfaces;
using ComicsApi.Infrastructure.Data;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;

namespace ComicsApi.Infrastructure.Repositories
{
    /// <summary>
    /// Lớp cơ sở cho các repository
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public virtual async Task<T> GetByIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Thêm hàng loạt các entity với xử lý duplicate
        /// </summary>
        public virtual async Task BulkInsertAsync(IList<T> entities)
        {
            try
            {
                var bulkConfig = new BulkConfig
                {
                    SetOutputIdentity = true,
                    BatchSize = 1000,
                    BulkCopyTimeout = 0
                };
                
                await _context.BulkInsertAsync(entities, bulkConfig);
            }
            catch (Exception ex) when (ex.Message.Contains("duplicate key") || ex.Message.Contains("unique constraint"))
            {
                // Bỏ qua lỗi duplicate key, log nếu cần
                // Có thể log warning ở đây nếu muốn
            }
        }
        
        /// <summary>
        /// Thêm hoặc cập nhật hàng loạt các entity (xử lý duplicate)
        /// </summary>
        public virtual async Task BulkInsertOrUpdateAsync(IList<T> entities)
        {
            var bulkConfig = new BulkConfig
            {
                SetOutputIdentity = true,
                BatchSize = 1000,
                BulkCopyTimeout = 0
            };
            
            await _context.BulkInsertOrUpdateAsync(entities, bulkConfig);
        }

        public virtual async Task<T> UpdateAsync(T entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return entity;
    }

    public virtual async Task BulkUpdateAsync(IList<T> entities)
    {
        await _context.BulkUpdateAsync(entities);
    }

        public virtual async Task RemoveAsync(T entity)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public virtual async Task RemoveRangeAsync(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
            await _context.SaveChangesAsync();
        }

        public virtual async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }
    }
}