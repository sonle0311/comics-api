using System;
using System.Threading.Tasks;
using ComicsApi.Application.Common;
using ComicsApi.Application.Features.Crawler.Commands;
using ComicsApi.Application.Features.Crawler.Commands.RunCrawler;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ComicsApi.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CrawlerController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CrawlerController> _logger;

        public CrawlerController(IMediator mediator, ILogger<CrawlerController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        /// <summary>
        /// Chạy crawler để lấy dữ liệu từ nguồn
        /// </summary>
        /// <param name="request">Thông tin request crawl</param>
        /// <returns>Kết quả crawl</returns>
        [HttpPost("run")]
        public async Task<IActionResult> RunCrawler([FromBody] CrawlRequest request)
        {
            try
            {
                _logger.LogInformation("Bắt đầu crawl dữ liệu từ trang {PageStart} đến trang {PageEnd}", request.PageStart, request.PageEnd);

                var command = new RunCrawlerCommand(request);
                var result = await _mediator.Send(command);

                if (result)
                {
                    var response = ApiResponse<object>.SuccessResult(null, "Crawl dữ liệu thành công");
                    return Ok(response);
                }
                else
                {
                    var errorResponse = ApiResponse<object>.ErrorResult("Crawl dữ liệu thất bại");
                    return BadRequest(errorResponse);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi chạy crawler");
                var errorResponse = ApiResponse<object>.ErrorResult("Đã xảy ra lỗi khi xử lý yêu cầu", ex.Message);
                return StatusCode(500, errorResponse);
            }
        }
    }
}