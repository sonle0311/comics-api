using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ComicsApi.Domain.Entities;

namespace ComicsApi.Domain.Interfaces
{
    /// <summary>
    /// Interface cho ChapterImage repository
    /// </summary>
    public interface IChapterImageRepository : IRepository<ChapterImage>
    {
        Task<IEnumerable<ChapterImage>> GetByChapterIdAsync(Guid chapterId);
        Task<ChapterImage> GetByChapterIdAndPageAsync(Guid chapterId, int page);
    }
}