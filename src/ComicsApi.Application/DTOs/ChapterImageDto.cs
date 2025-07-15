using System;

namespace ComicsApi.Application.DTOs
{
    /// <summary>
    /// DTO cho entity ChapterImage
    /// </summary>
    public class ChapterImageDto
    {
        public Guid Id { get; set; }
        public Guid ChapterId { get; set; }
        public int Page { get; set; }
        public string ImageFile { get; set; }
        public string ChapterPath { get; set; }
        public string DomainCdn { get; set; }
        
        /// <summary>
        /// URL đầy đủ của ảnh: {DomainCdn}/{ChapterPath}/{ImageFile}
        /// </summary>
        public string FullImageUrl => !string.IsNullOrEmpty(DomainCdn) && !string.IsNullOrEmpty(ChapterPath) && !string.IsNullOrEmpty(ImageFile)
            ? $"{DomainCdn.TrimEnd('/')}/{ChapterPath.Trim('/')}/{ImageFile}"
            : string.Empty;
    }
}