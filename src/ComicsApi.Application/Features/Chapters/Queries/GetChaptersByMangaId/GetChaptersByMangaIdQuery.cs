using System;
using System.Collections.Generic;
using MediatR;
using ComicsApi.Application.DTOs;

namespace ComicsApi.Application.Features.Chapters.Queries.GetChaptersByMangaId
{
    /// <summary>
    /// Query để lấy danh sách chapter theo manga id
    /// </summary>
    public class GetChaptersByMangaIdQuery : IRequest<IEnumerable<ChapterDto>>
    {
        public Guid MangaId { get; set; }
        
        public GetChaptersByMangaIdQuery(Guid mangaId)
        {
            MangaId = mangaId;
        }
    }
}