using System;

namespace ComicsApi.Domain.Entities
{
    /// <summary>
    /// Đại diện cho log của một chapter bị lỗi khi crawl
    /// </summary>
    public class ChapterLog
    {
        public Guid Id { get; set; }
        public string ChapterName { get; set; }
        public string ChapterApiData { get; set; }
        public string? ErrorMessage { get; set; }
    }
}