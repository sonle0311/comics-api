using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
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
                return Ok(imageDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách hình ảnh của chapter {ChapterId}", chapterId);
                return StatusCode(500, "Đã xảy ra lỗi khi xử lý yêu cầu");
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
                    return NotFound($"Không tìm thấy hình ảnh trang {page} của chapter {chapterId}");
                }
                
                var imageDto = _mapper.Map<ChapterImageDto>(image);
                return Ok(imageDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy hình ảnh trang {Page} của chapter {ChapterId}", page, chapterId);
                return StatusCode(500, "Đã xảy ra lỗi khi xử lý yêu cầu");
            }
        }
    }
}