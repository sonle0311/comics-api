using System;
using System.Collections.Generic;

namespace ComicsApi.Domain.Entities
{
    /// <summary>
    /// Đại diện cho một chương của truyện
    /// </summary>
    public class Chapter
    {
        public Guid Id { get; set; }
        public Guid MangaId { get; set; }
        public string Filename { get; set; }
        public string ChapterName { get; set; }
        public string? ChapterTitle { get; set; }
        public string ChapterApiData { get; set; }

        // Navigation properties
        public Manga Manga { get; set; }
        public ICollection<ChapterImage> Images { get; set; } = new List<ChapterImage>();
    }
}