using System;
using System.Threading.Tasks;
using ComicsApi.Application.Features.Categories.Queries.GetAllCategories;
using ComicsApi.Application.Features.Categories.Queries.GetCategoryBySlug;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ComicsApi.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(IMediator mediator, ILogger<CategoryController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        /// <summary>
        /// Lấy tất cả danh mục
        /// </summary>
        /// <returns>Danh sách danh mục</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            try
            {
                var query = new GetAllCategoriesQuery();
                var result = await _mediator.Send(query);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy tất cả danh mục");
                return StatusCode(500, "Đã xảy ra lỗi khi xử lý yêu cầu");
            }
        }

        /// <summary>
        /// Lấy danh mục theo slug
        /// </summary>
        /// <param name="slug">Slug của danh mục</param>
        /// <returns>Thông tin danh mục</returns>
        [HttpGet("{slug}")]
        public async Task<IActionResult> GetCategoryBySlug(string slug)
        {
            try
            {
                var query = new GetCategoryBySlugQuery(slug);
                var result = await _mediator.Send(query);

                if (result == null)
                {
                    return NotFound($"Không tìm thấy danh mục với slug: {slug}");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh mục theo slug {Slug}", slug);
                return StatusCode(500, "Đã xảy ra lỗi khi xử lý yêu cầu");
            }
        }
    }
}