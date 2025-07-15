using System;
using System.Collections.Generic;

namespace ComicsApi.Domain.Entities
{
    /// <summary>
    /// Đại diện cho log của quá trình crawl một truyện
    /// </summary>
    public class CrawlLog
    {
        public Guid Id { get; set; }
        public string MangaSlug { get; set; }
        public string MangaName { get; set; }
        public int TotalChapters { get; set; }
        public int ChaptersCrawledSuccess { get; set; }
        public int ChaptersCrawledFailed { get; set; }
        public DateTime CrawledAt { get; set; }
        public List<ChapterLog> FailedChapters { get; set; } = new List<ChapterLog>();
    }
}