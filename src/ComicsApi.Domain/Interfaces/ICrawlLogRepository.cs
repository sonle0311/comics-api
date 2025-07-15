using System.Collections.Generic;
using System.Threading.Tasks;
using ComicsApi.Domain.Entities;

namespace ComicsApi.Domain.Interfaces
{
    /// <summary>
    /// Interface cho CrawlLog repository
    /// </summary>
    public interface ICrawlLogRepository : IRepository<CrawlLog>
    {
        Task<CrawlLog> GetByMangaSlugAsync(string mangaSlug);
        Task<IEnumerable<CrawlLog>> GetLatestLogsAsync(int count);
    }
}