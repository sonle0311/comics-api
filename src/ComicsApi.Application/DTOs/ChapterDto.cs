using System;
using System.Collections.Generic;

namespace ComicsApi.Application.DTOs
{
    /// <summary>
    /// DTO cho entity Chapter
    /// </summary>
    public class ChapterDto
    {
        public Guid Id { get; set; }
        public Guid MangaId { get; set; }
        public string Filename { get; set; }
        public string ChapterName { get; set; }
        public string ChapterTitle { get; set; }
        public string ChapterApiData { get; set; }
        
        public List<ChapterImageDto> Images { get; set; }
    }
}