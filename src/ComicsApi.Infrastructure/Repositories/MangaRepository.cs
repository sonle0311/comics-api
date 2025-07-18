using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ComicsApi.Domain.Entities;
using ComicsApi.Domain.Interfaces;
using ComicsApi.Domain.Common;
using ComicsApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ComicsApi.Infrastructure.Repositories
{
    /// <summary>
    /// Repository cho entity Manga
    /// </summary>
    public class MangaRepository : Repository<Manga>, IMangaRepository
    {
        public MangaRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Manga> GetBySlugAsync(string slug)
        {
            return await _dbSet.FirstOrDefaultAsync(m => m.Slug == slug);
        }

        public async Task<Manga> GetBySlugWithDetailsAsync(string slug)
        {
            return await _dbSet
                .Include(m => m.Categories)
                .Include(m => m.Chapters)
                .Include(m => m.Seo)
                .FirstOrDefaultAsync(m => m.Slug == slug);
        }

        public async Task<PagedResult<Manga>> GetByPageAsync(int page, int pageSize)
        {
            var totalCount = await _dbSet.CountAsync();
            var items = await _dbSet
                .OrderByDescending(m => m.UpdatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Include(m => m.Categories)
                .ToListAsync();
            return new PagedResult<Manga>(items, totalCount, page, pageSize);
        }

        public async Task<PagedResult<Manga>> GetByCategoryAsync(string categorySlug, int page, int pageSize)
        {
            var query = _dbSet.Where(m => m.Categories.Any(c => c.Slug == categorySlug));
            var totalCount = await query.CountAsync();
            var items = await query
                .OrderByDescending(m => m.UpdatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Include(m => m.Categories)
                .ToListAsync();
            return new PagedResult<Manga>(items, totalCount, page, pageSize);
        }

        public async Task<int> GetTotalCountAsync()
        {
            return await _dbSet.CountAsync();
        }
    }
}