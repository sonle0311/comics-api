using System;
using System.Threading.Tasks;
using ComicsApi.Application.Features.Chapters.Queries.GetChapterDetail;
using ComicsApi.Application.Features.Chapters.Queries.GetChaptersByMangaId;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ComicsApi.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChapterController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ChapterController> _logger;

        public ChapterController(IMediator mediator, ILogger<ChapterController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        /// <summary>
        /// Lấy danh sách chapter của một manga
        /// </summary>
        /// <param name="mangaId">Id của manga</param>
        /// <returns>Danh sách chapter</returns>
        [HttpGet("manga/{mangaId}")]
        public async Task<IActionResult> GetChaptersByMangaId(Guid mangaId)
        {
            try
            {
                var query = new GetChaptersByMangaIdQuery(mangaId);
                var result = await _mediator.Send(query);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách chapter của manga {MangaId}", mangaId);
                return StatusCode(500, "Đã xảy ra lỗi khi xử lý yêu cầu");
            }
        }

        /// <summary>
        /// Lấy chi tiết chapter
        /// </summary>
        /// <param name="mangaId">Id của manga</param>
        /// <param name="chapterName">Tên chapter</param>
        /// <returns>Chi tiết chapter</returns>
        [HttpGet("manga/{mangaId}/{chapterName}")]
        public async Task<IActionResult> GetChapterDetail(Guid mangaId, string chapterName)
        {
            try
            {
                var query = new GetChapterDetailQuery(mangaId, chapterName);
                var result = await _mediator.Send(query);
                
                if (result == null)
                {
                    return NotFound($"Không tìm thấy chapter {chapterName} của manga {mangaId}");
                }
                
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy chi tiết chapter {ChapterName} của manga {MangaId}", chapterName, mangaId);
                return StatusCode(500, "Đã xảy ra lỗi khi xử lý yêu cầu");
            }
        }
    }
}