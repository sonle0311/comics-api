using System;
using System.Threading.Tasks;
using ComicsApi.Domain.Entities;
using ComicsApi.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ComicsApi.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SeoMetaController : ControllerBase
    {
        private readonly IRepository<SeoMeta> _seoMetaRepository;
        private readonly ILogger<SeoMetaController> _logger;

        public SeoMetaController(IRepository<SeoMeta> seoMetaRepository, ILogger<SeoMetaController> logger)
        {
            _seoMetaRepository = seoMetaRepository;
            _logger = logger;
        }

        /// <summary>
        /// Lấy thông tin SEO theo Id
        /// </summary>
        /// <param name="id">Id của SEO meta</param>
        /// <returns>Thông tin SEO</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var seoMeta = await _seoMetaRepository.GetByIdAsync(id);
                
                if (seoMeta == null)
                {
                    return NotFound($"Không tìm thấy thông tin SEO với id: {id}");
                }
                
                return Ok(seoMeta);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy thông tin SEO theo id {Id}", id);
                return StatusCode(500, "Đã xảy ra lỗi khi xử lý yêu cầu");
            }
        }
    }
}