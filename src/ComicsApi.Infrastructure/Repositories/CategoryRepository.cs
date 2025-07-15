using System.Collections.Generic;
using System.Threading.Tasks;
using ComicsApi.Domain.Entities;
using ComicsApi.Domain.Interfaces;
using ComicsApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ComicsApi.Infrastructure.Repositories
{
    /// <summary>
    /// Repository cho entity Category
    /// </summary>
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Category> GetBySlugAsync(string slug)
        {
            return await _dbSet.FirstOrDefaultAsync(c => c.Slug == slug);
        }

        public async Task<IEnumerable<Category>> GetAllWithMangasAsync()
        {
            return await _dbSet
                .Include(c => c.Mangas)
                .ToListAsync();
        }
    }
}