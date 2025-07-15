using System;
using MediatR;
using ComicsApi.Application.DTOs;

namespace ComicsApi.Application.Features.Mangas.Queries.GetMangaBySlug
{
    /// <summary>
    /// Query để lấy thông tin manga theo slug
    /// </summary>
    public class GetMangaBySlugQuery : IRequest<MangaDto>
    {
        public string Slug { get; set; }
        
        public GetMangaBySlugQuery(string slug)
        {
            Slug = slug;
        }
    }
}