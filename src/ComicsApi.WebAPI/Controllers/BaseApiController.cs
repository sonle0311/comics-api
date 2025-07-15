using ComicsApi.Application.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace ComicsApi.WebAPI.Controllers
{
    /// <summary>
    /// Base controller cung cấp các phương thức chung cho tất cả API controllers
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseApiController : ControllerBase
    {
        protected readonly ILogger _logger;

        protected BaseApiController(ILogger logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Xử lý exception và trả về response lỗi thống nhất
        /// </summary>
        protected ObjectResult HandleException(Exception ex, string operation)
        {
            _logger.LogError(ex, "Lỗi khi {Operation}", operation);
            var response = ApiResponse<object>.ErrorResult("Đã xảy ra lỗi khi xử lý yêu cầu", ex.Message);
            return StatusCode(500, response);
        }

        /// <summary>
        /// Tạo response thành công
        /// </summary>
        protected OkObjectResult SuccessResponse<T>(T data, string message = "Thành công")
        {
            var response = ApiResponse<T>.SuccessResult(data, message);
            return Ok(response);
        }

        /// <summary>
        /// Tạo response thành công với phân trang
        /// </summary>
        protected OkObjectResult SuccessResponse<T>(T data, PaginationInfo paginationInfo, string message = "Thành công")
        {
            var response = ApiResponse<T>.SuccessResult(data, message, paginationInfo);
            return Ok(response);
        }

        /// <summary>
        /// Tạo response lỗi Bad Request
        /// </summary>
        protected BadRequestObjectResult BadRequestResponse(string message, params string[] errors)
        {
            var response = ApiResponse<object>.ErrorResult(message, errors);
            return BadRequest(response);
        }

        /// <summary>
        /// Tạo response lỗi Not Found
        /// </summary>
        protected NotFoundObjectResult NotFoundResponse(string message)
        {
            var response = ApiResponse<object>.ErrorResult(message);
            return NotFound(response);
        }

        /// <summary>
        /// Tạo response lỗi Internal Server Error
        /// </summary>
        protected ObjectResult InternalServerErrorResponse(string message, string errorDetail = null)
        {
            var response = ApiResponse<object>.ErrorResult(message, errorDetail);
            return StatusCode(500, response);
        }
    }
}