using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using ComicsApi.Application.Common;
using ComicsApi.Application.DTOs;
using ComicsApi.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ComicsApi.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChapterImageController : ControllerBase
    {
        private readonly IChapterImageRepository _chapterImageRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ChapterImageController> _logger;

        public ChapterImageController(IChapterImageRepository chapterImageRepository, IMapper mapper, ILogger<ChapterImageController> logger)
        {
            _chapterImageRepository = chapterImageRepository;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Lấy danh sách hình ảnh của một chapter
        /// </summary>
        /// <param name="chapterId">Id của chapter</param>
        /// <returns>Danh sách hình ảnh</returns>
        [HttpGet("chapter/{chapterId}")]
        public async Task<IActionResult> GetByChapterId(Guid chapterId)
        {
            try
            {
                var images = await _chapterImageRepository.GetByChapterIdAsync(chapterId);
                var imageDtos = _mapper.Map<List<ChapterImageDto>>(images);
                
                var response = ApiResponse<List<ChapterImageDto>>.SuccessResult(imageDtos, "Lấy danh sách hình ảnh chapter thành công");
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách hình ảnh của chapter {ChapterId}", chapterId);
                var errorResponse = ApiResponse<object>.ErrorResult("Đã xảy ra lỗi khi xử lý yêu cầu", ex.Message);
                return StatusCode(500, errorResponse);
            }
        }

        /// <summary>
        /// Lấy hình ảnh của một chapter theo số trang
        /// </summary>
        /// <param name="chapterId">Id của chapter</param>
        /// <param name="page">Số trang</param>
        /// <returns>Hình ảnh</returns>
        [HttpGet("chapter/{chapterId}/page/{page}")]
        public async Task<IActionResult> GetByChapterIdAndPage(Guid chapterId, int page)
        {
            try
            {
                var image = await _chapterImageRepository.GetByChapterIdAndPageAsync(chapterId, page);
                
                if (image == null)
                {
                    var notFoundResponse = ApiResponse<object>.ErrorResult($"Không tìm thấy hình ảnh trang {page} của chapter {chapterId}");
                    return NotFound(notFoundResponse);
                }
                
                var imageDto = _mapper.Map<ChapterImageDto>(image);
                var response = ApiResponse<ChapterImageDto>.SuccessResult(imageDto, "Lấy hình ảnh chapter thành công");
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy hình ảnh trang {Page} của chapter {ChapterId}", page, chapterId);
                var errorResponse = ApiResponse<object>.ErrorResult("Đã xảy ra lỗi khi xử lý yêu cầu", ex.Message);
                return StatusCode(500, errorResponse);
            }
        }
    }
}