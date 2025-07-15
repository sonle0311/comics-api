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
    /// Repository cho entity Chapter
    /// </summary>
    public class ChapterRepository : Repository<Chapter>, IChapterRepository
    {
        public ChapterRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Chapter> GetByMangaIdAndChapterNameAsync(Guid mangaId, string chapterName)
        {
            return await _dbSet
                .Include(c => c.Images)
                .FirstOrDefaultAsync(c => c.MangaId == mangaId && c.ChapterName == chapterName);
        }

        public async Task<IEnumerable<Chapter>> GetByMangaIdAsync(Guid mangaId)
        {
            return await _dbSet
                .Where(c => c.MangaId == mangaId)
                .OrderByDescending(c => c.ChapterName)
                .ToListAsync();
        }

        public async Task<IEnumerable<Chapter>> GetLatestByMangaIdAsync(Guid mangaId, int count)
        {
            return await _dbSet
                .Where(c => c.MangaId == mangaId)
                .OrderByDescending(c => c.ChapterName)
                .Take(count)
                .ToListAsync();
        }
    }
}