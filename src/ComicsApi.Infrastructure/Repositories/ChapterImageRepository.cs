using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ComicsApi.Domain.Entities;
using ComicsApi.Domain.Interfaces;
using ComicsApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ComicsApi.Infrastructure.Repositories
{
    /// <summary>
    /// Repository cho entity ChapterImage
    /// </summary>
    public class ChapterImageRepository : Repository<ChapterImage>, IChapterImageRepository
    {
        public ChapterImageRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<ChapterImage>> GetByChapterIdAsync(Guid chapterId)
        {
            return await _dbSet
                .Where(ci => ci.ChapterId == chapterId)
                .OrderBy(ci => ci.Page)
                .ToListAsync();
        }

        public async Task<ChapterImage> GetByChapterIdAndPageAsync(Guid chapterId, int page)
        {
            return await _dbSet
                .FirstOrDefaultAsync(ci => ci.ChapterId == chapterId && ci.Page == page);
        }
    }
}