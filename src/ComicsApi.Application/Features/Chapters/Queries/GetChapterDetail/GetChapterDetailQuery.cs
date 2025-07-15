using System;
using MediatR;
using ComicsApi.Application.DTOs;

namespace ComicsApi.Application.Features.Chapters.Queries.GetChapterDetail
{
    /// <summary>
    /// Query để lấy chi tiết chapter theo manga id và chapter name
    /// </summary>
    public class GetChapterDetailQuery : IRequest<ChapterDto>
    {
        public Guid MangaId { get; set; }
        public string ChapterName { get; set; }
        
        public GetChapterDetailQuery(Guid mangaId, string chapterName)
        {
            MangaId = mangaId;
            ChapterName = chapterName;
        }
    }
}