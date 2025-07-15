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
    /// Repository cho entity CrawlLog
    /// </summary>
    public class CrawlLogRepository : Repository<CrawlLog>, ICrawlLogRepository
    {
        public CrawlLogRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<CrawlLog> GetByMangaSlugAsync(string mangaSlug)
        {
            return await _dbSet
                .Include(cl => cl.FailedChapters)
                .FirstOrDefaultAsync(cl => cl.MangaSlug == mangaSlug);
        }

        public async Task<IEnumerable<CrawlLog>> GetLatestLogsAsync(int count)
        {
            return await _dbSet
                .Include(cl => cl.FailedChapters)
                .OrderByDescending(cl => cl.CrawledAt)
                .Take(count)
                .ToListAsync();
        }
    }
}