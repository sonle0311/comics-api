using System;

namespace ComicsApi.Domain.Entities
{
    /// <summary>
    /// Đại diện cho một ảnh trong chapter
    /// </summary>
    public class ChapterImage
    {
        public Guid Id { get; set; }
        public Guid ChapterId { get; set; }
        public int Page { get; set; }

        // Tên file ảnh, ví dụ: "page_1.jpg"
        public string ImageFile { get; set; }

        // Đường dẫn chapter tương đối, ví dụ: "uploads/20231226/xxx/chapter_1"
        public string ChapterPath { get; set; }

        // Domain CDN để tạo URL đầy đủ, ví dụ: "https://sv1.otruyencdn.com"
        public string DomainCdn { get; set; }

        // Navigation property
        public Chapter Chapter { get; set; }
    }
}