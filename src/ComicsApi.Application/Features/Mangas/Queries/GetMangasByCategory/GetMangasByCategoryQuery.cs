using System.Collections.Generic;
using MediatR;
using ComicsApi.Application.DTOs;

namespace ComicsApi.Application.Features.Mangas.Queries.GetMangasByCategory
{
    /// <summary>
    /// Query để lấy danh sách manga theo danh mục
    /// </summary>
    public class GetMangasByCategoryQuery : IRequest<IEnumerable<MangaDto>>
    {
        public string CategorySlug { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        
        public GetMangasByCategoryQuery(string categorySlug, int page = 1, int pageSize = 20)
        {
            CategorySlug = categorySlug;
            Page = page;
            PageSize = pageSize;
        }
    }
}