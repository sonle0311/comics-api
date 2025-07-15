using System.Collections.Generic;
using MediatR;
using ComicsApi.Application.DTOs;

namespace ComicsApi.Application.Features.Categories.Queries.GetAllCategories
{
    /// <summary>
    /// Query để lấy tất cả danh mục
    /// </summary>
    public class GetAllCategoriesQuery : IRequest<IEnumerable<CategoryDto>>
    {
    }
}