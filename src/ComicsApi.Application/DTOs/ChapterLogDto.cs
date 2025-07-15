using System;

namespace ComicsApi.Application.DTOs
{
    /// <summary>
    /// DTO cho entity ChapterLog
    /// </summary>
    public class ChapterLogDto
    {
        public Guid Id { get; set; }
        public string ChapterName { get; set; }
        public string ChapterApiData { get; set; }
        public string ErrorMessage { get; set; }
    }
}