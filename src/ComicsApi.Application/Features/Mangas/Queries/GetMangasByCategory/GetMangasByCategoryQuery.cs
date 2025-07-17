using System.ComponentModel.DataAnnotations;
using ComicsApi.Application.DTOs;
using ComicsApi.Domain.Common;
using MediatR;

namespace ComicsApi.Application.Features.Mangas.Queries.GetMangasByCategory
{
    /// <summary>
    /// Query để lấy danh sách manga theo danh mục
    /// </summary>
    public class GetMangasByCategoryQuery : IRequest<PagedResult<MangaDto>>
    {
        [Required]
        public string CategorySlug { get; set; }

        [Range(1, int.MaxValue)]
        public int Page { get; set; } = 1;

        [Range(1, 100)]
        public int PageSize { get; set; } = 20;
    }
}