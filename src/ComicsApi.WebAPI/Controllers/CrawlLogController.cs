using System;
using System.Threading.Tasks;
using ComicsApi.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ComicsApi.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CrawlLogController : ControllerBase
    {
        private readonly ICrawlLogRepository _crawlLogRepository;
        private readonly ILogger<CrawlLogController> _logger;

        public CrawlLogController(ICrawlLogRepository crawlLogRepository, ILogger<CrawlLogController> logger)
        {
            _crawlLogRepository = crawlLogRepository;
            _logger = logger;
        }

        /// <summary>
        /// Lấy lịch sử crawl mới nhất
        /// </summary>
        /// <param name="count">Số lượng bản ghi cần lấy</param>
        /// <returns>Danh sách lịch sử crawl</returns>
        [HttpGet("latest")]
        public async Task<IActionResult> GetLatestLogs([FromQuery] int count = 10)
        {
            try
            {
                var logs = await _crawlLogRepository.GetLatestLogsAsync(count);
                return Ok(logs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy lịch sử crawl mới nhất");
                return StatusCode(500, "Đã xảy ra lỗi khi xử lý yêu cầu");
            }
        }

        /// <summary>
        /// Lấy lịch sử crawl theo manga slug
        /// </summary>
        /// <param name="mangaSlug">Slug của manga</param>
        /// <returns>Lịch sử crawl của manga</returns>
        [HttpGet("manga/{mangaSlug}")]
        public async Task<IActionResult> GetByMangaSlug(string mangaSlug)
        {
            try
            {
                var log = await _crawlLogRepository.GetByMangaSlugAsync(mangaSlug);
                
                if (log == null)
                {
                    return NotFound($"Không tìm thấy lịch sử crawl cho manga: {mangaSlug}");
                }
                
                return Ok(log);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy lịch sử crawl theo manga slug {MangaSlug}", mangaSlug);
                return StatusCode(500, "Đã xảy ra lỗi khi xử lý yêu cầu");
            }
        }
    }
}