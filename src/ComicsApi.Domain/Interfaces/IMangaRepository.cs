using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ComicsApi.Domain.Entities;

namespace ComicsApi.Domain.Interfaces
{
    /// <summary>
    /// Interface cho Manga repository
    /// </summary>
    public interface IMangaRepository : IRepository<Manga>
    {
        Task<Manga> GetBySlugAsync(string slug);
        Task<Manga> GetBySlugWithDetailsAsync(string slug);
        Task<IEnumerable<Manga>> GetByPageAsync(int page, int pageSize);
        Task<IEnumerable<Manga>> GetByCategoryAsync(string categorySlug, int page, int pageSize);
        Task<int> GetTotalCountAsync();
    }
}