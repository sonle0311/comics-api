using MediatR;
using ComicsApi.Application.DTOs;

namespace ComicsApi.Application.Features.Categories.Queries.GetCategoryBySlug
{
    /// <summary>
    /// Query để lấy danh mục theo slug
    /// </summary>
    public class GetCategoryBySlugQuery : IRequest<CategoryDto>
    {
        public string Slug { get; set; }
        
        public GetCategoryBySlugQuery(string slug)
        {
            Slug = slug;
        }
    }
}