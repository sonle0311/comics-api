using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ComicsApi.Application.Common;
using ComicsApi.Domain.Common;
using ComicsApi.Application.Features.Mangas.Queries.GetMangaList;
using ComicsApi.Application.Features.Mangas.Queries.GetMangasByCategory;
using ComicsApi.Application.Features.Chapters.Queries.GetChaptersByMangaId;
using ComicsApi.Application.Features.Chapters.Queries.GetChapterDetail;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ComicsApi.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MangaController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<MangaController> _logger;

        public MangaController(IMediator mediator, ILogger<MangaController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        /// <summary>
        /// Lấy danh sách manga theo trang
        /// </summary>
        /// <param name="page">Số trang</param>
        /// <param name="pageSize">Số lượng item trên một trang</param>
        /// <returns>Danh sách manga</returns>
        [HttpGet]
        public async Task<IActionResult> GetMangas([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            try
            {
                var query = new GetMangaListQuery
                {
                    Page = page,
                    PageSize = pageSize
                };
                var result = await _mediator.Send(query);
                
                // TODO: Cần cập nhật GetMangaListQuery để trả về thông tin phân trang
                // Tạm thời sử dụng response không có pagination
                var response = ApiResponse<object>.SuccessResult(result, "Lấy danh sách manga thành công");
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách manga");
                var errorResponse = ApiResponse<object>.ErrorResult("Đã xảy ra lỗi khi xử lý yêu cầu", ex.Message);
                return StatusCode(500, errorResponse);
            }
        }

        /// <summary>
        /// Lấy danh sách manga theo danh mục
        /// </summary>
        /// <param name="categorySlug">Slug của danh mục</param>
        /// <param name="page">Số trang</param>
        /// <param name="pageSize">Số lượng item trên một trang</param>
        /// <returns>Danh sách manga thuộc danh mục</returns>
        [HttpGet("category/{categorySlug}")]
        public async Task<IActionResult> GetMangasByCategory(string categorySlug, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            try
            {
                var query = new GetMangasByCategoryQuery
                {
                    CategorySlug = categorySlug,
                    Page = page,
                    PageSize = pageSize
                };
                var result = await _mediator.Send(query);
                
                // TODO: Cần cập nhật GetMangasByCategoryQuery để trả về thông tin phân trang
                var response = ApiResponse<object>.SuccessResult(result, $"Lấy danh sách manga theo danh mục {categorySlug} thành công");
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách manga theo danh mục {CategorySlug}", categorySlug);
                var errorResponse = ApiResponse<object>.ErrorResult("Đã xảy ra lỗi khi xử lý yêu cầu", ex.Message);
                return StatusCode(500, errorResponse);
            }
        }

        /// <summary>
        /// Lấy danh sách chapter của một manga
        /// </summary>
        /// <param name="mangaId">Id của manga</param>
        /// <returns>Danh sách chapter</returns>
        [HttpGet("{mangaId}/chapters")]
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
        /// Lấy chi tiết chapter của một manga
        /// </summary>
        /// <param name="mangaId">Id của manga</param>
        /// <param name="chapterName">Tên chapter</param>
        /// <returns>Chi tiết chapter</returns>
        [HttpGet("{mangaId}/chapters/{chapterName}")]
        public async Task<IActionResult> GetChapterDetail(Guid mangaId, string chapterName)
        {
            try
            {
                var query = new GetChapterDetailQuery(mangaId, chapterName);
                var result = await _mediator.Send(query);
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