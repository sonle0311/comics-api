using System.Collections.Generic;
using MediatR;
using ComicsApi.Application.DTOs;

namespace ComicsApi.Application.Features.Mangas.Queries.GetMangaList
{
    /// <summary>
    /// Query để lấy danh sách manga theo trang
    /// </summary>
    public class GetMangaListQuery : IRequest<IEnumerable<MangaDto>>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        
        public GetMangaListQuery(int page = 1, int pageSize = 20)
        {
            Page = page;
            PageSize = pageSize;
        }
    }
}