using System.Threading.Tasks;
using ComicsApi.Application.Features.Crawler.Commands;

namespace ComicsApi.Application.Interfaces
{
    /// <summary>
    /// Interface cho service crawl dữ liệu
    /// </summary>
    public interface ICrawlerService
    {
        /// <summary>
        /// Crawl tất cả dữ liệu theo request
        /// </summary>
        /// <param name="request">Thông tin request crawl</param>
        /// <returns>True nếu thành công</returns>
        Task<bool> CrawlAllAsync(CrawlRequest request);
        
        /// <summary>
        /// Crawl danh mục
        /// </summary>
        /// <returns>True nếu thành công</returns>
        Task<bool> CrawlCategoriesAsync();
        
        /// <summary>
        /// Crawl danh sách slug từ các trang
        /// </summary>
        /// <param name="pageStart">Trang bắt đầu</param>
        /// <param name="pageEnd">Trang kết thúc</param>
        /// <returns>Danh sách slug</returns>
        Task<string[]> CrawlSlugsFromPagesAsync(int pageStart, int pageEnd);
        
        /// <summary>
        /// Crawl thông tin manga theo danh sách slug
        /// </summary>
        /// <param name="slugs">Danh sách slug</param>
        /// <param name="request">Thông tin request crawl</param>
        /// <returns>Danh sách manga đã crawl</returns>
        Task<string[]> CrawlMangasBySlugsAsync(string[] slugs, CrawlRequest request);
        
        /// <summary>
        /// Crawl ảnh của tất cả chapter
        /// </summary>
        /// <param name="mangaSlugs">Danh sách manga slug</param>
        /// <param name="request">Thông tin request crawl</param>
        /// <returns>True nếu thành công</returns>
        Task<bool> CrawlAllChapterImagesAsync(string[] mangaSlugs, CrawlRequest request);
    }
}