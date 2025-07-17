using System.ComponentModel.DataAnnotations;
using ComicsApi.Application.DTOs;
using ComicsApi.Domain.Common;
using MediatR;

namespace ComicsApi.Application.Features.Mangas.Queries.GetMangaList
{
    /// <summary>
    /// Query để lấy danh sách manga theo trang
    /// </summary>
    public class GetMangaListQuery : IRequest<PagedResult<MangaDto>>
    {
        [Range(1, int.MaxValue)]
        public int Page { get; set; } = 1;

        [Range(1, 100)]
        public int PageSize { get; set; } = 20;
    }
}