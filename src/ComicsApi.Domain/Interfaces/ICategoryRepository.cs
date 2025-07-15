using System.Collections.Generic;
using System.Threading.Tasks;
using ComicsApi.Domain.Entities;

namespace ComicsApi.Domain.Interfaces
{
    /// <summary>
    /// Interface cho Category repository
    /// </summary>
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<Category> GetBySlugAsync(string slug);
        Task<IEnumerable<Category>> GetAllWithMangasAsync();
    }
}