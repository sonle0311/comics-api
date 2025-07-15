using System;
using System.Collections.Generic;

namespace ComicsApi.Application.DTOs
{
    /// <summary>
    /// DTO cho entity CrawlLog
    /// </summary>
    public class CrawlLogDto
    {
        public Guid Id { get; set; }
        public string MangaSlug { get; set; }
        public string MangaName { get; set; }
        public int TotalChapters { get; set; }
        public int ChaptersCrawledSuccess { get; set; }
        public int ChaptersCrawledFailed { get; set; }
        public DateTime CrawledAt { get; set; }
        public List<ChapterLogDto> FailedChapters { get; set; }
    }
}