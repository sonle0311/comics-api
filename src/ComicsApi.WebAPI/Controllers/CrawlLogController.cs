using System;
using System.Threading.Tasks;
using ComicsApi.Application.Common;
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
                var response = ApiResponse<object>.SuccessResult(logs, "Lấy lịch sử crawl mới nhất thành công");
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy lịch sử crawl mới nhất");
                var errorResponse = ApiResponse<object>.ErrorResult("Đã xảy ra lỗi khi xử lý yêu cầu", ex.Message);
                return StatusCode(500, errorResponse);
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
                    var notFoundResponse = ApiResponse<object>.ErrorResult($"Không tìm thấy lịch sử crawl cho manga: {mangaSlug}");
                    return NotFound(notFoundResponse);
                }
                
                var response = ApiResponse<object>.SuccessResult(log, "Lấy lịch sử crawl theo manga thành công");
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy lịch sử crawl theo manga slug {MangaSlug}", mangaSlug);
                var errorResponse = ApiResponse<object>.ErrorResult("Đã xảy ra lỗi khi xử lý yêu cầu", ex.Message);
                return StatusCode(500, errorResponse);
            }
        }
    }
}