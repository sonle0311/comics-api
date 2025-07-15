using System;
using System.Threading.Tasks;
using ComicsApi.Application.Common;
using ComicsApi.Application.Features.Categories.Queries.GetAllCategories;
using ComicsApi.Application.Features.Categories.Queries.GetCategoryBySlug;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ComicsApi.WebAPI.Controllers
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
                var response = ApiResponse<object>.SuccessResult(result, "Lấy danh sách danh mục thành công");
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy tất cả danh mục");
                var errorResponse = ApiResponse<object>.ErrorResult("Đã xảy ra lỗi khi xử lý yêu cầu", ex.Message);
                return StatusCode(500, errorResponse);
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
                    var notFoundResponse = ApiResponse<object>.ErrorResult($"Không tìm thấy danh mục với slug: {slug}");
                    return NotFound(notFoundResponse);
                }

                var response = ApiResponse<object>.SuccessResult(result, "Lấy thông tin danh mục thành công");
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh mục theo slug {Slug}", slug);
                var errorResponse = ApiResponse<object>.ErrorResult("Đã xảy ra lỗi khi xử lý yêu cầu", ex.Message);
                return StatusCode(500, errorResponse);
            }
        }
    }
}