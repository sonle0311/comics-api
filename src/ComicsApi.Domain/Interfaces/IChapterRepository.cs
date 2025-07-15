using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ComicsApi.Domain.Entities;

namespace ComicsApi.Domain.Interfaces
{
    /// <summary>
    /// Interface cho Chapter repository
    /// </summary>
    public interface IChapterRepository : IRepository<Chapter>
    {
        Task<Chapter> GetByMangaIdAndChapterNameAsync(Guid mangaId, string chapterName);
        Task<IEnumerable<Chapter>> GetByMangaIdAsync(Guid mangaId);
        Task<IEnumerable<Chapter>> GetLatestByMangaIdAsync(Guid mangaId, int count);
    }
}